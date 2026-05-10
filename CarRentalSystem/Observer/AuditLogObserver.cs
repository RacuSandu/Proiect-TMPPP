namespace CarRentalSystem.Observer
{
    // Observator concret - salveaza in audit log
    public class AuditLogObserver : IObserver
    {
        private readonly List<string> _log = new();

        public string ObserverName => "AuditLog";

        public void Update(string eventType, string vehicleId,
                           string vehicleInfo, string customerInfo)
        {
            string entry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] " +
                           $"{eventType} | Vehicle: {vehicleId} - {vehicleInfo} | " +
                           $"Customer: {customerInfo}";
            _log.Add(entry);
            Console.WriteLine($"  [AUDIT LOG] Entry saved: {entry}");
        }

        public void PrintFullLog()
        {
            Console.WriteLine("\n[AUDIT LOG] Full log:");
            foreach (var entry in _log)
                Console.WriteLine($"  {entry}");
        }

        public int LogCount => _log.Count;
    }
}