using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookManagement.Services.IServices;
using BookManagement.ViewModels.Order;
using AutoMapper;
using BookManagement.ViewModels.Shared;
using BookManagement.ViewModels.User;
using BookManagement.Services.Services;

namespace BookManagement.Pages.Admin.Order
{
    public class IndexModel : PageModel
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public IndexModel(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
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
                TempData["Message"] = "Order status updated successfully.";
            }
            return RedirectToPage();
        }

    }
}