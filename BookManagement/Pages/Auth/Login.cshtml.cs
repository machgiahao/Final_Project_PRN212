using AutoMapper;
using BookManagement.Services.DTOs.Auth;
using BookManagement.Services.IServices;
using BookManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookManagement.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public LoginModel(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public void OnGet()
        {
        }

        [BindProperty]
        public LoginViewModel LoginViewModel { get; set; } = new LoginViewModel();
        public string? ErrorMessage { get; set; }
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();
            var loginDto = _mapper.Map<LoginDto>(LoginViewModel);
            var user = await _userService.LoginAsync(loginDto);

            if (user == null)
            {
                ErrorMessage = "Invalid email or password.";
                return Page();
            }
            return RedirectToPage("/Index");
        }
    }
}
