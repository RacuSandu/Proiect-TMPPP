using CarRentalSystem.Models;
using CarRentalSystem.Services;
using CarRentalSystem.Factories;
using CarRentalSystem.Builders;
using CarRentalSystem.Prototypes;
using CarRentalSystem.Singleton;
using CarRentalSystem.Composite;
using CarRentalSystem.Adapters;
using CarRentalSystem.Facade;
using CarRentalSystem.Decorators;
using CarRentalSystem.Strategy;
using CarRentalSystem.Observer;
using CarRentalSystem.Command;
using CarRentalSystem.State;

// OOP si principii SOLID
Console.WriteLine("╔══════════════════════════════════════════════╗");
Console.WriteLine("║                Car Rental System             ║");
Console.WriteLine("╚══════════════════════════════════════════════╝\n");

Console.WriteLine("════════════ OOP & SOLID ════════════\n");

var customer1 = new Customer("C001", "Ion", "Popescu",
                              "ion@mail.com", "+37360000001",
                              "MD123456", new DateTime(1990, 5, 15));
var customer2 = new Customer("C002", "Maria", "Ionescu",
                              "maria@mail.com", "+37360000002",
                              "MD654321", new DateTime(1995, 3, 20));
var customer3 = new Customer("C003", "Alexandru", "Marin",
                              "alex@mail.com", "+37360000004",
                              "MD111222", new DateTime(1988, 7, 10));
var employee  = new Employee("E001", "Andrei", "Rusu",
                              "andrei@carrental.com", "+37360000003",
                              "Manager", 15000m);

Console.WriteLine("--- Persoane inregistrate ---");
Console.WriteLine(customer1);
Console.WriteLine(customer2);
Console.WriteLine(employee);

var car1  = new Car("V001", "ABC123", "Toyota", "Corolla", 2022,
                    FuelType.Petrol, 200m, 4, "Automatic", true);
var truck1 = new Truck("V002", "TRK456", "Volvo", "FH16", 2020,
                       FuelType.Diesel, 500m, 10.5, true);
var moto1  = new Motorcycle("V003", "MTO789", "Honda", "CBR600", 2023,
                             FuelType.Petrol, 150m, "Sport", 600);

Console.WriteLine("\n--- Vehicule disponibile ---");
Console.WriteLine(car1);
Console.WriteLine(truck1);
Console.WriteLine(moto1);

Console.WriteLine("\n--- Polimorfism (cost 5 zile) ---");
foreach (var v in new Vehicle[] { car1, truck1, moto1 })
    Console.WriteLine($"  {v.Brand} {v.Model}: {v.CalculateRentalCost(5)} MDL");

var notificationService = new NotificationService();
var rentalService       = new RentalService(notificationService);
var paymentService      = new PaymentService();

var contract1 = rentalService.CreateContract(car1, customer1,
                DateTime.Now, DateTime.Now.AddDays(5));
Console.WriteLine(contract1.GenerateReport());
var payment1 = paymentService.ProcessPayment(contract1.ContractId,
               contract1.TotalCost, PaymentMethod.Card);
Console.WriteLine($"Plata: {payment1.Status}\n");

// FACTORY METHOD
Console.WriteLine("════════════ FACTORY METHOD ════════════\n");

VehicleFactory carFactory        = new CarFactory(4, "Automatic", true);
VehicleFactory truckFactory      = new TruckFactory(10.5, true);
VehicleFactory motorcycleFactory = new MotorcycleFactory("Sport", 600);

var car2   = carFactory.OrderVehicle("V004", "FAB001", "Ford", "Focus", 2022, FuelType.Petrol, 180m);
var truck2 = truckFactory.OrderVehicle("V005", "FAB002", "MAN", "TGX", 2021, FuelType.Diesel, 550m);
var moto2  = motorcycleFactory.OrderVehicle("V006", "FAB003", "Yamaha", "R6", 2023, FuelType.Petrol, 160m);

