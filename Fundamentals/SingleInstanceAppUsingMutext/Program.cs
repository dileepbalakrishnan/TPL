using System;
using System.Threading;

namespace SingleInstanceAppUsingMutext
{
    internal class Program
    {
        private const string AppName = "SingleInstanceAppUsingMutext.App";

        private static void Main(string[] args)
        {
            if (Mutex.TryOpenExisting(AppName, out var mtx))
            {
                Console.WriteLine("Another instance of this App is already running.");
                Console.WriteLine("Press any key to exit.");
                Console.ReadLine();
                return;
            }
            using (mtx = new Mutex(false, AppName))
            {
                Console.WriteLine("App is running...Press any key to exit.");
                Console.ReadLine();
            }
        }
    }
}