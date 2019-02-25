using System;
using System.Threading;
using System.Threading.Tasks;

namespace ParentAndChildTasks
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var parent = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Parent task starting...");
                var child = Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("Child task starting...");
                    Thread.Sleep(1000);
                    Console.WriteLine("Child task completed.");
                }, TaskCreationOptions.AttachedToParent);
            });
            parent.Wait();
            Console.WriteLine("Parent task completed.");
            Console.ReadLine();
        }
    }
}