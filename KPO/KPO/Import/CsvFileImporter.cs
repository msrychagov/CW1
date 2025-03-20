using KPO.Domain;

namespace KPO.Import
{
    public class CsvFileImporter : BaseFileImporter
    {
        protected override IEnumerable<Operation> ParseData(string data)
        {
            // Упрощённый парсер CSV (заглушка)
            // В реальном проекте парсим каждую строку, разделяем по разделителям и т.д.
            return new List<Operation>
            {
                new Operation(
                    id: 88,
                    type: "expense",
                    account: null,
                    amount: 500,
                    date: DateTime.Now,
                    category: null,
                    description: "CSV stub"
                )
            };
        }
    }
}