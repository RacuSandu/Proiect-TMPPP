using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarRentalSystem.API.Data;

namespace CarRentalSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiclesController : ControllerBase
    {
        private readonly AppDbContext _db;

        public VehiclesController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var vehicles = await _db.Vehicles.ToListAsync();
            return Ok(vehicles.Select(v => new
            {
                v.Id, v.Brand, v.Model, v.Year, v.LicensePlate,
                v.DailyRate, v.VehicleType, v.Status,
                IsAvailable = v.Status == "Available"
            }));
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailable()
        {
            var vehicles = await _db.Vehicles
                .Where(v => v.Status == "Available")
                .ToListAsync();
            return Ok(vehicles.Select(v => new
            {
                v.Id, v.Brand, v.Model, v.Year,
                v.LicensePlate, v.DailyRate,
                Type = v.VehicleType
            }));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var v = await _db.Vehicles.FindAsync(id);
            if (v == null) return NotFound($"Vehicle {id} not found.");
            return Ok(new
            {
                v.Id, v.Brand, v.Model, v.Year, v.LicensePlate,
                v.DailyRate, Type = v.VehicleType, v.Status,
                IsAvailable = v.Status == "Available"
            });
        }

        [HttpPost("car")]
        public async Task<IActionResult> AddCar([FromBody] AddCarRequest request)
        {
            try
            {
                var vehicle = new VehicleEntity
                {
                    Id           = Guid.NewGuid().ToString(),
                    LicensePlate = request.LicensePlate,
                    Brand        = request.Brand,
                    Model        = request.Model,
                    Year         = request.Year,
                    FuelType     = request.FuelType,
                    VehicleType  = "Car",
                    DailyRate    = request.DailyRate,
                    Status       = "Available",
                    Doors        = request.Doors,
                    Transmission = request.Transmission,
                    HasAC        = request.HasAC
                };
                _db.Vehicles.Add(vehicle);
                await _db.SaveChangesAsync();
                return Ok(new { message = "Car added.", vehicle.Id });
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPost("truck")]
        public async Task<IActionResult> AddTruck([FromBody] AddTruckRequest request)
        {
            try
            {
                var vehicle = new VehicleEntity
                {
                    Id                     = Guid.NewGuid().ToString(),
                    LicensePlate           = request.LicensePlate,
                    Brand                  = request.Brand,
                    Model                  = request.Model,
                    Year                   = request.Year,
                    FuelType               = request.FuelType,
                    VehicleType            = "Truck",
                    DailyRate              = request.DailyRate,
                    Status                 = "Available",
                    CargoCapacity          = request.CargoCapacity,
                    RequiresSpecialLicense = request.RequiresSpecialLicense
                };
                _db.Vehicles.Add(vehicle);
                await _db.SaveChangesAsync();
                return Ok(new { message = "Truck added.", vehicle.Id });
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPost("motorcycle")]
        public async Task<IActionResult> AddMotorcycle([FromBody] AddMotorcycleRequest request)
        {
            try
            {
                var vehicle = new VehicleEntity
                {
                    Id             = Guid.NewGuid().ToString(),
                    LicensePlate   = request.LicensePlate,
                    Brand          = request.Brand,
                    Model          = request.Model,
                    Year           = request.Year,
                    FuelType       = request.FuelType,
                    VehicleType    = "Motorcycle",
                    DailyRate      = request.DailyRate,
                    Status         = "Available",
                    MotorcycleType = request.MotorcycleType,
                    EngineCC       = request.EngineCC
                };
                _db.Vehicles.Add(vehicle);
                await _db.SaveChangesAsync();
                return Ok(new { message = "Motorcycle added.", vehicle.Id });
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var vehicle = await _db.Vehicles.FindAsync(id);
            if (vehicle == null) return NotFound($"Vehicle {id} not found.");
            if (vehicle.Status == "Rented")
                return BadRequest("Cannot delete a rented vehicle.");
            _db.Vehicles.Remove(vehicle);
            await _db.SaveChangesAsync();
            return Ok(new { message = $"Vehicle {id} deleted." });
        }
    }

    public record AddCarRequest(string Brand, string Model, int Year, string LicensePlate,
                                 string FuelType, decimal DailyRate, int Doors,
                                 string Transmission, bool HasAC);
    public record AddTruckRequest(string Brand, string Model, int Year, string LicensePlate,
                                   string FuelType, decimal DailyRate,
                                   double CargoCapacity, bool RequiresSpecialLicense);
    public record AddMotorcycleRequest(string Brand, string Model, int Year, string LicensePlate,
                                        string FuelType, decimal DailyRate,
                                        string MotorcycleType, int EngineCC);
}