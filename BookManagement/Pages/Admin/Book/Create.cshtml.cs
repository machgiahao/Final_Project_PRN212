using AutoMapper;
using BookManagement.BusinessObjects.Entities;
using BookManagement.Services.DTOs.Book;
using BookManagement.Services.IServices;
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

        public List<Category> Categories { get; set; } = new();

        public async Task<IActionResult> OnGetPartialForm()
        {
            Categories = (await _categoryService.GetAllCategoriesAsync())?.ToList() ?? new List<Category>();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Categories = (await _categoryService.GetAllCategoriesAsync())?.ToList() ?? new List<Category>();

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