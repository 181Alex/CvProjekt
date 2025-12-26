using System.Diagnostics;
using CvProjekt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CvProjekt.Controllers
{
    public class ProfileController : Controller
    {
        private readonly CvContext _context;

        private readonly UserManager<User> _userManager;

        public ProfileController(CvContext context, UserManager<User> user)
        {
            _context = context;
            _userManager = user;
        }   

/*       [Authorize] */
        public async Task<IActionResult> MyProfile()
        {
           var userId = _userManager.GetUserId(User);

            var user = await _context.Users
                        .Include(u => u.Projects)
                        .Include(u => u.Resume)
                            .ThenInclude(r => r.Qualifications)
                        .Include(u => u.Resume)
                            .ThenInclude(r => r.WorkList)
                        .Include(u => u.Resume)
                            .ThenInclude(r => r.EducationList)
                        .FirstOrDefaultAsync(u => u.Id == userId);

            if(user == null){
                return NotFound();
            }

            return View(user);

        }

    }
}
