using CarRentalSystem.Models;

namespace CarRentalSystem.Strategy
{
    // Strategie loyalty - discount bazat pe numarul de inchirieri anterioare
    public class LoyaltyPricingStrategy : IPricingStrategy
    {
        private readonly int _previousRentals;

        public string StrategyName => $"Loyalty Pricing ({_previousRentals} previous rentals)";

        public LoyaltyPricingStrategy(int previousRentals)
        {
            _previousRentals = previousRentals;
        }

        private decimal GetDiscountPercent()
        {
            return _previousRentals switch
            {
                >= 20 => 25m,  // client VIP
                >= 10 => 15m,  // client fidel
                >= 5  => 10m,  // client regular
                >= 1  => 5m,   // client nou
                _     => 0m    // fara discount
            };
        }

        public decimal CalculatePrice(Vehicle vehicle, int days)
        {
            decimal discount = 1 - (GetDiscountPercent() / 100);
            return vehicle.DailyRate * discount * days;
        }

        public string GetPriceBreakdown(Vehicle vehicle, int days)
        {
            decimal discountPercent = GetDiscountPercent();
            decimal total           = CalculatePrice(vehicle, days);
            return $"[Loyalty] {_previousRentals} rentals -> {discountPercent}% discount | " +
                   $"{vehicle.DailyRate} MDL/day x {days} days = {total} MDL";
        }
    }
}