using System;
using System.Threading.Tasks;

namespace TaskContinuation
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var task1 = Task.Factory.StartNew(() => "1");
            var task2 = Task.Factory.StartNew(() => "2");
            var task3 = Task.Factory.StartNew(() => "3");

            var anyTask = Task.Factory.ContinueWhenAny(new[] {task1, task2, task3},
                task => { Console.WriteLine($"Task {task.Result} finished first."); });

            var allTasks = Task.Factory.ContinueWhenAll(new[] {task1, task2, task3},
                tasks => { Console.WriteLine("All tasks finished."); });
            Task.WaitAll(anyTask, allTasks);
            Console.ReadLine();
        }
    }
}