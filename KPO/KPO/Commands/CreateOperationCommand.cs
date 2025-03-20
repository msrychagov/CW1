using KPO.Domain;
using KPO.Facades;

namespace KPO.Commands
{
    public class CreateOperationCommand : BaseCommand
    {
        private readonly Operation _operation;
        private readonly OperationFacade _operationFacade;

        public CreateOperationCommand(Operation operation, OperationFacade operationFacade)
        {
            _operation = operation;
            _operationFacade = operationFacade;
        }

        public override void Execute()
        {
            _operationFacade.Create(_operation);
        }
    }
}