using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarRentalSystem.API.Data;

namespace CarRentalSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly AppDbContext _db;

        public CustomersController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var customers = await _db.Customers.ToListAsync();
            return Ok(customers.Select(c => new
            {
                c.Id, c.FirstName, c.LastName,
                FullName = $"{c.FirstName} {c.LastName}",
                c.Email, c.Phone, c.DriverLicenseNumber,
                RentalCount = c.RentalCount
            }));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var c = await _db.Customers.FindAsync(id);
            if (c == null) return NotFound($"Customer {id} not found.");
            return Ok(new
            {
                c.Id, c.FirstName, c.LastName,
                FullName = $"{c.FirstName} {c.LastName}",
                c.Email, c.Phone, c.DriverLicenseNumber, c.RentalCount
            });
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddCustomerRequest request)
        {
            try
            {
                var customer = new CustomerEntity
                {
                    Id                  = Guid.NewGuid().ToString(),
                    FirstName           = request.FirstName,
                    LastName            = request.LastName,
                    Email               = request.Email,
                    Phone               = request.Phone,
                    DriverLicenseNumber = request.DriverLicenseNumber,
                    DateOfBirth         = request.DateOfBirth,
                    RentalCount         = 0
                };
                _db.Customers.Add(customer);
                await _db.SaveChangesAsync();
                return Ok(new { message = "Customer added.", customer.Id });
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }

    public record AddCustomerRequest(string FirstName, string LastName, string Email,
                                      string Phone, string DriverLicenseNumber,
                                      DateTime DateOfBirth);
}