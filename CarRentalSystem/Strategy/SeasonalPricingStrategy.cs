using CarRentalSystem.Models;

namespace CarRentalSystem.Strategy
{
    // Strategie sezoniera - pret mai mare in sezon de varf
    public class SeasonalPricingStrategy : IPricingStrategy
    {
        private readonly decimal _seasonMultiplier;
        private readonly string _seasonName;

        public string StrategyName => $"Seasonal Pricing ({_seasonName})";

        public SeasonalPricingStrategy(string seasonName = "Summer", decimal multiplier = 1.3m)
        {
            _seasonName       = seasonName;
            _seasonMultiplier = multiplier;
        }

        public decimal CalculatePrice(Vehicle vehicle, int days)
        {
            return vehicle.DailyRate * _seasonMultiplier * days;
        }

        public string GetPriceBreakdown(Vehicle vehicle, int days)
        {
            decimal total = CalculatePrice(vehicle, days);
            return $"[Seasonal-{_seasonName}] {vehicle.DailyRate} MDL/day " +
                   $"x {_seasonMultiplier} multiplier x {days} days = {total} MDL";
        }
    }
}