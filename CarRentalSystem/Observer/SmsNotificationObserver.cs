namespace CarRentalSystem.Observer
{
    // Observator concret - trimite notificari prin SMS
    public class SmsNotificationObserver : IObserver
    {
        private readonly string _phoneNumber;

        public string ObserverName => "SmsNotification";

        public SmsNotificationObserver(string phoneNumber = "+37360000000")
        {
            _phoneNumber = phoneNumber;
        }

        public void Update(string eventType, string vehicleId,
                           string vehicleInfo, string customerInfo)
        {
            Console.WriteLine($"  [SMS -> {_phoneNumber}] {eventType}: " +
                              $"{vehicleInfo} | {customerInfo}");
        }
    }
}