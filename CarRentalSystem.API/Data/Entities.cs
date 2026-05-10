namespace CarRentalSystem.API.Data
{
    // Entitate vehicul pentru baza de date
    public class VehicleEntity
    {
        public string Id { get; set; } = string.Empty;
        public string LicensePlate { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public string FuelType { get; set; } = string.Empty;
        public string VehicleType { get; set; } = string.Empty; // Car, Truck, Motorcycle
        public decimal DailyRate { get; set; }
        public string Status { get; set; } = "Available";

        // Proprietati specifice Car
        public int? Doors { get; set; }
        public string? Transmission { get; set; }
        public bool? HasAC { get; set; }

        // Proprietati specifice Truck
        public double? CargoCapacity { get; set; }
        public bool? RequiresSpecialLicense { get; set; }

        // Proprietati specifice Motorcycle
        public string? MotorcycleType { get; set; }
        public int? EngineCC { get; set; }
    }

    // Entitate client pentru baza de date
    public class CustomerEntity
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string DriverLicenseNumber { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public int RentalCount { get; set; } = 0;
    }

    // Entitate contract pentru baza de date
    public class ContractEntity
    {
        public string ContractId { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public string VehicleId { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalCost { get; set; }
        public string Status { get; set; } = "Active";
    }

    // Entitate plata pentru baza de date
    public class PaymentEntity
    {
        public string PaymentId { get; set; } = string.Empty;
        public string ContractId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Method { get; set; } = string.Empty;
        public string Status { get; set; } = "Completed";
        public DateTime PaymentDate { get; set; }
    }
}