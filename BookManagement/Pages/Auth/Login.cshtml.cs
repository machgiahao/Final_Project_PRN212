using AutoMapper;
using BookManagement.Services.DTOs.Auth;
using BookManagement.Services.IServices;
using BookManagement.ViewModels.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

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
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20)
            });

            return RedirectToPage("/Index");
        }
    }
}
