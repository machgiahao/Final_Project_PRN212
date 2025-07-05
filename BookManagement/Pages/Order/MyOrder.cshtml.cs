using AutoMapper;
using BookManagement.Services.IServices;
using BookManagement.ViewModels.Order;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookManagement.Pages.Order
{
    public class MyOrderModel : PageModel
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public MyOrderModel(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        public List<OrderViewModel> Orders { get; set; } = new();

        public async Task OnGetAsync(string? status)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var orders = await _orderService.GetOrdersByUserIdAsync(userId);
            Orders = _mapper.Map<List<OrderViewModel>>(orders);
            if (!string.IsNullOrEmpty(status))
            {
                Orders = Orders.Where(o => o.StatusText == status).ToList();
            }
        }

        public async Task<IActionResult> OnPostCancel(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            var orderViewModel = _mapper.Map<OrderViewModel>(order);
            if (orderViewModel != null && orderViewModel.StatusText == "Pending")
            {
                order.Status = 4;
                await _orderService.UpdateOrderAsync(order);
                TempData["SuccessMessage"] = "Order cancelled successfully.";
            }
            return RedirectToPage();
        }

    }
}
