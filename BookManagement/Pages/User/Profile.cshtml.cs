using AutoMapper;
using BookManagement.Services.IServices;
using BookManagement.ViewModels.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookManagement.Pages.User
{
    public class ProfileModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public ProfileModel(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [BindProperty]
        public UserViewModel? UserProfile { get; set; }

        public async Task OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userId))
            {
                var user = await _userService.GetByIdAsync(userId);
                UserProfile = _mapper.Map<UserViewModel>(user);
            }
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userId))
            {
                var user = _mapper.Map<BookManagement.Services.DTOs.User.UserUpdateDto>(UserProfile);
                await _userService.UpdateAsync(userId, user);
                TempData["SuccessMessage"] = "Profile updated successfully!";
            }
            return Page();
        }
    }
}
