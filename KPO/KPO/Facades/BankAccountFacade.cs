using KPO.Domain;

namespace KPO.Facades
{
    public class BankAccountFacade : IFacade<BankAccount>
    {
        private readonly List<BankAccount> _bankAccounts = new();

        public BankAccount Create(BankAccount item)
        {
            if (_bankAccounts.Any(a => a.Id == item.Id))
                throw new Exception($"Счёт с Id={item.Id} уже существует!");

            _bankAccounts.Add(item);
            return item;
        }

        public BankAccount Update(BankAccount item)
        {
            var existing = GetById(item.Id);
            if (existing == null) return null;

            // Удалим старый объект, добавим новый (упрощённо)
            _bankAccounts.Remove(existing);
            _bankAccounts.Add(item);
            return item;
        }

        public bool Delete(int id)
        {
            var existing = GetById(id);
            return existing != null && _bankAccounts.Remove(existing);
        }

        public BankAccount? GetById(int id)
        {
            return _bankAccounts.FirstOrDefault(a => a.Id == id);
        }

        public IEnumerable<BankAccount> GetAll()
        {
            return _bankAccounts;
        }
    }
}