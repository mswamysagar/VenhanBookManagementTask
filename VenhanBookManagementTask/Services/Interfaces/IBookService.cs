using VenhanBookManagementTask.Models;

namespace VenhanBookManagementTask.Services.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<BookModel>> GetAllAsync();
        Task<BookModel?> GetByIdAsync(Guid id);
        Task AddAsync(BookModel book);
        Task UpdateAsync(BookModel book);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<BookModel>> SearchAsync(string? title, string? author, string? genre);
    }
}
