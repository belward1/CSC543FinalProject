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
            Console.WriteLine("\n\nParalellForEachWithThreadLocal ============================== \n");
            int[] array = { 9, 3, 6, 4, 1, 8, 2, 7, 10, 5 };
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
                                            Console.WriteLine($"Thread: {Thread.CurrentThread.ManagedThreadId}, n={n}, LocalSum={localSum}");
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
