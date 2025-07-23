using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BookManagement.ViewModels.Category
{
    public class CreateCateViewModel
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name must be less than 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Range(0, 1, ErrorMessage = "Status must be 0 (inactive) or 1 (active).")]
        public int? Status { get; set; }

        [Display(Name = "Parent Category")]
        public int? ParentCategoryId { get; set; }
    }
}
