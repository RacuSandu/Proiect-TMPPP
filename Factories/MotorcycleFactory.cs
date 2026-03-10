using CarRentalSystem.Models;

namespace CarRentalSystem.Factories
{
    // Fabrica concreta pentru motociclete
    public class MotorcycleFactory : VehicleFactory
    {
        private readonly string _motorcycleType;
        private readonly int _engineCC;

        public MotorcycleFactory(string motorcycleType = "Sport", int engineCC = 600)
        {
            _motorcycleType = motorcycleType;
            _engineCC = engineCC;
        }

        public override Vehicle CreateVehicle(string id, string licensePlate, string brand,
                                               string model, int year, FuelType fuel,
                                               decimal dailyRate)
        {
            return new Motorcycle(id, licensePlate, brand, model, year, fuel,
                                  dailyRate, _motorcycleType, _engineCC);
        }
    }
}