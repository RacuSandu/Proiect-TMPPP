using CarRentalSystem.Models;

namespace CarRentalSystem.Builders
{
    // Builder concret - construieste contractul pas cu pas
    public class RentalContractBuilder : IRentalContractBuilder
    {
        private string _contractId   = Guid.NewGuid().ToString();
        private string _customerId   = string.Empty;
        private string _vehicleId    = string.Empty;
        private DateTime _startDate  = DateTime.Now;
        private DateTime _endDate    = DateTime.Now.AddDays(1);
        private decimal _totalCost   = 0m;
        private ContractStatus _status = ContractStatus.Active;

        public IRentalContractBuilder SetContractId(string id)
        {
            _contractId = id;
            return this; // metode fluente - permite lantul de apeluri
        }

        public IRentalContractBuilder SetCustomer(string customerId)
        {
            _customerId = customerId;
            return this;
        }

        public IRentalContractBuilder SetVehicle(string vehicleId)
        {
            _vehicleId = vehicleId;
            return this;
        }

        public IRentalContractBuilder SetRentalPeriod(DateTime start, DateTime end)
        {
            if (end <= start)
                throw new ArgumentException("End date must be after start date.");
            _startDate = start;
            _endDate   = end;
            return this;
        }

        public IRentalContractBuilder SetTotalCost(decimal cost)
        {
            if (cost < 0)
                throw new ArgumentException("Cost cannot be negative.");
            _totalCost = cost;
            return this;
        }

        public IRentalContractBuilder SetStatus(ContractStatus status)
        {
            _status = status;
            return this;
        }

        // Construieste obiectul final
        public RentalContract Build()
        {
            if (string.IsNullOrEmpty(_customerId))
                throw new InvalidOperationException("Customer ID is required.");
            if (string.IsNullOrEmpty(_vehicleId))
                throw new InvalidOperationException("Vehicle ID is required.");

            var contract = new RentalContract(_contractId, _customerId,
                           _vehicleId, _startDate, _endDate, _totalCost);
            contract.Status = _status;
            return contract;
        }

        // Reset builder pentru reutilizare
        public RentalContractBuilder Reset()
        {
            _contractId = Guid.NewGuid().ToString();
            _customerId = string.Empty;
            _vehicleId  = string.Empty;
            _startDate  = DateTime.Now;
            _endDate    = DateTime.Now.AddDays(1);
            _totalCost  = 0m;
            _status     = ContractStatus.Active;
            return this;
        }
    }
}