﻿using Microsoft.EntityFrameworkCore;
using BookManagement.BusinessObjects.Entities;
using BookManagement.DataAccess.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagement.DataAccess.Context;
using BookManagement.BusinessObjects.Commons;


namespace BookManagement.DataAccess.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BookStoreDbContext _context;
        public BookRepository(BookStoreDbContext context)
        {
            _context = context;
        }

        public async Task AddBookAsync(Book book)
        {
            try
            {
                await _context.Books.AddAsync(book);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Handle database update exceptions
                throw new Exception("An error occurred while adding the book to the database.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while adding the book.", ex);
            }
        }

        public async Task DeleteBookAsync(int id)
        {
            try
            {
                var book = await _context.Books.FindAsync(id);
                if (book != null)
                {
                    _context.Books.Remove(book);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new KeyNotFoundException($"Book with ID {id} not found.");
                }
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while deleting the book from the database.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while deleting the book.", ex);
            }
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            try
            {
                return await _context.Books
                    .Include(b => b.Category)
                    .OrderByDescending(b => b.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all books.", ex);
            }
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            try
            {
                return await _context.Books
                    .Include(b => b.Category)
                    .FirstOrDefaultAsync(b => b.BookId == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the book with ID {id}.", ex);
            }
        }

        public async Task UpdateBookAsync(Book book)
        {
            try
            {
                _context.Books.Update(book);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while updating the book.", ex);
            }
        }

        public async Task<PagedResult<Book>> GetBooksPagedAsync(int pageNumber, int pageSize, List<int> categoryIds = null, decimal? minPrice = null, decimal? maxPrice = null)
        {
            var query = _context.Books.Include(b => b.Category).AsQueryable();

            // Filter by category if provided
            if (categoryIds != null && categoryIds.Any())
            {
                query = query.Where(b => b.CategoryId.HasValue && categoryIds.Contains(b.CategoryId.Value));
            }
            if(minPrice.HasValue)
            {
                query = query.Where(b => b.Price >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                query = query.Where(b => b.Price <= maxPrice.Value);
            }

            int totalCount = await query.CountAsync();
            var books = await query
                .OrderByDescending(b => b.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Book>
            {
                Items = books,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
