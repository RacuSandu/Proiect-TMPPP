using CarRentalSystem.Models;

namespace CarRentalSystem.Singleton
{
    // Al doilea Singleton - baza de date pentru contracte si plati
    // Demonstreaza ca pattern-ul se aplica pentru orice resursa unica
    public class RentalDatabase
    {
        private static readonly Lazy<RentalDatabase> _instance =
            new Lazy<RentalDatabase>(() => new RentalDatabase());

        private RentalDatabase()
        {
            _contracts = new List<RentalContract>();
            _payments  = new List<Payment>();
            Console.WriteLine("[RentalDatabase] Instance created.");
        }

        public static RentalDatabase Instance => _instance.Value;

        private readonly List<RentalContract> _contracts;
        private readonly List<Payment> _payments;

        // Contracte
        public void SaveContract(RentalContract contract)
        {
            _contracts.Add(contract);
            Console.WriteLine($"[DB] Contract saved: {contract.ContractId}");
        }

        public RentalContract? GetContract(string contractId)
        {
            return _contracts.FirstOrDefault(c => c.ContractId == contractId);
        }

        public IEnumerable<RentalContract> GetAllContracts() => _contracts;

        // Plati
        public void SavePayment(Payment payment)
        {
            _payments.Add(payment);
            Console.WriteLine($"[DB] Payment saved: {payment.PaymentId}");
        }

        public IEnumerable<Payment> GetAllPayments() => _payments;

        public void PrintDatabaseStatus()
        {
            Console.WriteLine($"\n[RentalDatabase] Status:");
            Console.WriteLine($"  Contracts: {_contracts.Count}");
            Console.WriteLine($"  Payments:  {_payments.Count}");
        }
    }
}