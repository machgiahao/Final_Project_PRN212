using BookManagement.BusinessObjects.Commons;
using BookManagement.BusinessObjects.Entities;
using BookManagement.Services.DTOs.Book;
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
        Task<PagedResult<Book>> GetBooksPagedAsync(BookPagedQueryDto queryDto);

    }
}
