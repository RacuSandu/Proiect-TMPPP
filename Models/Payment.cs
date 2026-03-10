using CarRentalSystem.Interfaces;

namespace CarRentalSystem.Models
{
    public enum PaymentMethod { Cash, Card, BankTransfer }
    public enum PaymentStatus { Pending, Completed, Failed, Refunded }

    // SRP: clasa se ocupa DOAR de date despre plata
    public class Payment : IPayable
    {
        public string PaymentId { get; private set; }
        public string ContractId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod Method { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime PaymentDate { get; set; }

        public Payment(string paymentId, string contractId, decimal amount, PaymentMethod method)
        {
            PaymentId = paymentId;
            ContractId = contractId;
            Amount = amount;
            Method = method;
            Status = PaymentStatus.Pending;
            PaymentDate = DateTime.Now;
        }

        // IPayable
        public void ProcessPayment(decimal amount)
        {
            Amount = amount;
            Status = PaymentStatus.Completed;
            Console.WriteLine($"Payment {PaymentId} of {amount} MDL processed via {Method}.");
        }

        public bool ValidatePayment()
        {
            return Amount > 0 && Status == PaymentStatus.Pending;
        }
    }
}