using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarRentalSystem.API.Data;

namespace CarRentalSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContractsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ContractsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var contracts = await _db.Contracts.ToListAsync();
            return Ok(contracts.Select(c => new
            {
                c.ContractId, c.CustomerId, c.VehicleId,
                c.StartDate, c.EndDate,
                RentalDays = (c.EndDate - c.StartDate).Days,
                c.TotalCost, c.Status
            }));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var c = await _db.Contracts.FindAsync(id);
            if (c == null) return NotFound($"Contract {id} not found.");
            return Ok(new
            {
                c.ContractId, c.CustomerId, c.VehicleId,
                c.StartDate, c.EndDate,
                RentalDays = (c.EndDate - c.StartDate).Days,
                c.TotalCost, c.Status
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateContractRequest request)
        {
            try
            {
                var vehicle = await _db.Vehicles.FindAsync(request.VehicleId);
                if (vehicle == null) return NotFound($"Vehicle {request.VehicleId} not found.");
                if (vehicle.Status != "Available") return BadRequest("Vehicle is not available.");

                var customer = await _db.Customers.FindAsync(request.CustomerId);
                if (customer == null) return NotFound($"Customer {request.CustomerId} not found.");

                decimal totalCost = vehicle.DailyRate * request.Days;

                var contract = new ContractEntity
                {
                    ContractId = Guid.NewGuid().ToString(),
                    CustomerId = request.CustomerId,
                    VehicleId  = request.VehicleId,
                    StartDate  = request.StartDate,
                    EndDate    = request.EndDate,
                    TotalCost  = totalCost,
                    Status     = "Active"
                };

                vehicle.Status = "Rented";
                customer.RentalCount++;

                _db.Contracts.Add(contract);
                await _db.SaveChangesAsync();

                return Ok(new
                {
                    message    = "Contract created.",
                    contract.ContractId,
                    contract.TotalCost
                });
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> Cancel(string id)
        {
            var contract = await _db.Contracts.FindAsync(id);
            if (contract == null) return NotFound($"Contract {id} not found.");
            if (contract.Status == "Cancelled")
                return BadRequest("Contract already cancelled.");

            var vehicle = await _db.Vehicles.FindAsync(contract.VehicleId);
            if (vehicle != null) vehicle.Status = "Available";

            contract.Status = "Cancelled";
            await _db.SaveChangesAsync();

            return Ok(new { message = $"Contract {id} cancelled." });
        }
    }

    public record CreateContractRequest(string CustomerId, string VehicleId,
                                         DateTime StartDate, DateTime EndDate, int Days);
}