using CarRentalSystem.Models;
using CarRentalSystem.Singleton;

namespace CarRentalSystem.Command
{
    // Comanda concreta - inchiriaza un vehicul
    public class RentVehicleCommand : ICommand
    {
        private readonly Vehicle _vehicle;
        private readonly Customer _customer;
        private readonly RentalDatabase _database;
        private RentalContract? _createdContract;

        public string CommandName => $"RentVehicle [{_vehicle.Brand} {_vehicle.Model} -> {_customer.FullName}]";

        public RentVehicleCommand(Vehicle vehicle, Customer customer)
        {
            _vehicle  = vehicle;
            _customer = customer;
            _database = RentalDatabase.Instance;
        }

        public void Execute()
        {
            Console.WriteLine($"[Command] Executing: {CommandName}");
            _vehicle.Rent(_customer.Id);

            _createdContract = new RentalContract(
                Guid.NewGuid().ToString(),
                _customer.Id,
                _vehicle.Id,
                DateTime.Now,
                DateTime.Now.AddDays(3),
                _vehicle.CalculateRentalCost(3)
            );

            _customer.AddRentalToHistory(_createdContract.ContractId);
            _database.SaveContract(_createdContract);
            Console.WriteLine($"[Command] Contract created: {_createdContract.ContractId}");
        }

        public void Undo()
        {
            Console.WriteLine($"[Command] Undoing: {CommandName}");
            _vehicle.Return();

            if (_createdContract != null)
            {
                _createdContract.Status = ContractStatus.Cancelled;
                Console.WriteLine($"[Command] Contract cancelled: {_createdContract.ContractId}");
            }
        }
    }
}