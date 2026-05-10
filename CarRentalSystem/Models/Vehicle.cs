using CarRentalSystem.Interfaces;

namespace CarRentalSystem.Models
{
    public enum VehicleStatus { Available, Rented, UnderMaintenance }
    public enum FuelType { Petrol, Diesel, Electric, Hybrid }

    // OCP: clasa deschisa pentru extindere (Car, Truck, Motorcycle)
    //      dar inchisa pentru modificare
    public abstract class Vehicle : IVehicle, IRentable
    {
        public string Id { get; private set; }
        public string LicensePlate { get; private set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public FuelType Fuel { get; set; }
        public VehicleStatus Status { get; protected set; }
        public decimal DailyRate { get; set; }

        // IVehicle
        public bool IsAvailable => Status == VehicleStatus.Available;

        protected Vehicle(string id, string licensePlate, string brand,
                          string model, int year, FuelType fuel, decimal dailyRate)
        {
            Id = id;
            LicensePlate = licensePlate;
            Brand = brand;
            Model = model;
            Year = year;
            Fuel = fuel;
            DailyRate = dailyRate;
            Status = VehicleStatus.Available;
        }

        // IRentable
        public void Rent(string customerId)
        {
            if (!IsAvailable)
                throw new InvalidOperationException("Vehicle is not available for rent.");
            Status = VehicleStatus.Rented;
            Console.WriteLine($"{Brand} {Model} rented to customer {customerId}");
        }

        public void Return()
        {
            Status = VehicleStatus.Available;
            Console.WriteLine($"{Brand} {Model} has been returned.");
        }

        // OCP + Polimorfism: subclasele pot suprascrie calculul costului
        public virtual decimal CalculateRentalCost(int days)
        {
            return DailyRate * days;
        }

        public abstract string GetVehicleType();

        public override string ToString()
        {
            return $"[{GetVehicleType()}] {Brand} {Model} ({Year}) | {LicensePlate} | {Status} | {DailyRate} MDL/day";
        }
    }
}