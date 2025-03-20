using KPO.Domain;

namespace KPO.Facades
{
    public class CategoryFacade : IFacade<Category>
    {
        private readonly List<Category> _categories = new();

        public Category Create(Category item)
        {
            if (_categories.Any(c => c.Id == item.Id))
                throw new Exception($"Категория с Id={item.Id} уже существует!");

            _categories.Add(item);
            return item;
        }

        public Category Update(Category item)
        {
            var existing = GetById(item.Id);
            if (existing == null) return null;

            _categories.Remove(existing);
            _categories.Add(item);
            return item;
        }

        public bool Delete(int id)
        {
            var existing = GetById(id);
            return existing != null && _categories.Remove(existing);
        }

        public Category? GetById(int id)
        {
            return _categories.FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<Category> GetAll()
        {
            return _categories;
        }
    }
}