using System.Diagnostics;
using CvProjekt.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CvProjekt.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly CvContext _context;

        private readonly UserManager<User> _userManager;

        public ProfileController(CvContext context, UserManager<User> user)
        {
            _context = context;
            _userManager = user;
        }   

        [HttpGet]
        public async Task<IActionResult> Settings()
        {
            var nowUser = await _userManager.GetUserAsync(User);
            if(nowUser== null){
                return Content("Fel: Ingen inloggad användare hittades.");
            }
            return View(nowUser);
        }

        [HttpPost]
        public async Task<IActionResult> Avaktivera()
        {
            var user = await _userManager.GetUserAsync(User);
            user.IsActive = false;
            _context.Update(user);
            return RedirectToAction("MyProfile");
        }
        [HttpPost]
        public async Task<IActionResult> Aktivera()
        {
            var user = await _userManager.GetUserAsync(User);
            user.IsActive = true;
            _context.Update(user);
            return RedirectToAction("MyProfile");
        }
        [HttpPost]
        public async Task<IActionResult> PasswordTime()
        {
            var nowUser = await _userManager.GetUserAsync(User);
            if (nowUser == null)
            {
                return Content("Fel: Ingen inloggad användare hittades.");
            }
            return View(nowUser);
        }




        [HttpGet]
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

            if (user == null)
            {
                return Content($"Fel: Hittade ingen användare med ID '{userId}' i databasen. Har du kört database update?");
            }

            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> ViewProfile(string id)
            {
                if (string.IsNullOrEmpty(id))
                    return BadRequest("Ingen användar-id skickades.");

                var profileUser = await _context.Users
                    .Include(u => u.Projects)
                    .Include(u => u.Resume)
                        .ThenInclude(r => r.Qualifications)
                    .Include(u => u.Resume)
                        .ThenInclude(r => r.WorkList)
                    .Include(u => u.Resume)
                        .ThenInclude(r => r.EducationList)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (profileUser == null)
                    return NotFound("Användaren hittades inte.");

                // 🔹 Hämta inloggad användare (kan vara null)
                var currentUser = await _userManager.GetUserAsync(User);

                // 🔹 Räkna endast om man tittar på någon annans profil
                if (currentUser == null || currentUser.Id != profileUser.Id)
                {
                    profileUser.ProfileVisits++;
                    await _context.SaveChangesAsync();
                }

                return View("MyProfile", profileUser);
            }


        

        public async Task<IActionResult> EditResume()
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

            if (currentUser.Resume == null)
            {
                currentUser.Resume = new Resume();
            }

            currentUser.FirstName = updatedUser.FirstName;
            currentUser.LastName = updatedUser.LastName;
            currentUser.Adress = updatedUser.Adress;
            currentUser.Email = updatedUser.Email;
            currentUser.ImgUrl = updatedUser.ImgUrl;

            _context.Update(currentUser);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Uppdaterat basinformation";
            TempData["ActiveTab"] = "base";

            return RedirectToAction("EditResume");
        }

        [HttpPost]
        public async Task<IActionResult> EditQualifications(User updatedUser)
        {

            var currentUser = await _context.Users
                .Include(u => u.Resume)
                .ThenInclude(r => r.Qualifications)
            .FirstOrDefaultAsync(u => u.Id == updatedUser.Id);

            if (currentUser == null)
            {       
                return Content("Hittar ej användare i databas");
            }

            if (currentUser.Resume == null)
            {
                currentUser.Resume = new Resume();
            }

            currentUser.Resume.Qualifications.Clear();

            if(updatedUser.Resume?.Qualifications != null)
            {
                foreach(var q in updatedUser.Resume.Qualifications)
                {
                    if (!string.IsNullOrWhiteSpace(q.Name))
                    {
                        currentUser.Resume.Qualifications.Add(new Qualification{
                            Name = q.Name, 
                            ResumeId = currentUser.Resume.Id 
                        });
                    }
                }
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Kompetenser sparade";
            TempData["ActiveTab"] = "qualif";
            
            return RedirectToAction("EditResume");

        }

        [HttpPost]
        public async Task<IActionResult> EditEducation(User updatedUser)
        {

            var currentUser = await _context.Users
                .Include(u => u.Resume)
                .ThenInclude(r => r.EducationList)
            .FirstOrDefaultAsync(u => u.Id == updatedUser.Id);

            if (currentUser == null)
            {       
                return Content("Hittar ej användare i databas");
            }

            if (currentUser.Resume == null)
            {
                currentUser.Resume = new Resume();
            }

            currentUser.Resume.EducationList.Clear();

            if(updatedUser.Resume?.EducationList != null)
            {
                foreach(var e in updatedUser.Resume.EducationList)
                {
                    if (!string.IsNullOrWhiteSpace(e.SchoolName) && !string.IsNullOrWhiteSpace(e.DegreeName) && e.StartYear != null )
                    {
                        currentUser.Resume.EducationList.Add(new Education{
                            SchoolName = e.SchoolName, 
                            DegreeName = e.DegreeName,
                            StartYear = e.StartYear,
                            EndYear = e.EndYear,
                            Description = e.Description,
                            ResumeId = currentUser.Resume.Id 
                        });
                    }
                }
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Utbildning sparad";
            TempData["ActiveTab"] = "edu";
            
            return RedirectToAction("EditResume");

        }

        [HttpPost]
        public async Task<IActionResult> EditWork(User updatedUser)
        {

            var currentUser = await _context.Users
                .Include(u => u.Resume)
                .ThenInclude(r => r.WorkList)
            .FirstOrDefaultAsync(u => u.Id == updatedUser.Id);

            if (currentUser == null)
            {       
                return Content("Hittar ej användare i databas");
            }

            if (currentUser.Resume == null)
            {
                currentUser.Resume = new Resume();
            }

            currentUser.Resume.WorkList.Clear();

            if(updatedUser.Resume?.WorkList != null)
            {
                foreach(var w in updatedUser.Resume.WorkList)
                {
                    if (!string.IsNullOrWhiteSpace(w.CompanyName) && !string.IsNullOrWhiteSpace(w.Position) && w.StartDate != null )
                    {
                        currentUser.Resume.WorkList.Add(new Work{
                            CompanyName = w.CompanyName, 
                            Position = w.Position,
                            StartDate = w.StartDate,
                            EndDate = w.EndDate,
                            Description = w.Description,
                            ResumeId = currentUser.Resume.Id 
                        });
                    }
                }
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Arbete sparad";
            TempData["ActiveTab"] = "work";
            
            return RedirectToAction("EditResume");

        }

    }
}
