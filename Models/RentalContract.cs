using CarRentalSystem.Interfaces;

namespace CarRentalSystem.Models
{
    public enum ContractStatus { Active, Completed, Cancelled }

    // SRP: aceasta clasa se ocupa DOAR de contractul de inchiriere
    public class RentalContract : IReportable
    {
        public string ContractId { get; private set; }
        public string CustomerId { get; set; }
        public string VehicleId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalCost { get; set; }
        public ContractStatus Status { get; set; }

        public int RentalDays => (EndDate - StartDate).Days;

        public RentalContract(string contractId, string customerId, string vehicleId,
                               DateTime startDate, DateTime endDate, decimal totalCost)
        {
            ContractId = contractId;
            CustomerId = customerId;
            VehicleId = vehicleId;
            StartDate = startDate;
            EndDate = endDate;
            TotalCost = totalCost;
            Status = ContractStatus.Active;
        }

        // IReportable
        public string GenerateReport()
        {
            return $"Contract {ContractId} | Customer: {CustomerId} | Vehicle: {VehicleId} | " +
                   $"{StartDate:dd.MM.yyyy} - {EndDate:dd.MM.yyyy} | {RentalDays} days | " +
                   $"Total: {TotalCost} MDL | Status: {Status}";
        }
    }
}