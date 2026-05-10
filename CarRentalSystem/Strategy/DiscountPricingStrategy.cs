using CarRentalSystem.Models;

namespace CarRentalSystem.Strategy
{
    // Strategie cu discount - reducere procentuala
    public class DiscountPricingStrategy : IPricingStrategy
    {
        private readonly decimal _discountPercent;

        public string StrategyName => $"Discount Pricing ({_discountPercent}% off)";

        public DiscountPricingStrategy(decimal discountPercent = 15m)
        {
            _discountPercent = discountPercent;
        }

        public decimal CalculatePrice(Vehicle vehicle, int days)
        {
            decimal discount = 1 - (_discountPercent / 100);
            return vehicle.DailyRate * discount * days;
        }

        public string GetPriceBreakdown(Vehicle vehicle, int days)
        {
            decimal total    = CalculatePrice(vehicle, days);
            decimal saved    = vehicle.DailyRate * days - total;
            return $"[Discount-{_discountPercent}%] {vehicle.DailyRate} MDL/day " +
                   $"x {days} days - {_discountPercent}% = {total} MDL (saved: {saved} MDL)";
        }
    }
}