namespace CarRentalSystem.State
{
    // Stare: vehiculul este in mentenanta
    public class UnderMaintenanceState : IVehicleState
    {
        public string StateName => "UnderMaintenance";

        public void Rent(VehicleContext context, string customerId)
        {
            Console.WriteLine($"[State] {context.VehicleName}: Cannot rent, vehicle is under maintenance.");
        }

        public void Return(VehicleContext context)
        {
            Console.WriteLine($"[State] {context.VehicleName}: Cannot return, vehicle is under maintenance.");
        }

        public void SendToMaintenance(VehicleContext context)
        {
            Console.WriteLine($"[State] {context.VehicleName}: Already under maintenance.");
        }

        public void FinishMaintenance(VehicleContext context)
        {
            Console.WriteLine($"[State] {context.VehicleName}: UnderMaintenance -> Available");
            context.SetState(new AvailableState());
        }
    }
} 