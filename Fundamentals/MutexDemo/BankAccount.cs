using System.Threading;

namespace MutexDemo
{
    public class BankAccount
    {
        private readonly Mutex _mtx = new Mutex();

        public int Balance { get; set; }

        public void Deposit(int amount)
        {
            var lockTaken = false;
            try
            {
                lockTaken = _mtx.WaitOne();
                Balance += amount;
            }
            finally
            {
                if (lockTaken)
                    _mtx.ReleaseMutex();
            }
        }

        public void Withdraw(int amount)
        {
            var lockTaken = false;
            try
            {
                lockTaken = _mtx.WaitOne();
                Balance -= amount;
            }
            finally
            {
                if (lockTaken)
                    _mtx.ReleaseMutex();
            }
        }
    }
}