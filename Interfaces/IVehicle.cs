namespace CarRentalSystem.Interfaces
{
    // ISP: interfață specifică pentru vehicule
    public interface IVehicle
    {
        string LicensePlate { get; }
        string Brand { get; }
        bool IsAvailable { get; }
    }
}