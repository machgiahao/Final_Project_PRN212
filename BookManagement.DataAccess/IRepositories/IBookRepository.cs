using BookManagement.BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.DataAccess.IRepositories
{
    public interface IBookRepository
    {
        Task AddBookAsync(Book book);

        Task<Book?> GetBookByIdAsync(int id);
        Task<IEnumerable<Book>> GetAllBooksAsync();

        Task UpdateBookAsync(Book book);

        Task DeleteBookAsync(int id);
        Task<(IEnumerable<Book> Books, int TotalCount)> GetBooksPagedAsync(int pageNumber, int pageSize);
    }
}

