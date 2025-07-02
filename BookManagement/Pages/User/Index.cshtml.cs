using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BookManagement.BusinessObjects.Entities;
using BookManagement.DataAccess.Context;
using BookManagement.Services.IServices;
using AutoMapper;
using BookManagement.ViewModels.User;
using BookManagement.ViewModels.Shared;
using BookManagement.BusinessObjects.Commons;
using Microsoft.AspNetCore.Authorization;
using BookManagement.Services.DTOs.User;
using Microsoft.AspNetCore.SignalR;

namespace BookManagement.Pages.User
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {

        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public IndexModel(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public IList<UserViewModel> UserViewModel { get; set; } = default!;
        public PaginationViewModel Pagination { get; set; } = new();
        [BindProperty(SupportsGet = true)]
        public UserFilterViewModel Filter { get; set; } = new UserFilterViewModel();
        public async Task OnGetAsync(int pageNumber = 1)
        {
            var pagedResult = await _userService.GetAllUsersAsync(
                Filter.PageNumber,
                Filter.PageSize,
                Filter.Name,
                Filter.Email,
                Filter.Phone);

            UserViewModel = _mapper.Map<List<UserViewModel>>(pagedResult.Items);

            Pagination = new PaginationViewModel
            {
                CurrentPage = Filter.PageNumber,
                PageSize = Filter.PageSize,
                TotalCount = pagedResult.TotalCount,
                TotalPages = (int)Math.Ceiling((double)pagedResult.TotalCount / Filter.PageSize)
            };
        }

        [BindProperty]
        public CreateUserViewModel CreateUserViewModel { get; set; } = default!;
        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key]?.Errors;
                    if (errors != null && errors.Count > 0)
                    {
                        foreach (var error in errors)
                        {
                            Console.WriteLine($"ModelState error for '{key}': {error.ErrorMessage}");
                        }
                    }
                }
                ViewData["ShowCreateModal"] = true;
                return Page();
            }
            try
            {
                var createUserDto = _mapper.Map<CreateUserDto>(CreateUserViewModel);
                var user = await _userService.CreateAsync(createUserDto);
                TempData["Message"] = "User created successfully!";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"User created fail! {ex.Message}");
                ViewData["ShowCreateModal"] = true;
                return Page();
            }
        }

        public async Task<IActionResult> OnGetEditAsync(string id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();
            await OnGetAsync(Filter.PageNumber);
            EditUserViewModel = _mapper.Map<EditUserViewModel>(user);
            ViewData["ShowEditModal"] = true;
            return Page();
        }

        [BindProperty]
        public EditUserViewModel EditUserViewModel { get; set; } = default!;
        public async Task<IActionResult> OnPostEditAsync()
        {
            if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key]?.Errors;
                    if (errors != null && errors.Count > 0)
                    {
                        foreach (var error in errors)
                        {
                            Console.WriteLine($"ModelState error for '{key}': {error.ErrorMessage}");
                        }
                    }
                }
                ViewData["ShowEditModal"] = true;
                return Page();
            }
            var updateDto = _mapper.Map<UserUpdateDto>(EditUserViewModel);
            var result = await _userService.UpdateAsync(EditUserViewModel.UserId, updateDto);
            if (result)
            {
                TempData["Message"] = "User updated successfully!";
                return RedirectToPage("./Index");
            }
            ModelState.AddModelError(string.Empty, "Update failed!");
            ViewData["ShowEditModal"] = true;
            return Page();

        }

        [BindProperty]
        public int DeleteUserId { get; set; }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            await _userService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Deleted successfully!";
            return RedirectToPage("./Index");
        }

    }
}
