namespace CarRentalSystem.Observer
{
    // Observator concret - trimite notificari prin email
    public class EmailNotificationObserver : IObserver
    {
        private readonly string _adminEmail;

        public string ObserverName => "EmailNotification";

        public EmailNotificationObserver(string adminEmail = "admin@carrental.com")
        {
            _adminEmail = adminEmail;
        }

        public void Update(string eventType, string vehicleId,
                           string vehicleInfo, string customerInfo)
        {
            Console.WriteLine($"  [EMAIL -> {_adminEmail}] Event: {eventType} | " +
                              $"Vehicle: {vehicleInfo} (ID: {vehicleId}) | " +
                              $"Customer: {customerInfo}");
        }
    }
}