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
        // Create
        Task AddBookAsync(Book book);

        // Read
        Task<Book?> GetBookByIdAsync(int id);
        Task<IEnumerable<Book>> GetAllBooksAsync();

        // Update
        Task UpdateBookAsync(Book book);

        // Delete
        Task DeleteBookAsync(int id);
    }
}

