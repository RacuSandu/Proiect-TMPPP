namespace CarRentalSystem.Decorators
{
    // Interfata comuna pentru obiectul de baza SI decoratori
    public interface IRentalDecorator
    {
        string GetDescription();
        decimal GetTotalCost(int days);
        void DisplayDetails(int days);
    }
}