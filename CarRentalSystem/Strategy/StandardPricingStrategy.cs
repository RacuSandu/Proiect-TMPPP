using CarRentalSystem.Models;

namespace CarRentalSystem.Strategy
{
    // Strategie standard - pret de baza fara modificari
    public class StandardPricingStrategy : IPricingStrategy
    {
        public string StrategyName => "Standard Pricing";

        public decimal CalculatePrice(Vehicle vehicle, int days)
        {
            return vehicle.DailyRate * days;
        }

        public string GetPriceBreakdown(Vehicle vehicle, int days)
        {
            decimal total = CalculatePrice(vehicle, days);
            return $"[Standard] {vehicle.DailyRate} MDL/day x {days} days = {total} MDL";
        }
    }
}