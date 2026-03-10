namespace CarRentalSystem.Models
{
    // Mostenire + LSP: Car poate inlocui Vehicle
    public class Car : Vehicle
    {
        public int NumberOfDoors { get; set; }
        public string TransmissionType { get; set; } // Manual / Automatic
        public bool HasAirConditioning { get; set; }

        public Car(string id, string licensePlate, string brand, string model,
                   int year, FuelType fuel, decimal dailyRate,
                   int doors, string transmission, bool hasAC)
            : base(id, licensePlate, brand, model, year, fuel, dailyRate)
        {
            NumberOfDoors = doors;
            TransmissionType = transmission;
            HasAirConditioning = hasAC;
        }

        public override string GetVehicleType() => "Car";

        // OCP: extinde calculul fara a modifica clasa de baza
        public override decimal CalculateRentalCost(int days)
        {
            decimal baseCost = base.CalculateRentalCost(days);
            decimal acSurcharge = HasAirConditioning ? days * 10m : 0m;
            return baseCost + acSurcharge;
        }
    }
}