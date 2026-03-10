using CarRentalSystem.Models;

namespace CarRentalSystem.Factories
{
    // Abstract Factory - interfata pentru crearea unei familii de obiecte
    public interface IRentalPackageFactory
    {
        Vehicle CreateVehicle();
        RentalContract CreateContract(string customerId, string vehicleId);
        Payment CreatePayment(string contractId, decimal amount);
    }
}