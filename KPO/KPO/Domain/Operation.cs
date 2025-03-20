namespace KPO.Domain;

public class Operation
{
    public int Id { get; private set; }
    public string Type { get; private set; } // "income" | "expense"
    public BankAccount BankAccount { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime Date { get; private set; }
    public string Description { get; private set; }
    public Category Category { get; private set; }

    public Operation(int id, 
        string type, 
        BankAccount account, 
        decimal amount, 
        DateTime date, 
        Category category, 
        string description = "")
    {
        Id = id;
        Type = type;
        BankAccount = account;
        Amount = amount;
        Date = date;
        Category = category;
        Description = description;
    }
}