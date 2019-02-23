using System;
using System.Threading.Tasks;

namespace HelloTasks
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Task.Factory.StartNew(DoSomething);
            Console.WriteLine("Done");
            Console.ReadLine();
        }

        private static void DoSomething()
        {
            var times = 100;
            while (times-- > 0)
                Console.WriteLine("Some work...");
        }
    }
}