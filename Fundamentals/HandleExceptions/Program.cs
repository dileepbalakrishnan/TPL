using System;
using System.Threading.Tasks;

namespace HandleExceptions
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            TaskWithExceptions();
            Console.WriteLine("Completed");
            Console.ReadLine();
        }

        private static void TaskWithExceptions()
        {
            try
            {
                Task.Factory.StartNew(() => throw new InvalidOperationException());
                Task.Factory.StartNew(() => throw new ArgumentException());
            }
            catch (AggregateException ae)
            {
                ae.Handle(e => e is InvalidOperationException || e is ArgumentException);
            }
        }
    }
}