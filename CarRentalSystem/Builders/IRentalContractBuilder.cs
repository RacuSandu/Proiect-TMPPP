using CarRentalSystem.Models;

namespace CarRentalSystem.Builders
{
    // Interfata Builder - defineste pasii de constructie
    public interface IRentalContractBuilder
    {
        IRentalContractBuilder SetContractId(string id);
        IRentalContractBuilder SetCustomer(string customerId);
        IRentalContractBuilder SetVehicle(string vehicleId);
        IRentalContractBuilder SetRentalPeriod(DateTime start, DateTime end);
        IRentalContractBuilder SetTotalCost(decimal cost);
        IRentalContractBuilder SetStatus(ContractStatus status);
        RentalContract Build();
    }
}