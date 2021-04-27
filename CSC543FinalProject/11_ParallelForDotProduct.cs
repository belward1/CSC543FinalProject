using JcipAnnotations;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime;
using System.Threading.Tasks;

namespace CSC543FinalProject
{
    /**
     * <summary>ParallelForDotProduct class.  
     *          A set of Parallel.For/.ForEach examples performing a dot product.
     * </summary>
     * <remarks>Written By: Bob Elward - April 2021</remarks>
     */
    [Immutable()]
    public class ParallelForDotProduct
    {
        /**
         * <summary>Run method.  
         *          Invokes the set of examples.
         * </summary>
         */
        public static void Run()
        {
            Console.WriteLine("\n\n" + ("ParallelFor/Foreach - DotProduct" + " " + new string('=', 115)).Substring(0, 115));
            Program.HaltIfDebug();

            // Generate two random vectors
            const int VECTOR_SIZE = 100_000_000;
            double[] a = GenerateRandomVector(VECTOR_SIZE);
            double[] b = GenerateRandomVector(VECTOR_SIZE);
            Stopwatch stopwatch = new Stopwatch();
            double sum;
            object lockObj = new object();

            //******************************

            //Thread.Sleep(5 * 1000);
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            GC.Collect();
            Console.WriteLine("\nSequential For: \n");
            stopwatch.Reset();
            stopwatch.Start();
            sum = 0.0;
            for (int i = 0; i < VECTOR_SIZE; i++)
            {
                sum += a[i] * b[i];
            }
            stopwatch.Stop();
            Console.WriteLine($"Duration: {stopwatch.Elapsed} - Result: {sum,20:f1}");

            //******************************

            //Thread.Sleep(5 * 1000);
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            GC.Collect();
            Console.WriteLine("\nParallel For: \n");
            stopwatch.Reset();
            stopwatch.Start();
            sum = 0.0;
            Parallel.For(0                                          // from inclusive
                       , VECTOR_SIZE                                // to exclusive
                       , () =>                                      // initialize the thread local state
                         {
                             return 0.0;
                         }
                       , (i, loopState, localSum) =>                // body delegate
                         {
                             localSum += a[i] * b[i];               
                             return localSum;                       
                         }                                            
                       , (partialSum) =>                            // finalize: aggregate
                         {
                             lock (lockObj) sum += partialSum;
                         }
                        );
            stopwatch.Stop();
            Console.WriteLine($"Duration: {stopwatch.Elapsed} - Result: {sum,20:f1}");

            //******************************

            //Thread.Sleep(5 * 1000);
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            GC.Collect();
            Console.WriteLine("\nParallel ForEach w/ Range Partitioner: \n");
            stopwatch.Reset();
            stopwatch.Start();
            sum = 0.0;
            Parallel.ForEach(Partitioner.Create(0, VECTOR_SIZE)                // range partitioner
                           , () =>                                             // initialize the ThreadLocal variable
                             {
                                 return 0.0;
                             }
                           , (range, state, localSum) =>                       // body delegate
                             {
                                 for (int i = range.Item1; i < range.Item2; i++)
                                 {
                                     localSum += a[i] * b[i];
                                 }
                                 return localSum;
                             }
                           , (partialSum) =>                                   // thread aggregator
                             {
                                 lock (lockObj) sum += partialSum;
                             }
                            );
            stopwatch.Stop();
            Console.WriteLine($"Duration: {stopwatch.Elapsed} - Result: {sum,20:f1}");

            //******************************

            return;
        }

        /**
         * <summary>GenerateRandomVector method.  
         *          Generates a vector of random double values.
         * </summary>
         * <param name="length">Size of vector to generate.</param>
         * <returns>A vector of randum double values.</returns>
         */
        public static double[] GenerateRandomVector(int length)
        {
            Random rnd = new Random();
            double[] v = new double[length];
            for (int i = 0; i < length; i++)
            {
                v[i] = rnd.NextDouble();
            }
            return v;
        }
    }
}
