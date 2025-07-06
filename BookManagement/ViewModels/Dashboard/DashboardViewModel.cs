using BookManagement.ViewModels.Order;
using BookManagement.ViewModels.User;

namespace BookManagement.ViewModels.Dashboard
{
    public class DashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalOrders { get; set; }
        public int BooksSold { get; set; }
        public decimal TotalRevenue { get; set; }

        // In DashboardViewModel.cs
        public List<int> SalesByMonth { get; set; } = new();
        public List<string> CategoryLabels { get; set; } = new();
        public List<int> BooksSoldByCategory { get; set; } = new();
        public List<decimal> RevenueByCategory { get; set; } = new();
        public List<TopBookViewModel> TopSellingBooks { get; set; } = new();
    }
}
