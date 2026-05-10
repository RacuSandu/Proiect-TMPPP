namespace CarRentalSystem.Adapters
{
    // Interfata sistemului extern de plati - incompatibila cu IPayable
    // Aceasta simuleaza o librarie third-party pe care nu o putem modifica
    public interface IExternalPaymentService
    {
        void MakePayment(string transactionId, double amount, string currency);
        bool VerifyTransaction(string transactionId);
        string GetTransactionStatus(string transactionId);
    }
}