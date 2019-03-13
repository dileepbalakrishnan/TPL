using System;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace ParallelMatrixMultiplication
{
    public class Program
    {
        private const int MatrixSize = 1000;
        private static readonly int[,] M1 = new int[MatrixSize, MatrixSize];
        private static readonly int[,] M2 = new int[MatrixSize, MatrixSize];
        private static readonly int[,] Result = new int[MatrixSize, MatrixSize];

        private static void Main(string[] args)
        {
            for (var i = 0; i < MatrixSize; i++)
                for (var j = 0; j < MatrixSize; j++)
                {
                    M1[i, j] = i + 1;
                    M2[i, j] = i + 1;
                }
            var benchmarkResults = BenchmarkRunner.Run<Program>();
            Console.WriteLine(benchmarkResults);
            Console.ReadLine();
        }

        [Benchmark(Description = "Sequential Run")]
        public void MultiplyMatrices()
        {
            for (var i = 0; i < MatrixSize; i++)
                for (var j = 0; j < MatrixSize; j++)
                {
                    Result[i, j] = 0;
                    for (var k = 0; k < MatrixSize; k++)
                        Result[i, j] += M1[i, k] * M2[k, j];
                }
        }

        [Benchmark(Description = "Parallel Run")]
        public void ParallelMultiplyMatrices()
        {
            Parallel.For(0, MatrixSize, i =>
            {
                for (var j = 0; j < MatrixSize; j++)
                {
                    Result[i, j] = 0;
                    for (var k = 0; k < MatrixSize; k++)
                        Result[i, j] += M1[i, k] * M2[k, j];
                }
            });
        }
        /// <summary>
        /// This sample is adopted from Microsoft Reserach and slightly modified to fit this demo
        /// </summary>
        [Benchmark(Description = "Parallel Run using explicit threads")]
        public void ThreadMatrixMul()
        {
            var matrixSize = MatrixSize;
            var processorCount = 2 * Environment.ProcessorCount;
            var partitionSize = matrixSize / processorCount;
            var waitHandle = new AutoResetEvent(false);
            var counter = processorCount;
            for (var c = 0; c < processorCount; c++)
                ThreadPool.QueueUserWorkItem(
                    delegate(object o)
                    {
                        var lc = (int) o;
                        for (var i = lc * partitionSize;
                            i < (lc + 1 == processorCount ? matrixSize : (lc + 1) * partitionSize);
                            i++)
                        for (var j = 0; j < MatrixSize; j++)
                        {
                            Result[i, j] = 0;
                            for (var k = 0; k < MatrixSize; k++)
                                Result[i, j] += M1[i, k] * M2[k, j];
                        }
                        if (Interlocked.Decrement(ref counter) == 0)
                            waitHandle.Set();
                    }, c);
            waitHandle.WaitOne();
        }

        [Benchmark(Description = "Parallel Run With Multiple Index")]
        public void ParallelMultiplyMatricesUsingTwoIndices()
        {
            For2(MatrixSize, MatrixSize, delegate (int i, int j) {
                Result[i, j] = 0;
                for (var k = 0; k < MatrixSize; k++)
                {
                    Result[i, j] += M1[i, k] * M2[k, j];
                }
            });
        }

        public void For2(int size1, int size2, Action<int, int> body)
        {
            Parallel.For(0, size1 * size2, delegate (int i) {
                body(i / size2, i % size2);
            });
        }
    }
}