using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http; // Đảm bảo có namespace này

namespace BookManagement.Services.DTOs.Book
{
    public class CreateBookDto
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title can not be greater than 200 letters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Author is required")]
        [StringLength(100, ErrorMessage = "Author can not be greater than 100 letters")]
        public string Author { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select 1 Category")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "Price must being greater than 0")]
        public decimal? Price { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must being equal or greater than 0 ")]
        public int Quantity { get; set; } 

        [DataType(DataType.Upload)]
        [Required(ErrorMessage = "Please select a picture of book")]
        public IFormFile ImageFile { get; set; }

        public string? ImageUrl { get; set; }

        public string? Description { get; set; }
    }
}