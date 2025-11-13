using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VenhanBookManagementTask.Models;

namespace VenhanBookManagementTask.Services.Interfaces
{
    public interface IBorrowService
    {
        Task<IEnumerable<BorrowRecordModel>> GetAllAsync();
        Task BorrowBookAsync(Guid bookId, Guid borrowerId);
        Task ReturnBookAsync(Guid borrowRecordId);
    }
}
