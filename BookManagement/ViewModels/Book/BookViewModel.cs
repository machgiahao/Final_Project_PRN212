﻿using BookManagement.BusinessObjects.Enum;

namespace BookManagement.ViewModels.Book
{
    public class BookViewModel
    {
        public int BookId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int Stock { get; set; }
        public int? Sold { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? Author { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime? CreatedAt { get; set; }
        public BookStatus? Status { get; set; }
        public string? StatusDisplay { get; set; }
    }
}
