using Microsoft.EntityFrameworkCore;
using VenhanBookManagementTask.DAL;
using VenhanBookManagementTask.Models;
using VenhanBookManagementTask.Repository.Interfaces;

namespace VenhanBookManagementTask.Repository
{
    public class BorrowRepository : IBorrowRepository
    {
        private readonly BookContext _context;
        private readonly ILogger<BorrowRepository> _logger;

        public BorrowRepository(BookContext context, ILogger<BorrowRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ✅ Get All Borrow Records
        public async Task<IEnumerable<BorrowRecordModel>> GetAllAsync()
        {
            try
            {
                return await _context.BorrowRecords
                    .Include(b => b.Book)
                    .Include(b => b.Borrower)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all borrow records from database.");
                throw new ApplicationException("Failed to fetch borrow records.", ex);
            }
        }

        // ✅ Get Borrow Record by ID
        public async Task<BorrowRecordModel?> GetByIdAsync(Guid borrowRecordId)
        {
            try
            {
                var record = await _context.BorrowRecords
                    .Include(b => b.Book)
                    .Include(b => b.Borrower)
                    .FirstOrDefaultAsync(b => b.BorrowRecordId == borrowRecordId);

                if (record == null)
                    throw new ApplicationException($"Borrow record with ID '{borrowRecordId}' not found.");

                return record;
            }
            catch (ApplicationException)
            {
                throw; // rethrow known domain error
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching borrow record with ID {BorrowRecordId}", borrowRecordId);
                throw new Exception("Unexpected error occurred while fetching borrow record.", ex);
            }
        }

        // ✅ Add Borrow Record
        public async Task AddAsync(BorrowRecordModel record)
        {
            try
            {
                if (record == null)
                    throw new ApplicationException("Invalid borrow record data.");

                await _context.BorrowRecords.AddAsync(record);
                await _context.SaveChangesAsync();
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update failed while adding a new borrow record.");
                throw new ApplicationException("Failed to add borrow record. Please check data constraints.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while adding a new borrow record.");
                throw new Exception("Unexpected error occurred while adding borrow record.", ex);
            }
        }

        // ✅ Update Borrow Record
        public async Task UpdateAsync(BorrowRecordModel record)
        {
            try
            {
                var existing = await _context.BorrowRecords
                    .AsNoTracking()
                    .FirstOrDefaultAsync(r => r.BorrowRecordId == record.BorrowRecordId);

                if (existing == null)
                    throw new ApplicationException("Borrow record not found for update.");

                _context.BorrowRecords.Update(record);
                await _context.SaveChangesAsync();
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning(ex, "Concurrency conflict while updating borrow record ID {BorrowRecordId}", record.BorrowRecordId);
                throw new ApplicationException("Borrow record was updated by another process. Please refresh and try again.", ex);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error while updating borrow record ID {BorrowRecordId}", record.BorrowRecordId);
                throw new ApplicationException("Failed to update borrow record. Database constraint violation occurred.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while updating borrow record ID {BorrowRecordId}", record.BorrowRecordId);
                throw new Exception("Unexpected error occurred while updating borrow record.", ex);
            }
        }
    }
}
