using BookManagement.BusinessObjects.Entities;
using BookManagement.DataAccess.IRepositories;
using BookManagement.Services.IServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookManagement.Services.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            try
            {
                return await _categoryRepository.GetAllCategoriesAsync();
            }
            catch (Exception ex)
            {

                return new List<Category>();
            }
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            try
            {
                return await _categoryRepository.GetCategoryByIdAsync(id);
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public async Task AddCategoryAsync(Category category)
        {
            try
            {
                await _categoryRepository.AddCategoryAsync(category);
            }
            catch (Exception ex)
            {

                throw new Exception("Error adding category.", ex);
            }
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            try
            {
                await _categoryRepository.UpdateCategoryAsync(category);
            }
            catch (Exception ex)
            {

                throw new Exception("Error updating category.", ex);
            }
        }

        public async Task DeleteCategoryAsync(int id)
        {
            try
            {
                await _categoryRepository.DeleteCategoryAsync(id);
            }
            catch (Exception ex)
            {

                throw new Exception("Error deleting category.", ex);
            }
        }
    }
}
