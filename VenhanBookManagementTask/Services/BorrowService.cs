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

      
        public async Task BorrowBookAsync(Guid bookId, Guid borrowerId)
        {
            try
            {
               
                var book = await _bookRepo.GetByIdAsync(bookId)
                    ?? throw new ApplicationException("Book not found.");

                
                var borrower = await _borrowerRepo.GetByIdAsync(borrowerId)
                    ?? throw new ApplicationException("Borrower not found.");

                
                if (book.Quantity <= 0)
                    throw new ApplicationException("Book is not available for borrowing.");

                
                book.Quantity -= 1;
                await _bookRepo.UpdateAsync(book);

                
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
                
                throw;
            }
            catch (Exception ex)
            {
                
                throw new Exception("Unexpected error occurred while borrowing a book.", ex);
            }
        }

        public async Task ReturnBookAsync(Guid borrowRecordId)
        {
            try
            {
                
                var record = await _borrowRepo.GetByIdAsync(borrowRecordId)
                    ?? throw new ApplicationException("Borrow record not found.");

                if (record.IsReturned)
                    throw new ApplicationException("This book has already been returned.");

                
                record.IsReturned = true;
                record.ReturnDate = DateTime.UtcNow;
                await _borrowRepo.UpdateAsync(record);

               
                var book = await _bookRepo.GetByIdAsync(record.BookId);
                if (book != null)
                {
                    book.Quantity += 1;
                    await _bookRepo.UpdateAsync(book);
                }
            }
            catch (ApplicationException)
            {
                
                throw;
            }
            catch (Exception ex)
            {

                throw new Exception("Unexpected error occurred while returning a book.", ex);
            }
        }
    }
}
