using AutoMapper;
using BookManagement.Services.DTOs.Mail;
using BookManagement.Services.IServices;
using BookManagement.ViewModels.Book;
using BookManagement.ViewModels.Category;
using BookManagement.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookManagement.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        private readonly IBookService _bookService;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public IndexModel(ICategoryService categoryService, IBookService bookService, IMapper mapper, IEmailService emailService)
        {
            _categoryService = categoryService;
            _bookService = bookService;
            _mapper = mapper;
            _emailService = emailService;
        }

        public HomeViewModel Home { get; set; } = new();
        public async Task OnGetAsync()
        {
            // Get featured categories (e.g., top 6)
            var categories = await _categoryService.GetAllCategoriesAsync(1, 6, null, null, null);
            Home.FeaturedCategories = _mapper.Map<List<CateViewModel>>(categories.Items);

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

        [BindProperty]
        public string SubscriberEmail { get; set; }

        public async Task<IActionResult> OnPostSubscribeAsync()
        {
            if (string.IsNullOrWhiteSpace(SubscriberEmail))
            {
                TempData["Message"] = "Please enter a valid email address.";
                return RedirectToPage();
            }

            var emailDto = new EmailDto
            {
                ToEmail = SubscriberEmail,
                Subject = "Welcome to FBook Newsletter!",
                Body = @"
                <div style='font-family:Segoe UI,Arial,sans-serif;background:#f6f8fa;padding:32px;'>
                    <div style='max-width:520px;margin:auto;background:#fff;border-radius:16px;box-shadow:0 4px 24px rgba(60,72,88,0.10);padding:32px;'>
                        <h2 style='color:#222;font-size:2rem;font-weight:700;margin-bottom:8px;text-align:center;'>
                            Thank you for subscribing!
                        </h2>
                        <p style='text-align:center;font-size:1.1rem;margin-bottom:24px;'>
                            You have successfully subscribed to the FBook newsletter.<br/>
                            We’ll keep you updated with the latest books, promotions, and community news.
                        </p>
                        <p style='margin-top:32px;text-align:center;font-size:1.1rem;color:#1976d2;'>
                            Welcome to our book lovers community!
                        </p>
                        <p style='margin-top:24px;text-align:center;color:#222;'>Best regards,<br/><strong>FBook Team</strong></p>
                        <hr style='margin:32px 0;border:none;border-top:1px solid #e0e6ed;'/>
                        <small style='color:#888;text-align:center;display:block;'>This is an automated email. Please do not reply.</small>
                    </div>
                </div>"
            };

            await _emailService.SendEmail(emailDto);
            TempData["Message"] = "Subscription successful! Please check your email for confirmation.";
            return RedirectToPage();
        }
    }
}
