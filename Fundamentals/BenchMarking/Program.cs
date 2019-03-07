using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace BenchMarking
{
    public class Program
    {
        private const int Count = 10000;

        private static void Main(string[] args)
        {
            var benchmarkResults = BenchmarkRunner.Run<Program>();
            Console.WriteLine(benchmarkResults);
            Console.ReadLine();
            //ParallelMethod();
        }

        [Benchmark]
        public void NonParallelMethod()
        {
            var result = new int[Count];
            var input = Enumerable.Range(0, Count).ToList();
            for (var i = 0; i < Count; i++)
                result[i] = (int) Math.Pow(input[i], 2);
        }

        [Benchmark]
        public void ParallelMethod()
        {
            var result = new int[Count];
            //var input = Enumerable.Range(0, Count).ToList();
            var part = Partitioner.Create(0, Count, 4000);
            Parallel.ForEach(part, range =>
            {
                for (var i = range.Item1; i < range.Item2; i++)
                    result[i] = (int) Math.Pow(i, 2);
            });
        }
    }
}