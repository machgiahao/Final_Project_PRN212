using AutoMapper;
using BookManagement.BusinessObjects.Entities;
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
        private readonly IOrderDetailService _orderDetailService;
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;

        public MyOrderModel(IOrderService orderService, IOrderDetailService orderDetailService, IBookService bookService, IMapper mapper)
        {
            _orderService = orderService;
            _orderDetailService = orderDetailService;
            _bookService = bookService;
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
                IEnumerable<OrderDetail> orderDetails = await _orderDetailService.GetOrderDetailsByOrderIdAsync(order.OrderId);
                foreach (var orderDetail in orderDetails)
                {
                    var book = await _bookService.GetBookByIdAsync(orderDetail.BookId);
                    if (book != null)
                    {
                        if (book.Stock == 0)
                        {
                            book.Status = 0;
                        }
                        book.Stock += orderDetail.Quantity;
                        book.Sold -= orderDetail.Quantity;
                        await _bookService.UpdateBookAsync(book);
                    }
                }
                await _orderService.UpdateOrderAsync(order);
                TempData["SuccessMessage"] = "Order cancelled successfully.";
            }
            return RedirectToPage();
        }

    }
}
