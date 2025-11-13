using Microsoft.EntityFrameworkCore;
using VenhanBookManagementTask.DAL;
using VenhanBookManagementTask.Models;
using VenhanBookManagementTask.Repository.Interfaces;

namespace VenhanBookManagementTask.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly BookContext _context;
        private readonly ILogger<BookRepository> _logger;

        public BookRepository(BookContext context, ILogger<BookRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ✅ Get All Books
        public async Task<IEnumerable<BookModel>> GetAllAsync()
        {
            try
            {
                return await _context.Books.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving book list from database.");
                throw new ApplicationException("Failed to fetch book list.", ex);
            }
        }

        // ✅ Get Book by ID
        public async Task<BookModel?> GetByIdAsync(Guid bookId)
        {
            try
            {
                // Return null when not found (don't throw here; caller decides)
                        var book = await _context.Books.FindAsync(bookId);
                return book;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching book by ID {BookId}", bookId);
                throw new Exception("Unexpected error occurred while fetching book details.", ex);
            }
        }

        // ✅ Get Book by ISBN
        public async Task<BookModel?> GetByISBNAsync(string isbn)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(isbn))
                    throw new ApplicationException("ISBN cannot be empty.");

                return await _context.Books.FirstOrDefaultAsync(b => b.ISBN == isbn);
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving book by ISBN {ISBN}", isbn);
                throw new Exception("Unexpected error occurred while fetching book by ISBN.", ex);
            }
        }

        // ✅ Add New Book
        public async Task AddAsync(BookModel book)
        {
            try
            {
                if (book == null)
                    throw new ApplicationException("Invalid book data.");

                await _context.Books.AddAsync(book);
                await _context.SaveChangesAsync();
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update failed while adding book {ISBN}", book?.ISBN);
                throw new ApplicationException("Failed to add book. Check for duplicate ISBN or constraint errors.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error adding book {ISBN}", book?.ISBN);
                throw new Exception("Unexpected error occurred while adding a book.", ex);
            }
        }

        // ✅ Update Existing Book
        public async Task UpdateAsync(BookModel book)
        {
            try
            {
                var existing = await _context.Books.AsNoTracking()
                    .FirstOrDefaultAsync(b => b.BookId == book.BookId);

                if (existing == null)
                    throw new ApplicationException("Book not found for update.");

                _context.Books.Update(book);
                await _context.SaveChangesAsync();
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning(ex, "Concurrency conflict while updating book ID {BookId}", book.BookId);
                throw new ApplicationException("Book record was modified by another process. Please refresh and try again.", ex);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update failed while updating book ID {BookId}", book.BookId);
                throw new ApplicationException("Failed to update book. Check for data integrity issues.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating book ID {BookId}", book.BookId);
                throw new Exception("Unexpected error occurred while updating book.", ex);
            }
        }

        // ✅ Delete Book
        public async Task DeleteAsync(Guid bookId)
        {
            try
            {
                var book = await _context.Books.FindAsync(bookId);
                if (book == null)
                    throw new ApplicationException("Book not found for deletion.");

                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update failed while deleting book ID {BookId}", bookId);
                throw new ApplicationException("Failed to delete book. It may have related borrow records.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error deleting book ID {BookId}", bookId);
                throw new Exception("Unexpected error occurred while deleting book.", ex);
            }
        }
    }
}
