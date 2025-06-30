using BookManagement.BusinessObjects.Commons;
using BookManagement.BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.Services.IServices
{
    public interface IBookService
    {
        Task AddBookAsync(Book book);
        Task<Book?> GetBookByIdAsync(int id);
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task UpdateBookAsync(Book book);
        Task DeleteBookAsync(int id);
        Task<PagedResult<Book>> GetBooksPagedAsync(int pageNumber, int pageSize, List<int> categoryIds = null, decimal? minPrice = null,
        decimal? maxPrice = null);

    }
}
