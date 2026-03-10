using CarRentalSystem.Models;
using CarRentalSystem.Services;

// DIP: injectam NotificationService in RentalService
var notificationService = new NotificationService();
var rentalService = new RentalService(notificationService);
var paymentService = new PaymentService();

// Creare vehicule
var car = new Car("V001", "ABC123", "Toyota", "Corolla", 2022,
                  FuelType.Petrol, 200m, 4, "Automatic", true);
var truck = new Truck("V002", "TRK456", "Volvo", "FH16", 2020,
                      FuelType.Diesel, 500m, 10.5, true);

// Creare client
var customer = new Customer("C001", "Ion", "Popescu",
                             "ion@mail.com", "+37360000001",
                             "MD123456", new DateTime(1990, 5, 15));

Console.WriteLine("=== Car Rental System - Lab 1 ===\n");
Console.WriteLine(car);
Console.WriteLine(truck);
Console.WriteLine(customer);

// Demonstrare polimorfism
Console.WriteLine($"\nCost inchiriere Toyota (5 zile): {car.CalculateRentalCost(5)} MDL");
Console.WriteLine($"Cost inchiriere Volvo (5 zile): {truck.CalculateRentalCost(5)} MDL");

// Creare contract
var contract = rentalService.CreateContract(car, customer,
               DateTime.Now, DateTime.Now.AddDays(5));

Console.WriteLine($"\n{contract.GenerateReport()}");

// Procesare plata
var payment = paymentService.ProcessPayment(contract.ContractId,
              contract.TotalCost, PaymentMethod.Card);

Console.WriteLine($"\nPlata finalizata: {payment.Status}");