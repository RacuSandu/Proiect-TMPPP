namespace CarRentalSystem.State
{
    // Stare: vehiculul este disponibil
    public class AvailableState : IVehicleState
    {
        public string StateName => "Available";

        public void Rent(VehicleContext context, string customerId)
        {
            Console.WriteLine($"[State] {context.VehicleName}: Available -> Rented by {customerId}");
            context.CurrentCustomerId = customerId;
            context.SetState(new RentedState());
        }

        public void Return(VehicleContext context)
        {
            Console.WriteLine($"[State] {context.VehicleName}: Already available, cannot return.");
        }

        public void SendToMaintenance(VehicleContext context)
        {
            Console.WriteLine($"[State] {context.VehicleName}: Available -> UnderMaintenance");
            context.SetState(new UnderMaintenanceState());
        }

        public void FinishMaintenance(VehicleContext context)
        {
            Console.WriteLine($"[State] {context.VehicleName}: Not in maintenance.");
        }
    }
}