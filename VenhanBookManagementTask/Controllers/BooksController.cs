using System;
using System.Linq;
using VenhanBookManagementTask.Models;
using VenhanBookManagementTask.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace VenhanBookManagementTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _svc;
        private readonly ILogger<BooksController> _logger;

        public BooksController(IBookService svc, ILogger<BooksController> logger)
        {
            _svc = svc;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var books = await _svc.GetAllAsync();
                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching books");
                return StatusCode(500, new { error = "Failed to fetch books", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var book = await _svc.GetByIdAsync(id);
                if (book == null)
                    return NotFound(new { error = "Book not found" });

                return Ok(book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching book details");
                return StatusCode(500, new { error = "Error fetching book details", details = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookModel book)
        {
            if (!ModelState.IsValid)
            {
                var details = ModelState
                    .Where(kv => kv.Value.Errors.Count > 0)
                    .ToDictionary(
                        kv => kv.Key,
                        kv => kv.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                _logger.LogWarning("ModelState invalid for Create Book: {@Details}", details);
                return BadRequest(new { error = "Invalid payload", details });
            }

            try
            {
                await _svc.AddAsync(book);
                return CreatedAtAction(nameof(Get), new { id = book.BookId }, book);
            }
            catch (ApplicationException ex)
            {
                _logger.LogWarning(ex, "Validation or business error while adding book");
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error adding book");
                return StatusCode(500, new { error = "Failed to add book", details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] BookModel book)
        {
            if (id != book.BookId)
                return BadRequest(new { error = "ID mismatch between URL and request body" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _svc.UpdateAsync(book);
                return Ok(new { message = "Book updated successfully" });
            }
            catch (ApplicationException ex)
            {
                _logger.LogWarning(ex, "Validation or business error while updating book");
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating book");
                return StatusCode(500, new { error = "Failed to update book", details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _svc.DeleteAsync(id);
                return Ok(new { message = "Book deleted successfully" });
            }
            catch (ApplicationException ex)
            {
                _logger.LogWarning(ex, "Validation or business error while deleting book");
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error deleting book");
                return StatusCode(500, new { error = "Failed to delete book", details = ex.Message });
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? title, [FromQuery] string? author, [FromQuery] string? genre)
        {
            try
            {
                var result = await _svc.SearchAsync(title, author, genre);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching books");
                return StatusCode(500, new { error = "Failed to search books", details = ex.Message });
            }
        }
    }
}