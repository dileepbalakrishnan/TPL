using System;
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
    }
}