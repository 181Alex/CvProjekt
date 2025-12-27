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
            
            var userId = "user-1";
               
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

        public async Task<IActionResult> EditResume()
        {
            
            var userId = "user-1";
               
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

        [HttpPost]

        public async Task<IActionResult> EditResumeInfo(User updatedUser)
        {

            if (!ModelState.IsValid)
            {

                return View("EditResume", updatedUser);
            }

            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == updatedUser.Id);

            if (currentUser == null)
            {       
                return Content("Hittar ej användare i databas");
            }

            currentUser.FirstName = updatedUser.FirstName;
            currentUser.LastName = updatedUser.LastName;
            currentUser.Adress = updatedUser.Adress;
            currentUser.Email = updatedUser.Email;
            currentUser.ImgUrl = updatedUser.ImgUrl;

            _context.Update(currentUser);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Uppdaterad";

            return RedirectToAction("EditResume");
        }

    }
}
