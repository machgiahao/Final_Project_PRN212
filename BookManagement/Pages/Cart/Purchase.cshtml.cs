using AutoMapper;
using BookManagement.BusinessObjects.Entities;
using BookManagement.Services.IServices;
using BookManagement.ViewModels.Cart;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace BookManagement.Pages.Cart
{
    public class PurchaseModel : PageModel
    {
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IMapper _mapper;
        public PurchaseModel(ICartService cartService, IOrderService orderService, IOrderDetailService orderDetailService , IMapper mapper)
        {
            _cartService = cartService;
            _orderService = orderService;
            _orderDetailService = orderDetailService;
            _mapper = mapper;
        }

        [BindProperty]
        public List<int> SelectedCartItemIds { get; set; } = new();

        public List<CartItemViewModel> SelectedItems { get; set; } = new();

        public decimal Total { get; set; }

        public async Task<IActionResult> OnPost()
        {
            // Lấy cart từ session hoặc database
            var cart = await GetCartFromSessionOrDb();

            // Lọc các item được chọn
            SelectedItems = cart
                .Where(x => SelectedCartItemIds.Contains(x.CartItemId))
                .ToList();

            Total = SelectedItems.Sum(x => x.Total);

            return Page();
        }

        private async Task<List<CartItemViewModel>> GetCartFromSessionOrDb()
        {
            var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier) ?? "";
            var cartItems = await _cartService.GetCartItemsByUserIdAsync(userId);
            return _mapper.Map<List<CartItemViewModel>>(cartItems);
        }

        [BindProperty]
        public PurchaseViewModel Purchase { get; set; }
        public async Task<IActionResult> OnPostConfirmPurchaseAsync()
        {
            var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier) ?? "";
            var cartItems = await _cartService.GetCartItemsByUserIdAsync(userId);
            
            var selectedItems = cartItems
                .Where(x => SelectedCartItemIds.Contains(x.CartItemId))
                .ToList();
            if (!selectedItems.Any())
            {
                ModelState.AddModelError("", "No items selected for purchase.");
                return Page();
            }
            //Total price
            var totalPrice = selectedItems.Sum(x => x.Book.Price * x.Quantity);
            var order = _mapper.Map<BookManagement.BusinessObjects.Entities.Order>(Purchase);
            order.UserId = userId;
            order.OrderDate = DateTime.Now;
            order.Status = 0; // Pending, hoặc giá trị phù hợp với hệ thống của bạn
            order.TotalPrice = totalPrice ?? 0;

            await _orderService.AddOrderAsync(order);

            // Save order detail
            foreach (var item in selectedItems)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = order.OrderId,
                    BookId = item.BookId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Book.Price.GetValueOrDefault()
                };
                await _orderDetailService.AddOrderDetailAsync(orderDetail);
            }

            // Xóa các item đã thanh toán khỏi cart
            await _cartService.RemoveCartItemsAsync(userId, SelectedCartItemIds);
            TempData["SuccessMessage"] = "Purchase completed successfully!";
            return RedirectToPage("/Order/MyOrder");
        }
    }
}
