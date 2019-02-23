using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskCancellation
{
    class Program
    {
        static void Main(string[] args)
        {
            var cts = new CancellationTokenSource();
            Task.Factory.StartNew(() => { TheTask(cts.Token); },cts.Token);
            Console.ReadKey();
            cts.Cancel();
            Console.WriteLine("Cancelled");
            Console.ReadLine();
        }

        static void TheTask(CancellationToken token)
        {
            while (true)
            {
                token.ThrowIfCancellationRequested();// Throws an exception and ends the task if it is cancelled.
                Console.Write(".");
            }
        }
    }
}
