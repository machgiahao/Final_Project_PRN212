namespace BookManagement.ViewModels.Category
{
    public class CateFilterViewModel
    {
        public string? Name { get; set; }
        public int? Status { get; set; }
        public string? ParentCategoryName { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
    }
}
