using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VenhanBookManagementTask.Models;
using VenhanBookManagementTask.Repository.Interfaces;
using VenhanBookManagementTask.Services.Interfaces;

namespace VenhanBookManagementTask.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepo;

        public BookService(IBookRepository bookRepo)
        {
            _bookRepo = bookRepo;
        }

        // ✅ Get All Books
        public async Task<IEnumerable<BookModel>> GetAllAsync()
        {
            try
            {
                return await _bookRepo.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error retrieving books.", ex);
            }
        }
        //

        // ✅ Get Book by ID
        // Return null when not found; controller will map to 404
        public async Task<BookModel?> GetByIdAsync(Guid id)
        {
            try
            {
                return await _bookRepo.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while fetching the book.", ex);
            }
        }

        // ✅ Add New Book
        public async Task AddAsync(BookModel book)
        {
            try
            {
                // Validation: Check for duplicate ISBN
                var existing = await _bookRepo.GetByISBNAsync(book.ISBN);
                if (existing != null)
                    throw new ApplicationException("A book with this ISBN already exists.");

                await _bookRepo.AddAsync(book);
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while adding the book.", ex);
            }
        }

        // ✅ Update Existing Book
        public async Task UpdateAsync(BookModel book)
        {
            try
            {
                var existing = await _bookRepo.GetByIdAsync(book.BookId)
                    ?? throw new ApplicationException("Book not found for update.");

                // Update only allowed fields
                existing.Title = book.Title;
                existing.Author = book.Author;
                existing.Genre = book.Genre;
                existing.Quantity = book.Quantity;

                await _bookRepo.UpdateAsync(existing);
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while updating the book.", ex);
            }
        }

        // ✅ Delete Book
        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var book = await _bookRepo.GetByIdAsync(id)
                    ?? throw new ApplicationException("Book not found for deletion.");

                await _bookRepo.DeleteAsync(id);
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while deleting the book.", ex);
            }
        }

        // ✅ Search Books by Title, Author, or Genre
        public async Task<IEnumerable<BookModel>> SearchAsync(string? title, string? author, string? genre)
        {
            try
            {
                var allBooks = await _bookRepo.GetAllAsync();

                // LINQ-based dynamic filtering
                var filtered = allBooks.Where(b =>
                    (string.IsNullOrWhiteSpace(title) || b.Title.Contains(title, StringComparison.OrdinalIgnoreCase)) &&
                    (string.IsNullOrWhiteSpace(author) || b.Author.Contains(author, StringComparison.OrdinalIgnoreCase)) &&
                    (string.IsNullOrWhiteSpace(genre) || (b.Genre ?? "").Contains(genre, StringComparison.OrdinalIgnoreCase))
                );

                return filtered.ToList();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error occurred while searching books.", ex);
            }
        }
    }
}
