using AutoMapper;
using BookManagement.BusinessObjects.Entities;
using BookManagement.Services.DTOs.Book;
using BookManagement.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims; // Cần thiết cho SelectList

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

        // Handler này được gọi khi AJAX request từ Index.cshtml để tải form vào modal
        public async Task<IActionResult> OnGetPartialForm()
        {
            Categories = (await _categoryService.GetAllCategoriesAsync())?.ToList() ?? new List<Category>();
            return Page(); // Trả về chính Page này (Create.cshtml) vì nó đã được đặt Layout = null
        }

        // Handler cho POST request khi submit form từ modal
        public async Task<IActionResult> OnPostAsync()
        {
            // Tải lại Categories trước khi kiểm tra ModelState nếu có lỗi
            // để Dropdownlist không bị trống khi form được trả về
            Categories = (await _categoryService.GetAllCategoriesAsync())?.ToList() ?? new List<Category>();

            if (!ModelState.IsValid)
            {
                // Nếu validation thất bại, trả về lại form với các lỗi để hiển thị trong modal
                return Page();
            }

            // Xử lý upload ảnh
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
                bookEntity.Status = BusinessObjects.Enum.BookStatus.Available; // Đặt trạng thái mặc định

                await _bookService.AddBookAsync(bookEntity);
                // Trả về JSON khi thành công để AJAX trong Index.cshtml xử lý
                return new JsonResult(new { success = true, message = "Book created successfully!" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error creating book: {ex.Message}");
                // Nếu có lỗi server, trả về lại form với lỗi để hiển thị trong modal
                return Page();
            }
        }
    }
}