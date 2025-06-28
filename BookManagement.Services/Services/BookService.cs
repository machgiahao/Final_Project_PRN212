using BookManagement.BusinessObjects.Entities;
using BookManagement.DataAccess.IRepositories;
using BookManagement.Services.IServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookManagement.Services.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task AddBookAsync(Book book)
        {
            try
            {
                await _bookRepository.AddBookAsync(book);
            }
            catch (Exception ex)
            {

                throw new Exception("Error adding book.", ex);
            }
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            try
            {
                return await _bookRepository.GetBookByIdAsync(id);
            }
            catch (Exception ex)
            {

                // Return null if error occurs
                return null;
            }
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            try
            {
                return await _bookRepository.GetAllBooksAsync();
            }
            catch (Exception ex)
            {
                return new List<Book>();
            }
        }

        public async Task UpdateBookAsync(Book book)
        {
            try
            {
                await _bookRepository.UpdateBookAsync(book);
            }
            catch (Exception ex)
            {

                throw new Exception("Error updating book.", ex);
            }
        }

        public async Task DeleteBookAsync(int id)
        {
            try
            {
                await _bookRepository.DeleteBookAsync(id);
            }
            catch (Exception ex)
            {

                throw new Exception("Error deleting book.", ex);
            }
        }
        public async Task<(IEnumerable<Book> Books, int TotalCount)> GetBooksPagedAsync(int pageNumber, int pageSize)
        {
            try
            {
                return await _bookRepository.GetBooksPagedAsync(pageNumber, pageSize);

            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving paged books.", ex);
            }
        }

    }
}
