namespace CarRentalSystem.Models
{
    // Clasa de baza abstracta - Abstractizare
    public abstract class Person
    {
        // Encapsulare: campuri private cu proprietati publice
        private string _id;
        private string _firstName;
        private string _lastName;
        private string _email;
        private string _phone;

        public string Id
        {
            get => _id;
            set => _id = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string FirstName
        {
            get => _firstName;
            set => _firstName = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string LastName
        {
            get => _lastName;
            set => _lastName = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string Email
        {
            get => _email;
            set => _email = value;
        }

        public string Phone
        {
            get => _phone;
            set => _phone = value;
        }

        public string FullName => $"{FirstName} {LastName}";

        protected Person(string id, string firstName, string lastName, string email, string phone)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
        }

        // Metoda abstracta - fiecare subclasa o implementeaza diferit (Polimorfism)
        public abstract string GetRole();

        public override string ToString()
        {
            return $"[{GetRole()}] {FullName} | Email: {Email} | Phone: {Phone}";
        }
    }
}