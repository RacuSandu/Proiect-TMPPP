using CarRentalSystem.Models;

namespace CarRentalSystem.Factories
{
    // Fabrica concreta pentru camioane
    public class TruckFactory : VehicleFactory
    {
        private readonly double _cargoCapacity;
        private readonly bool _requiresSpecialLicense;

        public TruckFactory(double cargoCapacity = 5.0, bool requiresSpecialLicense = true)
        {
            _cargoCapacity = cargoCapacity;
            _requiresSpecialLicense = requiresSpecialLicense;
        }

        public override Vehicle CreateVehicle(string id, string licensePlate, string brand,
                                               string model, int year, FuelType fuel,
                                               decimal dailyRate)
        {
            return new Truck(id, licensePlate, brand, model, year, fuel,
                             dailyRate, _cargoCapacity, _requiresSpecialLicense);
        }
    }
}