using CarRentalSystem.Models;

namespace CarRentalSystem.Command
{
    // Comanda concreta - returneaza un vehicul
    public class ReturnVehicleCommand : ICommand
    {
        private readonly Vehicle _vehicle;
        private readonly Customer _customer;
        private VehicleStatus _previousStatus;

        public string CommandName => $"ReturnVehicle [{_vehicle.Brand} {_vehicle.Model}]";

        public ReturnVehicleCommand(Vehicle vehicle, Customer customer)
        {
            _vehicle  = vehicle;
            _customer = customer;
        }

        public void Execute()
        {
            Console.WriteLine($"[Command] Executing: {CommandName}");
            _previousStatus = _vehicle.Status;
            _vehicle.Return();
        }

        public void Undo()
        {
            Console.WriteLine($"[Command] Undoing: {CommandName} - restoring to {_previousStatus}");
            if (_previousStatus == VehicleStatus.Rented)
                _vehicle.Rent(_customer.Id);
        }
    }
}