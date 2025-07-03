
using BookManagement.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookManagement.ViewComponents
{
    public class CartBadgeViewComponent : ViewComponent
    {
        private readonly ICartService _cartService;
        public CartBadgeViewComponent(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = User as ClaimsPrincipal;
            string userId = user?.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            var cartCount = await _cartService.GetCartCountAsync(userId);
            return View(cartCount);
        }
    }
}
