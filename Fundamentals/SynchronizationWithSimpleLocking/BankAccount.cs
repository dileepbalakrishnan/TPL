namespace SynchronizationWithSimpleLocking
{
    public class BankAccount
    {
        private readonly object _synchro = new object();
        public int Balance { get; set; }

        public void Deposit(int amount)
        {
            lock (_synchro)
            {
                Balance += amount;
            }
        }

        public void Withdraw(int amount)
        {
            lock (_synchro)
            {
                Balance -= amount;
            }
        }
    }
}