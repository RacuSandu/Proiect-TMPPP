using CarRentalSystem.Models;

namespace CarRentalSystem.Strategy
{
    // Context - foloseste strategia si permite schimbarea ei la runtime
    public class PricingContext
    {
        private IPricingStrategy _strategy;

        public PricingContext(IPricingStrategy strategy)
        {
            _strategy = strategy;
        }

        // Schimba strategia la runtime fara a modifica codul client
        public void SetStrategy(IPricingStrategy strategy)
        {
            Console.WriteLine($"[Strategy] Switching to: {strategy.StrategyName}");
            _strategy = strategy;
        }

        public decimal CalculatePrice(Vehicle vehicle, int days)
        {
            return _strategy.CalculatePrice(vehicle, days);
        }

        public void PrintPriceBreakdown(Vehicle vehicle, int days)
        {
            Console.WriteLine(_strategy.GetPriceBreakdown(vehicle, days));
        }

        public string CurrentStrategy => _strategy.StrategyName;
    }
}