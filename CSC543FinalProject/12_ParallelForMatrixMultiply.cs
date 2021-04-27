using JcipAnnotations;
using System;
using System.Diagnostics;
using System.Runtime;
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
    public class ParallelForMatrixMultiply
    {
        /**
         * <summary>Run method.  
         *          Invokes the set of examples.
         * </summary>
         */
        public static void Run()
        {
            Console.WriteLine("\n\n" + ("ParallelFor/Foreach - MatrixMultiply" + " " + new string('=', 115)).Substring(0, 115));
            Program.HaltIfDebug();

            // Generate two random matrices and a third to hold the product
            const int aRows = 1_000;
            const int aCols = 1_000;
            double[,] a = GenerateRandomMatrix(aRows, aCols);
            const int bRows = aCols;
            const int bCols = 1_000;
            double[,] b = GenerateRandomMatrix(bRows, bCols);
            const int cRows = aRows;
            const int cCols = bCols;
            double[,] c = new double[cRows, cCols];
            Stopwatch stopwatch = new Stopwatch();
            object lockObj = new object();

            //******************************

            //Thread.Sleep(5 * 1000);
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            GC.Collect();
            Console.WriteLine("\nSequential For: \n");
            stopwatch.Reset();
            stopwatch.Start();
            for (long i = 0; i < cRows; ++i)
            {
                for (long j = 0; j < cCols; ++j)
                {
                    c[i, j] = 0.0;
                    for (long k = 0; k < aCols; ++k)
                    {
                        c[i, j] += a[i, k] * b[k, j];
                    }
                }
            }
            stopwatch.Stop();
            Console.WriteLine($"Duration: {stopwatch.Elapsed}");

            //******************************

            //Thread.Sleep(5 * 1000);
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            GC.Collect();
            Console.WriteLine("\nParallel For: \n");
            stopwatch.Reset();
            stopwatch.Start();
            Parallel.For(0                                           // from inclusive
                       , cRows                                       // to exclusive
                       , (i) =>                                      // body delegate
                       {
                           Parallel.For(0                            // from inclusive
                                      , cCols                        // to exclusive
                                      , j =>                         // body delegate
                                      {
                                          c[i, j] = 0.0;
                                          for (long k = 0; k < aCols; ++k)
                                          {
                                              c[i, j] += a[i, k] * b[k, j];
                                          }
                                      }
                                       );
                       }
                        );
            stopwatch.Stop();
            Console.WriteLine($"Duration: {stopwatch.Elapsed}");

            //******************************

            return;
        }

        /**
         * <summary>GenerateRandomizeMatrix method.  
         *          Generates a matrix of random double values.
         * </summary>
         * <param name="rows">Numer of rows in the matrix.</param>
         * <param name="cols">Numer of columnss in the matrix.</param>
         * <returns>A matrix of randum double values.</returns>
         */
        public static double[,] GenerateRandomMatrix(int rows, int cols)
        {
            Random rnd = new Random();
            double[,] matrix = new double[rows, cols];
            for (long i = 0; i < rows; ++i)
            {
                for (long j = 0; j < cols; ++j)
                {
                    matrix[i, j] = rnd.NextDouble();
                }
            }

            return matrix;
        }
    }
}
