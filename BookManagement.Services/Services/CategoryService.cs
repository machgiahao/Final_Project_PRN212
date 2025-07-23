using AutoMapper;
using BookManagement.BusinessObjects.Commons;
using BookManagement.BusinessObjects.Entities;
using BookManagement.DataAccess.IRepositories;
using BookManagement.Services.DTOs.Category;
using BookManagement.Services.IServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookManagement.Services.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<CateDto>> GetAllCategoriesAsync(int pageNumber, int pageSize, string? name, int? status, string? parentCategoryName)
        {
            try
            {
                var categories = await _categoryRepository.GetAllCategoriesAsync(pageNumber, pageSize, name, status, parentCategoryName);
                return _mapper.Map<PagedResult<CateDto>>(categories);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error retrieving all categories: {ex.Message}", ex);
            }
        }

        public async Task<Category[]> GetAllParentCategoriesAsync()
        {
            try
            {
                return await _categoryRepository.GetAllParentCategoriesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error retrieving parent categories: {ex.Message}", ex);
            }
        }

        public async Task<CateDto?> GetCategoryByIdAsync(int id)
        {
            try
            {
                var category = await _categoryRepository.GetCategoryByIdAsync(id);
                return category == null ? null : _mapper.Map<CateDto>(category);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error retrieving category with id '{id}': {ex.Message}", ex);
            }
        }

        public async Task<CateDto> AddCategoryAsync(CreateCateDto categoryDto)
        {
            try
            {
                var category = _mapper.Map<Category>(categoryDto);

                await _categoryRepository.AddCategoryAsync(category);
                return _mapper.Map<CateDto>(category);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error adding category: {ex.Message}", ex);
            }
        }

        public async Task<bool> UpdateCategoryAsync(int id, UpdateCateDto updateCateDto)
        {
            try
            {
                var category = await _categoryRepository.GetCategoryByIdAsync(id);
                if (category == null) return false;

                _mapper.Map(updateCateDto, category);
                await _categoryRepository.UpdateCategoryAsync(category);
                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error updating category '{id}': {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            try
            {
                var category = await _categoryRepository.GetCategoryByIdAsync(id);
                if (category == null) return false;

                await _categoryRepository.DeleteCategoryAsync(id);
                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error deleting category '{id}': {ex.Message}", ex);
            }
        }
    }
}
