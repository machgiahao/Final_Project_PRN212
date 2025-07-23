using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookManagement.BusinessObjects.Enum;

namespace BookManagement.Services.DTOs.Book
{
    public class BookPagedQueryDto
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public List<int>? SelectedCategories { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? Title { get; set; }
        public string? Role { get; set; }
        public BookStatus? Status { get; set; }
    }
}
