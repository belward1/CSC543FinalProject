using JcipAnnotations;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSC543FinalProject
{
    /**
     * <summary>ParallelMapReduce class.  
     *          A set of Parallel.ForEach examples performing a Map and Reduce algorithm.
     *          A large set of text is mapped into individual words and 
     *          reduced to the number of times each word occurs.
     * </summary>
     * <see cref="http://www.jakemdrew.com/blog/mapreduce.htm">Based on this article by Jake Drew,01/08/2013.</see>
     * <remarks>Written By: Bob Elward - April 2021</remarks>
     */
    [Immutable()]
    public class ParallelMapReduce_MemoryT
    {
        /**
         * <summary>Run method.  
         *          Invokes the set of examples.
         * </summary>
         */
        public static void Run()
        {
            Console.WriteLine("\n\nParallelMapReduce_MemoryT_Tasks ===================================== \n");

            Dictionary<string, int> wordStoreDict = new Dictionary<string, int>();
            ConcurrentDictionary<string, int> wordStore = new ConcurrentDictionary<string, int>();
            ParallelOptions parallelOptions = new ParallelOptions() { MaxDegreeOfParallelism = -1 }; // Set to -1 for .NET to determine
            int totalWords;
            KeyValuePair<string, int> minKvp;
            KeyValuePair<string, int> maxKvp;
            object lockObj = new object();

            Stopwatch stopwatch = new Stopwatch();

            // Get the text string for a long text file
            int multiplier = 200;
            Memory<char> fileText = GetFileText(multiplier);

            //Thread.Sleep(5 * 1000);
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            GC.Collect();
            Console.WriteLine("\nSequential For: \n");
            wordStore.Clear();
            stopwatch.Reset();
            stopwatch.Start();

            // Map the words
            foreach (Memory<char> wordBlock in ProduceWordBlocks(fileText))
            {
                StringBuilder wordBuffer = new StringBuilder();
                List<string> wordsList = new List<string>();

                // Split the words, cleanup each word and map it
                char character;
                for (int i = 0; i < wordBlock.Length; i++)
                {
                    character = wordBlock.Span[i];

                    if (character == ' ') continue;

                    while (character != ' ')
                    {
                        if (char.IsLetterOrDigit(character) || character == '\'' || character == '-')
                            wordBuffer.Append(character);
                        if (++i >= wordBlock.Length) break;
                        character = Convert.ToChar(wordBlock.Slice(i, 1).ToString());
                    }
                    // Save the word
                    if (wordBuffer.Length > 0)
                    {
                        wordsList.Add(wordBuffer.ToString());
                        wordBuffer.Clear();
                    }
                }

                //Console.WriteLine($"Aggregating {wordsList.Count} words for thread: {Thread.CurrentThread.ManagedThreadId}");
                foreach (string word in wordsList)
                {
                    int value = 0;
                    wordStoreDict.TryGetValue(word, out value); // Get the current value for this word if it exists or zero
                    wordStoreDict[word] = ++value;              // Increment the value and update or add the word
                }
            }

            stopwatch.Stop();
            Console.WriteLine($"Duration: {stopwatch.Elapsed}");

            // Report word counts
            totalWords = wordStoreDict.Sum(kvp => kvp.Value);
            Console.WriteLine($"\nNumber of unique found: {wordStoreDict.Count} - Total words found: {totalWords,1:#,###,###,###,##0}");
            minKvp = wordStoreDict.OrderBy(kvp => kvp.Key).Aggregate((l_kvp, r_kvp) => l_kvp.Value < r_kvp.Value ? l_kvp : r_kvp);
            maxKvp = wordStoreDict.OrderBy(kvp => kvp.Key).Aggregate((l_kvp, r_kvp) => l_kvp.Value > r_kvp.Value ? l_kvp : r_kvp);
            Console.WriteLine($"Word: {minKvp.Key,15:s1} - occurs the minimum number of times: {minKvp.Value,1:#,###,###,###,##0}");
            Console.WriteLine($"Word: {maxKvp.Key,15:s1} - occurs the minimum number of times: {maxKvp.Value,1:#,###,###,###,##0}");

            //Thread.Sleep(5 * 1000);
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            GC.Collect();
            Console.WriteLine("\nParallel For: \n");
            wordStore.Clear();
            stopwatch.Reset();
            stopwatch.Start();

            // Map the words
            Parallel.ForEach<Memory<char>, List<string>>(ProduceWordBlocks(fileText)                               // input enumerator
                                                       , parallelOptions                                           // parallel options - MaxDegreeOfparallelism 
                                                       , () =>                                                     // initialize the ThreadLocal variable
                                                       {
                                                           return new List<string>();
                                                       }
                                                       , (wordBlock, loopState, wordsList) =>                      // body delegate
                                                       {
                                                           StringBuilder wordBuffer = new StringBuilder();

                                                           // Split the words, cleanup each word and map it
                                                           char character;
                                                           for (int i = 0; i < wordBlock.Length; i++)
                                                           {
                                                               character = wordBlock.Span[i];

                                                               if (character == ' ') continue;

                                                               while (character != ' ')
                                                               {
                                                                   if (char.IsLetterOrDigit(character) || character == '\'' || character == '-')
                                                                       wordBuffer.Append(character);
                                                                   if (++i >= wordBlock.Length) break;
                                                                   character = Convert.ToChar(wordBlock.Slice(i, 1).ToString());
                                                               }
                                                               // Save the word
                                                               if (wordBuffer.Length > 0)
                                                               {
                                                                   wordsList.Add(wordBuffer.ToString());
                                                                   wordBuffer.Clear();
                                                               }
                                                           }
                                                           return wordsList;
                                                       }
                                                       , (wordsList) =>                                            // thread aggregator
                                                       {
                                                           //Console.WriteLine($"Aggregating {wordsList.Count} words for thread: {Thread.CurrentThread.ManagedThreadId}");
                                                           Parallel.ForEach(wordsList
                                                                          , parallelOptions
                                                                          , word =>
                                                                          {   
                                                                              // if the word exists, use a thread safe delegate to increment the value by 1
                                                                              // otherwise, add the word with a default value of 1
                                                                              wordStore.AddOrUpdate(word, 1, (key, oldValue) => Interlocked.Increment(ref oldValue));
                                                                          }
                                                                           );
                                                       }
                                                        );

            stopwatch.Stop();
            Console.WriteLine($"Duration: {stopwatch.Elapsed}");

            // Report word counts
            totalWords = wordStore.Sum(kvp => kvp.Value);
            Console.WriteLine($"\nNumber of unique found: {wordStore.Count} - Total words found: {totalWords,1:#,###,###,###,##0}");
            minKvp = wordStore.OrderBy(kvp => kvp.Key).Aggregate((l_kvp, r_kvp) => l_kvp.Value < r_kvp.Value ? l_kvp : r_kvp);
            maxKvp = wordStore.OrderBy(kvp => kvp.Key).Aggregate((l_kvp, r_kvp) => l_kvp.Value > r_kvp.Value ? l_kvp : r_kvp);
            Console.WriteLine($"Word: {minKvp.Key,15:s1} - occurs the minimum number of times: {minKvp.Value,1:#,###,###,###,##0}");
            Console.WriteLine($"Word: {maxKvp.Key,15:s1} - occurs the minimum number of times: {maxKvp.Value,1:#,###,###,###,##0}");

            return;
        }

        /**
         * <summary>ProduceWordBlocks method.  
         *          Invokes the set of examples.
         * </summary>
         * <param name="fileText">Text to parse into blocks of words.</param>
         * <returns>A block of words as an IEnumerable<string>.</returns>
         */
        private static IEnumerable<Memory<char>> ProduceWordBlocks(Memory<char> fileText)
        {
            int blockSize = 10_000; // 250;
            int startPos = 0;
            int len = 0;

            for (int i = 0; i < fileText.Length; i++)
            {
                i = ((i + blockSize) > (fileText.Length - 1)) ? (fileText.Length - 1) : (i + blockSize);

                while (i >= startPos && fileText.Span[i] != ' ')
                {
                    i--;
                }

                if (i == startPos)
                {
                    i = i + blockSize > (fileText.Length - 1) ? fileText.Length - 1 : i + blockSize;
                    len = (i - startPos) + 1;
                }
                else
                {
                    len = i - startPos;
                }

                yield return fileText.Slice(startPos, len);
                startPos = i;
            }
        }

        /**
         * <summary>GetFileText method.  
         *          Gets the text to be parsed from the specified file.
         *          This will repeat the file text multiple times to build up
         *          a large set of text.
         * </summary>
         * <param name="multiplier">Number of times to repeat the text found in the file.</param>
         * <returns>A string of text.</returns>
         */
        private static Memory<char> GetFileText(int multiplier)
        {
            // Stopwatch for timing
            Stopwatch stopWatch = new Stopwatch();

            // Get text from a file
            string filename = "world192.txt";
            string filepath = "../../../" + filename;
            if (File.Exists("./" + filename)) filepath = "./" + filename;
            string fileText = File.ReadAllText(filepath).Replace("\n", "");

            // Start stopwatch
            stopWatch = Stopwatch.StartNew();

            // Make multiple copies of this file to increase the size
            int capacity = fileText.Length * multiplier;
            StringBuilder sbFileText = new StringBuilder(fileText, capacity);
            for (int i = 1; i < multiplier; i++)
            {
                sbFileText.Append(fileText);
            }

            // Collect elapsed time and report
            stopWatch.Stop();
            double textCreateTimeSecs = (double)stopWatch.ElapsedMilliseconds / 1000.0;
            Console.WriteLine($"Text create time: {textCreateTimeSecs} (secs)");

            /*
             * Quick converasion factors:
             * 
             *   1 KB =   1 * (2**10) =              1,024 bytes
             *   1 MB =   1 * (2**20) =          1,048,576 bytes
             *   1 GB =   1 * (2**30) =      1,073,741,824 bytes   
             *   1 TB =   1 * (2**40) =  1,099,511,627,776 bytes
             *   
             *   1 KB =   1 * (2**10) =              1,024 bytes
             *  10 KB =  10 * (2**10) =             10,240 bytes
             * 100 KB = 100 * (2**10) =            102,400 bytes
             *   1 MB =   1 * (2**20) =          1,048,576 bytes
             *  10 MB =  10 * (2**20) =         10,485,760 bytes
             * 100 MB = 100 * (2**20) =        104,857,600 bytes
             *   1 GB =   1 * (2**30) =      1,073,741,824 bytes   
             *  10 GB =  10 * (2**30) =     10,737,418,240 bytes
             * 100 GB = 100 * (2**30) =    107,374,182,400 bytes
             *   1 TB =   1 * (2**40) =  1,099,511,627,776 bytes
             *  10 TB =  10 * (2**40) = 10,995,116,277,760 bytes
             * 
             */
            Console.WriteLine($"File text size: {(sbFileText.Length / (Math.Pow(2, 30))),1:f3} (GB) - {sbFileText.Length,1:#,###,###,###,##0} (Bytes)");

            Memory<char> memory = new Memory<char>(sbFileText.ToString().ToCharArray());
            sbFileText = null;

            return memory;
        }
    }
}
