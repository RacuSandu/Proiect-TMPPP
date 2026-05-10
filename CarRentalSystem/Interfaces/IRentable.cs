namespace CarRentalSystem.Interfaces
{
    // ISP: interfață separată doar pentru operații de închiriere
    public interface IRentable
    {
        void Rent(string customerId);
        void Return();
        decimal CalculateRentalCost(int days);
    }
}