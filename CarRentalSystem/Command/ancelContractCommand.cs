using CarRentalSystem.Models;

namespace CarRentalSystem.Command
{
    // Comanda concreta - anuleaza un contract
    public class CancelContractCommand : ICommand
    {
        private readonly RentalContract _contract;
        private readonly Vehicle _vehicle;
        private ContractStatus _previousStatus;

        public string CommandName => $"CancelContract [{_contract.ContractId}]";

        public CancelContractCommand(RentalContract contract, Vehicle vehicle)
        {
            _contract = contract;
            _vehicle  = vehicle;
        }

        public void Execute()
        {
            Console.WriteLine($"[Command] Executing: {CommandName}");
            _previousStatus    = _contract.Status;
            _contract.Status   = ContractStatus.Cancelled;
            _vehicle.Return();
            Console.WriteLine($"[Command] Contract {_contract.ContractId} cancelled.");
        }

        public void Undo()
        {
            Console.WriteLine($"[Command] Undoing: {CommandName}");
            _contract.Status = _previousStatus;
            _vehicle.Rent(_contract.CustomerId);
            Console.WriteLine($"[Command] Contract {_contract.ContractId} restored to {_previousStatus}.");
        }
    }
}