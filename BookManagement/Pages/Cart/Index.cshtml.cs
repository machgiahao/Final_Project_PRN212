using AutoMapper;
using BookManagement.Services.IServices;
using BookManagement.ViewModels.Cart;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace BookManagement.Pages.Cart
{
    public class IndexModel : PageModel
    {
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;
        public IndexModel(ICartService cartService, IMapper mapper)
        {
            _cartService = cartService;
            _mapper = mapper;
        }

        public List<CartItemViewModel> CartItems { get; set; } = new();
        public decimal Subtotal => CartItems?.Sum(x => x.Total) ?? 0;

        public async Task OnGetAsync()
        {
            var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier) ?? "";
            var items = await _cartService.GetCartItemsByUserIdAsync(userId);
            CartItems = _mapper.Map<List<CartItemViewModel>>(items);
        }

        public async Task<IActionResult> OnPostRemoveItemAsync(int cartItemId)
        {
            var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier) ?? "";
            var item = await _cartService.GetCartItemByIdAsync(cartItemId);
            if (item == null || item.UserId != userId)
            {
                return NotFound();
            }
            await _cartService.DeleteCartItemAsync(cartItemId);
            return RedirectToPage();
        }
    }
}
