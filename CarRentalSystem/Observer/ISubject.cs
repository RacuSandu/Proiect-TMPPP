namespace CarRentalSystem.Observer
{
    // Interfata subiect - gestioneaza lista de observatori
    public interface ISubject
    {
        void Attach(IObserver observer);
        void Detach(IObserver observer);
        void Notify(string eventType, string vehicleId,
                    string vehicleInfo, string customerInfo);
    }
}