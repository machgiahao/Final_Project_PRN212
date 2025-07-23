using AutoMapper;
using BookManagement.BusinessObjects.Entities;
using BookManagement.Services.DTOs.Mail;
using BookManagement.Services.IServices;
using BookManagement.Services.Services;
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
        private readonly IBookService _bookService;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        public PurchaseModel(ICartService cartService, IOrderService orderService, IOrderDetailService orderDetailService, IMapper mapper, IBookService bookService, IEmailService emailService)
        {
            _cartService = cartService;
            _orderService = orderService;
            _orderDetailService = orderDetailService;
            _mapper = mapper;
            _bookService = bookService;
            _emailService = emailService;
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
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
            order.Status = 0;
            order.TotalPrice = totalPrice ?? 0;

            await _orderService.AddOrderAsync(order);
            List<int> listSoldOutBookIds = new List<int>();

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
                var bookEntity = await _bookService.GetBookByIdAsync(item.BookId);
                if (item.Quantity == bookEntity.Stock)
                {
                    bookEntity.Status = BusinessObjects.Enum.BookStatus.SoldOut;
                    listSoldOutBookIds.Add(bookEntity.BookId);
                }
                await _orderDetailService.AddOrderDetailAsync(orderDetail);
                await _bookService.UpdateBookStockAsync(item.BookId, -item.Quantity);
            }

            // Delete item in cart has been purchased
            await _cartService.RemoveCartItemsAsync(userId, SelectedCartItemIds);
            await _cartService.DeleteSoldOutItems(listSoldOutBookIds);
            // Send email notification
            string email = User.FindFirstValue(ClaimTypes.Email) ?? "";
            string name = User.FindFirstValue(ClaimTypes.Name) ?? "";
            int orderId = order.OrderId;
            var emailDto = new EmailDto
            {
                ToEmail = email,
                RecipientName = Purchase.RecipientName,
                Address = Purchase.Address,
                PhoneNumber = Purchase.PhoneNumber,
                OrderId = order.OrderId,
                Subject = "🎉 Your Order Has Been Confirmed!"
            };
            emailDto.Body = EmailBody(emailDto);

            await _emailService.SendEmail(emailDto);
            TempData["SuccessMessage"] = "Purchase completed successfully!";
            return RedirectToPage("/Order/MyOrder");
        }

        private string EmailBody(EmailDto emailDto)
        {
            return $@"
            <div style='font-family:Segoe UI,Arial,sans-serif;background:#f6f8fa;padding:32px;'>
                <div style='max-width:520px;margin:auto;background:#fff;border-radius:16px;box-shadow:0 4px 24px rgba(60,72,88,0.10);padding:32px;'>
                    <h2 style='color:#222;font-size:2rem;font-weight:700;margin-bottom:8px;text-align:center;'>
                        Thank you, <span style='color:#1976d2'>{emailDto.RecipientName}</span>, for shopping at <span style='color:#e67e22;'>FBook</span>!
                    </h2>
                    <p style='text-align:center;font-size:1.1rem;margin-bottom:24px;'>
                        Your order <strong>#{emailDto.OrderId}</strong> has been successfully received.
                    </p>
                    <table style='width:100%;border-collapse:collapse;background:#f3f6f9;border-radius:8px;overflow:hidden;'>
                        <tr>
                            <td style='padding:12px 16px;font-weight:600;color:#555;border-bottom:1px solid #e0e6ed;'>Order ID</td>
                            <td style='padding:12px 16px;color:#222;border-bottom:1px solid #e0e6ed;'>{emailDto.OrderId}</td>
                        </tr>
                        <tr>
                            <td style='padding:12px 16px;font-weight:600;color:#555;border-bottom:1px solid #e0e6ed;'>Recipient Name</td>
                            <td style='padding:12px 16px;color:#222;border-bottom:1px solid #e0e6ed;'>{emailDto.RecipientName}</td>
                        </tr>
                        <tr>
                            <td style='padding:12px 16px;font-weight:600;color:#555;border-bottom:1px solid #e0e6ed;'>Address</td>
                            <td style='padding:12px 16px;color:#222;border-bottom:1px solid #e0e6ed;'>{emailDto.Address}</td>
                        </tr>
                        <tr>
                            <td style='padding:12px 16px;font-weight:600;color:#555;border-bottom:1px solid #e0e6ed;'>Phone Number</td>
                            <td style='padding:12px 16px;color:#222;border-bottom:1px solid #e0e6ed;'>{emailDto.PhoneNumber}</td>
                        </tr>
                        <tr>
                            <td style='padding:12px 16px;font-weight:600;color:#555;'>Email</td>
                            <td style='padding:12px 16px;color:#222;'>{emailDto.ToEmail}</td>
                        </tr>
                    </table>
                    <p style='margin-top:32px;text-align:center;font-size:1.1rem;color:#1976d2;'>
                        We will process and deliver your order as soon as possible.
                    </p>
                    <p style='margin-top:24px;text-align:center;color:#222;'>Best regards,<br/><strong>FBook</strong></p>
                    <hr style='margin:32px 0;border:none;border-top:1px solid #e0e6ed;'/>
                    <small style='color:#888;text-align:center;display:block;'>This is an automated email. Please do not reply.</small>
                </div>
            </div>";
        }
    }
}
