using CarRentalSystem.Models;

namespace CarRentalSystem.Composite
{
    // Leaf - obiect individual, nu contine alti copii
    public class VehicleLeaf : IFleetComponent
    {
        private readonly Vehicle _vehicle;

        public string Name => $"{_vehicle.Brand} {_vehicle.Model} ({_vehicle.LicensePlate})";

        public VehicleLeaf(Vehicle vehicle)
        {
            _vehicle = vehicle;
        }

        public void Display(int depth = 0)
        {
            string indent = new string('-', depth * 2);
            Console.WriteLine($"{indent}[{_vehicle.GetVehicleType()}] {Name} | " +
                              $"{_vehicle.DailyRate} MDL/day | " +
                              $"Status: {_vehicle.Status}");
        }

        public decimal GetTotalDailyRate() => _vehicle.DailyRate;

        public int GetVehicleCount() => 1;

        public bool IsAvailable() => _vehicle.IsAvailable;
    }
}