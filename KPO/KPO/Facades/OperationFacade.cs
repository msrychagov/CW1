using KPO.Domain;

namespace KPO.Facades
{
    public class OperationFacade : IFacade<Operation>
    {
        private readonly List<Operation> _operations = new();

        public Operation Create(Operation item)
        {
            if (_operations.Any(o => o.Id == item.Id))
                throw new Exception($"Операция с Id={item.Id} уже существует!");

            // Автоматически изменим баланс на счёте
            if (item.Type == "income")
                item.BankAccount.IncreaseBalance(item.Amount);
            else if (item.Type == "expense")
                item.BankAccount.DecreaseBalance(item.Amount);

            _operations.Add(item);
            return item;
        }

        public Operation Update(Operation item)
        {
            // При необходимости можно реализовать логику обновления
            throw new NotImplementedException("Обновление операции не реализовано.");
        }

        public bool Delete(int id)
        {
            var existing = GetById(id);
            if (existing == null) return false;

            // Откатим изменения баланса при удалении
            if (existing.Type == "income")
                existing.BankAccount.DecreaseBalance(existing.Amount);
            else if (existing.Type == "expense")
                existing.BankAccount.IncreaseBalance(existing.Amount);

            return _operations.Remove(existing);
        }

        public Operation? GetById(int id)
        {
            return _operations.FirstOrDefault(o => o.Id == id);
        }

        public IEnumerable<Operation> GetAll()
        {
            return _operations;
        }
    }
}