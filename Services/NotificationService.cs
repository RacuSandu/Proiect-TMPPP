using CarRentalSystem.Interfaces;

namespace CarRentalSystem.Services
{
    // SRP: se ocupa DOAR de trimiterea notificarilor
    // DIP: implementeaza interfata INotifiable
    public class NotificationService : INotifiable
    {
        public void SendNotification(string message)
        {
            Console.WriteLine($"[NOTIFICATION] {DateTime.Now:HH:mm:ss} - {message}");
        }
    }
}