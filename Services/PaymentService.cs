using CarRentalSystem.Models;

namespace CarRentalSystem.Services
{
    // SRP: se ocupa DOAR de procesarea platilor
    public class PaymentService
    {
        private readonly List<Payment> _payments;

        public PaymentService()
        {
            _payments = new List<Payment>();
        }

        public Payment ProcessPayment(string contractId, decimal amount, PaymentMethod method)
        {
            var payment = new Payment(Guid.NewGuid().ToString(), contractId, amount, method);

            if (payment.ValidatePayment())
            {
                payment.ProcessPayment(amount);
                _payments.Add(payment);
            }

            return payment;
        }

        public List<Payment> GetAllPayments() => _payments;
    }
}