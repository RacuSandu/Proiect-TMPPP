namespace CarRentalSystem.Decorators
{
    // Decorator concret - adauga asigurare
    public class InsuranceDecorator : RentalDecorator
    {
        private readonly string _insuranceType;
        private readonly decimal _dailyCost;

        public InsuranceDecorator(IRentalDecorator decorated,
                                   string insuranceType = "Standard",
                                   decimal dailyCost = 50m)
            : base(decorated)
        {
            _insuranceType = insuranceType;
            _dailyCost     = dailyCost;
        }

        public override string GetDescription()
        {
            return $"{_decorated.GetDescription()} + Insurance({_insuranceType})";
        }

        public override decimal GetTotalCost(int days)
        {
            return _decorated.GetTotalCost(days) + (_dailyCost * days);
        }

        public override void DisplayDetails(int days)
        {
            _decorated.DisplayDetails(days);
            Console.WriteLine($"  Insurance:  {_insuranceType} | +{_dailyCost * days} MDL ({_dailyCost} MDL/day)");
        }
    }
}