using BookManagement.BusinessObjects.Commons;
using BookManagement.BusinessObjects.Entities;
using BookManagement.Services.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.Services.IServices
{
    public interface ICategoryService
    {
        Task<PagedResult<CateDto>> GetAllCategoriesAsync(int pageNumber, int pageSize, string? name, int? status, string? parentCategoryName);

        Task<Category[]> GetAllParentCategoriesAsync();
        Task<CateDto?> GetCategoryByIdAsync(int id);
        Task<CateDto> AddCategoryAsync(CreateCateDto category);
        Task<bool> UpdateCategoryAsync(int id, UpdateCateDto updateCateDto);
        Task<bool> DeleteCategoryAsync(int id);
    }
}
