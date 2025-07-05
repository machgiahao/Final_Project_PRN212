namespace BookManagement.ViewModels.Cart
{
    public class PurchaseViewModel
    {
        public List<CartItemViewModel> SelectedItems { get; set; } = new();
        public List<int> SelectedCartItemIds { get; set; } = new();
        public string RecipientName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }
}
