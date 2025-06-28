using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookManagement.ViewModels;
using BookManagement.ViewModels.Auth;
using System.Threading.Tasks;
using BookManagement.Services.DTOs.Auth;
using AutoMapper;
using BookManagement.Services.IServices;

namespace BookManagement.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public RegisterModel(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [BindProperty]
        public RegisterViewModel RegisterViewModel { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var registerDto = _mapper.Map<RegisterDto>(RegisterViewModel);
            var user = await _userService.RegisterAsync(registerDto);
            ViewData["SuccessMessage"] = "Registration successfully !";
            return RedirectToPage("/Auth/Login");
        }
    }
}
