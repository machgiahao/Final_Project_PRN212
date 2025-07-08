using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookManagement.Services.IServices;
using BookManagement.ViewModels.Book;
using BookManagement.ViewModels.Shared;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

namespace BookManagement.Pages.Admin.Book
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;

        public IndexModel(IBookService bookService, IMapper mapper)
        {
            _bookService = bookService;
            _mapper = mapper;
        }

        public IList<BookViewModel> BookViewModel { get; set; } = default!;
        public PaginationViewModel Pagination { get; set; } = new();
        [BindProperty(SupportsGet = true)]
        public BookFilterViewModel Filter { get; set; } = new BookFilterViewModel();

        public async Task OnGetAsync(int pageNumber = 1)
        {
            var books = await _bookService.GetBooksPagedAsync(
                Filter.PageNumber,
                Filter.PageSize,
                Filter.SelectedCategories,
                Filter.MinPrice,
                Filter.MaxPrice);

            Pagination = new PaginationViewModel
            {
                CurrentPage = Filter.PageNumber,
                PageSize = Filter.PageSize,
                TotalCount = books.TotalCount, 
                TotalPages = (int)Math.Ceiling((double)books.TotalCount / Filter.PageSize) 
            };

            BookViewModel = _mapper.Map<IList<BookViewModel>>(books.Items.ToList());
        }
    }
}
