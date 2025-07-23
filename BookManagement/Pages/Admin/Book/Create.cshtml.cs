using AutoMapper;
using BookManagement.BusinessObjects.Entities;
using BookManagement.Services.DTOs.Book;
using BookManagement.Services.DTOs.Category;
using BookManagement.Services.IServices;
using BookManagement.ViewModels.Category;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace BookManagement.Pages.Admin.Book
{
    public class CreateModel : PageModel
    {
        private readonly IBookService _bookService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CreateModel(IBookService bookService, ICategoryService categoryService, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _bookService = bookService;
            _categoryService = categoryService;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        [BindProperty]
        public CreateBookDto NewBook { get; set; }

        public List<CateFilterViewModel> Categories { get; set; } = new();

        public async Task<IActionResult> OnGetPartialForm()
        {
            var catePagedResult = await _categoryService.GetAllCategoriesAsync(1, 9, null, null, null);

            Categories = _mapper.Map<List<CateFilterViewModel>>(catePagedResult.Items);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var catePagedResult = await _categoryService.GetAllCategoriesAsync(1, 9, null, null, null);

            Categories = _mapper.Map<List<CateFilterViewModel>>(catePagedResult.Items);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Handle upload picture
            if (NewBook.ImageFile != null && NewBook.ImageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img", "books");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(NewBook.ImageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await NewBook.ImageFile.CopyToAsync(fileStream);
                }

                NewBook.ImageUrl = "/img/books/" + uniqueFileName;
            }
            else
            {
                NewBook.ImageUrl = "/img/no-image-available.png"; // Placeholder
            }

            try
            {
                var bookEntity = _mapper.Map<BusinessObjects.Entities.Book>(NewBook);
                bookEntity.Sold = 0;
                bookEntity.CreatedAt = DateTime.Now;
                bookEntity.UpdatedAt = null;
                bookEntity.CreatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
                bookEntity.UpdatedBy = null;
                bookEntity.Status = BusinessObjects.Enum.BookStatus.Available;

                await _bookService.AddBookAsync(bookEntity);
                return new JsonResult(new { success = true, message = "Book created successfully!" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error creating book: {ex.Message}");
                return Page();
            }
        }
    }
}