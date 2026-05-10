namespace CarRentalSystem.Decorators
{
    // Decorator concret - adauga GPS
    public class GPSDecorator : RentalDecorator
    {
        private readonly decimal _dailyCost;

        public GPSDecorator(IRentalDecorator decorated, decimal dailyCost = 20m)
            : base(decorated)
        {
            _dailyCost = dailyCost;
        }

        public override string GetDescription()
        {
            return $"{_decorated.GetDescription()} + GPS";
        }

        public override decimal GetTotalCost(int days)
        {
            return _decorated.GetTotalCost(days) + (_dailyCost * days);
        }

        public override void DisplayDetails(int days)
        {
            _decorated.DisplayDetails(days);
            Console.WriteLine($"  GPS:        +{_dailyCost * days} MDL ({_dailyCost} MDL/day)");
        }
    }
}