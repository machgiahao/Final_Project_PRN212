using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookManagement.Services.IServices;
using BookManagement.ViewModels.Dashboard;
using AutoMapper;
using System.Linq;
using BookManagement.Services.Services;

namespace BookManagement.Pages.Dashboard
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;
        private readonly ICategoryService _categoryService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IMapper _mapper;

        public IndexModel(
            IUserService userService,
            IOrderService orderService,
            IOrderDetailService orderDetailService,
            ICategoryService categoryService,
            IMapper mapper)
        {
            _userService = userService;
            _orderService = orderService;
            _orderDetailService = orderDetailService;
            _categoryService = categoryService;
            _mapper = mapper;
        }

        public DashboardViewModel Dashboard { get; set; } = new();

        public async Task OnGetAsync()
        {
            // Tổng số user
            var pagedUsers = await _userService.GetAllUsersAsync(1, int.MaxValue, null, null, null);
            Dashboard.TotalUsers = pagedUsers.TotalCount;

            // Tổng số order
            var allOrders = (await _orderService.GetAllOrdersAsync()).ToList();
            Dashboard.TotalOrders = allOrders.Count;

            // Tổng số sách đã bán
            var allOrderDetails = (await _orderDetailService.GetAllOrderDetailsAsync()).ToList();
            Dashboard.BooksSold = allOrderDetails.Sum(od => od.Quantity);

            // Tổng doanh thu (chỉ tính đơn đã hoàn thành, ví dụ Status == 3)
            Dashboard.TotalRevenue = allOrders
                .Where(o => o.Status.HasValue && o.Status.Value == 3)
                .Sum(o => o.TotalPrice);

            // Dữ liệu cho biểu đồ
            Dashboard.SalesByMonth = Enumerable.Range(1, 12)
                .Select(m => allOrders.Count(o => o.OrderDate?.Month == m && o.OrderDate?.Year == DateTime.Now.Year))
                .ToList();

            // Lấy danh sách category từ DB
            var categories = await _categoryService.GetAllCategoriesAsync(1, int.MaxValue, null, null, null);
            Dashboard.CategoryLabels = categories.Items.Select(c => c.Name).ToList();

            // Lấy tất cả order details và books
            var allBooks = allOrderDetails.Select(od => od.Book).ToList();

            // BooksSoldByCategory
            Dashboard.BooksSoldByCategory = categories.Items
                .Select(cat =>
                    allOrderDetails
                        .Where(od => od.Book != null && od.Book.CategoryId == cat.CategoryId)
                        .Sum(od => od.Quantity)
                ).ToList();

            // RevenueByCategory
            Dashboard.RevenueByCategory = categories.Items
                .Select(cat =>
                    allOrderDetails
                        .Where(od => od.Book != null && od.Book.CategoryId == cat.CategoryId)
                        .Sum(od => od.Quantity * od.UnitPrice)
                ).ToList();

            // Lấy danh sách sách bán chạy nhất
            Dashboard.TopSellingBooks = allOrderDetails
                .GroupBy(od => od.Book.Title)
                .Select(g => new TopBookViewModel
                {
                    Title = g.Key,
                    QuantitySold = g.Sum(od => od.Quantity)
                })
                .OrderByDescending(x => x.QuantitySold)
                .Take(5)
                .ToList();

        }
    }
}
