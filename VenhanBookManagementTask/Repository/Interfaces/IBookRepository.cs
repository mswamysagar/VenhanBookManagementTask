using VenhanBookManagementTask.Models;

namespace VenhanBookManagementTask.Repository.Interfaces
{
    public interface IBookRepository
    {
        Task<IEnumerable<BookModel>> GetAllAsync();
        Task<BookModel?> GetByIdAsync(Guid bookId);
        Task<BookModel?> GetByISBNAsync(string isbn);
        Task AddAsync(BookModel book);
        Task UpdateAsync(BookModel book);
        Task DeleteAsync(Guid bookId);
        
    }
}
