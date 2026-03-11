using CarRentalSystem.Models;
using CarRentalSystem.Services;
using CarRentalSystem.Factories;
using CarRentalSystem.Builders;
using CarRentalSystem.Prototypes;
using CarRentalSystem.Singleton;

// =============================================
// LAB 1 - OOP si principii SOLID
// =============================================
Console.WriteLine("      Car Rental System - Lab 1, 2, 3    ");

Console.WriteLine(" LAB 1: OOP & SOLID \n");

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

Console.WriteLine("\n--- Demonstrare Polimorfism (cost 5 zile) ---");
Vehicle[] vehicles = { car1, truck1, moto1 };
foreach (var v in vehicles)
    Console.WriteLine($"  {v.Brand} {v.Model} [{v.GetVehicleType()}]: {v.CalculateRentalCost(5)} MDL");

var notificationService = new NotificationService();
var rentalService       = new RentalService(notificationService);
var paymentService      = new PaymentService();

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
Console.WriteLine(" LAB 2: FACTORY METHOD \n");

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
foreach (var v in new Vehicle[] { car2, truck2, moto2 })
    Console.WriteLine($"  {v.Brand} {v.Model}: {v.CalculateRentalCost(7)} MDL");

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
Console.WriteLine(" LAB 2: ABSTRACT FACTORY \n");

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

// =============================================
// LAB 3 - BUILDER
// =============================================
Console.WriteLine(" LAB 3: BUILDER \n");

var builder  = new RentalContractBuilder();
var director = new RentalContractDirector(builder);

Console.WriteLine("--- Contracte create prin Director ---");
var stdContract = director.BuildStandardContract("C001", "V001", 200m);
Console.WriteLine($"[Standard]  {stdContract.GenerateReport()}");

builder.Reset();
var prmContract = director.BuildPremiumContract("C002", "V004", 180m);
Console.WriteLine($"[Premium]   {prmContract.GenerateReport()}");

builder.Reset();
var bizContract = director.BuildBusinessContract("C001", "V005", 550m);
Console.WriteLine($"[Business]  {bizContract.GenerateReport()}");

// Builder folosit direct cu metode fluente
Console.WriteLine("\n--- Contract creat manual cu Builder (metode fluente) ---");
var customContract = new RentalContractBuilder()
    .SetContractId("CUSTOM-001")
    .SetCustomer("C002")
    .SetVehicle("V006")
    .SetRentalPeriod(DateTime.Now, DateTime.Now.AddDays(10))
    .SetTotalCost(1600m)
    .SetStatus(ContractStatus.Active)
    .Build();
Console.WriteLine(customContract.GenerateReport());

// =============================================
// LAB 3 - PROTOTYPE
// =============================================
Console.WriteLine("\n LAB 3: PROTOTYPE \n");

// Cream prototipuri din vehiculele existente
var carPrototype  = new VehiclePrototype(car1);
var truckPrototype = new VehiclePrototype(truck1);

// Inregistram prototipurile in registru
var registry = new PrototypeRegistry();
registry.Register("standard-car",   carPrototype);
registry.Register("standard-truck", truckPrototype);

Console.WriteLine("\n--- Clonare vehicule din Prototype Registry ---");

// Clonam si modificam - nu cream de la zero
var clonedCar1 = registry.GetClone("standard-car");
clonedCar1.Id           = "V007";
clonedCar1.LicensePlate = "CLN001";
clonedCar1.DailyRate    = 220m; // modificam doar pretul
var vehicle7 = clonedCar1.ToVehicle(clonedCar1.Id, clonedCar1.LicensePlate);

var clonedCar2 = registry.GetClone("standard-car");
clonedCar2.Id           = "V008";
clonedCar2.LicensePlate = "CLN002";
clonedCar2.Year         = 2023; // modificam doar anul
var vehicle8 = clonedCar2.ToVehicle(clonedCar2.Id, clonedCar2.LicensePlate);

Console.WriteLine($"\nOriginal:  {car1}");
Console.WriteLine($"Clona 1:   {vehicle7}");
Console.WriteLine($"Clona 2:   {vehicle8}");

// Demonstrare Shallow vs Deep Copy
Console.WriteLine("\n--- Shallow Copy vs Deep Copy ---");
var shallow = carPrototype.ShallowCopy();
var deep    = carPrototype.DeepCopy();
shallow.Brand = "SHALLOW_MODIFIED";
deep.Brand    = "DEEP_MODIFIED";
Console.WriteLine($"Original dupa modificari: {carPrototype.Brand}");
Console.WriteLine($"Shallow copy brand:       {shallow.Brand}");
Console.WriteLine($"Deep copy brand:          {deep.Brand}");

// =============================================
// LAB 3 - SINGLETON
// =============================================
Console.WriteLine("\n LAB 3: SINGLETON \n");

// FleetManager - o singura instanta globala
var fleet  = FleetManager.Instance;
var fleet2 = FleetManager.Instance; // aceeasi instanta!
Console.WriteLine($"Aceeasi instanta FleetManager: {object.ReferenceEquals(fleet, fleet2)}");

// Adaugam vehiculele in flota
fleet.AddVehicle(car1);
fleet.AddVehicle(truck1);
fleet.AddVehicle(moto1);
fleet.AddVehicle(car2);
fleet.AddVehicle(vehicle7);
fleet.AddVehicle(vehicle8);

fleet.PrintFleetStatus();

// RentalDatabase Singleton
var db  = RentalDatabase.Instance;
var db2 = RentalDatabase.Instance;
Console.WriteLine($"\nAceeasi instanta RentalDatabase: {object.ReferenceEquals(db, db2)}");

db.SaveContract(stdContract);
db.SaveContract(prmContract);
db.SaveContract(bizContract);
db.SaveContract(customContract);
db.SavePayment(payment1);
db.SavePayment(payment2);

db.PrintDatabaseStatus();

// Sumar final
Console.WriteLine("\n SUMAR FINAL ");
Console.WriteLine($"Vehicule in flota:         {fleet.TotalVehicles}");
Console.WriteLine($"Vehicule disponibile:      {fleet.AvailableCount}");
Console.WriteLine($"Contracte in baza de date: {db.GetAllContracts().Count()}");
Console.WriteLine($"Plati in baza de date:     {db.GetAllPayments().Count()}");