using JcipAnnotations;
using System;
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
    public class ParallelForSimple
    {
        /**
         * <summary>Run method.  
         *          Invokes the set of examples.
         * </summary>
         */
        public static void Run()
        {
            Console.WriteLine("\n\n" + ("ParallellFor - Simple" + " " + new string('=', 115)).Substring(0, 115));
            Program.HaltIfDebug();

            int loopMax = 10;

            //******************************

            Console.WriteLine("\nSequential For: \n");
            for (int i = 0; i < loopMax; i++)
            {
                Console.WriteLine($"i = {i} on thread: {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(1000);
            }

            try
            {
                //******************************

                Console.WriteLine("\nParallel For: \n");
                Parallel.For(0                           // from inclusive
                           , loopMax                     // to exclusive
                           , (i) =>                      // body delegate
                           {
                               Console.WriteLine($"i = {i} on thread: {Thread.CurrentThread.ManagedThreadId}");
                               Thread.Sleep(1000);
                           }
                            );

                //******************************

                Console.WriteLine("\nParallel For w/ DegreeOfParallelism: \n");
                ParallelOptions parallelOptions = new ParallelOptions() { MaxDegreeOfParallelism = 3 };
                Parallel.For(0                           // from inclusive
                           , loopMax                     // to exclusive
                           , parallelOptions             // parallel options
                           , (i) =>                      // body delegate
                           {
                               Console.WriteLine($"i = {i} on thread: {Thread.CurrentThread.ManagedThreadId}");
                               Thread.Sleep(1000);
                           }
                            );


                //******************************

                Console.WriteLine("\nParallel For - Anonymous Method: \n");
                Parallel.For(0                           // from inclusive
                           , loopMax                     // to exclusive
                           , delegate(int i)             // body delegate
                           {
                               Console.WriteLine($"i = {i} on thread: {Thread.CurrentThread.ManagedThreadId}");
                               Thread.Sleep(1000);
                           }
                            );


                //******************************

                Console.WriteLine("\nParallel For - Delegate Method: \n");
                Parallel.For(0                           // from inclusive
                           , loopMax                     // to exclusive
                           , DoWork                      // body delegate
                            );
            }
            catch (AggregateException aggEx)
            {
                Console.WriteLine($"ERROR-AggregateException: {aggEx.Message}");

                //Program.HaltIfDebug();
            }

            //******************************

            return;
        }

        /**
         * <summary>DoWork method.
         *          Body delegate for a Parallel.For operation
         * </summary>
         * <param name="i">The increment value.</param>
         */
        private static void DoWork(int i)
        {
            Console.WriteLine($"i = {i} on thread: {Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(1000);

            if (i > 7)
            {
                string excaptionMessage = String.Format($"Exception in DoWork - i = {i}");
                Console.WriteLine(excaptionMessage);
                throw new Exception(excaptionMessage);
            }

            return;
        }
    }
}
