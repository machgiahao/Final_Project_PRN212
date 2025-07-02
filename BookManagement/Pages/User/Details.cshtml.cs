using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BookManagement.BusinessObjects.Entities;
using BookManagement.DataAccess.Context;
using BookManagement.ViewModels.User;

namespace BookManagement.Pages.User
{
    public class DetailsModel : PageModel
    {
        private readonly BookManagement.DataAccess.Context.BookStoreDbContext _context;

        public DetailsModel(BookManagement.DataAccess.Context.BookStoreDbContext context)
        {
            _context = context;
        }

        public UserViewModel User { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            return Page();
        }
    }
}
