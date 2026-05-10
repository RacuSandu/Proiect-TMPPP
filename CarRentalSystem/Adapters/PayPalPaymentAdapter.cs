using CarRentalSystem.Interfaces;

namespace CarRentalSystem.Adapters
{
    // Adaptor - face PayPal compatibil cu IPayable
    // Nu modificam PayPalPaymentService, doar il "traducem"
    public class PayPalPaymentAdapter : IPayable
    {
        private readonly IExternalPaymentService _payPalService;
        private decimal _amount;
        private string _transactionId;

        public PayPalPaymentAdapter(IExternalPaymentService payPalService)
        {
            _payPalService = payPalService;
            _transactionId = Guid.NewGuid().ToString();
        }

        // IPayable - interfata noastra
        public void ProcessPayment(decimal amount)
        {
            _amount = amount;
            // Traducem: decimal -> double, adaugam currency MDL
            _payPalService.MakePayment(_transactionId, (double)amount, "MDL");
        }

        public bool ValidatePayment()
        {
            if (_amount <= 0) return false;
            return _payPalService.VerifyTransaction(_transactionId);
        }

        public string GetStatus()
        {
            return _payPalService.GetTransactionStatus(_transactionId);
        }
    }
}