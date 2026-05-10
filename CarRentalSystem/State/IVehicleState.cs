namespace CarRentalSystem.State
{
    // Interfata stare - defineste actiunile posibile
    public interface IVehicleState
    {
        string StateName { get; }
        void Rent(VehicleContext context, string customerId);
        void Return(VehicleContext context);
        void SendToMaintenance(VehicleContext context);
        void FinishMaintenance(VehicleContext context);
    }
}