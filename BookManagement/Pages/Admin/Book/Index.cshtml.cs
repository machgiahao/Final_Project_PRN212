using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookManagement.Services.IServices;
using BookManagement.ViewModels.Book;
using BookManagement.ViewModels.Shared;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using BookManagement.BusinessObjects.Entities;
using BookManagement.Services.DTOs.Book;
using BookManagement.ViewModels.Category;

namespace BookManagement.Pages.Admin.Book
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IBookService _bookService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        public IndexModel(IBookService bookService, ICategoryService categoryService, IMapper mapper)
        {
            _bookService = bookService;
            _categoryService = categoryService;
            _mapper = mapper;
        }

        public List<BusinessObjects.Entities.Book> Books { get; set; } = new();
        public List<CateFilterViewModel> Categories { get; set; } = new();
        public PaginationViewModel Pagination { get; set; } = new();
        public IList<BookViewModel> BookViewModel { get; private set; }
        [BindProperty(SupportsGet = true)]
        public BookFilterViewModel Filter { get; set; } = new();

        public async Task OnGetAsync(int pageNumber = 1)
        {
            Filter.Role = "Admin";

            var filter = _mapper.Map<BookPagedQueryDto>(Filter);
            AllCategories = (await _categoryService.GetAllCategoriesAsync()).ToList();
            var pagedResult = await _bookService.GetBooksPagedAsync(filter);

            Books = pagedResult.Items.ToList();

            Pagination = new PaginationViewModel
            {
                CurrentPage = Filter.PageNumber,
                PageSize = Filter.PageSize,
                TotalCount = pagedResult.TotalCount,
                TotalPages = (int)Math.Ceiling((double)pagedResult.TotalCount / Filter.PageSize)
            };

            BookViewModel = _mapper.Map<IList<BookViewModel>>(Books);
        }

        public async Task<IActionResult> OnPostDelete(int id)
        {
            var result = await _bookService.DeleteBookAsync(id);
            if (result)
            {
                return new JsonResult(new { success = true, message = "Book deleted successfully." });
            }
            else
            {
                return new JsonResult(new { success = false, message = "Error deleting book. Please try again." });
            }
        }
    }
}
