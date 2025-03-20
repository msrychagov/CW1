using KPO.Domain;
using KPO.Facades;
using KPO.Commands;
using KPO.Import;

namespace FinancialAccountingApp
{
    public class Program
    {
        // Фасады будем хранить в статических полях, чтобы они "жили" всё время
        private static BankAccountFacade _accountFacade = new();
        private static CategoryFacade _categoryFacade = new();
        private static OperationFacade _operationFacade = new();

        public static void Main(string[] args)
        {
            // Для примера: создадим один тестовый аккаунт и пару категорий
            SeedTestData();

            // Основной цикл программы
            while (true)
            {
                Console.Clear();
                Console.WriteLine("======== ГЛАВНОЕ МЕНЮ ========");
                Console.WriteLine("1) Управление счетами");
                Console.WriteLine("2) Управление категориями");
                Console.WriteLine("3) Управление операциями");
                Console.WriteLine("4) Демонстрация импорта (Template Method)");
                Console.WriteLine("0) Выход");
                Console.Write("Выберите пункт меню: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ManageBankAccounts();
                        break;
                    case "2":
                        ManageCategories();
                        break;
                    case "3":
                        ManageOperations();
                        break;
                    case "4":
                        DemoImport();
                        break;
                    case "0":
                        Console.WriteLine("Выход из программы...");
                        return;
                    default:
                        Console.WriteLine("Неверный пункт меню!");
                        Pause();
                        break;
                }
            }
        }

        /// <summary>
        /// Создаём тестовые данные, чтобы было на чем экспериментировать.
        /// </summary>
        private static void SeedTestData()
        {
            // Создадим тестовый счёт
            _accountFacade.Create(new BankAccount(id: 1, name: "Основной счёт", balance: 1000m));

            // Создадим категории
            _categoryFacade.Create(new Category(id: 1, type: "expense", name: "Кафе"));
            _categoryFacade.Create(new Category(id: 2, type: "income", name: "Зарплата"));
        }

