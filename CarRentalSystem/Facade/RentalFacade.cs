using CarRentalSystem.Models;
using CarRentalSystem.Services;
using CarRentalSystem.Factories;
using CarRentalSystem.Builders;
using CarRentalSystem.Singleton;
using CarRentalSystem.Composite;
using CarRentalSystem.Adapters;
using CarRentalSystem.Interfaces;

namespace CarRentalSystem.Facade
{
    // Facade - interfata simplificata catre intregul subsistem
    // Clientul nu trebuie sa stie nimic despre Factory, Builder,
    // Singleton, Composite — Facade se ocupa de tot
    public class RentalFacade
    {
        // Subsisteme interne
        private readonly RentalService _rentalService;
        private readonly PaymentService _paymentService;
        private readonly NotificationService _notificationService;
        private readonly RentalContractDirector _director;
        private readonly FleetManager _fleetManager;
        private readonly RentalDatabase _database;

        public RentalFacade()
        {
            // Facade initializeaza si gestioneaza toate subsistemele
            _notificationService = new NotificationService();
            _rentalService       = new RentalService(_notificationService);
            _paymentService      = new PaymentService();
            _director            = new RentalContractDirector(new RentalContractBuilder());
            _fleetManager        = FleetManager.Instance;
            _database            = RentalDatabase.Instance;
        }

        // ------------------------------------------------
        // Operatie simpla: adauga vehicul in flota
        // ------------------------------------------------
        public Vehicle AddCarToFleet(string id, string plate, string brand,
                                      string model, int year, decimal dailyRate,
                                      bool hasAC = true)
        {
            VehicleFactory factory = new CarFactory(4, "Automatic", hasAC);
            var vehicle = factory.OrderVehicle(id, plate, brand, model,
                                               year, FuelType.Petrol, dailyRate);
            _fleetManager.AddVehicle(vehicle);
            return vehicle;
        }

        public Vehicle AddTruckToFleet(string id, string plate, string brand,
                                        string model, int year, decimal dailyRate,
                                        double cargo = 5.0)
        {
            VehicleFactory factory = new TruckFactory(cargo, true);
            var vehicle = factory.OrderVehicle(id, plate, brand, model,
                                               year, FuelType.Diesel, dailyRate);
            _fleetManager.AddVehicle(vehicle);
            return vehicle;
        }

        // ------------------------------------------------
        // Operatie simpla: inchiriaza vehicul (tot procesul)
        // ------------------------------------------------
        public (RentalContract contract, Payment payment) RentVehicle(
            Vehicle vehicle,
            Customer customer,
            int days,
            PaymentMethod paymentMethod = PaymentMethod.Card)
        {
            Console.WriteLine($"\n[Facade] Starting rental process for " +
                              $"{customer.FullName} -> {vehicle.Brand} {vehicle.Model}");

            // 1. Creeaza contractul prin Builder+Director
            var contract = _director.BuildStandardContract(
                customer.Id, vehicle.Id, vehicle.DailyRate);

            // 2. Inchiriaza vehiculul
            vehicle.Rent(customer.Id);
            customer.AddRentalToHistory(contract.ContractId);

            // 3. Proceseaza plata
            var payment = _paymentService.ProcessPayment(
                contract.ContractId, contract.TotalCost, paymentMethod);

            // 4. Salveaza in baza de date
            _database.SaveContract(contract);
            _database.SavePayment(payment);

            // 5. Notifica clientul
            customer.SendNotification(
                $"Rental confirmed! Contract: {contract.ContractId} | " +
                $"Total: {contract.TotalCost} MDL | Days: {contract.RentalDays}");

            Console.WriteLine($"[Facade] Rental process completed successfully.\n");

            return (contract, payment);
        }

        // ------------------------------------------------
        // Operatie simpla: plata prin sistem extern
        // ------------------------------------------------
        public void RentWithExternalPayment(
            Vehicle vehicle,
            Customer customer,
            int days,
            string paymentProvider = "paypal")
        {
            Console.WriteLine($"\n[Facade] Processing with external payment: {paymentProvider}");

            IPayable adapter = paymentProvider.ToLower() switch
            {
                "paypal" => new PayPalPaymentAdapter(new PayPalPaymentService()),
                "stripe" => new StripePaymentAdapter(new StripePaymentService()),
                _ => throw new ArgumentException($"Unknown provider: {paymentProvider}")
            };

            var contract = _director.BuildStandardContract(
                customer.Id, vehicle.Id, vehicle.DailyRate);

            vehicle.Rent(customer.Id);

            if (adapter.ValidatePayment())
                adapter.ProcessPayment(contract.TotalCost);

            _database.SaveContract(contract);
            customer.SendNotification($"Payment via {paymentProvider} completed!");
        }

        // ------------------------------------------------
        // Operatie simpla: afiseaza flota organizata
        // ------------------------------------------------
        public void DisplayOrganizedFleet()
        {
            Console.WriteLine("\n[Facade] Organized Fleet View:");

            var allVehicles = _fleetManager.GetAllVehicles().ToList();

            // Organizeaza in grupuri folosind Composite
            var carsGroup  = new VehicleGroup("Cars");
            var trucksGroup = new VehicleGroup("Trucks");
            var motoGroup  = new VehicleGroup("Motorcycles");

            foreach (var v in allVehicles)
            {
                var leaf = new VehicleLeaf(v);
                if (v is Car) carsGroup.Add(leaf);
                else if (v is Truck) trucksGroup.Add(leaf);
                else motoGroup.Add(leaf);
            }

            var totalFleet = new VehicleGroup("Total Fleet");
            totalFleet.Add(carsGroup);
            totalFleet.Add(trucksGroup);
            totalFleet.Add(motoGroup);

            Console.WriteLine();
            totalFleet.Display();
        }

        // ------------------------------------------------
        // Operatie simpla: raport general
        // ------------------------------------------------
        public void PrintFullReport()
        {
            Console.WriteLine("\n[Facade] ===== FULL SYSTEM REPORT =====");
            _fleetManager.PrintFleetStatus();
            _database.PrintDatabaseStatus();

            Console.WriteLine("\n--- All Contracts ---");
            foreach (var c in _database.GetAllContracts())
                Console.WriteLine($"  {c.GenerateReport()}");

            Console.WriteLine("\n--- All Payments ---");
            foreach (var p in _database.GetAllPayments())
                Console.WriteLine($"  Payment {p.PaymentId} | " +
                                  $"{p.Amount} MDL | {p.Method} | {p.Status}");
        }
    }
}