using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarRentalSystem.API.Data;

namespace CarRentalSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public PaymentsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var payments = await _db.Payments.ToListAsync();
            return Ok(payments.Select(p => new
            {
                p.PaymentId, p.ContractId,
                p.Amount, p.Method, p.Status, p.PaymentDate
            }));
        }

        [HttpPost]
        public async Task<IActionResult> Process([FromBody] ProcessPaymentRequest request)
        {
            try
            {
                var contract = await _db.Contracts.FindAsync(request.ContractId);
                if (contract == null)
                    return NotFound($"Contract {request.ContractId} not found.");

                var payment = new PaymentEntity
                {
                    PaymentId   = Guid.NewGuid().ToString(),
                    ContractId  = request.ContractId,
                    Amount      = request.Amount,
                    Method      = request.Method,
                    Status      = "Completed",
                    PaymentDate = DateTime.Now
                };

                _db.Payments.Add(payment);
                await _db.SaveChangesAsync();

                return Ok(new
                {
                    message = "Payment processed.",
                    payment.PaymentId,
                    payment.Status
                });
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }

    public record ProcessPaymentRequest(string ContractId, decimal Amount, string Method);
}