using System.Threading;

namespace LockFreeSynchronization
{
    public class BankAccount
    {
        private int _balance;

        public int Balance
        {
            get => _balance;
            set => _balance = value;
        }

        public void Deposit(int amount)
        {
            Interlocked.Add(ref _balance, amount);
        }

        public void Withdraw(int amount)
        {
            Interlocked.Add(ref _balance, -amount);
        }
    }
}