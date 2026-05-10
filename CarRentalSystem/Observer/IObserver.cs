namespace CarRentalSystem.Observer
{
    // Interfata observator - toti observatorii o implementeaza
    public interface IObserver
    {
        string ObserverName { get; }
        void Update(string eventType, string vehicleId,
                    string vehicleInfo, string customerInfo);
    }
}