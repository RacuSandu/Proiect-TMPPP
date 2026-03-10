namespace CarRentalSystem.Interfaces
{
    // ISP: interfață separată pentru plăți
    public interface IPayable
    {
        void ProcessPayment(decimal amount);
        bool ValidatePayment();
    }
}