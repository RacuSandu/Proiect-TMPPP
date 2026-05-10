using CarRentalSystem.Models;

namespace CarRentalSystem.Observer
{
    // Subiect concret - notifica observatorii la schimbari de stare
    public class VehicleStatusSubject : ISubject
    {
        private readonly List<IObserver> _observers = new();

        public void Attach(IObserver observer)
        {
            _observers.Add(observer);
            Console.WriteLine($"[Subject] Observer attached: {observer.ObserverName}");
        }

        public void Detach(IObserver observer)
        {
            _observers.Remove(observer);
            Console.WriteLine($"[Subject] Observer detached: {observer.ObserverName}");
        }

        public void Notify(string eventType, string vehicleId,
                           string vehicleInfo, string customerInfo)
        {
            Console.WriteLine($"\n[Subject] Notifying {_observers.Count} observers: {eventType}");
            foreach (var observer in _observers)
                observer.Update(eventType, vehicleId, vehicleInfo, customerInfo);
        }

        // Metode care declanseaza notificari
        public void VehicleRented(Vehicle vehicle, Customer customer)
        {
            vehicle.Rent(customer.Id);
            Notify("VEHICLE_RENTED", vehicle.Id,
                   $"{vehicle.Brand} {vehicle.Model}",
                   $"{customer.FullName} ({customer.Email})");
        }

        public void VehicleReturned(Vehicle vehicle, Customer customer)
        {
            vehicle.Return();
            Notify("VEHICLE_RETURNED", vehicle.Id,
                   $"{vehicle.Brand} {vehicle.Model}",
                   $"{customer.FullName} ({customer.Email})");
        }

        public void VehicleAddedToFleet(Vehicle vehicle)
        {
            Notify("VEHICLE_ADDED", vehicle.Id,
                   $"{vehicle.Brand} {vehicle.Model}",
                   "System");
        }
    }
}