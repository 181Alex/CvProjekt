using CvProjekt.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Xml.Serialization;

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
            return View(nowUser);
        }
        [HttpGet]
        public async Task<IActionResult> DownloadData()
        {
            var user = await _userManager.GetUserAsync(User);
            //tar fram användaren och alla dessa erfarenheter osv. VIktigt är att denna är seperat från resume till en blrjan!!!
            var userObject = await _context.Users
                .Include (u=>u.Projects)
                .Include(u => u.Resume)
                .ThenInclude(r => r.WorkList)
                .Include(u => u.Resume)
                .ThenInclude(r => r.Qualifications)
                .Include(u => u.Resume)
                .ThenInclude(r => r.EducationList)
                .Where(u => u.Id == user.Id)
                .FirstOrDefaultAsync();
            if (userObject == null) { ModelState.AddModelError("SenderName", "User not found"); }
            var exporten = new UserExportDto
            {
                FirstName = userObject.FirstName,
                LastName = userObject.LastName,
                Email = userObject.Email,
                Adress = userObject.Adress,
                ImgUrl = userObject.ImgUrl,
                Projects = userObject.Projects.Select(p => new ProjectExportDto // som det står i exportmodels så skapar man en lista av projektexportdto, istälelt för lsita av project
                {
                    Title = p.Title,
                    Language = p.Language,
                    Description = p.Description,
                    GithubLink = p.GithubLink,
                    Year = p.Year

                }).ToList()
            };
            exporten.Resume = new ResumeExportDto
            {
                Qualifications = userObject.Resume.Qualifications.Select(q => new QualificationExportDto
                {
                    Name = q.Name
                }).ToList(),

                WorkList = userObject.Resume.WorkList.Select(w => new WorkExportDto
                {
                    CompanyName = w.CompanyName,
                    Position = w.Position,
                    Description = w.Description,
                    StartDate = w.StartDate.ToString("yyyy-MM-dd"), //detta är en datetime/timeonly i work kalssen, gör om till string så den kan skrivas ut lättare
                    EndDate = w.EndDate.HasValue ? w.EndDate.Value.ToString("yyyy-MM-dd") : "Pågående" //om enddate är null skriv pågående ut istället.
                }).ToList(),

                EducationList = userObject.Resume.EducationList.Select(e => new EducationExportDto
                {
                    SchoolName = e.SchoolName,
                    DegreeName = e.DegreeName,
                    StartYear = e.StartYear,
                    EndYear = e.EndYear.HasValue ? e.EndYear.Value.ToString() : "Pågående",// som beskriver ovan
                    Description = e.Description ?? ""
                }).ToList()

            };
            var serializer = new XmlSerializer (typeof(UserExportDto));
            using (var streaming = new MemoryStream())//memory stream används för att användaren ska kunna ladda ner informationen
            {
                serializer.Serialize (streaming, exporten); //serialiserar allt
                //returnerar med hjälpa av metoden file.
                // först streaming vilket är själva datan, sen står det bilken srta data (XML)
                //$"profil.... är helt enkelöt vad filen ska heta, kommer se ut exempelvis profil_anna_anderson.xml
                return File(streaming.ToArray(), "application/xml", $"profil_{userObject.FirstName}_{userObject.LastName}.xml");
            }
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
            var qualifKey = ModelState.Keys.Where(k => k.StartsWith("Resume.Qualifications") && !k.EndsWith(".Resume")).ToList();
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
                                                && !k.EndsWith(".Resume"))
                                                .ToList();
            var allKeys = ModelState.Keys.ToList();
            
            foreach (var key in allKeys)
            {
                if (!eduKey.Contains(key))
                {
                    ModelState.Remove(key);
                }
            }

   
            //kontrollerar om utbildning är giltiga, men kopierar över datan så att det användaren skrev är kvar 
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
                                                && !k.EndsWith(".Resume"))
                                                .ToList();
            var allKeys = ModelState.Keys.ToList();
            
            foreach (var key in allKeys)
            {
                if (!eduKey.Contains(key))
                {
                    ModelState.Remove(key);
                }
            }

   
            //kontrollerar om arbete är giltiga, men kopierar över datan så att det användaren skrev är kvar 
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

        public async Task<IActionResult> EditProjects()
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
        public async Task<IActionResult> EditProjectInfo(User updatedUser)
        {

            var currentUser = await _context.Users
                .Include(u => u.Resume)
                .ThenInclude(r => r.WorkList)
            .FirstOrDefaultAsync(u => u.Id == updatedUser.Id);

            if (currentUser == null)
            {       
                return Content("Hittar ej användare i databas");
            }
            
            return RedirectToAction("EditProjects");

        }

    }
}
