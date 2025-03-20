using KPO.Domain;

namespace KPO.Import
{
    public class JsonFileImporter : BaseFileImporter
    {
        protected override IEnumerable<Operation> ParseData(string data)
        {
            // Здесь должен быть реальный парсинг JSON
            // Для упрощения вернем заглушку
            // В реальном проекте используйте:
            //   using Newtonsoft.Json;
            //   var ops = JsonConvert.DeserializeObject<List<Operation>>(data);
            //   return ops;
            return new List<Operation>
            {
                new Operation(
                    id: 99,
                    type: "income",
                    account: null,  // В реальном случае нужно также привязать к счёту
                    amount: 1234,
                    date: DateTime.Now,
                    category: null,
                    description: "JSON stub"
                )
            };
        }
    }
}