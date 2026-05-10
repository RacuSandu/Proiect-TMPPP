namespace CarRentalSystem.Decorators
{
    // Decorator concret - adauga scaun pentru copil
    public class ChildSeatDecorator : RentalDecorator
    {
        private readonly decimal _dailyCost;

        public ChildSeatDecorator(IRentalDecorator decorated, decimal dailyCost = 15m)
            : base(decorated)
        {
            _dailyCost = dailyCost;
        }

        public override string GetDescription()
        {
            return $"{_decorated.GetDescription()} + ChildSeat";
        }

        public override decimal GetTotalCost(int days)
        {
            return _decorated.GetTotalCost(days) + (_dailyCost * days);
        }

        public override void DisplayDetails(int days)
        {
            _decorated.DisplayDetails(days);
            Console.WriteLine($"  Child Seat: +{_dailyCost * days} MDL ({_dailyCost} MDL/day)");
        }
    }
}