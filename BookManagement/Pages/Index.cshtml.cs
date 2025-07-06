using AutoMapper;
using BookManagement.Services.IServices;
using BookManagement.ViewModels.Book;
using BookManagement.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookManagement.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;

        public IndexModel(ICategoryService categoryService, IBookService bookService, IMapper mapper)
        {
            _categoryService = categoryService;
            _bookService = bookService;
            _mapper = mapper;
        }

        public HomeViewModel Home { get; set; } = new();
        public async Task OnGetAsync()
        {
            // Get featured categories (e.g., top 6)
            var categories = await _categoryService.GetAllCategoriesAsync();
            Home.FeaturedCategories = categories.Take(6).ToList();

            // Get all books
            var books = await _bookService.GetAllBooksAsync();
            var bookViewModel = _mapper.Map<List<BookViewModel>>(books);

            // Bestselling books (top 6 by Sold)
            Home.BestsellingBooks = bookViewModel
                .OrderByDescending(b => b.Sold)
                .Take(6)
                .ToList();

            // Featured books
            Home.NewReleaseBooks = bookViewModel
                .OrderByDescending(b => b.CreatedAt)
                .Take(12)
                .ToList();
        }
    }
}
