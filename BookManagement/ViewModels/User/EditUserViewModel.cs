using System.ComponentModel.DataAnnotations;

namespace BookManagement.ViewModels.User
{
    public class EditUserViewModel
    {
        public string? UserId { get; set; } = default!;
        [StringLength(100, ErrorMessage = "Full name must be less than 100 characters")]
        public string? FullName { get; set; }
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }
        [StringLength(20, ErrorMessage = "Phone number must be less than 20 characters")]
        public string? PhoneNumber { get; set; }
        [StringLength(200, ErrorMessage = "Address must be less than 200 characters")]
        public string? Address { get; set; }
        [StringLength(500, ErrorMessage = "Bio must be less than 500 characters")]
        public string? Bio { get; set; }
        [Required(ErrorMessage = "Role is required")]
        [StringLength(50, ErrorMessage = "Role must be less than 50 characters")]
        public string? Role { get; set; }
    }
}