Console.WriteLine(car2);
Console.WriteLine(truck2);
Console.WriteLine(moto2);

var contract2 = rentalService.CreateContract(car2, customer2,
                DateTime.Now, DateTime.Now.AddDays(7));
var payment2  = paymentService.ProcessPayment(contract2.ContractId,
                contract2.TotalCost, PaymentMethod.BankTransfer);
Console.WriteLine($"Plata: {payment2.Status}\n");

// ABSTRACT FACTORY
Console.WriteLine("════════════ ABSTRACT FACTORY ════════════\n");

static void ProcessRentalPackage(IRentalPackageFactory factory, Customer c)
{
    var vehicle  = factory.CreateVehicle();
    var contract = factory.CreateContract(c.Id, vehicle.Id);
    var payment  = factory.CreatePayment(contract.ContractId, contract.TotalCost);
    payment.ProcessPayment(contract.TotalCost);
    Console.WriteLine($"  {vehicle.Brand} {vehicle.Model} | {contract.RentalDays} zile | {contract.TotalCost} MDL | {payment.Method}\n");
}

Console.WriteLine("--- Pachet STANDARD ---");
ProcessRentalPackage(new StandardPackageFactory(), customer1);
Console.WriteLine("--- Pachet PREMIUM ---");
ProcessRentalPackage(new PremiumPackageFactory(), customer2);
Console.WriteLine("--- Pachet BUSINESS ---");
ProcessRentalPackage(new BusinessPackageFactory(), customer1);

// BUILDER
Console.WriteLine("════════════ BUILDER ════════════\n");

var builder  = new RentalContractBuilder();
var director = new RentalContractDirector(builder);

var stdContract = director.BuildStandardContract("C001", "V001", 200m);
Console.WriteLine($"[Standard]  {stdContract.GenerateReport()}");
builder.Reset();
var prmContract = director.BuildPremiumContract("C002", "V004", 180m);
Console.WriteLine($"[Premium]   {prmContract.GenerateReport()}");
builder.Reset();
var bizContract = director.BuildBusinessContract("C001", "V005", 550m);
Console.WriteLine($"[Business]  {bizContract.GenerateReport()}");

var customContract = new RentalContractBuilder()
    .SetContractId("CUSTOM-001")
    .SetCustomer("C002")
    .SetVehicle("V006")
    .SetRentalPeriod(DateTime.Now, DateTime.Now.AddDays(10))
    .SetTotalCost(1600m)
    .SetStatus(ContractStatus.Active)
    .Build();
Console.WriteLine($"[Custom]    {customContract.GenerateReport()}");

// PROTOTYPE
Console.WriteLine("\n════════════ PROTOTYPE ════════════\n");

var registry = new PrototypeRegistry();
registry.Register("standard-car",   new VehiclePrototype(car1));
registry.Register("standard-truck", new VehiclePrototype(truck1));

var clonedCar1 = registry.GetClone("standard-car");
clonedCar1.Id = "V007"; clonedCar1.LicensePlate = "CLN001"; clonedCar1.DailyRate = 220m;
var vehicle7 = clonedCar1.ToVehicle(clonedCar1.Id, clonedCar1.LicensePlate);

var clonedCar2 = registry.GetClone("standard-car");
clonedCar2.Id = "V008"; clonedCar2.LicensePlate = "CLN002"; clonedCar2.Year = 2023;
var vehicle8 = clonedCar2.ToVehicle(clonedCar2.Id, clonedCar2.LicensePlate);

Console.WriteLine($"Original: {car1}");
Console.WriteLine($"Clona 1:  {vehicle7}");
Console.WriteLine($"Clona 2:  {vehicle8}");

// SINGLETON
Console.WriteLine("\n════════════ SINGLETON ════════════\n");

var fleet  = FleetManager.Instance;
var fleet2 = FleetManager.Instance;
Console.WriteLine($"Aceeasi instanta FleetManager:  {object.ReferenceEquals(fleet, fleet2)}");

