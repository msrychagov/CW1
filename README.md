# CW1

## Где и как выполняются требования

### 1. **Доменная модель**
- **BankAccount**, **Category**, **Operation** в папке `Domain` – описывают основную бизнес-логику и данные (ID, тип, суммы, баланс и т.д.).
- Учет финансовых операций ведётся за счёт изменения баланса в `BankAccount`.

### 2. **Текстовое консольное меню (CRUD)**
- В файле `Program.cs` реализовано **текстовое меню**, которое позволяет:
    - Создавать, просматривать, редактировать и удалять **счета**.
    - Создавать, просматривать, редактировать и удалять **категории**.
    - Создавать, просматривать, редактировать и удалять **операции**.
- По каждому пункту (счета, категории, операции) пользователь вводит данные (ID, сумму, тип, описание и т.д.), после чего программа вызывает соответствующий фасад для CRUD-операций.

### 3. **Принципы SOLID**
1. **SRP (Single Responsibility Principle)**
    - Каждый фасад (BankAccountFacade, CategoryFacade, OperationFacade) отвечает только за CRUD своей сущности.
    - Классы в `Domain` (BankAccount, Category, Operation) отвечают только за хранение данных и простую бизнес-логику.
2. **OCP (Open-Closed Principle)**
    - Для добавления новых импортёров данных (JSON, CSV, XML и т.д.) достаточно создать новый класс, унаследованный от `BaseFileImporter`.
    - Логика счёта, категории и операций может расширяться (через новые методы или наследование) без изменения существующих классов.
3. **LSP (Liskov Substitution Principle)**
    - `JsonFileImporter` и `CsvFileImporter` могут заменять базовый `BaseFileImporter` без нарушения корректности работы.
4. **ISP (Interface Segregation Principle)**
    - Интерфейс `IFacade<T>` определяет ровно те методы, которые нужны для CRUD (Create, Read, Update, Delete).
    - Интерфейс `ICommand` содержит только один метод `Execute()`.
5. **DIP (Dependency Inversion Principle)**
    - Вся работа с данными выполняется через интерфейсы `IFacade<T>` и `ICommand`.
    - В `Program.cs` используется фабрика (в упрощённом виде) – мы напрямую создаём объекты фасадов и команд, но могли бы внедрять их через DI-контейнер.

### 4. **GRASP**
- **High Cohesion / Low Coupling**:
    - Каждая сущность (Account, Category, Operation) имеет чётко очерченную зону ответственности.
    - Фасады изолируют логику работы с наборами объектов от остального кода.
- **Controller**:
    - Метод `Main` в классе `Program` играет роль «контроллера», принимающего ввод и направляющего его в фасады.
- **Creator**:
    - Фасады (например, `OperationFacade`) являются «создателями» соответствующих объектов внутри программы (добавляют в списки, управляют балансом).

### 5. **Паттерны GoF**
1. **Facade**
    - `BankAccountFacade`, `CategoryFacade`, `OperationFacade` инкапсулируют работу со списками объектов и CRUD-операции.
2. **Command**
    - `CreateOperationCommand` и `DeleteOperationCommand` инкапсулируют создание и удаление операции.
    - Эти команды вызываются из меню `Program.cs` при соответствующих действиях пользователя.
3. **Decorator**
    - `CommandTimeMeasurementDecorator` оборачивает объекты, реализующие `ICommand`, и выводит время выполнения команды в консоль.
    - Используется при создании и удалении операций (декоратор оборачивает `CreateOperationCommand` и `DeleteOperationCommand`).
4. **Template Method**
    - `BaseFileImporter` содержит общий алгоритм импорта: `ReadFile`, `ParseData`, `SaveItems`.
    - `JsonFileImporter` и `CsvFileImporter` переопределяют только логику парсинга (`ParseData`), сохраняя общий «каркас» импорта без изменений.
