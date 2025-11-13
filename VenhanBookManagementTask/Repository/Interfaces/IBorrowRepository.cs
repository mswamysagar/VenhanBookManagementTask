using VenhanBookManagementTask.Models;

namespace VenhanBookManagementTask.Repository.Interfaces
{
    public interface IBorrowRepository
    {
        Task<IEnumerable<BorrowRecordModel>> GetAllAsync();
        Task<BorrowRecordModel?> GetByIdAsync(Guid borrowRecordId);
        Task AddAsync(BorrowRecordModel record);
        Task UpdateAsync(BorrowRecordModel record);
    }
}
