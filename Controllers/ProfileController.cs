using System.Diagnostics;
using CvProjekt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CvProjekt.Controllers
{
    public class ProfileController : Controller
    {
        private readonly CvContext context;

        private readonly UserManager<User> _userManager;

        public ProfileController(CvContext context, UserManager<User> user)
        {
            this.context = context;
            _userManager = user;
        }   

        [Authorize]
        public async Task<IActionResult> MyProfile()
        {
           var userId = _userManager.GetUserId(User);

            var user = await _context.Users
                        .Include(u => u.Projects)
                        .Include(u => u.Resume)
                            .ThenInclude(r => r.Quali)

        }

    }
}
