namespace BookManagement.ViewModels.Order
{
    public class OrderFilterViewModel
    {
        public string? RecipientName { get; set; }
        public int? Status { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
