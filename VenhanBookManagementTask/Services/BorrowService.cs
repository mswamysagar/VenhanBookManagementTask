using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VenhanBookManagementTask.Models;
using VenhanBookManagementTask.Repository.Interfaces;
using VenhanBookManagementTask.Services.Interfaces;

namespace VenhanBookManagementTask.Services
{
    public class BorrowService : IBorrowService
    {
        private readonly IBorrowRepository _borrowRepo;
        private readonly IBookRepository _bookRepo;
        private readonly IBorrowerRepository _borrowerRepo;

        public BorrowService(
            IBorrowRepository borrowRepo,
            IBookRepository bookRepo,
            IBorrowerRepository borrowerRepo)
        {
            _borrowRepo = borrowRepo ?? throw new ArgumentNullException(nameof(borrowRepo));
            _bookRepo = bookRepo ?? throw new ArgumentNullException(nameof(bookRepo));
            _borrowerRepo = borrowerRepo ?? throw new ArgumentNullException(nameof(borrowerRepo));
        }

        // ✅ Get all borrow records
        public async Task<IEnumerable<BorrowRecordModel>> GetAllAsync()
        {
            try
            {
                return await _borrowRepo.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error retrieving borrow records from database.", ex);
            }
        }

        // ✅ Borrow a book
        public async Task BorrowBookAsync(Guid bookId, Guid borrowerId)
        {
            try
            {
                // Check if book exists
                var book = await _bookRepo.GetByIdAsync(bookId)
                    ?? throw new ApplicationException("Book not found.");

                // Check if borrower exists
                var borrower = await _borrowerRepo.GetByIdAsync(borrowerId)
                    ?? throw new ApplicationException("Borrower not found.");

                // Check book availability
                if (book.Quantity <= 0)
                    throw new ApplicationException("Book is not available for borrowing.");

                // Decrease book quantity
                book.Quantity -= 1;
                await _bookRepo.UpdateAsync(book);

                // Create borrow record
                var record = new BorrowRecordModel
                {
                    BookId = book.BookId,
                    BorrowerId = borrower.BorrowerId,
                    BorrowDate = DateTime.UtcNow,
                    DueDate = DateTime.UtcNow.AddDays(7),
                    IsReturned = false
                };

                await _borrowRepo.AddAsync(record);
            }
            catch (ApplicationException)
            {
                // Known validation or business errors
                throw;
            }
            catch (Exception ex)
            {
                // Unexpected system/database issues
                throw new Exception("Unexpected error occurred while borrowing a book.", ex);
            }
        }

        // ✅ Return a borrowed book
        public async Task ReturnBookAsync(Guid borrowRecordId)
        {
            try
            {
                // Check if borrow record exists
                var record = await _borrowRepo.GetByIdAsync(borrowRecordId)
                    ?? throw new ApplicationException("Borrow record not found.");

                // Check if already returned
                if (record.IsReturned)
                    throw new ApplicationException("This book has already been returned.");

                // Mark as returned
                record.IsReturned = true;
                record.ReturnDate = DateTime.UtcNow;
                await _borrowRepo.UpdateAsync(record);

                // Increase book quantity
                var book = await _bookRepo.GetByIdAsync(record.BookId);
                if (book != null)
                {
                    book.Quantity += 1;
                    await _bookRepo.UpdateAsync(book);
                }
            }
            catch (ApplicationException)
            {
                // Known validation or business logic error
                throw;
            }
            catch (Exception ex)
            {
                // Unexpected errors (database, null refs, etc.)
                throw new Exception("Unexpected error occurred while returning a book.", ex);
            }
        }
    }
}
