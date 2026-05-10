using CarRentalSystem.Models;

namespace CarRentalSystem.Strategy
{
    // Interfata comuna pentru toate strategiile de pret
    public interface IPricingStrategy
    {
        string StrategyName { get; }
        decimal CalculatePrice(Vehicle vehicle, int days);
        string GetPriceBreakdown(Vehicle vehicle, int days);
    }
}