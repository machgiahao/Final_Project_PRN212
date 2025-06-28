using Microsoft.EntityFrameworkCore;
using BookManagement.DataAccess.Context;
using BookManagement.BusinessObjects.Entities;
using BookManagement.DataAccess.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagement.DataAccess.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BookStoreDbContext _context;

        public CategoryRepository(BookStoreDbContext context)
        {
            _context = context;
        }

        public async Task AddCategoryAsync(Category category)
        {
            try
            {
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while adding the category to the database.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while adding the category.", ex);
            }
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            try
            {
                return await _context.Categories
                    .Include(c => c.Books)
                    .Include(c => c.ParentCategory)
                    .Include(c => c.InverseParentCategory)
                    .FirstOrDefaultAsync(c => c.CategoryId == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the category with ID {id}.", ex);
            }
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            try
            {
                return await _context.Categories
                    .Include(c => c.Books)
                    .Include(c => c.ParentCategory)
                    .Include(c => c.InverseParentCategory)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all categories.", ex);
            }
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            try
            {
                _context.Categories.Update(category);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new Exception("A concurrency error occurred while updating the category.", ex);
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while updating the category in the database.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while updating the category.", ex);
            }
        }

        public async Task DeleteCategoryAsync(int id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category != null)
                {
                    _context.Categories.Remove(category);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new KeyNotFoundException($"Category with ID {id} not found.");
                }
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while deleting the category from the database.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while deleting the category.", ex);
            }
        }
    }
}