        /// <summary>
        /// Меню для работы со счетами (CRUD).
        /// </summary>
        private static void ManageBankAccounts()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("==== Управление счетами ====");
                Console.WriteLine("1) Просмотреть все");
                Console.WriteLine("2) Создать");
                Console.WriteLine("3) Редактировать");
                Console.WriteLine("4) Удалить");
                Console.WriteLine("0) Назад");
                Console.Write("Ваш выбор: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowAllAccounts();
                        break;
                    case "2":
                        CreateAccount();
                        break;
                    case "3":
                        UpdateAccount();
                        break;
                    case "4":
                        DeleteAccount();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный пункт меню!");
                        Pause();
                        break;
                }
            }
        }

        /// <summary>
        /// Показать все счета.
        /// </summary>
        private static void ShowAllAccounts()
        {
            Console.Clear();
            var all = _accountFacade.GetAll();
            Console.WriteLine("СЧЕТА:");
            foreach (var acc in all)
            {
                Console.WriteLine($"  Id={acc.Id}, Name={acc.Name}, Balance={acc.Balance}");
            }
            Pause();
        }

        /// <summary>
        /// Создать новый счет.
        /// </summary>
        private static void CreateAccount()
        {
            Console.Clear();
            Console.WriteLine("=== Создание счета ===");
            Console.Write("Введите Id: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Некорректный Id!");
                Pause();
                return;
            }
            Console.Write("Введите имя счета: ");
            var name = Console.ReadLine() ?? "";

            Console.Write("Введите начальный баланс (0 по умолчанию): ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal balance))
                balance = 0;

            try
            {
                var account = new BankAccount(id, name, balance);
                _accountFacade.Create(account);
                Console.WriteLine("Счет успешно создан!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            Pause();
        }

        /// <summary>
        /// Обновление существующего счета (в данном случае переименуем счет).
        /// </summary>
        private static void UpdateAccount()
        {
            Console.Clear();
            Console.WriteLine("=== Редактирование счета ===");
            Console.Write("Введите Id счета: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Некорректный Id!");
                Pause();
                return;
            }

            var existing = _accountFacade.GetById(id);
            if (existing == null)
            {
                Console.WriteLine("Счет не найден!");
                Pause();
                return;
            }

            Console.Write($"Текущее имя: {existing.Name}. Введите новое имя: ");
            var newName = Console.ReadLine() ?? existing.Name;

            Console.Write($"Текущий баланс: {existing.Balance}. Новый баланс (Enter - оставить): ");
            var balanceStr = Console.ReadLine();
            decimal newBalance = existing.Balance;
            if (!string.IsNullOrWhiteSpace(balanceStr))
            {
                if (decimal.TryParse(balanceStr, out var b))
                    newBalance = b;
            }

            // Собираем новый объект (Id тот же)
            var updated = new BankAccount(id, newName, newBalance);
            _accountFacade.Update(updated);

            Console.WriteLine("Счет успешно обновлён!");
            Pause();
        }

        /// <summary>
        /// Удалить счет.
        /// </summary>
        private static void DeleteAccount()
        {
            Console.Clear();
            Console.WriteLine("=== Удаление счета ===");
            Console.Write("Введите Id счета для удаления: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Некорректный Id!");
                Pause();
                return;
            }

            if (_accountFacade.Delete(id))
                Console.WriteLine("Счет удалён!");
            else
                Console.WriteLine("Счет не найден!");

            Pause();
        }

        /// <summary>
        /// Меню для работы с категориями (CRUD).
        /// </summary>
        private static void ManageCategories()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("==== Управление категориями ====");
                Console.WriteLine("1) Просмотреть все");
                Console.WriteLine("2) Создать");
                Console.WriteLine("3) Редактировать");
                Console.WriteLine("4) Удалить");
                Console.WriteLine("0) Назад");
                Console.Write("Ваш выбор: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowAllCategories();
                        break;
                    case "2":
                        CreateCategory();
                        break;
                    case "3":
                        UpdateCategory();
                        break;
                    case "4":
                        DeleteCategory();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный пункт меню!");
                        Pause();
                        break;
                }
            }
        }

        /// <summary>
        /// Показать все категории.
        /// </summary>
        private static void ShowAllCategories()
        {
            Console.Clear();
            var all = _categoryFacade.GetAll();
            Console.WriteLine("КАТЕГОРИИ:");
            foreach (var cat in all)
            {
                Console.WriteLine($"  Id={cat.Id}, Type={cat.Type}, Name={cat.Name}");
            }
            Pause();
        }

        /// <summary>
        /// Создать новую категорию.
        /// </summary>
        private static void CreateCategory()
        {
            Console.Clear();
            Console.WriteLine("=== Создание категории ===");
            Console.Write("Введите Id: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Некорректный Id!");
                Pause();
                return;
            }

            Console.Write("Введите тип (income/expense): ");
            var type = Console.ReadLine() ?? "";

            Console.Write("Введите название категории: ");
            var name = Console.ReadLine() ?? "";

            try
            {
                var category = new Category(id, type, name);
                _categoryFacade.Create(category);
                Console.WriteLine("Категория успешно создана!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            Pause();
        }

        /// <summary>
        /// Редактировать категорию.
        /// </summary>
        private static void UpdateCategory()
        {
            Console.Clear();
            Console.WriteLine("=== Редактирование категории ===");
            Console.Write("Введите Id категории: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Некорректный Id!");
                Pause();
                return;
            }

            var existing = _categoryFacade.GetById(id);
            if (existing == null)
            {
                Console.WriteLine("Категория не найдена!");
                Pause();
                return;
            }

            Console.Write($"Текущий тип: {existing.Type}. Новый тип (Enter - оставить): ");
            var newType = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(newType))
                newType = existing.Type;

            Console.Write($"Текущее название: {existing.Name}. Новое название (Enter - оставить): ");
            var newName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(newName))
                newName = existing.Name;

            var updated = new Category(id, newType, newName);
            _categoryFacade.Update(updated);

            Console.WriteLine("Категория успешно обновлена!");
            Pause();
        }

        /// <summary>
        /// Удалить категорию.
        /// </summary>
        private static void DeleteCategory()
        {
            Console.Clear();
            Console.WriteLine("=== Удаление категории ===");
            Console.Write("Введите Id категории: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Некорректный Id!");
                Pause();
                return;
            }

            if (_categoryFacade.Delete(id))
                Console.WriteLine("Категория удалена!");
            else
                Console.WriteLine("Категория не найдена!");

            Pause();
        }

        /// <summary>
        /// Меню для работы с операциями (CRUD).
        /// </summary>
        private static void ManageOperations()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("==== Управление операциями ====");
                Console.WriteLine("1) Просмотреть все");
                Console.WriteLine("2) Создать (Command/Decorator)");
                Console.WriteLine("3) Редактировать");
                Console.WriteLine("4) Удалить (Command/Decorator)");
                Console.WriteLine("0) Назад");
                Console.Write("Ваш выбор: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowAllOperations();
                        break;
                    case "2":
                        CreateOperation();
                        break;
                    case "3":
                        UpdateOperation();
                        break;
                    case "4":
                        DeleteOperation();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный пункт меню!");
                        Pause();
                        break;
                }
            }
        }

        /// <summary>
        /// Показать все операции.
        /// </summary>
        private static void ShowAllOperations()
        {
            Console.Clear();
            var all = _operationFacade.GetAll();
            Console.WriteLine("ОПЕРАЦИИ:");
            foreach (var op in all)
            {
                Console.WriteLine($"  Id={op.Id}, Type={op.Type}, Amount={op.Amount}, " +
                                  $"BankAccountId={op.BankAccount?.Id}, CategoryId={op.Category?.Id}, " +
                                  $"Date={op.Date}, Desc={op.Description}");
            }
            Pause();
        }

        /// <summary>
        /// Создание новой операции через паттерн Command (+ Decorator).
        /// </summary>
        private static void CreateOperation()
        {
            Console.Clear();
            Console.WriteLine("=== Создание операции ===");
            Console.Write("Введите Id операции: ");
            if (!int.TryParse(Console.ReadLine(), out int opId))
            {
                Console.WriteLine("Некорректный Id операции!");
                Pause();
                return;
            }

            Console.Write("Тип (income/expense): ");
            var type = Console.ReadLine() ?? "expense";

            Console.Write("Введите Id счёта: ");
            if (!int.TryParse(Console.ReadLine(), out int accId))
            {
                Console.WriteLine("Некорректный Id счета!");
                Pause();
                return;
            }
            var account = _accountFacade.GetById(accId);
            if (account == null)
            {
                Console.WriteLine("Счет не найден!");
                Pause();
                return;
            }

            Console.Write("Введите сумму: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                Console.WriteLine("Некорректная сумма!");
                Pause();
                return;
            }

            Console.Write("Введите Id категории: ");
            if (!int.TryParse(Console.ReadLine(), out int catId))
            {
                Console.WriteLine("Некорректный Id категории!");
                Pause();
                return;
            }
            var category = _categoryFacade.GetById(catId);
            if (category == null)
            {
                Console.WriteLine("Категория не найдена!");
                Pause();
                return;
            }

            Console.Write("Описание (необязательно): ");
            var desc = Console.ReadLine() ?? "";

            var operation = new Operation(
                id: opId,
                type: type,
                account: account,
                amount: amount,
                date: DateTime.Now,
                category: category,
                description: desc
            );

            // Используем паттерн Команда
            ICommand cmd = new CreateOperationCommand(operation, _operationFacade);
            // Оборачиваем в декоратор, чтобы замерить время
            ICommand decoratedCmd = new CommandTimeMeasurementDecorator(cmd);

            try
            {
                decoratedCmd.Execute();
                Console.WriteLine("Операция успешно создана!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            Pause();
        }

        /// <summary>
        /// Редактировать операцию (баланс будет скорректирован при изменении типа/суммы).
        /// </summary>
        private static void UpdateOperation()
        {
            Console.Clear();
            Console.WriteLine("=== Редактирование операции ===");
            Console.Write("Введите Id операции: ");
            if (!int.TryParse(Console.ReadLine(), out int opId))
            {
                Console.WriteLine("Некорректный Id!");
                Pause();
                return;
            }

            var existing = _operationFacade.GetById(opId);
            if (existing == null)
            {
                Console.WriteLine("Операция не найдена!");
                Pause();
                return;
            }

            Console.Write($"Текущий тип ({existing.Type}). Новый (Enter - оставить): ");
            var typeStr = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(typeStr))
                typeStr = existing.Type;

            Console.Write($"Текущая сумма ({existing.Amount}). Новая (Enter - оставить): ");
            var amountStr = Console.ReadLine();
            decimal newAmount = existing.Amount;
            if (!string.IsNullOrWhiteSpace(amountStr) && decimal.TryParse(amountStr, out var tmpAmount))
            {
                newAmount = tmpAmount;
            }

            Console.Write($"Текущий счёт (ID={existing.BankAccount.Id}). Новый Id (Enter - оставить): ");
            var accStr = Console.ReadLine();
            BankAccount newAcc = existing.BankAccount;
            if (!string.IsNullOrWhiteSpace(accStr) && int.TryParse(accStr, out int newAccId))
            {
                var foundAcc = _accountFacade.GetById(newAccId);
                if (foundAcc != null)
                    newAcc = foundAcc;
                else
                    Console.WriteLine("Счёт с таким Id не найден, оставим старый.");
            }

            Console.Write($"Текущая категория (ID={existing.Category?.Id}). Новый Id (Enter - оставить): ");
            var catStr = Console.ReadLine();
            Category newCat = existing.Category;
            if (!string.IsNullOrWhiteSpace(catStr) && int.TryParse(catStr, out int newCatId))
            {
                var foundCat = _categoryFacade.GetById(newCatId);
                if (foundCat != null)
                    newCat = foundCat;
                else
                    Console.WriteLine("Категория с таким Id не найдена, оставим старую.");
            }

            Console.Write($"Текущее описание: {existing.Description}. Новое (Enter - оставить): ");
            var descStr = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(descStr))
                descStr = existing.Description;

            var updatedOp = new Operation(
                id: opId,
                type: typeStr,
                account: newAcc,
                amount: newAmount,
                date: existing.Date, 
                category: newCat,
                description: descStr
            );

            try
            {
                _operationFacade.Update(updatedOp);
                Console.WriteLine("Операция успешно обновлена!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            Pause();
        }

        /// <summary>
        /// Удалить операцию (через паттерн Команда + Декоратор).
        /// </summary>
        private static void DeleteOperation()
        {
            Console.Clear();
            Console.WriteLine("=== Удаление операции ===");
            Console.Write("Введите Id операции: ");
            if (!int.TryParse(Console.ReadLine(), out int opId))
            {
                Console.WriteLine("Некорректный Id!");
                Pause();
                return;
            }

            // Паттерн Команда
            ICommand cmd = new DeleteOperationCommand(opId, _operationFacade);
            // Декоратор для замера времени
            ICommand decoratedCmd = new CommandTimeMeasurementDecorator(cmd);

            try
            {
                decoratedCmd.Execute();
                Console.WriteLine("Операция удалена (если существовала).");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            Pause();
        }

        /// <summary>
        /// Демонстрация шаблонного метода (импорт).
        /// </summary>
        private static void DemoImport()
        {
            Console.Clear();
            Console.WriteLine("=== Демонстрация импорта (Template Method) ===");
            Console.WriteLine("1) Импорт из JSON (JsonFileImporter)");
            Console.WriteLine("2) Импорт из CSV (CsvFileImporter)");
            Console.WriteLine("0) Назад");
            Console.Write("Выберите: ");
            var ch = Console.ReadLine();

            switch (ch)
            {
                case "1":
                    DemoImportJson();
                    break;
                case "2":
                    DemoImportCsv();
                    break;
                default:
                    break;
            }

            Pause();
        }

        private static void DemoImportJson()
        {
            Console.Write("Введите путь к JSON-файлу (или оставьте пустым для заглушки): ");
            var path = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(path))
                path = "fake_path.json";

            var importer = new JsonFileImporter();
            try
            {
                importer.Import(path);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при импорте: {ex.Message}");
            }
        }

        private static void DemoImportCsv()
        {
            Console.Write("Введите путь к CSV-файлу (или оставьте пустым для заглушки): ");
            var path = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(path))
                path = "fake_path.csv";

            var importer = new CsvFileImporter();
            try
            {
                importer.Import(path);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при импорте: {ex.Message}");
            }
        }

        /// <summary>
        /// Ожидание нажатия клавиши (для удобства).
        /// </summary>
        private static void Pause()
        {
            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
            Console.ReadKey();
        }
    }
}
