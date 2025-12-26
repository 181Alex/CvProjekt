using System.Diagnostics;
using CvProjekt.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CvProjekt.Controllers
{
    //[Authorize]
    public class ProfileController : Controller
    {
        private readonly CvContext _context;

        private readonly UserManager<User> _userManager;

        public ProfileController(CvContext context, UserManager<User> user)
        {
            _context = context;
            _userManager = user;
        }   


        public async Task<IActionResult> MyProfile()
        {
            Console.WriteLine("Nu körs MyProfile action!");
            
            var userId = "u3";
               
               //_userManager.GetUserId(User);

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
                return Content($"Fel: Hittade ingen användare med ID '{userId}' i databasen. Har du kört database update?");
            }

            return View(user);

        }

    }
}
