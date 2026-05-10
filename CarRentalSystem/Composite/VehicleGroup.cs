namespace CarRentalSystem.Composite
{
    // Composite - contine mai multi IFleetComponent (pot fi Leaf sau alte Group-uri)
    // Trateaza colectia exact ca pe un obiect individual
    public class VehicleGroup : IFleetComponent
    {
        private readonly List<IFleetComponent> _components = new();

        public string Name { get; }

        public VehicleGroup(string name)
        {
            Name = name;
        }

        // Gestionare copii
        public void Add(IFleetComponent component)
        {
            _components.Add(component);
            Console.WriteLine($"[Group: {Name}] Added: {component.Name}");
        }

        public void Remove(IFleetComponent component)
        {
            _components.Remove(component);
            Console.WriteLine($"[Group: {Name}] Removed: {component.Name}");
        }

        // Afiseaza ierarhia intreaga recursiv
        public void Display(int depth = 0)
        {
            string indent = new string('-', depth * 2);
            Console.WriteLine($"{indent}[GROUP] {Name} | " +
                              $"Vehicles: {GetVehicleCount()} | " +
                              $"Total Rate: {GetTotalDailyRate()} MDL/day");

            foreach (var component in _components)
                component.Display(depth + 1);
        }

        // Calculeaza recursiv pentru toti copiii
        public decimal GetTotalDailyRate()
        {
            return _components.Sum(c => c.GetTotalDailyRate());
        }

        public int GetVehicleCount()
        {
            return _components.Sum(c => c.GetVehicleCount());
        }

        public bool IsAvailable()
        {
            return _components.Any(c => c.IsAvailable());
        }

        public IEnumerable<IFleetComponent> GetAvailableComponents()
        {
            return _components.Where(c => c.IsAvailable());
        }
    }
}