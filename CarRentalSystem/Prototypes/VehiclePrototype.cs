using CarRentalSystem.Models;
using CarRentalSystem.Prototypes;

namespace CarRentalSystem.Prototypes
{
    // Wrapper Prototype pentru Vehicle
    // Permite clonarea vehiculelor existente
    public class VehiclePrototype : IPrototype<VehiclePrototype>
    {
        public string Id           { get; set; }
        public string LicensePlate { get; set; }
        public string Brand        { get; set; }
        public string Model        { get; set; }
        public int Year            { get; set; }
        public FuelType Fuel       { get; set; }
        public decimal DailyRate   { get; set; }
        public string VehicleType  { get; set; } // "Car", "Truck", "Motorcycle"

        // Proprietati specifice Car
        public int Doors              { get; set; }
        public string Transmission    { get; set; }
        public bool HasAC             { get; set; }

        // Proprietati specifice Truck
        public double CargoCapacity         { get; set; }
        public bool RequiresSpecialLicense  { get; set; }

        // Proprietati specifice Motorcycle
        public string MotorcycleType { get; set; }
        public int EngineCC          { get; set; }

        public VehiclePrototype(Vehicle vehicle)
        {
            Id           = vehicle.Id;
            LicensePlate = vehicle.LicensePlate;
            Brand        = vehicle.Brand;
            Model        = vehicle.Model;
            Year         = vehicle.Year;
            Fuel         = vehicle.Fuel;
            DailyRate    = vehicle.DailyRate;
            VehicleType  = vehicle.GetVehicleType();

            if (vehicle is Car car)
            {
                Doors        = car.NumberOfDoors;
                Transmission = car.TransmissionType;
                HasAC        = car.HasAirConditioning;
            }
            else if (vehicle is Truck truck)
            {
                CargoCapacity          = truck.CargoCapacityTons;
                RequiresSpecialLicense = truck.RequiresSpecialLicense;
            }
            else if (vehicle is Motorcycle moto)
            {
                MotorcycleType = moto.MotorcycleType;
                EngineCC       = moto.EngineCC;
            }
        }

        // Constructor privat pentru clonare interna
        private VehiclePrototype() { }

        // Shallow Copy - copiaza referintele (pentru tipuri simple e suficient)
        public VehiclePrototype ShallowCopy()
        {
            return (VehiclePrototype)this.MemberwiseClone();
        }

        // Deep Copy - creeaza un obiect complet nou
        public VehiclePrototype DeepCopy()
        {
            return new VehiclePrototype
            {
                Id                     = this.Id,
                LicensePlate           = string.Copy(this.LicensePlate),
                Brand                  = string.Copy(this.Brand),
                Model                  = string.Copy(this.Model),
                Year                   = this.Year,
                Fuel                   = this.Fuel,
                DailyRate              = this.DailyRate,
                VehicleType            = string.Copy(this.VehicleType),
                Doors                  = this.Doors,
                Transmission           = this.Transmission != null ? string.Copy(this.Transmission) : null,
                HasAC                  = this.HasAC,
                CargoCapacity          = this.CargoCapacity,
                RequiresSpecialLicense = this.RequiresSpecialLicense,
                MotorcycleType         = this.MotorcycleType != null ? string.Copy(this.MotorcycleType) : null,
                EngineCC               = this.EngineCC
            };
        }

        // Converteste prototipul inapoi intr-un Vehicle concret
        public Vehicle ToVehicle(string newId, string newLicensePlate)
        {
            return VehicleType switch
            {
                "Car" => new Car(newId, newLicensePlate, Brand, Model, Year,
                                 Fuel, DailyRate, Doors, Transmission, HasAC),
                "Truck" => new Truck(newId, newLicensePlate, Brand, Model, Year,
                                     Fuel, DailyRate, CargoCapacity, RequiresSpecialLicense),
                "Motorcycle" => new Motorcycle(newId, newLicensePlate, Brand, Model, Year,
                                               Fuel, DailyRate, MotorcycleType, EngineCC),
                _ => throw new InvalidOperationException($"Unknown vehicle type: {VehicleType}")
            };
        }

        public override string ToString()
        {
            return $"[Prototype:{VehicleType}] {Brand} {Model} ({Year}) | {LicensePlate} | {DailyRate} MDL/day";
        }
    }
}