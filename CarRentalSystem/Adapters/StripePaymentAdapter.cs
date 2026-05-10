using CarRentalSystem.Interfaces;

namespace CarRentalSystem.Adapters
{
    // Adaptor - face Stripe compatibil cu IPayable
    public class StripePaymentAdapter : IPayable
    {
        private readonly IExternalPaymentService _stripeService;
        private decimal _amount;
        private string _transactionId;

        public StripePaymentAdapter(IExternalPaymentService stripeService)
        {
            _stripeService = stripeService;
            _transactionId = Guid.NewGuid().ToString();
        }

        public void ProcessPayment(decimal amount)
        {
            _amount = amount;
            _stripeService.MakePayment(_transactionId, (double)amount, "MDL");
        }

        public bool ValidatePayment()
        {
            if (_amount <= 0) return false;
            return _stripeService.VerifyTransaction(_transactionId);
        }

        public string GetStatus()
        {
            return _stripeService.GetTransactionStatus(_transactionId);
        }
    }
}