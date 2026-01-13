using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CvProjekt.ViewModels;
using Models;
using DAL;



namespace CvProjekt.ViewComponents
{
    public class UnreadMessagesViewComponent :ViewComponent{

        private readonly CvContext _context;
        private readonly UserManager<User> _userManager;

        public UnreadMessagesViewComponent(CvContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            // Om ingen är inloggad, visa 0
            if (user == null) return View(0);

            // Räkna olästa meddelanden till mig
            var count = await _context.Messages
                .Where(m => m.ToUserId == user.Id && !m.Read)
                .CountAsync();

            return View(count);
        }
    }
}










