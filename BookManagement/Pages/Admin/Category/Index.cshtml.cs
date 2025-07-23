using AutoMapper;
using BookManagement.Services.DTOs.Category;
using BookManagement.Services.IServices;
using BookManagement.ViewModels.Category;
using BookManagement.ViewModels.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookManagement.Pages.Admin.Category
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public IndexModel(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        public IList<CateViewModel> CategoryViewModel { get; set; } = default!;
        public PaginationViewModel Pagination { get; set; } = new();
        [BindProperty(SupportsGet = true)]
        public CateFilterViewModel Filter { get; set; } = new CateFilterViewModel();
        public List<SelectListItem> ParentCategorySelectList { get; set; } = new();

        private async Task LoadParentCategoriesAsync()
        {
            var allCategories = await _categoryService.GetAllParentCategoriesAsync();
            ParentCategorySelectList = allCategories.Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.Name
            }).ToList();
            ViewData["ParentCategories"] = ParentCategorySelectList;
        }

        public async Task OnGetAsync(int pageNumber = 1)
        {
            await LoadParentCategoriesAsync();
            var pagedResult = await _categoryService.GetAllCategoriesAsync(
                Filter.PageNumber,
                Filter.PageSize,
                Filter.Name,
                Filter.Status,
                Filter.ParentCategoryName);

            CategoryViewModel = _mapper.Map<List<CateViewModel>>(pagedResult.Items);

            Pagination = new PaginationViewModel
            {
                CurrentPage = Filter.PageNumber,
                PageSize = Filter.PageSize,
                TotalCount = pagedResult.TotalCount,
                TotalPages = (int)Math.Ceiling((double)pagedResult.TotalCount / Filter.PageSize)
            };
        }

        [BindProperty]
        public CreateCateViewModel CreateCategoryViewModel { get; set; } = default!;
        public async Task<IActionResult> OnPostCreateAsync()
        {
            await LoadParentCategoriesAsync();
            if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key]?.Errors;
                    if (errors != null && errors.Count > 0)
                    {
                        foreach (var error in errors)
                        {
                            Console.WriteLine($"ModelState error for '{key}': {error.ErrorMessage}");
                        }
                    }
                }
                ViewData["ShowCreateModal"] = true;
                return Page();
            }
            try
            {
                var createCategoryDto = _mapper.Map<CreateCateDto>(CreateCategoryViewModel);
                var category = await _categoryService.AddCategoryAsync(createCategoryDto);
                TempData["Message"] = "Category created successfully!";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Category created fail! {ex.Message}");
                ViewData["ShowCreateModal"] = true;
                return Page();
            }
        }

        public async Task<IActionResult> OnGetEditAsync(string id)
        {
            await LoadParentCategoriesAsync();
            int idInt = int.Parse(id);
            var category = await _categoryService.GetCategoryByIdAsync(idInt);
            if (category == null) return NotFound();
            await OnGetAsync(Filter.PageNumber);
            EditCategoryViewModel = _mapper.Map<EditCateViewModel>(category);
            ViewData["ShowEditModal"] = true;
            return Page();
        }

        [BindProperty]
        public EditCateViewModel EditCategoryViewModel { get; set; } = default!;
        public async Task<IActionResult> OnPostEditAsync()
        {
            await LoadParentCategoriesAsync();
            if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key]?.Errors;
                    if (errors != null && errors.Count > 0)
                    {
                        foreach (var error in errors)
                        {
                            Console.WriteLine($"ModelState error for '{key}': {error.ErrorMessage}");
                        }
                    }
                }
                ViewData["ShowEditModal"] = true;
                return Page();
            }
            var updateDto = _mapper.Map<UpdateCateDto>(EditCategoryViewModel);
            var result = await _categoryService.UpdateCategoryAsync(EditCategoryViewModel.CategoryId, updateDto);
            if (result)
            {
                TempData["Message"] = "Category updated successfully!";
                return RedirectToPage("./Index");
            }
            ModelState.AddModelError(string.Empty, "Update failed!");
            ViewData["ShowEditModal"] = true;
            return Page();

        }

        [BindProperty]
        public int DeleteCategoryId { get; set; }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            await LoadParentCategoriesAsync();
            int idInt = int.Parse(id);
            await _categoryService.DeleteCategoryAsync(idInt);
            TempData["SuccessMessage"] = "Deleted successfully!";
            return RedirectToPage("./Index");
        }
    }
}
