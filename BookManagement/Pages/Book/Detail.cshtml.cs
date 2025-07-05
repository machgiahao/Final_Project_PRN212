using AutoMapper;
using BookManagement.BusinessObjects.Entities;
using BookManagement.Services.IServices;
using BookManagement.ViewModels.Cart;
using BookManagement.ViewModels.Book;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace BookManagement.Pages.Book
{
    public class DetailModel : PageModel
    {
        private readonly IBookService _bookService;
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;

        public DetailModel(IBookService bookService, ICartService cartService, IMapper mapper)
        {
            _bookService = bookService;
            _cartService = cartService;
            _mapper = mapper;
        }


        public BookViewModel Book { get; set; } = default!;

        [BindProperty]
        public int Quantity { get; set; } = 1;
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var bookEntity = await _bookService.GetBookByIdAsync(Id);
            if (bookEntity == null)
            {
                return NotFound();
            }
            Book = _mapper.Map<BookViewModel>(bookEntity);
            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync()
        {
            var bookEntity = await _bookService.GetBookByIdAsync(Id);
            if (bookEntity == null)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Auth/Login");
            }
            if(Quantity > bookEntity.Stock)
            {
                TempData["ErrorMessage"] = "Out of Stock!";
            }
            var cartItem = new CartItem
            {
                BookId = bookEntity.BookId,
                UserId = userId,
                Quantity = Quantity
            };
            var isExist = await _cartService.GetCartItemByBookIdAsync(bookEntity.BookId, userId);
            if(isExist == null)
            {
                await _cartService.AddCartItemAsync(cartItem);
            } else
            {
                isExist.Quantity += Quantity;
                await _cartService.UpdateCartItemAsync(isExist);
            }
            TempData["SuccessMessage"] = "Added to cart!";
            return RedirectToPage(new { id = Id });
        }

        public async Task<IActionResult> OnPostBuyNowAsync()
        {
            var result = await OnPostAddToCartAsync();
            if (result is RedirectToPageResult)
            {
                return RedirectToPage("/Cart/Index");
            }
            return result;
        }
    }
}
