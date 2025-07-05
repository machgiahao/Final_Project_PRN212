namespace BookManagement.ViewModels.Cart
{
    public class CartItemViewModel
    {
        public int CartItemId { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; } = "";
        public string BookImageUrl { get; set; } = "";
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public decimal Total => Price * Quantity;
    }
}
