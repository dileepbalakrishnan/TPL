using System.Threading;

namespace SpinLockDemo
{
    public class BankAccount
    {
        private SpinLock sl = new SpinLock();

        public int Balance { get; set; }

        public void Deposit(int amount)
        {
            var lockTaken = false;
            try
            {
                sl.Enter(ref lockTaken);
                Balance += amount;
            }
            finally
            {
                if (lockTaken)
                    sl.Exit();
            }
        }

        public void Withdraw(int amount)
        {
            var lockTaken = false;
            try
            {
                sl.Enter(ref lockTaken);
                Balance -= amount;
            }
            finally
            {
                if (lockTaken)
                    sl.Exit();
            }
        }
    }
}