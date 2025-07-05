using BookManagement.BusinessObjects.Entities;

namespace BookManagement.ViewModels.Order
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public string? UserId { get; set; }
        public DateTime? OrderDate { get; set; }
        public int? Status { get; set; }
        public string StatusText => Status switch
        {
            0 => "Pending",
            1 => "Confirmed",
            2 => "Shipping",
            3 => "Completed",
            4 => "Cancelled",
            _ => "Unknown"
        };
        public decimal TotalPrice { get; set; }
        public string? RecipientName { get; set; }
        public string? ShippingAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
