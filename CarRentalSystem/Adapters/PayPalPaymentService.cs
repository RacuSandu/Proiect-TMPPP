namespace CarRentalSystem.Adapters
{
    // Sistem extern PayPal - are propria interfata, nu o putem modifica
    public class PayPalPaymentService : IExternalPaymentService
    {
        public void MakePayment(string transactionId, double amount, string currency)
        {
            Console.WriteLine($"[PayPal] Processing payment: {amount} {currency} | Transaction: {transactionId}");
        }

        public bool VerifyTransaction(string transactionId)
        {
            Console.WriteLine($"[PayPal] Verifying transaction: {transactionId}");
            return true;
        }

        public string GetTransactionStatus(string transactionId)
        {
            return $"[PayPal] Transaction {transactionId}: COMPLETED";
        }
    }
}