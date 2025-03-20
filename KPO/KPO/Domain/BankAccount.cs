namespace KPO.Domain
{
    public class BankAccount
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public decimal Balance { get; private set; }

        public BankAccount(int id, string name, decimal balance = 0m)
        {
            Id = id;
            Name = name;
            Balance = balance;
        }

        // Логика изменения баланса
        public void IncreaseBalance(decimal amount)
        {
            Balance += amount;
        }

        public void DecreaseBalance(decimal amount)
        {
            Balance -= amount;
        }

        // Дополнительные методы, проверки, валидация...
    }
}