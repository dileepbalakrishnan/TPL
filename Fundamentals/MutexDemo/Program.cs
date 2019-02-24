using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MutexDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var bankAccount = new BankAccount();
            var workers = new List<Task>();
            for (var i = 0; i < 1000; i++)
            {
                workers.Add(Task.Factory.StartNew(() => { bankAccount.Deposit(1000); }));
                workers.Add(Task.Factory.StartNew(() => { bankAccount.Withdraw(1000); }));
            }
            Task.WaitAll(workers.ToArray());
            Console.WriteLine($"Current Balance {bankAccount.Balance}");
            Console.ReadLine();
        }
    }
}
