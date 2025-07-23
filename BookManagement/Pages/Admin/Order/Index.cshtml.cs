using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookManagement.Services.IServices;
using BookManagement.ViewModels.Order;
using AutoMapper;
using BookManagement.ViewModels.Shared;
using BookManagement.ViewModels.User;
using BookManagement.Services.Services;
using Microsoft.AspNetCore.SignalR;
using BookManagement.SignalR;
using BookManagement.BusinessObjects.Entities;
using System.Security.Claims;
using BookManagement.Services.DTOs.Mail;

namespace BookManagement.Pages.Admin.Order
{
    public class IndexModel : PageModel
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IHubContext<SignalRServer> _hubContext;
        private readonly IEmailService _emailService;
        public IndexModel(IOrderService orderService, IMapper mapper, IHubContext<SignalRServer> hubContext, IEmailService emailService)
        {
            _orderService = orderService;
            _mapper = mapper;
            _hubContext = hubContext;
            _emailService = emailService;
        }

        public IList<OrderViewModel> OrderViewModel { get; set; }
        public PaginationViewModel Pagination { get; set; } = new();
        [BindProperty(SupportsGet = true)]
        public OrderFilterViewModel Filter { get; set; } = new();
        public async Task OnGetAsync(int pageNumber = 1)
        {
            var pagedResult = await _orderService.GetAllOrdersAsync(
                                        Filter.PageNumber,
                                        Filter.PageSize,
                                        Filter.RecipientName,
                                        Filter.Status);
            OrderViewModel = _mapper.Map<List<OrderViewModel>>(pagedResult.Items);
            Pagination = new PaginationViewModel
            {
                CurrentPage = Filter.PageNumber,
                PageSize = Filter.PageSize,
                TotalCount = pagedResult.TotalCount,
                TotalPages = (int)Math.Ceiling((double)pagedResult.TotalCount / Filter.PageSize)
            };
        }

        public async Task<IActionResult> OnPostUpdateStatusAsync(int orderId, int newStatus)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order != null)
            {
                order.Status = newStatus;
                await _orderService.UpdateOrderAsync(order);
                var statusText = GetStatusText(newStatus);
                var emailDto = new EmailDto
                {
                    ToEmail = order.User.Email,
                    RecipientName = order.RecipientName,
                    Address = order.ShippingAddress,
                    PhoneNumber = order.PhoneNumber,
                    OrderId = order.OrderId,
                    Subject = $"Your order #{order.OrderId} status: {statusText}",
                    Body = EmailBody(order, statusText)
                };
                await _emailService.SendEmail(emailDto);

                await _hubContext.Clients.User(order.UserId).SendAsync("OrderStatusChanged", orderId, GetStatusText(newStatus));
                TempData["Message"] = "Order status updated successfully.";
            }
            return RedirectToPage();
        }

        private string GetStatusText(int status)
        {
            return status switch
            {
                0 => "Pending",
                1 => "Confirmed",
                2 => "Shipping",
                3 => "Completed",
                4 => "Cancelled",
                _ => "Unknown"
            };
        }
        private string EmailBody(BusinessObjects.Entities.Order order, string statusText)
        {
            return $@"
        <div style='font-family:Segoe UI,Arial,sans-serif;background:#f6f8fa;padding:32px;'>
            <div style='max-width:520px;margin:auto;background:#fff;border-radius:16px;box-shadow:0 4px 24px rgba(60,72,88,0.10);padding:32px;'>
                <h2 style='color:#222;font-size:2rem;font-weight:700;margin-bottom:8px;text-align:center;'>
                    Order Status Update
                </h2>
                <p style='text-align:center;font-size:1.1rem;margin-bottom:24px;'>
                    Your order <strong>#{order.OrderId}</strong> status has been updated to <strong style='color:#1976d2'>{statusText}</strong>.
                </p>
                <table style='width:100%;border-collapse:collapse;background:#f3f6f9;border-radius:8px;overflow:hidden;'>
                    <tr>
                        <td style='padding:12px 16px;font-weight:600;color:#555;border-bottom:1px solid #e0e6ed;'>Order ID</td>
                        <td style='padding:12px 16px;color:#222;border-bottom:1px solid #e0e6ed;'>{order.OrderId}</td>
                    </tr>
                    <tr>
                        <td style='padding:12px 16px;font-weight:600;color:#555;border-bottom:1px solid #e0e6ed;'>Recipient Name</td>
                        <td style='padding:12px 16px;color:#222;border-bottom:1px solid #e0e6ed;'>{order.RecipientName}</td>
                    </tr>
                    <tr>
                        <td style='padding:12px 16px;font-weight:600;color:#555;border-bottom:1px solid #e0e6ed;'>Address</td>
                        <td style='padding:12px 16px;color:#222;border-bottom:1px solid #e0e6ed;'>{order.ShippingAddress}</td>
                    </tr>
                    <tr>
                        <td style='padding:12px 16px;font-weight:600;color:#555;border-bottom:1px solid #e0e6ed;'>Phone Number</td>
                        <td style='padding:12px 16px;color:#222;border-bottom:1px solid #e0e6ed;'>{order.PhoneNumber}</td>
                    </tr>
                    <tr>
                        <td style='padding:12px 16px;font-weight:600;color:#555;'>Email</td>
                        <td style='padding:12px 16px;color:#222;'>{order.User?.Email}</td>
                    </tr>
                </table>
                <p style='margin-top:32px;text-align:center;font-size:1.1rem;color:#1976d2;'>
                    If you have any questions, please contact us.
                </p>
                <p style='margin-top:24px;text-align:center;color:#222;'>Best regards,<br/><strong>FBook</strong></p>
                <hr style='margin:32px 0;border:none;border-top:1px solid #e0e6ed;'/>
                <small style='color:#888;text-align:center;display:block;'>This is an automated email. Please do not reply.</small>
            </div>
        </div>";
        }


    }
}