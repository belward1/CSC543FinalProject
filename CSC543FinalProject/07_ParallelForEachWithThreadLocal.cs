using JcipAnnotations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CSC543FinalProject
{
    /**
     * <summary>ParallelForEachWithThreadLocal class.  
     *          A set of Parallel.ForEach examples.
     * </summary>
     * <remarks>Written By: Bob Elward - April 2021</remarks>
     */
    [Immutable()]
    public class ParallelForEachWithThreadLocal
    {
        /**
         * <summary>Run method.  
         *          Invokes the set of examples.
         * </summary>
         */
        public static void Run()
        {
            Console.WriteLine("\n\n" + ("ParalellForEach - With ThreadLocal" + " " + new string('=', 115)).Substring(0, 115));
            Program.HaltIfDebug();

            int[] array = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            Console.Write("\narray: ");
            for (int i = 0; i < array.Length; i++) { Console.Write((i != 0 ? ", " : "") + array[i]); }
            Console.WriteLine();
            int finalSum = 0;

            //******************************

            Console.WriteLine("\nParallel ForEach: \n");
            Parallel.ForEach<int, int>(array                                                // input array
                                        , () =>                                             // initialize the ThreadLocal variable
                                        { 
                                            return 0; 
                                        }
                                        , (n, loopState, localSum) =>                       // body delegate
                                        {
                                            // Sum each element passed to this task
                                            localSum += n;
                                            Console.WriteLine($"xThread: {Thread.CurrentThread.ManagedThreadId, 3:##0}, n={n,3:##0}, LocalSum={localSum,3:##0}");
                                            return localSum;
                                        }
                                        , (localSum) =>                                     // finalize (aggregator)
                                        {
                                            Interlocked.Add(ref finalSum, localSum);
                                        }
                                        );
            Console.WriteLine($"\nSum = {finalSum}");

            //******************************

            return;
        }
    }
}
