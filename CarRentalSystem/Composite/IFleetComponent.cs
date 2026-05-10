namespace CarRentalSystem.Composite
{
    // Interfata comuna pentru obiecte individuale SI colectii
    // Atat VehicleLeaf cat si VehicleGroup implementeaza aceasta interfata
    public interface IFleetComponent
    {
        string Name { get; }
        void Display(int depth = 0);
        decimal GetTotalDailyRate();
        int GetVehicleCount();
        bool IsAvailable();
    }
}