using System;
using System.Linq;
using System.Threading;

namespace TaskCompletionSourceDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var tp = new TextProcessor("Data.txt");
            try
            {
                foreach (var kvp in tp.WordCountTask.Result.OrderByDescending(kv => kv.Value))
                {
                    Console.WriteLine($"{kvp.Key} - {kvp.Value}");
                }
            }
            catch (AggregateException ex)
            {
                foreach (var exception in ex.InnerExceptions)
                {
                    Console.WriteLine($"Exception : {ex.GetType()} Message : {ex.Message}");
                }
            }
            Console.ReadLine();
        }
    }
}