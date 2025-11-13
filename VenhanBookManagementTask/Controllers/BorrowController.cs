


using Microsoft.AspNetCore.Mvc;
using VenhanBookManagementTask.Models;
using VenhanBookManagementTask.Services.Interfaces;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace VenhanBookManagementTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BorrowController : ControllerBase
    {
        private readonly IBorrowService _svc;
        private readonly ILogger<BorrowController> _logger;

        public BorrowController(IBorrowService svc, ILogger<BorrowController> logger)
        {
            _svc = svc;
            _logger = logger;
        }

        // ✅ Get all borrow records
        [HttpGet("records")]
        public async Task<IActionResult> GetAllRecords()
        {
            try
            {
                var result = await _svc.GetAllAsync();
                return Ok(result); // Must return JSON array
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching borrow records");
                return StatusCode(500, new { error = "Failed to retrieve borrow records", details = ex.Message });
            }
        }

        // ✅ Borrow a book
        [HttpPost]
        public async Task<IActionResult> Borrow([FromBody] BorrowRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _svc.BorrowBookAsync(dto.BookId, dto.BorrowerId);
                return Ok(new { message = "Book borrowed successfully!" });
            }
            catch (ApplicationException ex)
            {
                _logger.LogWarning(ex, "Validation failed during borrow");
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during borrow");
                return StatusCode(500, new { error = "Internal Server Error", details = ex.Message });
            }
        }

        // ✅ Return a book
        [HttpPost("return/{id}")]
        public async Task<IActionResult> Return(Guid id)
        {
            try
            {
                await _svc.ReturnBookAsync(id);
                return Ok(new { message = "Book returned successfully!" });
            }
            catch (ApplicationException ex)
            {
                _logger.LogWarning(ex, "Return failed due to invalid record");
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error returning book");
                return StatusCode(500, new { error = "Unexpected error occurred", details = ex.Message });
            }
        }
    }

    public class BorrowRequestDto
    {
        public Guid BookId { get; set; }
        public Guid BorrowerId { get; set; }
    }
}
