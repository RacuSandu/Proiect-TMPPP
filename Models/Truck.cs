namespace CarRentalSystem.Models
{
    // Mostenire + LSP
    public class Truck : Vehicle
    {
        public double CargoCapacityTons { get; set; }
        public bool RequiresSpecialLicense { get; set; }

        public Truck(string id, string licensePlate, string brand, string model,
                     int year, FuelType fuel, decimal dailyRate,
                     double cargoCapacity, bool requiresSpecialLicense)
            : base(id, licensePlate, brand, model, year, fuel, dailyRate)
        {
            CargoCapacityTons = cargoCapacity;
            RequiresSpecialLicense = requiresSpecialLicense;
        }

        public override string GetVehicleType() => "Truck";

        public override decimal CalculateRentalCost(int days)
        {
            decimal baseCost = base.CalculateRentalCost(days);
            // Camionul are un tarif suplimentar bazat pe capacitate
            decimal capacitySurcharge = (decimal)(CargoCapacityTons * 50) * days;
            return baseCost + capacitySurcharge;
        }
    }
}