fleet.AddVehicle(car1);
fleet.AddVehicle(truck1);
fleet.AddVehicle(moto1);
fleet.AddVehicle(car2);
fleet.AddVehicle(vehicle7);
fleet.AddVehicle(vehicle8);
fleet.PrintFleetStatus();

var db = RentalDatabase.Instance;
db.SaveContract(stdContract);
db.SaveContract(prmContract);
db.SaveContract(bizContract);
db.SaveContract(customContract);
db.SavePayment(payment1);
db.SavePayment(payment2);
db.PrintDatabaseStatus();

// ADAPTER
Console.WriteLine("\n════════════ ADAPTER ════════════\n");

var payPalAdapter = new PayPalPaymentAdapter(new PayPalPaymentService());
if (payPalAdapter.ValidatePayment()) payPalAdapter.ProcessPayment(1050m);
Console.WriteLine(payPalAdapter.GetStatus());

var stripeAdapter = new StripePaymentAdapter(new StripePaymentService());
if (stripeAdapter.ValidatePayment()) stripeAdapter.ProcessPayment(2870m);
Console.WriteLine(stripeAdapter.GetStatus());

// COMPOSITE
Console.WriteLine("\n════════════ COMPOSITE ════════════\n");

var carsGroup   = new VehicleGroup("Cars");
var trucksGroup = new VehicleGroup("Trucks");
var motoGroup   = new VehicleGroup("Motorcycles");

carsGroup.Add(new VehicleLeaf(car1));
carsGroup.Add(new VehicleLeaf(car2));
carsGroup.Add(new VehicleLeaf(vehicle7));
carsGroup.Add(new VehicleLeaf(vehicle8));
trucksGroup.Add(new VehicleLeaf(truck1));
trucksGroup.Add(new VehicleLeaf(truck2));
motoGroup.Add(new VehicleLeaf(moto1));
motoGroup.Add(new VehicleLeaf(moto2));

var totalFleet = new VehicleGroup("Total Fleet");
totalFleet.Add(carsGroup);
totalFleet.Add(trucksGroup);
totalFleet.Add(motoGroup);
totalFleet.Display();
Console.WriteLine($"Total: {totalFleet.GetVehicleCount()} vehicule | {totalFleet.GetTotalDailyRate()} MDL/zi");

// FACADE
Console.WriteLine("\n════════════ FACADE ════════════\n");

var facade       = new RentalFacade();
var facadeCar1   = facade.AddCarToFleet("V009", "FAC001", "Skoda", "Octavia", 2023, 250m, true);
var facadeTruck1 = facade.AddTruckToFleet("V010", "FAC002", "DAF", "XF", 2022, 600m, 8.0);

var (rentalContract, rentalPayment) = facade.RentVehicle(facadeCar1, customer3, 5, PaymentMethod.Card);
Console.WriteLine($"Contract: {rentalContract.GenerateReport()}");
Console.WriteLine($"Plata:    {rentalPayment.Status}");
facade.RentWithExternalPayment(facadeTruck1, customer3, 3, "stripe");
facade.DisplayOrganizedFleet();

// LAB 5 - DECORATOR
Console.WriteLine("\n════════════ DECORATOR ════════════\n");

int rentalDays = 5;
IRentalDecorator baseRental = new BaseVehicleRental(car1);
baseRental.DisplayDetails(rentalDays);
Console.WriteLine($"  TOTAL: {baseRental.GetTotalCost(rentalDays)} MDL\n");

IRentalDecorator withInsurance = new InsuranceDecorator(new BaseVehicleRental(car1), "Premium", 75m);
withInsurance.DisplayDetails(rentalDays);
Console.WriteLine($"  TOTAL: {withInsurance.GetTotalCost(rentalDays)} MDL\n");

IRentalDecorator fullPackage = new ChildSeatDecorator(
    new GPSDecorator(
        new InsuranceDecorator(new BaseVehicleRental(car1), "Premium", 75m)));
