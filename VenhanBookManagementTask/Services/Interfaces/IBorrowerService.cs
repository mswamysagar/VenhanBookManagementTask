using VenhanBookManagementTask.Models;

namespace VenhanBookManagementTask.Services.Interfaces
{
    public interface IBorrowerService
    {
        Task<IEnumerable<BorrowerModel>> GetAllAsync();
        Task<BorrowerModel?> GetByIdAsync(Guid id);
        Task AddAsync(BorrowerModel borrower);
        Task UpdateAsync(BorrowerModel borrower);
        Task DeleteAsync(Guid id);
    }
}
