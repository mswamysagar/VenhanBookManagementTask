using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VenhanBookManagementTask.Models;

namespace VenhanBookManagementTask.Repository.Interfaces
{
    public interface IBorrowerRepository
    {
        Task<IEnumerable<BorrowerModel>> GetAllAsync();
        Task<BorrowerModel?> GetByIdAsync(Guid borrowerId);
        Task<BorrowerModel?> GetByMembershipIdAsync(string membershipId);
        Task AddAsync(BorrowerModel borrower);
        Task UpdateAsync(BorrowerModel borrower);
        Task DeleteAsync(Guid borrowerId);
    }
}
