using CarRentalSystem.Models;

namespace CarRentalSystem.Singleton
{
    // Singleton - o singura instanta gestioneaza toata flota
    // Thread-safe folosind Lazy<T>
    public class FleetManager
    {
        // Lazy<T> garanteaza thread-safety si instantiere la prima folosire
        private static readonly Lazy<FleetManager> _instance =
            new Lazy<FleetManager>(() => new FleetManager());

        // Constructor privat - nimeni nu poate face "new FleetManager()"
        private FleetManager()
        {
            _vehicles = new List<Vehicle>();
            Console.WriteLine("[FleetManager] Instance created.");
        }

        // Singura cale de a accesa instanta
        public static FleetManager Instance => _instance.Value;

        private readonly List<Vehicle> _vehicles;

        public void AddVehicle(Vehicle vehicle)
        {
            if (_vehicles.Any(v => v.Id == vehicle.Id))
                throw new InvalidOperationException($"Vehicle {vehicle.Id} already exists in fleet.");

            _vehicles.Add(vehicle);
            Console.WriteLine($"[FleetManager] Added: {vehicle.Brand} {vehicle.Model}");
        }

        public void RemoveVehicle(string vehicleId)
        {
            var vehicle = _vehicles.FirstOrDefault(v => v.Id == vehicleId);
            if (vehicle == null)
                throw new KeyNotFoundException($"Vehicle {vehicleId} not found.");

            _vehicles.Remove(vehicle);
            Console.WriteLine($"[FleetManager] Removed vehicle: {vehicleId}");
        }

        public Vehicle? GetVehicle(string vehicleId)
        {
            return _vehicles.FirstOrDefault(v => v.Id == vehicleId);
        }

        public IEnumerable<Vehicle> GetAvailableVehicles()
        {
            return _vehicles.Where(v => v.IsAvailable);
        }

        public IEnumerable<Vehicle> GetAllVehicles() => _vehicles;

        public int TotalVehicles    => _vehicles.Count;
        public int AvailableCount   => _vehicles.Count(v => v.IsAvailable);
        public int RentedCount      => _vehicles.Count(v => !v.IsAvailable);

        public void PrintFleetStatus()
        {
            Console.WriteLine($"\n[FleetManager] Fleet Status:");
            Console.WriteLine($"  Total:     {TotalVehicles}");
            Console.WriteLine($"  Available: {AvailableCount}");
            Console.WriteLine($"  Rented:    {RentedCount}");
            foreach (var v in _vehicles)
                Console.WriteLine($"  - {v}");
        }
    }
}