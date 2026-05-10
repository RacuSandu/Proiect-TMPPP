using CarRentalSystem.Models;

namespace CarRentalSystem.Factories
{
    // Fabrica concreta - Pachet Premium
    // Vehicul: Masina premium cu AC, Contract: 7 zile, Plata: Card
    public class PremiumPackageFactory : IRentalPackageFactory
    {
        public Vehicle CreateVehicle()
        {
            Console.WriteLine("[Premium Factory] Creating premium car...");
            return new Car("V-PRM-001", "PRM001", "BMW", "Seria 5", 2023,
                           FuelType.Hybrid, 400m, 4, "Automatic", true);
        }

        public RentalContract CreateContract(string customerId, string vehicleId)
        {
            Console.WriteLine("[Premium Factory] Creating premium contract (7 days)...");
            var start = DateTime.Now;
            var end = start.AddDays(7);
            decimal cost = (400m + 10m) * 7; // 7 zile cu AC surcharge
            return new RentalContract(Guid.NewGuid().ToString(),
                                      customerId, vehicleId, start, end, cost);
        }

        public Payment CreatePayment(string contractId, decimal amount)
        {
            Console.WriteLine("[Premium Factory] Creating card payment...");
            return new Payment(Guid.NewGuid().ToString(),
                               contractId, amount, PaymentMethod.Card);
        }
    }
}