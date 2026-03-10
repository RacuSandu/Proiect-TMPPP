using CarRentalSystem.Models;

namespace CarRentalSystem.Factories
{
    // Fabrica concreta pentru masini
    public class CarFactory : VehicleFactory
    {
        private readonly int _doors;
        private readonly string _transmission;
        private readonly bool _hasAC;

        public CarFactory(int doors = 4, string transmission = "Automatic", bool hasAC = true)
        {
            _doors = doors;
            _transmission = transmission;
            _hasAC = hasAC;
        }

        // Implementarea Factory Method - decide ca se creeaza un Car
        public override Vehicle CreateVehicle(string id, string licensePlate, string brand,
                                               string model, int year, FuelType fuel,
                                               decimal dailyRate)
        {
            return new Car(id, licensePlate, brand, model, year, fuel,
                           dailyRate, _doors, _transmission, _hasAC);
        }
    }
}