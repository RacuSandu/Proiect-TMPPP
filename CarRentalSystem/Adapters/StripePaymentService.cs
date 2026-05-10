namespace CarRentalSystem.Adapters
{
    // Sistem extern Stripe - alta interfata incompatibila
    public class StripePaymentService : IExternalPaymentService
    {
        public void MakePayment(string transactionId, double amount, string currency)
        {
            Console.WriteLine($"[Stripe] Charging {amount} {currency} | Ref: {transactionId}");
        }

        public bool VerifyTransaction(string transactionId)
        {
            Console.WriteLine($"[Stripe] Verifying ref: {transactionId}");
            return true;
        }

        public string GetTransactionStatus(string transactionId)
        {
            return $"[Stripe] Ref {transactionId}: SUCCESS";
        }
    }
}