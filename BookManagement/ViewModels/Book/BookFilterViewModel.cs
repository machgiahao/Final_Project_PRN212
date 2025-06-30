namespace BookManagement.ViewModels.Book
{
    public class BookFilterViewModel
    {
        public List<int> SelectedCategories { get; set; } = new();
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
