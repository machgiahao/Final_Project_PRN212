using BookManagement.BusinessObjects.Entities;
using BookManagement.Services.IServices;
using BookManagement.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

public class IndexModel : PageModel
{
    private readonly IBookService _bookService;
    private readonly ICategoryService _categoryService;

    public IndexModel(IBookService bookService, ICategoryService categoryService)
    {
        _bookService = bookService;
        _categoryService = categoryService;
    }

    public List<Book> Books { get; set; } = new();
    public List<Category> Categories { get; set; } = new();
    public PaginationViewModel Pagination { get; set; } = new();

    public async Task OnGetAsync(int pageNumber = 1)
    {
        int pageSize = 10;
        var pagedResult = await _bookService.GetBooksPagedAsync(pageNumber, pageSize);
        Books = pagedResult.Books.ToList();

        Pagination = new PaginationViewModel
        {
            CurrentPage = pageNumber,
            PageSize = pageSize,
            TotalCount = pagedResult.TotalCount,
            TotalPages = (int)Math.Ceiling((double)pagedResult.TotalCount / pageSize)
        };

        var categories = await _categoryService.GetAllCategoriesAsync();
        Categories = categories?.ToList() ?? new List<Category>();
    }


}
