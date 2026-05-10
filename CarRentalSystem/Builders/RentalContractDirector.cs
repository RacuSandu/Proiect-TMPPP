using CarRentalSystem.Models;

namespace CarRentalSystem.Builders
{
    // Director - stie cum sa construiasca tipuri predefinite de contracte
    // Clientul nu trebuie sa stie pasii exacti
    public class RentalContractDirector
    {
        private readonly IRentalContractBuilder _builder;

        public RentalContractDirector(IRentalContractBuilder builder)
        {
            _builder = builder;
        }

        // Contract standard - 3 zile
        public RentalContract BuildStandardContract(string customerId, string vehicleId, decimal dailyRate)
        {
            var start = DateTime.Now;
            var end   = start.AddDays(3);
            return _builder
                .SetContractId($"STD-{Guid.NewGuid().ToString()[..8]}")
                .SetCustomer(customerId)
                .SetVehicle(vehicleId)
                .SetRentalPeriod(start, end)
                .SetTotalCost(dailyRate * 3)
                .SetStatus(ContractStatus.Active)
                .Build();
        }

        // Contract premium - 7 zile
        public RentalContract BuildPremiumContract(string customerId, string vehicleId, decimal dailyRate)
        {
            var start = DateTime.Now;
            var end   = start.AddDays(7);
            decimal cost = dailyRate * 7 * 0.9m; // 10% discount
            return _builder
                .SetContractId($"PRM-{Guid.NewGuid().ToString()[..8]}")
                .SetCustomer(customerId)
                .SetVehicle(vehicleId)
                .SetRentalPeriod(start, end)
                .SetTotalCost(cost)
                .SetStatus(ContractStatus.Active)
                .Build();
        }

        // Contract business - 14 zile
        public RentalContract BuildBusinessContract(string customerId, string vehicleId, decimal dailyRate)
        {
            var start = DateTime.Now;
            var end   = start.AddDays(14);
            decimal cost = dailyRate * 14 * 0.8m; // 20% discount
            return _builder
                .SetContractId($"BIZ-{Guid.NewGuid().ToString()[..8]}")
                .SetCustomer(customerId)
                .SetVehicle(vehicleId)
                .SetRentalPeriod(start, end)
                .SetTotalCost(cost)
                .SetStatus(ContractStatus.Active)
                .Build();
        }
    }
}