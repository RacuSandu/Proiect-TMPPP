using CarRentalSystem.Models;

namespace CarRentalSystem.Prototypes
{
    // Registru de prototipuri - stocheaza prototipuri predefinite
    // Singleton-like: o singura instanta a registrului
    public class PrototypeRegistry
    {
        private readonly Dictionary<string, VehiclePrototype> _prototypes = new();

        public void Register(string key, VehiclePrototype prototype)
        {
            _prototypes[key] = prototype;
            Console.WriteLine($"[Registry] Prototype registered: {key}");
        }

        public VehiclePrototype GetClone(string key)
        {
            if (!_prototypes.TryGetValue(key, out var prototype))
                throw new KeyNotFoundException($"Prototype '{key}' not found in registry.");

            return prototype.DeepCopy();
        }

        public IEnumerable<string> GetRegisteredKeys() => _prototypes.Keys;
    }
}