using CarRentalSystem.Models;

namespace CarRentalSystem.Factories
{
    // Fabrica concreta - Pachet Business
    // Vehicul: Camion, Contract: 14 zile, Plata: Transfer bancar
    public class BusinessPackageFactory : IRentalPackageFactory
    {
        public Vehicle CreateVehicle()
        {
            Console.WriteLine("[Business Factory] Creating business truck...");
            return new Truck("V-BIZ-001", "BIZ001", "Mercedes", "Actros", 2022,
                             FuelType.Diesel, 600m, 15.0, true);
        }

        public RentalContract CreateContract(string customerId, string vehicleId)
        {
            Console.WriteLine("[Business Factory] Creating business contract (14 days)...");
            var start = DateTime.Now;
            var end = start.AddDays(14);
            decimal cost = (600m + (decimal)(15.0 * 50)) * 14; // 14 zile cu capacity surcharge
            return new RentalContract(Guid.NewGuid().ToString(),
                                      customerId, vehicleId, start, end, cost);
        }

        public Payment CreatePayment(string contractId, decimal amount)
        {
            Console.WriteLine("[Business Factory] Creating bank transfer payment...");
            return new Payment(Guid.NewGuid().ToString(),
                               contractId, amount, PaymentMethod.BankTransfer);
        }
    }
}