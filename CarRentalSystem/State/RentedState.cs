namespace CarRentalSystem.State
{
    // Stare: vehiculul este inchiriat
    public class RentedState : IVehicleState
    {
        public string StateName => "Rented";

        public void Rent(VehicleContext context, string customerId)
        {
            Console.WriteLine($"[State] {context.VehicleName}: Already rented, cannot rent again.");
        }

        public void Return(VehicleContext context)
        {
            Console.WriteLine($"[State] {context.VehicleName}: Rented -> Available (returned by {context.CurrentCustomerId})");
            context.CurrentCustomerId = null;
            context.SetState(new AvailableState());
        }

        public void SendToMaintenance(VehicleContext context)
        {
            Console.WriteLine($"[State] {context.VehicleName}: Cannot send to maintenance while rented.");
        }

        public void FinishMaintenance(VehicleContext context)
        {
            Console.WriteLine($"[State] {context.VehicleName}: Not in maintenance.");
        }
    }
}