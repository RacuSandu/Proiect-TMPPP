namespace CarRentalSystem.State
{
    // Context - mentine starea curenta si delega actiunile catre ea
    public class VehicleContext
    {
        private IVehicleState _currentState;

        public string VehicleName        { get; }
        public string? CurrentCustomerId { get; set; }

        public VehicleContext(string vehicleName)
        {
            VehicleName    = vehicleName;
            _currentState  = new AvailableState(); // stare initiala
        }

        public void SetState(IVehicleState state)
        {
            _currentState = state;
        }

        public string GetStateName() => _currentState.StateName;

        // Delega actiunile catre starea curenta
        public void Rent(string customerId)        => _currentState.Rent(this, customerId);
        public void Return()                        => _currentState.Return(this);
        public void SendToMaintenance()             => _currentState.SendToMaintenance(this);
        public void FinishMaintenance()             => _currentState.FinishMaintenance(this);

        public void PrintStatus()
        {
            string customer = CurrentCustomerId ?? "none";
            Console.WriteLine($"[State] {VehicleName} | State: {GetStateName()} | Customer: {customer}");
        }
    }
}