fullPackage.DisplayDetails(rentalDays);
Console.WriteLine($"  TOTAL: {fullPackage.GetTotalCost(rentalDays)} MDL");
Console.WriteLine($"  Descriere: {fullPackage.GetDescription()}");

// STRATEGY
Console.WriteLine("\n════════════ STRATEGY ════════════\n");

var pricingContext = new PricingContext(new StandardPricingStrategy());
pricingContext.PrintPriceBreakdown(car1, 5);
pricingContext.SetStrategy(new SeasonalPricingStrategy("Summer", 1.3m));
pricingContext.PrintPriceBreakdown(car1, 5);
pricingContext.SetStrategy(new DiscountPricingStrategy(15m));
pricingContext.PrintPriceBreakdown(car1, 5);
pricingContext.SetStrategy(new LoyaltyPricingStrategy(12));
pricingContext.PrintPriceBreakdown(car1, 5);

// OBSERVER
Console.WriteLine("\n════════════ OBSERVER ════════════\n");

var vehicleSubject = new VehicleStatusSubject();
var emailObserver  = new EmailNotificationObserver("admin@carrental.com");
var smsObserver    = new SmsNotificationObserver("+37360000000");
var auditObserver  = new AuditLogObserver();

vehicleSubject.Attach(emailObserver);
vehicleSubject.Attach(smsObserver);
vehicleSubject.Attach(auditObserver);

var observerCar = new Car("V011", "OBS001", "Renault", "Megane", 2022,
                           FuelType.Petrol, 180m, 4, "Manual", false);
vehicleSubject.VehicleAddedToFleet(observerCar);
vehicleSubject.VehicleRented(observerCar, customer1);
vehicleSubject.VehicleReturned(observerCar, customer1);
vehicleSubject.Detach(smsObserver);
vehicleSubject.VehicleRented(observerCar, customer2);
auditObserver.PrintFullLog();

// COMMAND
Console.WriteLine("\n════════════ COMMAND ════════════\n");

var invoker = new CommandInvoker();
var cmdCar  = new Car("V012", "CMD001", "Peugeot", "308", 2021,
                       FuelType.Petrol, 170m, 4, "Automatic", true);

invoker.ExecuteCommand(new RentVehicleCommand(cmdCar, customer1));
invoker.Undo();
invoker.Redo();

var cancelContract = new RentalContract(
    Guid.NewGuid().ToString(), customer1.Id, cmdCar.Id,
    DateTime.Now, DateTime.Now.AddDays(3), 510m);
invoker.ExecuteCommand(new CancelContractCommand(cancelContract, cmdCar));
invoker.Undo();
invoker.PrintHistory();

// STATE
Console.WriteLine("\n════════════ STATE ════════════\n");

var vehicleContext = new VehicleContext("Toyota Corolla V001");
vehicleContext.PrintStatus();
vehicleContext.Rent("C001");
vehicleContext.PrintStatus();
vehicleContext.Rent("C002");
vehicleContext.Return();
vehicleContext.PrintStatus();
vehicleContext.SendToMaintenance();
vehicleContext.PrintStatus();
vehicleContext.Rent("C001");
vehicleContext.FinishMaintenance();
vehicleContext.PrintStatus();

// SUMAR FINAL
Console.WriteLine("\n════════════ SUMAR FINAL ════════════");
Console.WriteLine($"Vehicule in flota:         {FleetManager.Instance.TotalVehicles}");
Console.WriteLine($"Vehicule disponibile:      {FleetManager.Instance.AvailableCount}");
Console.WriteLine($"Contracte in baza de date: {RentalDatabase.Instance.GetAllContracts().Count()}");
Console.WriteLine($"Plati in baza de date:     {RentalDatabase.Instance.GetAllPayments().Count()}");
Console.WriteLine($"Audit log entries:         {auditObserver.LogCount}");