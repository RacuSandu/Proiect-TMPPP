using CarRentalSystem.Interfaces;
using CarRentalSystem.Models;

namespace CarRentalSystem.Services
{
    // SRP: se ocupa DOAR de logica de inchiriere
    // DIP: depinde de abstractii (IVehicle, IRentable), nu de implementari concrete
    public class RentalService
    {
        private readonly List<RentalContract> _contracts;
        private readonly INotifiable _notifier;

        // DIP: Injectie de dependente prin constructor
        public RentalService(INotifiable notifier)
        {
            _contracts = new List<RentalContract>();
            _notifier = notifier;
        }

        public RentalContract CreateContract(Vehicle vehicle, Customer customer,
                                              DateTime start, DateTime end)
        {
            if (!vehicle.IsAvailable)
                throw new InvalidOperationException("Vehicle is not available.");

            int days = (end - start).Days;
            decimal cost = vehicle.CalculateRentalCost(days);

            var contract = new RentalContract(
                Guid.NewGuid().ToString(),
                customer.Id,
                vehicle.Id,
                start, end, cost
            );

            vehicle.Rent(customer.Id);
            customer.AddRentalToHistory(contract.ContractId);
            _contracts.Add(contract);

            _notifier.SendNotification($"Contract {contract.ContractId} created. Total: {cost} MDL");

            return contract;
        }

        public List<RentalContract> GetAllContracts() => _contracts;
    }
}