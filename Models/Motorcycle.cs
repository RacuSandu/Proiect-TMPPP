namespace CarRentalSystem.Models
{
    public class Motorcycle : Vehicle
    {
        public string MotorcycleType { get; set; } // Sport, Cruiser, Touring
        public int EngineCC { get; set; }

        public Motorcycle(string id, string licensePlate, string brand, string model,
                          int year, FuelType fuel, decimal dailyRate,
                          string motoType, int engineCC)
            : base(id, licensePlate, brand, model, year, fuel, dailyRate)
        {
            MotorcycleType = motoType;
            EngineCC = engineCC;
        }

        public override string GetVehicleType() => "Motorcycle";
    }
}