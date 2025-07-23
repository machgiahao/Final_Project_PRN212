namespace BookManagement.ViewModels.Category
{
    public class CateViewModel
    {
        public int CategoryId { get; set; }
        public string? Name { get; set; }
        public int? Status { get; set; }
        public int? ParentCategoryId { get; set; }
        public string? ParentCategoryName { get; set; }
    }
}
