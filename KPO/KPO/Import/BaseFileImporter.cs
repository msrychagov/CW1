using KPO.Domain;

namespace KPO.Import
{
    public abstract class BaseFileImporter
    {
        // Шаблонный метод
        public void Import(string filePath)
        {
            var data = ReadFile(filePath);
            var items = ParseData(data);
            SaveItems(items);
        }

        protected virtual string ReadFile(string path)
        {
            // Для упрощения – читаем весь текст
            // В реальном проекте нужно проверить существование файла
            // или обрабатывать исключения
            return File.ReadAllText(path);
        }

        protected abstract IEnumerable<Operation> ParseData(string data);

        protected virtual void SaveItems(IEnumerable<Operation> items)
        {
            // По умолчанию – выводим в консоль
            // В реальном случае можно сохранять через фасады, чтобы изменить баланс счёта
            foreach (var op in items)
            {
                Console.WriteLine($"Импортирована операция Id={op.Id}, сумма={op.Amount}, тип={op.Type}");
            }
        }
    }
}