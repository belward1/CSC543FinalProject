using JcipAnnotations;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSC543FinalProject
{
    /**
     * <summary>ParallelForSimple class.  
     *          A set of simple Parallel.For examples.
     * </summary>
     * <remarks>Written By: Bob Elward - April 2021</remarks>
     */
    [Immutable()]
    public class ParallelInvoke
    {
        /**
         * <summary>Run method.  
         *          Invokes the set of examples.
         * </summary>
         */
        public static void Run()
        {
            Console.WriteLine("\n\n" + ("ParallellInvoke" + " " + new string('=', 115)).Substring(0, 115));
            Program.HaltIfDebug();

            Stopwatch stopwatch = new Stopwatch();
            Stopwatch stopwatchT = new Stopwatch();

            // Retrieve Goncharov's "Oblomov" from Gutenberg.org.
            int multiplier = 500;
            string[] words = CreateWordArray(@"http://www.gutenberg.org/files/54700/54700-0.txt", multiplier);
            Console.WriteLine($"\nTotal words: {words.Length,1:#,###,###,###,##0}");

            //******************************

            //Thread.Sleep(5 * 1000);
            GC.Collect();
            Console.WriteLine("\nSequential: \n");
            stopwatchT.Reset();
            stopwatchT.Start();

            // First Action
            Console.WriteLine("Begin first...");
            stopwatch.Reset();
            stopwatch.Start();
            GetMostCommonWords(words);
            Console.WriteLine($"Duration: {stopwatch.Elapsed}");

            // Second Action
            Console.WriteLine("Begin second...");
            stopwatch.Reset();
            stopwatch.Start();
            GetCountForWord(words, "sleep");
            Console.WriteLine($"Duration: {stopwatch.Elapsed}");

            // Third Action
            Console.WriteLine("Begin third...");
            stopwatch.Reset();
            stopwatch.Start();
            GetLongestWord(words);
            Console.WriteLine($"Duration: {stopwatch.Elapsed}");

            stopwatchT.Stop();
            Console.WriteLine($"\nTotal Duration: {stopwatchT.Elapsed}");

            //******************************

            Program.HaltIfDebug();

            //******************************

            try
            {
                //******************************

                Console.WriteLine("\nParallel Invoke: \n");
                stopwatch.Reset();
                stopwatch.Start();
                
                Parallel.Invoke(() => {   // First Action
                                          Console.WriteLine("Begin first task...");
                                          GetMostCommonWords(words);
                                      },                    
                                () => {   // Second Action
                                          Console.WriteLine("Begin second task...");
                                          GetCountForWord(words, "sleep");
                                      }, 
                                () => {   // Third Action
                                          Console.WriteLine("Begin third task...");
                                          GetLongestWord(words);
                                      }
                               ); // Close parallel.invoke
                
                stopwatch.Stop();
                Console.WriteLine($"Duration: {stopwatch.Elapsed}");
            }
            catch (AggregateException aggEx)
            {
                Console.WriteLine($"ERROR-AggregateException: {aggEx.Message}");
            }

            //******************************

            return;
        }

        /// <summary>
        /// Finds the most common words from an array of words.
        /// </summary>
        /// <param name="words">Array of words to search.</param>
        private static void GetMostCommonWords(string[] words)
        {
            var frequencyOrder = from word in words
                                 where word.Length > 6
                                 group word by word into g
                                 orderby g.Count() descending
                                 select g.Key;

            var commonWords = frequencyOrder.Take(10);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Task 1 -- The most common words are:");
            foreach (var v in commonWords)
            {
                sb.AppendLine("  " + v);
            }
            Console.WriteLine(sb.ToString());
        }

        /// <summary>
        /// Counts the number of times aspecified word occurs in an array of words.
        /// </summary>
        /// <param name="words">Array of words to search.</param>
        /// <param name="term">Word to find.</param>
        private static void GetCountForWord(string[] words, string term)
        {
            var findWord = from word in words
                           where word.ToUpper().Contains(term.ToUpper())
                           select word;

            Console.WriteLine($@"Task 2 -- The word ""{term}"" occurs {findWord.Count()} times.");
        }

        /// <summary>
        /// Finds the longerst word in an array of words.
        /// </summary>
        /// <param name="words">Array of words to search.</param>
        /// <returns></returns>
        private static string GetLongestWord(string[] words)
        {
            var longestWord = (from w in words
                               orderby w.Length descending
                               select w).First();

            Console.WriteLine($"Task 3 -- The longest word is {longestWord}.");
            return longestWord;
        }

        /// <summary>
        /// Retrieves a web page and parses it into an array of words.
        /// </summary>
        /// <param name="uri">Web page Uri.</param>
        /// <returns></returns>
        static string[] CreateWordArray(string uri, int multiplier)
        {
            Console.WriteLine($"\nRetrieving from {uri}");

            // Download a web page the easy way.
            string page = new WebClient().DownloadString(uri);
            StringBuilder pages = new StringBuilder(multiplier * page.Length + multiplier);
            for (int  i = 0; i < multiplier; i++)
            {
                pages.Append(page + " ");
            }

            // Separate string into an array of words, removing some common punctuation.
            return pages.ToString().Split(new char[] { ' ', '\u000A', ',', '.', ';', ':', '-', '_', '/' }
                                        , StringSplitOptions.RemoveEmptyEntries);
        }

    }
}
