using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VenhanBookManagementTask.Models;
using VenhanBookManagementTask.Repository.Interfaces;
using VenhanBookManagementTask.Services.Interfaces;

namespace VenhanBookManagementTask.Services
{
    public class BorrowerService : IBorrowerService
    {
        private readonly IBorrowerRepository _repo;

        public BorrowerService(IBorrowerRepository repo)
        {
            _repo = repo;
        }

        // ✅ Get All Borrowers
        public async Task<IEnumerable<BorrowerModel>> GetAllAsync()
        {
            try
            {
                return await _repo.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error retrieving borrower list.", ex);
            }
        }

        // ✅ Get Borrower by ID
        public async Task<BorrowerModel?> GetByIdAsync(Guid id)
        {
            try
            {
                var borrower = await _repo.GetByIdAsync(id);
                if (borrower == null)
                    throw new ApplicationException("Borrower not found.");

                return borrower;
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error occurred while fetching borrower details.", ex);
            }
        }

        // ✅ Add New Borrower
        public async Task AddAsync(BorrowerModel borrower)
        {
            try
            {
                var existing = await _repo.GetByMembershipIdAsync(borrower.MembershipId);
                if (existing != null)
                    throw new ApplicationException("Membership ID already exists. Please use a unique ID.");

                await _repo.AddAsync(borrower);
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error occurred while adding borrower.", ex);
            }
        }

        // ✅ Update Borrower
        public async Task UpdateAsync(BorrowerModel borrower)
        {
            try
            {
                var existing = await _repo.GetByIdAsync(borrower.BorrowerId)
                    ?? throw new ApplicationException("Borrower not found for update.");

                // Update fields
                existing.Name = borrower.Name;
                existing.Email = borrower.Email;
                existing.ContactNumber = borrower.ContactNumber;
                existing.MembershipId = borrower.MembershipId;

                await _repo.UpdateAsync(existing);
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error occurred while updating borrower.", ex);
            }
        }

        // ✅ Delete Borrower
        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var borrower = await _repo.GetByIdAsync(id)
                    ?? throw new ApplicationException("Borrower not found for deletion.");

                await _repo.DeleteAsync(id);
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error occurred while deleting borrower.", ex);
            }
        }
    }
}
