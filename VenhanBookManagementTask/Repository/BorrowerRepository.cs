using Microsoft.EntityFrameworkCore;
using VenhanBookManagementTask.DAL;
using VenhanBookManagementTask.Models;
using VenhanBookManagementTask.Repository.Interfaces;

namespace VenhanBookManagementTask.Repository
{
    public class BorrowerRepository : IBorrowerRepository
    {
        private readonly BookContext _context;
        private readonly ILogger<BorrowerRepository> _logger;

        public BorrowerRepository(BookContext context, ILogger<BorrowerRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ✅ Get All Borrowers
        public async Task<IEnumerable<BorrowerModel>> GetAllAsync()
        {
            try
            {
                return await _context.Borrowers
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving borrower list from database.");
                throw new ApplicationException("Failed to fetch borrower list.", ex);
            }
        }

        // ✅ Get Borrower by ID
        public async Task<BorrowerModel?> GetByIdAsync(Guid borrowerId)
        {
            try
            {
                var borrower = await _context.Borrowers.FindAsync(borrowerId);
                if (borrower == null)
                    throw new ApplicationException($"Borrower with ID '{borrowerId}' not found.");

                return borrower;
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching borrower by ID {BorrowerId}", borrowerId);
                throw new Exception("Unexpected error occurred while fetching borrower details.", ex);
            }
        }

        // ✅ Get Borrower by Membership ID
        public async Task<BorrowerModel?> GetByMembershipIdAsync(string membershipId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(membershipId))
                    throw new ApplicationException("Membership ID cannot be empty.");

                var borrower = await _context.Borrowers
                    .FirstOrDefaultAsync(b => b.MembershipId == membershipId);

                return borrower;
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching borrower by membership ID {MembershipId}", membershipId);
                throw new Exception("Unexpected error occurred while fetching borrower by membership ID.", ex);
            }
        }

        // ✅ Add Borrower
        public async Task AddAsync(BorrowerModel borrower)
        {
            try
            {
                if (borrower == null)
                    throw new ApplicationException("Invalid borrower data.");

                await _context.Borrowers.AddAsync(borrower);
                await _context.SaveChangesAsync();
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update failed while adding borrower {Email}", borrower.Email);
                throw new ApplicationException("Failed to add borrower. Please check database constraints or duplicate data.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error adding borrower {Email}", borrower.Email);
                throw new Exception("Unexpected error occurred while adding borrower.", ex);
            }
        }

        // ✅ Update Borrower
        public async Task UpdateAsync(BorrowerModel borrower)
        {
            try
            {
                var existing = await _context.Borrowers.AsNoTracking()
                    .FirstOrDefaultAsync(b => b.BorrowerId == borrower.BorrowerId);

                if (existing == null)
                    throw new ApplicationException("Borrower not found for update.");

                _context.Borrowers.Update(borrower);
                await _context.SaveChangesAsync();
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning(ex, "Concurrency conflict while updating borrower ID {BorrowerId}", borrower.BorrowerId);
                throw new ApplicationException("Borrower record was modified by another process. Please refresh and try again.", ex);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error while updating borrower ID {BorrowerId}", borrower.BorrowerId);
                throw new ApplicationException("Failed to update borrower. Please check data constraints.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating borrower ID {BorrowerId}", borrower.BorrowerId);
                throw new Exception("Unexpected error occurred while updating borrower.", ex);
            }
        }

        // ✅ Delete Borrower
        public async Task DeleteAsync(Guid borrowerId)
        {
            try
            {
                var borrower = await _context.Borrowers.FindAsync(borrowerId);
                if (borrower == null)
                    throw new ApplicationException("Borrower not found for deletion.");

                _context.Borrowers.Remove(borrower);
                await _context.SaveChangesAsync();
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update failed while deleting borrower ID {BorrowerId}", borrowerId);
                throw new ApplicationException("Failed to delete borrower. Database constraint violation may exist.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error deleting borrower ID {BorrowerId}", borrowerId);
                throw new Exception("Unexpected error occurred while deleting borrower.", ex);
            }
        }
    }
}
