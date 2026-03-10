using CarRentalSystem.Models;
using CarRentalSystem.Services;
using CarRentalSystem.Factories;

// =============================================
// LAB 1 - OOP si principii SOLID
// =============================================
Console.WriteLine("╔══════════════════════════════════════════╗");
Console.WriteLine("║   Car Rental System - Laborator 1 & 2   ║");
Console.WriteLine("╚══════════════════════════════════════════╝\n");

Console.WriteLine("════════════ LAB 1: OOP & SOLID ════════════\n");

// Creare clienti
var customer1 = new Customer("C001", "Ion", "Popescu",
                              "ion@mail.com", "+37360000001",
                              "MD123456", new DateTime(1990, 5, 15));

var customer2 = new Customer("C002", "Maria", "Ionescu",
                              "maria@mail.com", "+37360000002",
                              "MD654321", new DateTime(1995, 3, 20));

var employee = new Employee("E001", "Andrei", "Rusu",
                             "andrei@carrental.com", "+37360000003",
                             "Manager", 15000m);

Console.WriteLine("--- Persoane inregistrate ---");
Console.WriteLine(customer1);
Console.WriteLine(customer2);
Console.WriteLine(employee);

// Creare vehicule direct (Lab 1 - fara Factory)
var car1 = new Car("V001", "ABC123", "Toyota", "Corolla", 2022,
                   FuelType.Petrol, 200m, 4, "Automatic", true);

var truck1 = new Truck("V002", "TRK456", "Volvo", "FH16", 2020,
                       FuelType.Diesel, 500m, 10.5, true);

var moto1 = new Motorcycle("V003", "MTO789", "Honda", "CBR600", 2023,
                            FuelType.Petrol, 150m, "Sport", 600);

Console.WriteLine("\n--- Vehicule disponibile ---");
Console.WriteLine(car1);
Console.WriteLine(truck1);
Console.WriteLine(moto1);

// Polimorfism - CalculateRentalCost se comporta diferit per tip
Console.WriteLine("\n--- Demonstrare Polimorfism (cost 5 zile) ---");
Vehicle[] vehicles = { car1, truck1, moto1 };
foreach (var v in vehicles)
    Console.WriteLine($"  {v.Brand} {v.Model} [{v.GetVehicleType()}]: {v.CalculateRentalCost(5)} MDL");

// Servicii (SRP + DIP)
var notificationService = new NotificationService();
var rentalService       = new RentalService(notificationService);
var paymentService      = new PaymentService();

// Contract si plata
Console.WriteLine("\n--- Creare Contract & Plata ---");
var contract1 = rentalService.CreateContract(car1, customer1,
                DateTime.Now, DateTime.Now.AddDays(5));
Console.WriteLine(contract1.GenerateReport());

var payment1 = paymentService.ProcessPayment(contract1.ContractId,
               contract1.TotalCost, PaymentMethod.Card);
Console.WriteLine($"Plata finalizata: {payment1.Status}\n");

// =============================================
// LAB 2 - FACTORY METHOD
// =============================================
Console.WriteLine("════════════ LAB 2: FACTORY METHOD ════════════\n");

// Fabricile concrete - nu mai folosim "new Car(...)" direct
VehicleFactory carFactory        = new CarFactory(4, "Automatic", true);
VehicleFactory truckFactory      = new TruckFactory(10.5, true);
VehicleFactory motorcycleFactory = new MotorcycleFactory("Sport", 600);

Console.WriteLine("--- Vehicule create prin Factory Method ---");
var car2   = carFactory.OrderVehicle("V004", "FAB001", "Ford", "Focus", 2022, FuelType.Petrol, 180m);
var truck2 = truckFactory.OrderVehicle("V005", "FAB002", "MAN", "TGX", 2021, FuelType.Diesel, 550m);
var moto2  = motorcycleFactory.OrderVehicle("V006", "FAB003", "Yamaha", "R6", 2023, FuelType.Petrol, 160m);

Console.WriteLine();
Console.WriteLine(car2);
Console.WriteLine(truck2);
Console.WriteLine(moto2);

Console.WriteLine("\n--- Cost inchiriere 7 zile (Factory Method) ---");
Vehicle[] factoryVehicles = { car2, truck2, moto2 };
foreach (var v in factoryVehicles)
    Console.WriteLine($"  {v.Brand} {v.Model}: {v.CalculateRentalCost(7)} MDL");

// Contract pentru vehicul creat prin factory
Console.WriteLine("\n--- Contract pentru vehicul creat prin Factory ---");
var contract2 = rentalService.CreateContract(car2, customer2,
                DateTime.Now, DateTime.Now.AddDays(7));
Console.WriteLine(contract2.GenerateReport());

var payment2 = paymentService.ProcessPayment(contract2.ContractId,
               contract2.TotalCost, PaymentMethod.BankTransfer);
Console.WriteLine($"Plata finalizata: {payment2.Status}\n");

// =============================================
// LAB 2 - ABSTRACT FACTORY
// =============================================
Console.WriteLine("════════════ LAB 2: ABSTRACT FACTORY ════════════\n");

static void ProcessRentalPackage(IRentalPackageFactory factory, Customer c)
{
    var vehicle  = factory.CreateVehicle();
    var contract = factory.CreateContract(c.Id, vehicle.Id);
    var payment  = factory.CreatePayment(contract.ContractId, contract.TotalCost);

    payment.ProcessPayment(contract.TotalCost);

    Console.WriteLine($"  Vehicul:  {vehicle.Brand} {vehicle.Model} [{vehicle.GetVehicleType()}]");
    Console.WriteLine($"  Contract: {contract.RentalDays} zile | Total: {contract.TotalCost} MDL");
    Console.WriteLine($"  Plata:    {payment.Method} | Status: {payment.Status}");
    Console.WriteLine();
}

Console.WriteLine("--- Pachet STANDARD ---");
ProcessRentalPackage(new StandardPackageFactory(), customer1);

Console.WriteLine("--- Pachet PREMIUM ---");
ProcessRentalPackage(new PremiumPackageFactory(), customer2);

Console.WriteLine("--- Pachet BUSINESS ---");
ProcessRentalPackage(new BusinessPackageFactory(), customer1);

// Sumar final
Console.WriteLine("════════════ SUMAR FINAL ════════════\n");
Console.WriteLine($"Total contracte incheiate: {rentalService.GetAllContracts().Count}");
Console.WriteLine($"Total plati procesate:     {paymentService.GetAllPayments().Count}");
Console.WriteLine("\nContracte:");
foreach (var c in rentalService.GetAllContracts())
    Console.WriteLine($"  {c.GenerateReport()}");