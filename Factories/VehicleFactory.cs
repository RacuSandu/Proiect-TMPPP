using CarRentalSystem.Models;

namespace CarRentalSystem.Factories
{
    // Clasa abstracta cu Factory Method
    // OCP: pentru a adauga un nou tip de vehicul,
    // cream o noua fabrica, nu modificam codul existent
    public abstract class VehicleFactory
    {
        // Acesta este Factory Method-ul
        // Subclasele decid CE obiect se creeaza
        public abstract Vehicle CreateVehicle(string id, string licensePlate, string brand,
                                               string model, int year, FuelType fuel,
                                               decimal dailyRate);

        // Metoda template care foloseste Factory Method-ul
        public Vehicle OrderVehicle(string id, string licensePlate, string brand,
                                     string model, int year, FuelType fuel, decimal dailyRate)
        {
            // Fabrica creeaza vehiculul
            var vehicle = CreateVehicle(id, licensePlate, brand, model, year, fuel, dailyRate);

            Console.WriteLine($"[Factory] Vehicle created: {vehicle.Brand} {vehicle.Model} ({vehicle.GetVehicleType()})");

            return vehicle;
        }
    }
}