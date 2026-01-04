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
            await _userManager.UpdateAsync(user);
            _context.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("MyProfile");
        }

        [HttpPost]
        public async Task<IActionResult> Aktivera()
        {
            var user = await _userManager.GetUserAsync(User);
            user.IsActive = true;
            await _userManager.UpdateAsync(user);
            _context.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("MyProfile");
        }
        [HttpPost]
        public async Task<IActionResult> GoPublic()
        {
            var user = await _userManager.GetUserAsync(User);
            user.IsPrivate = false;
            await _userManager.UpdateAsync(user);
            _context.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("MyProfile");
        }

        [HttpPost]
        public async Task<IActionResult> GoPrivate()
        {
            var user = await _userManager.GetUserAsync(User);
            user.IsPrivate = true;
            await _userManager.UpdateAsync(user);
            _context.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("MyProfile");
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var nowUser = await _userManager.GetUserAsync(User);
            if (nowUser == null)
            {
                return Content("Fel: Ingen inloggad användare hittades.");
            }
            return View(new ChangePasswordViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> ChangePasswordNow(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("ChangePassword", model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Ändra lösenordet
            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View("ChangePassword", model);
            }

            return RedirectToAction("Settings");
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

                var currentUser = await _userManager.GetUserAsync(User);

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

            //ignorera alla modelstate som inte har att göra med kompetenser
            var qualifKey = ModelState.Keys.Where(k => k.StartsWith("Resume.Qualifications") && k.EndsWith(".Name")).ToList();
            var allKeys = ModelState.Keys.ToList();
            
            foreach (var key in allKeys)
            {
                if (!qualifKey.Contains(key))
                {
                    ModelState.Remove(key);
                }
            }

   
            //kontrollerar om kompetenser är giltiga, men kopierar över datan så att det användaren skrev är kvar 
            if (!ModelState.IsValid)
            {
                
                currentUser.Resume.Qualifications.Clear();
                if (updatedUser.Resume?.Qualifications != null)
                {
                    foreach(var q in updatedUser.Resume.Qualifications)
                    {
                        currentUser.Resume.Qualifications.Add(new Qualification { 
                            Name = q.Name, 
                            ResumeId = currentUser.Resume.Id 
                        });
                    }
                }

                TempData["ActiveTab"] = "qualif";
                return View("EditResume", currentUser);
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

            //ignorera alla modelstate som inte har att göra med utbildning
            var eduKey = ModelState.Keys.Where(k => k.StartsWith("Resume.EducationList") 
                                                && k.EndsWith(".SchoolName") || 
                                                k.EndsWith(".DegreeName") ||
                                                k.EndsWith(".StartYear"))
                                                .ToList();
            var allKeys = ModelState.Keys.ToList();
            
            foreach (var key in allKeys)
            {
                if (!eduKey.Contains(key))
                {
                    ModelState.Remove(key);
                }
            }

   
            //kontrollerar om kompetenser är giltiga, men kopierar över datan så att det användaren skrev är kvar 
            if (!ModelState.IsValid)
            {
                
                currentUser.Resume.EducationList.Clear();

                if(updatedUser.Resume?.EducationList != null)
                {
                    foreach(var e in updatedUser.Resume.EducationList)
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

                TempData["ActiveTab"] = "edu";
                return View("EditResume", currentUser);
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

            //ignorera alla modelstate som inte har att göra med arbete
            var eduKey = ModelState.Keys.Where(k => k.StartsWith("Resume.WorkList") 
                                                && k.EndsWith(".CompanyName") || 
                                                k.EndsWith(".Position") ||
                                                k.EndsWith(".StartDate"))
                                                .ToList();
            var allKeys = ModelState.Keys.ToList();
            
            foreach (var key in allKeys)
            {
                if (!eduKey.Contains(key))
                {
                    ModelState.Remove(key);
                }
            }

   
            //kontrollerar om kompetenser är giltiga, men kopierar över datan så att det användaren skrev är kvar 
            if (!ModelState.IsValid)
            {
                
                currentUser.Resume.WorkList.Clear();

                if(updatedUser.Resume?.WorkList != null)
                {
                    foreach(var w in updatedUser.Resume.WorkList)
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

                TempData["ActiveTab"] = "work";
                return View("EditResume", currentUser);
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
