using KPO.Facades;

namespace KPO.Commands
{
    public class DeleteOperationCommand : BaseCommand
    {
        private readonly int _operationId;
        private readonly OperationFacade _operationFacade;

        public DeleteOperationCommand(int operationId, OperationFacade operationFacade)
        {
            _operationId = operationId;
            _operationFacade = operationFacade;
        }

        public override void Execute()
        {
            _operationFacade.Delete(_operationId);
        }
    }
}