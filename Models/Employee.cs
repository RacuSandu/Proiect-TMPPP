using CarRentalSystem.Interfaces;

namespace CarRentalSystem.Models
{
    // LSP: Employee poate inlocui Person
    public class Employee : Person, INotifiable
    {
        public string Position { get; set; }
        public decimal Salary { get; set; }
        public DateTime HireDate { get; set; }

        public Employee(string id, string firstName, string lastName,
                        string email, string phone, string position, decimal salary)
            : base(id, firstName, lastName, email, phone)
        {
            Position = position;
            Salary = salary;
            HireDate = DateTime.Now;
        }

        public override string GetRole() => $"Employee - {Position}";

        public void SendNotification(string message)
        {
            Console.WriteLine($"[Internal] Notification to {FullName}: {message}");
        }
    }
}