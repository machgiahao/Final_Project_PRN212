using BookManagement.BusinessObjects.Commons;
using BookManagement.BusinessObjects.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookManagement.DataAccess.IRepositories
{
    public interface ICategoryRepository
    {
        Task AddCategoryAsync(Category category);
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<PagedResult<Category>> GetAllCategoriesAsync(int pageNumber, int pageSize, string? name, int? status, string? parentCategoryName);

        Task<Category[]> GetAllParentCategoriesAsync();
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(int id);
    }
}
