using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace BlockingCollectionDemo
{
    internal class Program
    {
        private static readonly BlockingCollection<int> Data = new BlockingCollection<int>(new ConcurrentBag<int>());
        private static readonly Random Random = new Random();
        private static readonly CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();

        private static void Main()
        {
            Task.Factory.StartNew(ProduceAndConsume);
            Console.ReadKey();
            CancellationTokenSource.Cancel();
            Console.WriteLine("Cancelled");
            Console.ReadLine();
        }

        private static void ProduceAndConsume()
        {
            var t1 = Task.Factory.StartNew(Producer);
            var t2 = Task.Factory.StartNew(Consumer);
            try
            {
                Task.WaitAll(t1, t2);
            }
            catch (AggregateException ae)
            {
                ae.Handle(e => true);
            }
        }

        private static void Producer()
        {
            while (true)
            {
                CancellationTokenSource.Token.ThrowIfCancellationRequested();
                var item = Random.Next(1000);
                Data.Add(item);
                Console.WriteLine($"+{item}");
                Thread.Sleep(200);
            }
            // ReSharper disable once FunctionNeverReturns
        }

        private static void Consumer()
        {
            CancellationTokenSource.Token.ThrowIfCancellationRequested();
            foreach (var item in Data.GetConsumingEnumerable())
            {
                Console.WriteLine($"-{item}");
                Thread.Sleep(200);
            }
        }
    }
}