using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookManagement.Services.IServices;
using BookManagement.ViewModels.Order;
using AutoMapper;

namespace BookManagement.Pages.Admin.Order
{
    public class DetailModel : PageModel
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public DetailModel(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        public OrderViewModel? Order { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            Order = _mapper.Map<OrderViewModel>(order);
            return Page();
        }
    }
}
