using BookManagement.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookManagement.Pages
{
    public class ChatModel : PageModel
    {
        private readonly IChatBotService _chatBotService;

        public ChatModel(IChatBotService chatBotService)
        {
            _chatBotService = chatBotService;
        }

        [BindProperty]
        public string Prompt { get; set; }

        public string Response { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!string.IsNullOrWhiteSpace(Prompt))
            {
                Response = await _chatBotService.AskAsync(Prompt);
            }

            return Page();
        }
    }
}
