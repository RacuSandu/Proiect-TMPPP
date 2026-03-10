namespace CarRentalSystem.Interfaces
{
    // ISP: interfață separată pentru notificări
    public interface INotifiable
    {
        void SendNotification(string message);
    }
}