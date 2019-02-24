using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReaderWriterLockDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            int number = 0;

            ReaderWriterLockSlim rwls = new ReaderWriterLockSlim();

            var t1 = Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 200; i++)
                {
                    rwls.EnterWriteLock();
                    number++;
                    Console.WriteLine($"Write : {number}.");
                    Thread.Sleep(50);
                    rwls.ExitWriteLock();
                }
            });

            var t2 = Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 200; i++)
                {
                    rwls.EnterReadLock();
                    Console.WriteLine($"Read : {number}.");
                    Thread.Sleep(50);
                    rwls.ExitReadLock();
                }
            });
            Task.WaitAll(t1, t2);
            Console.WriteLine("Completed.");
            Console.ReadLine();
        }
    }
}
