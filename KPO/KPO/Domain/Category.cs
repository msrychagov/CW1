namespace KPO.Domain
{
    public class Category
    {
        public int Id { get; private set; }
        public string Type { get; private set; } // "income" | "expense"
        public string Name { get; private set; }

        public Category(int id, string type, string name)
        {
            Id = id;
            Type = type;
            Name = name;
        }
    }
}