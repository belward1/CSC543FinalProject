using JcipAnnotations;
using System;
using System.Diagnostics;

/**
 * <summary>Final Project:
 *          CSC 543 - Multi-processing and Concurrent programming
 *          Spring 2021 Semester
 *          Proferssor: Dr. Dale parson
 *          Kutztown University, Kutztoen, PA
 *          
 *          This program highlights to Task Parallel Library which is part
 *          of Microsoft's .NET C# language.
 * </summary>
 * <see cref="https://github.com/belward1/CSC543FinalProject">Github repository</see>
 * <remarks>Written By: Bob Elward - April 2021</remarks>
 */
namespace CSC543FinalProject
{
    /**
     * <summary>Driver class.  
     *          Invokes the examples.
     * </summary>
     */
    [Immutable()]
    public class Program
    {
        /**
         * <summary>Main method.  
         *          Entry point for the program. Invokes the examples.
         * </summary>
         * <param name="args">Commandline arguments passed to the program.</param>
         */
        public static void Main(string[] args)
        {
            Console.WriteLine("CSC543 Final Project - Spring 2021 - Bob Elward");
            Console.WriteLine($"OS: {Environment.OSVersion.VersionString}");
            Console.WriteLine($"Computer Name: {Environment.MachineName}");

            //ParallelInvoke.Run();
            //ParallelForSimple.Run();
            //ParallelForEachWithThreadLocal.Run();
            //////ParallelForEach_SimulatedWork.Run();
            //ParallelForDotProduct.Run();
            //ParallelForMatrixMultiply.Run();
            ParallelForPiCalculation.Run();
            //ParallelMapReduce.Run();
            ////ParallelMapReduce_MemoryT.Run();

            return;
        }

        /**
         * <summary>Time methods.  Wraps a timmer around the execution of a method.</summary>
         * <param name="label">Descriptive lable to print.</param>
         * <param name="method">Method to execute.</param>
         */
        static void Time(string label, Action method)
        {
            var sw = Stopwatch.StartNew();
            method();
            Console.WriteLine($"{label,-40:s1} - Duration: {sw.Elapsed}");
        }

        //[Conditional("DEBUG")]
        public static void HaltIfDebug()
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                Console.WriteLine();

                ConsoleColor foreColor = Console.ForegroundColor;
                ConsoleColor backColor = Console.BackgroundColor;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.BackgroundColor = ConsoleColor.Yellow;

                Console.Write("Press Enter to continue... ");

                Console.ForegroundColor = foreColor;
                Console.BackgroundColor = backColor;

                Console.ReadLine();
            }

            return;
        }
    }
}
