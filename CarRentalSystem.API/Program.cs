using CarRentalSystem.API.Data;
using CarRentalSystem.Models;
using CarRentalSystem.Factories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurare SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=carrental.db"));

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors();
app.UseAuthorization();
app.MapControllers();
app.UseDefaultFiles();
app.UseStaticFiles();

// Creare automata baza de date si populare date initiale
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    // Adauga date initiale doar daca baza e goala
    if (!db.Vehicles.Any())
    {
        SeedData(db);
    }
}

app.Run();

void SeedData(AppDbContext db)
{
    db.Vehicles.AddRange(
        new VehicleEntity
        {
            Id = "V001", LicensePlate = "ABC123", Brand = "Toyota", Model = "Corolla",
            Year = 2022, FuelType = "Petrol", VehicleType = "Car", DailyRate = 200m,
            Status = "Available", Doors = 4, Transmission = "Automatic", HasAC = true
        },
        new VehicleEntity
        {
            Id = "V002", LicensePlate = "XYZ456", Brand = "Ford", Model = "Focus",
            Year = 2021, FuelType = "Petrol", VehicleType = "Car", DailyRate = 180m,
            Status = "Available", Doors = 4, Transmission = "Manual", HasAC = true
        },
        new VehicleEntity
        {
            Id = "V003", LicensePlate = "TRK001", Brand = "Volvo", Model = "FH16",
            Year = 2020, FuelType = "Diesel", VehicleType = "Truck", DailyRate = 500m,
            Status = "Available", CargoCapacity = 10.5, RequiresSpecialLicense = true
        },
        new VehicleEntity
        {
            Id = "V004", LicensePlate = "MTO001", Brand = "Honda", Model = "CBR600",
            Year = 2023, FuelType = "Petrol", VehicleType = "Motorcycle", DailyRate = 150m,
            Status = "Available", MotorcycleType = "Sport", EngineCC = 600
        }
    );

    db.Customers.AddRange(
        new CustomerEntity
        {
            Id = "C001", FirstName = "Ion", LastName = "Popescu",
            Email = "ion@mail.com", Phone = "+37360000001",
            DriverLicenseNumber = "MD123456",
            DateOfBirth = new DateTime(1990, 5, 15)
        },
        new CustomerEntity
        {
            Id = "C002", FirstName = "Maria", LastName = "Ionescu",
            Email = "maria@mail.com", Phone = "+37360000002",
            DriverLicenseNumber = "MD654321",
            DateOfBirth = new DateTime(1995, 3, 20)
        }
    );

    db.SaveChanges();
    Console.WriteLine("[DB] Database seeded successfully.");
}