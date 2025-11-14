using System;
using System.Linq;
using VenhanBookManagementTask.Models;
using VenhanBookManagementTask.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace VenhanBookManagementTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public class BorrowersController : ControllerBase
    {
        private readonly IBorrowerService _svc;
        private readonly ILogger<BorrowersController> _logger;

        public BorrowersController(IBorrowerService svc, ILogger<BorrowersController> logger)
        {
            _svc = svc;
            _logger = logger;
        }

        // Debugger helper (avoids missing method compile/diagnostic issues)
        private string GetDebuggerDisplay() => $"BorrowersController (svc: {_svc?.GetType().Name ?? "null"})";

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var borrowers = await _svc.GetAllAsync();
                return Ok(borrowers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all borrowers");
                return StatusCode(500, new { error = "Failed to fetch borrowers", details = ex.Message });
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new { error = "Invalid borrower ID" });

            try
            {
                var borrower = await _svc.GetByIdAsync(id);
                if (borrower == null)
                    return NotFound(new { error = "Borrower not found", id });

                return Ok(borrower);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching borrower by ID");
                return StatusCode(500, new { error = "Error fetching borrower", details = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BorrowerModel borrower)
        {
            if (!ModelState.IsValid)
            {
                var details = ModelState
                    .Where(kv => kv.Value.Errors.Count > 0)
                    .ToDictionary(
                        kv => kv.Key,
                        kv => kv.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                _logger.LogWarning("ModelState invalid for Create Borrower: {@Details}", details);
                return BadRequest(new { error = "Invalid payload", details });
            }

            try
            {
                await _svc.AddAsync(borrower);
                return CreatedAtAction(nameof(Get), new { id = borrower.BorrowerId }, borrower);
            }
            catch (ApplicationException ex)
            {
                _logger.LogWarning(ex, "Validation error while creating borrower");
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error creating borrower");
                return StatusCode(500, new { error = "Failed to create borrower", details = ex.Message });
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] BorrowerModel borrower)
        {
            if (id != borrower.BorrowerId)
                return BadRequest(new { error = "ID mismatch between URL and body" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _svc.UpdateAsync(borrower);
                return Ok(new { message = "Borrower updated successfully" });
            }
            catch (ApplicationException ex)
            {
                _logger.LogWarning(ex, "Validation error while updating borrower");
                if (ex.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
                    return NotFound(new { error = ex.Message });

                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating borrower");
                return StatusCode(500, new { error = "Failed to update borrower", details = ex.Message });
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new { error = "Invalid borrower ID" });

            try
            {
                await _svc.DeleteAsync(id);
                return Ok(new { message = "Borrower deleted successfully" });
            }
            catch (ApplicationException ex)
            {
                _logger.LogWarning(ex, "Validation error while deleting borrower");
                if (ex.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
                    return NotFound(new { error = ex.Message });

                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error deleting borrower");
                return StatusCode(500, new { error = "Failed to delete borrower", details = ex.Message });
            }
        }
        
    }
}