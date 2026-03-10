using CarRentalSystem.Interfaces;

namespace CarRentalSystem.Models
{
    // Mostenire din Person + implementare INotifiable
    // LSP: Customer poate inlocui Person fara probleme
    public class Customer : Person, INotifiable
    {
        public string DriverLicenseNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public List<string> RentalHistory { get; private set; }

        public Customer(string id, string firstName, string lastName,
                        string email, string phone, string driverLicense, DateTime dob)
            : base(id, firstName, lastName, email, phone)
        {
            DriverLicenseNumber = driverLicense;
            DateOfBirth = dob;
            RentalHistory = new List<string>();
        }

        // LSP + Polimorfism: implementare specifica Customer
        public override string GetRole() => "Customer";

        // ISP: implementare INotifiable
        public void SendNotification(string message)
        {
            Console.WriteLine($"Notification to {FullName} ({Email}): {message}");
        }

        public void AddRentalToHistory(string contractId)
        {
            RentalHistory.Add(contractId);
        }
    }
}