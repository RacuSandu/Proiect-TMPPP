using CarRentalSystem.Models;

namespace CarRentalSystem.Factories
{
    // Fabrica concreta - Pachet Standard
    // Vehicul: Masina simpla, Contract: 3 zile, Plata: Cash
    public class StandardPackageFactory : IRentalPackageFactory
    {
        public Vehicle CreateVehicle()
        {
            Console.WriteLine("[Standard Factory] Creating standard car...");
            return new Car("V-STD-001", "STD001", "Dacia", "Logan", 2020,
                           FuelType.Petrol, 150m, 4, "Manual", false);
        }

        public RentalContract CreateContract(string customerId, string vehicleId)
        {
            Console.WriteLine("[Standard Factory] Creating standard contract (3 days)...");
            var start = DateTime.Now;
            var end = start.AddDays(3);
            decimal cost = 150m * 3; // 3 zile fara extra
            return new RentalContract(Guid.NewGuid().ToString(),
                                      customerId, vehicleId, start, end, cost);
        }

        public Payment CreatePayment(string contractId, decimal amount)
        {
            Console.WriteLine("[Standard Factory] Creating cash payment...");
            return new Payment(Guid.NewGuid().ToString(),
                               contractId, amount, PaymentMethod.Cash);
        }
    }
}