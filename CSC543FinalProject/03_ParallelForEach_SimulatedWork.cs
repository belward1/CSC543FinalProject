using JcipAnnotations;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CSC543FinalProject
{
    /**
     * <summary>ParallelForEach_SimulatedWork class.  
     *          Simulates a set of work by sleeping 1 ms for each task.
     *          This demonstrates to benefit of creating multiple tasks 
     *          to perform this work.
     * </summary>
     * <remarks>Written By: Bob Elward - April 2021</remarks>
     */
    [Immutable()]
    public class ParallelForEach_SimulatedWork
    {
        /**
         * <summary>Run method.  
         *          Invokes the set of examples.
         * </summary>
         * <remarks>Written By: Bob Elward - April 2021</remarks>
         */
        public static void Run()
        {
            Console.WriteLine("\n\n" + ("ParallelForEach - SimulatedWork" + " " + new string('=', 115)).Substring(0, 115));
            Program.HaltIfDebug();

            Stopwatch stopwatch = new Stopwatch();
            IEnumerable<int> range = Enumerable.Range(0, 1000);
            Partitioner<int> partitioner = Partitioner.Create<int>(range);

            //******************************

            //Thread.Sleep(5 * 1000);
            GC.Collect();
            Console.WriteLine("\nSequential For: \n");
            stopwatch.Reset();
            stopwatch.Start();

            foreach (int i in range)
            {
                // Workload that requires 1 ms to complete
                Thread.Sleep(1);
            }

            stopwatch.Stop();
            Console.WriteLine($"Duration: {stopwatch.Elapsed}");

            //******************************

            //Thread.Sleep(5 * 1000);
            GC.Collect();
            Console.WriteLine("\nParallel ForEach (Defaults): \n");
            stopwatch.Reset();
            stopwatch.Start();

            Parallel.ForEach<int>(partitioner
                                , i =>
                                {
                                    // Workload that requires 1 ms to complete
                                    Thread.Sleep(1);
                                }
                                 );

            stopwatch.Stop();
            Console.WriteLine($"Duration: {stopwatch.Elapsed}");

            //******************************

            //Thread.Sleep(5 * 1000);
            GC.Collect();
            int maxParallelism = Environment.ProcessorCount;
            Console.WriteLine("\nParallel ForEach (maxParallelism = {0}): \n", maxParallelism);
            stopwatch.Reset();
            stopwatch.Start();

            ParallelOptions parallelOptions = new ParallelOptions() { MaxDegreeOfParallelism = maxParallelism };

            Parallel.ForEach<int>(partitioner
                                , parallelOptions
                                , i =>
                                {
                                    // Workload that requires 1 ms to complete
                                    Thread.Sleep(1);
                                }
                                 );

            stopwatch.Stop();
            Console.WriteLine($"Duration: {stopwatch.Elapsed}");

            //******************************

            //Thread.Sleep(5 * 1000);
            GC.Collect();
            int minThreads =  100;
            Console.WriteLine("\nParallel ForEach (minThreads: {0}): \n", minThreads);
            stopwatch.Reset();
            stopwatch.Start();

            ThreadPool.SetMinThreads(minThreads, 4);

            Parallel.ForEach<int>(partitioner
                                , i =>
                                {
                                    // Workload that requires 1 ms to complete
                                    Thread.Sleep(1);
                                }
                                 );

            stopwatch.Stop();
            Console.WriteLine($"Duration: {stopwatch.Elapsed}");

            //******************************

            return;
        }
    }
}
