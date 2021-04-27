using JcipAnnotations;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime;
using System.Threading;
using System.Threading.Tasks;

namespace CSC543FinalProject
{
    /**
     * <summary>ParallelForMatrixMultiply class.  
     *          A set of Parallel.For/.ForEach examples performing a dot product.
     * </summary>
     * <remarks>Written By: Bob Elward - April 2021</remarks>
     */
    [Immutable()]
    public class ParallelForPiCalculation
    {
        /**
         * <summary>Run method.  
         *          Invokes the set of examples.
         * </summary>
         */
        public static void Run()
        {
            Console.WriteLine("\n\n" + ("ParallelForPiCalculation " + new string('=', 100)).Substring(0, 100) + "\n");
            Console.WriteLine($"Pi Refernce value from c#'s Math.PI const                - Pi = {Math.PI,19:f16}");
            Console.WriteLine("From: https://en.wikipedia.org/wiki/Pi                   - Pi =  3.1415926535897932384626433");

            // Generate two random matrices and a third to hold the product
            const long maxSteps = 2_000_000_000;
            double pi;
            long iter;
            double result;
            double prior;
            double factor1;
            long n;
            Stopwatch stopwatch = new Stopwatch();
            object lockObj = new object();

            //******************************

            //Thread.Sleep(5 * 1000);
            GC.Collect();
            Console.WriteLine("\nSequential For (Leibniz Infinite Series): \n");
            stopwatch.Reset();
            stopwatch.Start();
            pi = 0.0;
            //// Toggle sign - step by 2
            //long incr;
            //incr = 2;
            //double sign;
            //sign = 1.0;
            //for (long i = 0; i < (maxSteps * incr); i += incr)
            //{
            //    pi += sign
            //        * (4.0 / (1.0 + i));
            //    sign *= -1.0;
            //}
            // Calculate sign on odd/even iteration
            for (long i = 0; i < maxSteps; i++)
            {
                pi += (1.0 - (2L * (i & 1L))) 
                    * (4.0 / (1.0 + 2L * i));
            }
            iter = maxSteps;
            stopwatch.Stop();
            Console.WriteLine($"Duration: {stopwatch.Elapsed} - Iterations: {iter,15:###,###,###,##0} - Pi = {pi,19:f16}");

            //******************************

            //Thread.Sleep(5 * 1000);
            GC.Collect();
            Console.WriteLine("\nParallel For (Leibniz Infinite Series): \n");
            stopwatch.Reset();
            stopwatch.Start();
            pi = 0.0;
            Parallel.For<double>(0L, maxSteps
                               , () => 0.0
                               , (i, loopState, localState) =>
                               {
                                   localState += (1.0 - (2L * (i & 1L))) 
                                               * (4.0 / (1.0 + 2L * i));
                                   return localState;
                               }
                               , (localState) =>
                               {
                                   lock (lockObj)
                                   {
                                       pi += localState;
                                   };
                               }
                                );
            iter = maxSteps;
            stopwatch.Stop();
            Console.WriteLine($"Duration: {stopwatch.Elapsed} - Iterations: {iter,15:###,###,###,##0} - Pi = {pi,19:f16}");

            //******************************

            //Thread.Sleep(5 * 1000);
            GC.Collect();
            Console.WriteLine("\nParallel ForEach - Toggle (Leibniz Infinite Series): \n");
            stopwatch.Reset();
            stopwatch.Start();
            pi = 0.0;
            Parallel.ForEach<Tuple<long, long>
                           , (double pi, double sign)>(Partitioner.Create(0L, maxSteps)
                                                     , () => (0.0, 0.0)
                                                     , (range, loopState, localState) =>
                                                     {
                                                         localState.sign = (1.0 - (2L * (range.Item1 & 1L)));
                                                         for (long i = range.Item1; i < range.Item2; i++)
                                                         {
                                                             localState.pi += localState.sign 
                                                                            * (4.0 / (1.0 + 2L * i));
                                                             localState.sign *= -1.0;
                                                         }
                                                         return localState;
                                                     }
                                                     , (localState) =>
                                                     {
                                                         lock (lockObj)
                                                         {
                                                             pi += localState.pi;
                                                         };
                                                     }
                                                      );
            iter = maxSteps;
            stopwatch.Stop();
            Console.WriteLine($"Duration: {stopwatch.Elapsed} - Iterations: {iter,15:###,###,###,##0} - Pi = {pi,19:f16}");

            //******************************

            //Thread.Sleep(5 * 1000);
            GC.Collect();
            Console.WriteLine("\nParallel ForEach - Odd/Even (Leibniz Infinite Series): \n");
            stopwatch.Reset();
            stopwatch.Start();
            pi = 0.0;
            Parallel.ForEach<Tuple<long, long>, double>(Partitioner.Create(0L, maxSteps)
                                                      , () => 0.0
                                                      , (range, loopState, localState) =>
                                                      {
                                                          for (long i = range.Item1; i < range.Item2; i++)
                                                          {
                                                              localState += (1.0 - (2L * (i & 1L))) 
                                                                          * (4.0 / (1.0 + 2L * i));
                                                          }
                                                          return localState;
                                                      }
                                                      , (localState) =>
                                                      {
                                                          lock (lockObj)
                                                          {
                                                              pi += localState;
                                                          };
                                                      }
                                                       );
            iter = maxSteps;
            stopwatch.Stop();
            Console.WriteLine($"Duration: {stopwatch.Elapsed} - Iterations: {iter,15:###,###,###,##0} - Pi = {pi,19:f16}");

            //******************************

            //Thread.Sleep(5 * 1000);
            GC.Collect();
            Console.WriteLine("\nSequential For (Nilakantha Infinite Series): \n");
            stopwatch.Reset();
            stopwatch.Start();
            pi = 3.0;
            //// Toggle sign - step by 2
            //incr = 2;
            //sign = 1.0;
            //for (long i = 0; i < (maxSteps * incr); i += incr)
            //{
            //    pi += sign
            //        * (4.0 / ((i + 2.0) * (i + 3.0) * (i + 4.0)));
            //    sign *= -1.0;
            //}
            // Calculate sign on odd/even iteration
            for (long i = 0; i < maxSteps; i ++)
            {
                pi += (1.0 - (2L * (i & 1L))) 
                    * (4.0 / ((2L * i + 2.0) * (2L * i + 3.0) * (2L * i + 4.0)));
            }
            iter = maxSteps;
            stopwatch.Stop();
            Console.WriteLine($"Duration: {stopwatch.Elapsed} - Iterations: {iter,15:###,###,###,##0} - Pi = {pi,19:f16}");

            //******************************

            //Thread.Sleep(5 * 1000);
            GC.Collect();
            Console.WriteLine("\nParallel For (Nilakantha Infinite Series): \n");
            stopwatch.Reset();
            stopwatch.Start();
            pi = 3.0;
            Parallel.For<double>(0L, maxSteps
                               , () => 0.0
                               , (i, loopState, localState) =>
                               {
                                   localState += (1.0 - (2L * (i & 1L))) 
                                               * (4.0 / ((2L * i + 2.0) * (2L * i + 3.0) * (2L * i + 4.0)));
                                   return localState;
                               }
                               , (localState) =>
                               {
                                   lock (lockObj)
                                   {
                                       pi += localState;
                                   };
                               }
                                );
            iter = maxSteps;
            stopwatch.Stop();
            Console.WriteLine($"Duration: {stopwatch.Elapsed} - Iterations: {iter,15:###,###,###,##0} - Pi = {pi,19:f16}");

            //******************************

            //Thread.Sleep(5 * 1000);
            GC.Collect();
            Console.WriteLine("\nParallel ForEach - Toggle (Nilakantha Infinite Series): \n");
            stopwatch.Reset();
            stopwatch.Start();
            pi = 3.0;
            Parallel.ForEach<Tuple<long, long>
                           , (double pi, double sign)>(Partitioner.Create(0L, maxSteps)
                               , () => (0.0, 0.0)
                               , (range, loopState, localState) =>
                               {
                                   localState.sign = (1.0 - (2L * (range.Item1 & 1L)));
                                   for (long i = range.Item1; i < range.Item2; i++)
                                   {
                                       localState.pi += localState.sign 
                                                      * (4.0 / ((2L * i + 2.0) * (2L * i + 3.0) * (2L * i + 4.0)));
                                       localState.sign *= -1.0;
                                   }
                                   return localState;
                               }
                               , (localState) =>
                               {
                                   lock (lockObj)
                                   {
                                       pi += localState.pi;
                                   };
                               }
                                );
            iter = maxSteps;
            stopwatch.Stop();
            Console.WriteLine($"Duration: {stopwatch.Elapsed} - Iterations: {iter,15:###,###,###,##0} - Pi = {pi,19:f16}");

            //******************************

            //Thread.Sleep(5 * 1000);
            GC.Collect();
            Console.WriteLine("\nParallel ForEach - Odd/Even (Nilakantha Infinite Series): \n");
            stopwatch.Reset();
            stopwatch.Start();
            pi = 3.0;
            Parallel.ForEach<Tuple<long, long>, double>(Partitioner.Create(0L, maxSteps)
                                                      , () => 0.0
                                                      , (range, loopState, localState) =>
                                                      {
                                                          for (long i = range.Item1; i < range.Item2; i++)
                                                          {
                                                              localState += (1.0 - (2L * (i & 1L))) 
                                                                          * (4.0 / ((2L * i + 2.0) * (2L * i + 3.0) * (2L * i + 4.0)));
                                                          }
                                                          return localState;
                                                      }
                                                      , (localState) =>
                                                      {
                                                          lock (lockObj)
                                                          {
                                                              pi += localState;
                                                          };
                                                      }
                                                       );
            iter = maxSteps;
            stopwatch.Stop();
            Console.WriteLine($"Duration: {stopwatch.Elapsed} - Iterations: {iter,15:###,###,###,##0} - Pi = {pi,19:f16}");

            //******************************

            Console.WriteLine("\n\nPress Enter to continue...");
            Console.ReadLine();

            //******************************

            //Thread.Sleep(5 * 1000);
            GC.Collect();
            Console.WriteLine("\nSequential For (Ramanujan 1, 1914 formula): \n");
            stopwatch.Reset();
            stopwatch.Start();
            result = 0.0;
            prior = -1.0;
            n = 0;
            factor1 = Math.Sqrt(8.0) / Math.Pow(99.0, 2.0);
            while (Math.Abs(prior - result) > 1.0e-19)
            {
                prior = result;

                result += ((double)Factorial((new BigInteger(4 * n))) / ((Math.Pow((Math.Pow(4.0, n) * (double)Factorial((new BigInteger(n)))), 4))))
                        * ((double)((new BigInteger(1103)) + ((new BigInteger(26390)) * (new BigInteger(n)))) / Math.Pow(99.0, 4 * n));

                n++;
            }
            iter = n;
            pi = (1.0 / (result *= factor1));
            stopwatch.Stop();
            Console.WriteLine($"Duration: {stopwatch.Elapsed} - Iterations: {iter,15:###,###,###,##0} - Pi = {pi,19:f16}");

            //******************************

            //Thread.Sleep(5 * 1000);
            GC.Collect();
            Console.WriteLine("\nSequential For (Ramanujan 2, 1914 formula): \n");
            stopwatch.Reset();
            stopwatch.Start();
            result = 0.0;
            prior = -1.0;
            n = 0;
            factor1 = 1.0 / 882.0;
            while (Math.Abs(prior - result) > 1.0e-19)
            {
                prior = result;

                result += ((Math.Pow(-1.0, n) * (double)Factorial((new BigInteger(4 * n)))) / ((Math.Pow((Math.Pow(4.0, n) * (double)Factorial((new BigInteger(n)))), 4))))
                        * ((double)((new BigInteger(1123)) + ((new BigInteger(21460)) * (new BigInteger(n)))) / Math.Pow(882.0, 2 * n));

                n++;
            }
            iter = n;
            pi = (4.0 / (result *= factor1));
            stopwatch.Stop();
            Console.WriteLine($"Duration: {stopwatch.Elapsed} - Iterations: {iter,15:###,###,###,##0} - Pi = {pi,19:f16}");

            //******************************

            //Thread.Sleep(5 * 1000);
            GC.Collect();
            Console.WriteLine("\nSequential For (Chudonovsky, 1987 formula): \n");
            stopwatch.Reset();
            stopwatch.Start();
            result = 0.0;
            prior = -1.0;
            n = 0;
            factor1 = 12.0;
            while (Math.Abs(prior - result) > 1.0e-19)
            {
                prior = result;

                result += ((Math.Pow(-1.0, n) * (double)Factorial((new BigInteger(6 * n)))) / ((double)Factorial(3 * n) * Math.Pow((double)Factorial(n), 3)))
                        * ((double)((new BigInteger(13591409)) + ((new BigInteger(545140134)) * (new BigInteger(n)))) / Math.Pow(Math.Pow(640320.0, 3), (n + 0.5)));

                n++;
            }
            iter = n;
            pi = (1.0 / (result *= factor1));
            stopwatch.Stop();
            Console.WriteLine($"Duration: {stopwatch.Elapsed} - Iterations: {iter,15:###,###,###,##0} - Pi = {pi,19:f16}");

            //******************************

            return;
        }

        /** <summary>Calculates the factorial of a number using recursion.
         *  </summary>
         *  <param name="number">Number to perform factorial calculation on.</param>
         */ 
        static BigInteger Factorial(BigInteger number)
        {
            if (number <= 1)
                return 1;
            else
                return number * Factorial(number - 1);
        }
    }
}
