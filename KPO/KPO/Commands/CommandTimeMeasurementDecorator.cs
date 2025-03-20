namespace KPO.Commands
{
    public class CommandTimeMeasurementDecorator : ICommand
    {
        private readonly ICommand _wrappedCommand;

        public CommandTimeMeasurementDecorator(ICommand wrappedCommand)
        {
            _wrappedCommand = wrappedCommand;
        }

        public void Execute()
        {
            var start = DateTime.Now;
            _wrappedCommand.Execute();
            var end = DateTime.Now;

            Console.WriteLine($"[Decorator] Время выполнения команды: {(end - start).TotalMilliseconds} мс");
        }
    }
}