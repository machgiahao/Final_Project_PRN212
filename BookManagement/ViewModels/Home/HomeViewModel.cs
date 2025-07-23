using BookManagement.BusinessObjects.Entities;
using BookManagement.ViewModels.Book;
using BookManagement.ViewModels.Category;

namespace BookManagement.ViewModels.Home
{
    public class HomeViewModel
    {
        public List<CateViewModel> FeaturedCategories { get; set; } = new();
        public List<BookViewModel> BestsellingBooks { get; set; } = new();
        public List<BookViewModel> NewReleaseBooks { get; set; } = new();
    }

}

