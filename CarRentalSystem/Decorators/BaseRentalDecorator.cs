using CarRentalSystem.Models;

namespace CarRentalSystem.Decorators
{
    // Componenta de baza - vehiculul fara extra-uri
    public class BaseVehicleRental : IRentalDecorator
    {
        private readonly Vehicle _vehicle;

        public BaseVehicleRental(Vehicle vehicle)
        {
            _vehicle = vehicle;
        }

        public string GetDescription()
        {
            return $"{_vehicle.Brand} {_vehicle.Model} [{_vehicle.GetVehicleType()}]";
        }

        public decimal GetTotalCost(int days)
        {
            return _vehicle.CalculateRentalCost(days);
        }

        public void DisplayDetails(int days)
        {
            Console.WriteLine($"  Base:       {GetDescription()} | {GetTotalCost(days)} MDL ({days} days)");
        }
    }

    // Decorator abstract - tine referinta catre obiectul decorat
    public abstract class RentalDecorator : IRentalDecorator
    {
        protected readonly IRentalDecorator _decorated;

        protected RentalDecorator(IRentalDecorator decorated)
        {
            _decorated = decorated;
        }

        public virtual string GetDescription()
        {
            return _decorated.GetDescription();
        }

        public virtual decimal GetTotalCost(int days)
        {
            return _decorated.GetTotalCost(days);
        }

        public virtual void DisplayDetails(int days)
        {
            _decorated.DisplayDetails(days);
        }
    }
}