using BookManagement.BusinessObjects.Entities;
using BookManagement.ViewModels.Book;

namespace BookManagement.ViewModels.Home
{
    public class HomeViewModel
    {
        public List<Category> FeaturedCategories { get; set; } = new();
        public List<BookViewModel> BestsellingBooks { get; set; } = new();
        public List<BookViewModel> NewReleaseBooks { get; set; } = new();
    }

}

