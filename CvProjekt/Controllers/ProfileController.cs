using CvProjekt.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Xml.Serialization;
using System.Linq;
using DAL;
using Models;

namespace CvProjekt.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly CvContext _context;

        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProfileController(CvContext context, UserManager<User> user, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _userManager = user;
            _hostEnvironment = hostEnvironment;
        }
        //gå till settings
        [HttpGet]
        public async Task<IActionResult> Settings()
        {
            var nowUser = await _userManager.GetUserAsync(User);
            return View(nowUser);
        }
        [HttpGet]
        public async Task<IActionResult> DownloadData()
        {
            // i metoden har vi använt oss av en exportkalss utan referenser som refererar
            // till varandra (ex user har flera projekt och projekt har user, har crashat då)
            //dessutom kan vi göra om variabler till exempelvis string i fallet av datum
            var user = await _userManager.GetUserAsync(User);
            //tar fram användaren och alla dessa erfarenheter osv. VIktigt är att denna är seperat från resume till en blrjan!!!
            //tar då också med alla relationer, så exempeklvis alla projekt
            var userObject = await _context.Users
                .Include(u => u.Projects)
                .Include(u => u.Resume)
                .ThenInclude(r => r.WorkList)
                .Include(u => u.Resume)
                .ThenInclude(r => r.Qualifications)
                .Include(u => u.Resume)
                .ThenInclude(r => r.EducationList)
                .Include(u => u.ProjectMembers)
                .ThenInclude(pm => pm.project)
                .Where(u => u.Id == user.Id)
                .FirstOrDefaultAsync();

            if (userObject == null) { ModelState.AddModelError("All", "User not found"); }
            //tar fram vilka projekt man deltar i och deras Id
            var memberProjectIds = userObject.ProjectMembers.Select(pm => pm.MProjectId).ToList();
            var memberProjects = await _context.Projects
                .Where(p => memberProjectIds.Contains(p.Id))
                .ToListAsync();
            //Skapar det vi faktiskt ska exportera, dvs användaren och allt till den som cv osv.
            // detta görs genom ett helt nytt "clean" objekt utan några referenser 
            //allt detta ska ske i kalss form export/dto så att inga connctions mellan kalsserna görs.
            var exporten = new UserExportDto
            {
                FirstName = userObject.FirstName,
                LastName = userObject.LastName,
                Email = userObject.Email,
                Adress = userObject.Adress,
                ImgUrl = userObject.ImgUrl,
                ProjectMember = memberProjects.Select(mp => new ProjectMembersDto
                {
                    Title = mp.Title,
                    Language = mp.Language,
                    Description = mp.Description,
                    GithubLink = mp.GithubLink,
                    Year = mp.Year
                }).ToList(),
                Projects = userObject.Projects.Select(p => new ProjectExportDto // som det står i exportmodels så skapar man en lista av projektexportdto, istälelt för lsita av project
                {
                    Title = p.Title,
                    Language = p.Language,
                    Description = p.Description,
                    GithubLink = p.GithubLink,
                    Year = p.Year

                }).ToList()
            };
            if (userObject.Resume != null)
            {
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
            }
            var serializer = new XmlSerializer(typeof(UserExportDto));
            using (var streaming = new MemoryStream())//memory stream används för att användaren ska kunna ladda ner informationen
            {
                serializer.Serialize(streaming, exporten); //serialiserar 
                //returnerar med hjälpa av metoden file.
                // först streaming vilket är själva datan, sen står det bilken srta data (XML)
                //$"profil.... är helt enkelöt vad filen ska heta, kommer se ut exempelvis profil_anna_anderson.xml
                return File(streaming.ToArray(), "application/xml", $"profil_{userObject.FirstName}_{userObject.LastName}.xml");
            }
        }
        //några enkla metoder för att gå privat och avaktiver konto.
        [HttpPost]
        public async Task<IActionResult> Avaktivera()
        {
            var user = await _userManager.GetUserAsync(User);
            user.IsActive = false;
            await _userManager.UpdateAsync(user);
            _context.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Settings");
        }

        [HttpPost]
        public async Task<IActionResult> Aktivera()
        {
            var user = await _userManager.GetUserAsync(User);
            user.IsActive = true;
            await _userManager.UpdateAsync(user);
            _context.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Settings");
        }
        [HttpPost]
        public async Task<IActionResult> GoPublic()
        {
            var user = await _userManager.GetUserAsync(User);
            user.IsPrivate = false;
            await _userManager.UpdateAsync(user);
            _context.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Settings");
        }

        [HttpPost]
        public async Task<IActionResult> GoPrivate()
        {
            var user = await _userManager.GetUserAsync(User);
            user.IsPrivate = true;
            await _userManager.UpdateAsync(user);
            _context.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Settings");
        }
        //skickar till byt lösenords vyn
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
                    ModelState.AddModelError("All", error.Description);
                }
                return View("ChangePassword", model);
            }

            return RedirectToAction("Settings");
        }

        //returnerar myprofile vyn
        [HttpGet]
        public async Task<IActionResult> MyProfile()
        {
            var userId = _userManager.GetUserId(User);

            //inkludera alla fält från foreign keys och sedan deras foreign keys
            var user = await _context.Users
                .Include(u => u.Projects)
                    .ThenInclude(p => p.ProjectMembers)
                        .ThenInclude(pm => pm.user)
                .Include(u => u.Resume)
                    .ThenInclude(r => r.Qualifications)
                .Include(u => u.Resume)
                    .ThenInclude(r => r.WorkList)
                .Include(u => u.Resume)
                    .ThenInclude(r => r.EducationList)
                .FirstOrDefaultAsync(u => u.Id == userId);

            //om ingen användare
            if (user == null)
            {
                return Content($"Fel: Hittade ingen användare med ID '{userId}' i databasen. Har du kört database update?");
            }


            var memberProjectIds = await _context.ProjectMembers
                .Where(pm => pm.MemberId == userId)
                .Select(pm => pm.MProjectId)
                .Distinct()
                .ToListAsync();

            var memberProjects = await _context.Projects
                .Where(p => memberProjectIds.Contains(p.Id))
                .Include(p => p.Creator)
                .Include(p => p.ProjectMembers)
                    .ThenInclude(pm => pm.user)
                .ToListAsync();

            ViewBag.MemberProjects = memberProjects;

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



        //Returnerar editresume view
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

            if (user == null)
            {
                return Content($"Fel: Hittade ingen användare med ID '{userId}' i databasen. Har du kört database update?");
            }

            return View(user);

        }

        [HttpPost]
        //redigerar bas info
        public async Task<IActionResult> EditResumeInfo(User updatedUser, IFormFile? image)
        {
            ModelState.Remove("ImgUrl");
            //kontrollerar att alla fält är validerade
            if (!ModelState.IsValid)
            {

                return View("EditResume", updatedUser);
            }

            //hämtar nuvarande person från DB
            var currentUser = await _userManager.FindByIdAsync(updatedUser.Id);

            if (currentUser == null)
            {
                return Content("Hittar ej användare i databas");
            }


            if (image != null)
            {
                
                //vart filen ska skickas
                string uploadDir = Path.Combine(_hostEnvironment.WebRootPath, "uploads");
                //om upload mapp inte finns skapas det
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }
                //vad filen ska kallas
                string fileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                //exakt slutplats, dvs mapp namnet plus filnamnet
                string filePath = Path.Combine(uploadDir, fileName);
                //här sparas faktiskt bilden så att den sen kan visas
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }

                currentUser.ImgUrl = "/uploads/" + fileName;
            }

            //om cv är tomt skapas nytt
            if (currentUser.Resume == null)
            {
                currentUser.Resume = new Resume();
            }

            //uppdaterar information på den nuvaranda till den nya informationen
            currentUser.FirstName = updatedUser.FirstName;
            currentUser.LastName = updatedUser.LastName;
            currentUser.Adress = updatedUser.Adress;

            //uppdaterar mail inklusive andvändarinlogg
            currentUser.Email = updatedUser.Email;
            currentUser.UserName = updatedUser.Email;

            var result = await _userManager.UpdateAsync(currentUser);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Basinformation sparad!";
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View("EditResume", updatedUser);
            }
            TempData["ActiveTab"] = "base";

            return RedirectToAction("EditResume");
        }

        [HttpPost]
        //redigera kvalifikationer
        public async Task<IActionResult> EditQualifications(User updatedUser)
        {

            //hämtar nuvarande användare från db
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
                    foreach (var q in updatedUser.Resume.Qualifications)
                    {
                        currentUser.Resume.Qualifications.Add(new Qualification
                        {
                            Name = q.Name,
                            ResumeId = currentUser.Resume.Id
                        });
                    }
                }

                //returnerar vilken tab som ska vara öppen när man återgår till sidan
                TempData["ActiveTab"] = "qualif";
                return View("EditResume", currentUser);
            }

            currentUser.Resume.Qualifications.Clear();

            if (updatedUser.Resume?.Qualifications != null)
            {
                foreach (var q in updatedUser.Resume.Qualifications)
                {
                    if (!string.IsNullOrWhiteSpace(q.Name))
                    {
                        currentUser.Resume.Qualifications.Add(new Qualification
                        {
                            Name = q.Name,
                            ResumeId = currentUser.Resume.Id
                        });
                    }
                }
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Kompetenser sparade";

            //returnerar vilken tab som ska vara öppen när man återgår till sidan
            TempData["ActiveTab"] = "qualif";

            return RedirectToAction("EditResume");

        }

        [HttpPost]
        //redigera utbildning
        public async Task<IActionResult> EditEducation(User updatedUser)
        {
            //hämtar nuvarande användare
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

            //tar bort alla modelstate keys som inte har att göra med utbildning
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

                if (updatedUser.Resume?.EducationList != null)
                {
                    foreach (var e in updatedUser.Resume.EducationList)
                    {
                        currentUser.Resume.EducationList.Add(new Education
                        {
                            SchoolName = e.SchoolName,
                            DegreeName = e.DegreeName,
                            StartYear = e.StartYear,
                            EndYear = e.EndYear,
                            Description = e.Description,
                            ResumeId = currentUser.Resume.Id
                        });
                    }
                }

                //returnerar vilken tab som ska vara öppen när man återgår till sidan
                TempData["ActiveTab"] = "edu";
                return View("EditResume", currentUser);
            }

            currentUser.Resume.EducationList.Clear();

            if (updatedUser.Resume?.EducationList != null)
            {
                foreach (var e in updatedUser.Resume.EducationList)
                {
                    if (!string.IsNullOrWhiteSpace(e.SchoolName) && !string.IsNullOrWhiteSpace(e.DegreeName) && e.StartYear != null)
                    {
                        currentUser.Resume.EducationList.Add(new Education
                        {
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

            //returnerar vilken tab som ska vara öppen när man återgår till sidan
            TempData["ActiveTab"] = "edu";

            return RedirectToAction("EditResume");

        }

        [HttpPost]
        //redigera arbetserfarenhet
        public async Task<IActionResult> EditWork(User updatedUser)
        {
            //hämtar nuvarande användare
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
            var workKey = ModelState.Keys.Where(k => k.StartsWith("Resume.WorkList")
                                                && !k.EndsWith(".Resume"))
                                                .ToList();
            var allKeys = ModelState.Keys.ToList();

            //tar bort alla keys som inte har att göra med work
            foreach (var key in allKeys)
            {
                if (!workKey.Contains(key))
                {
                    ModelState.Remove(key);
                }
            }


            //kontrollerar om arbete är giltiga, men kopierar över datan så att det användaren skrev är kvar 
            if (!ModelState.IsValid)
            {

                currentUser.Resume.WorkList.Clear();

                if (updatedUser.Resume?.WorkList != null)
                {
                    foreach (var w in updatedUser.Resume.WorkList)
                    {

                        currentUser.Resume.WorkList.Add(new Work
                        {
                            CompanyName = w.CompanyName,
                            Position = w.Position,
                            StartDate = w.StartDate,
                            EndDate = w.EndDate,
                            Description = w.Description,
                            ResumeId = currentUser.Resume.Id
                        });

                    }
                }

                //returnerar vilken tab som ska vara öppen när man återgår till sidan
                TempData["ActiveTab"] = "work";
                return View("EditResume", currentUser);
            }

            currentUser.Resume.WorkList.Clear();

            if (updatedUser.Resume?.WorkList != null)
            {
                foreach (var w in updatedUser.Resume.WorkList)
                {
                    if (!string.IsNullOrWhiteSpace(w.CompanyName) && !string.IsNullOrWhiteSpace(w.Position) && w.StartDate != null)
                    {
                        currentUser.Resume.WorkList.Add(new Work
                        {
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
            //returnerar vilken tab som ska vara öppen när man återgår till sidan
            TempData["ActiveTab"] = "work";

            return RedirectToAction("EditResume");

        }

        //returnerar vy för editprojects
        public async Task<IActionResult> EditProjects()
        {
            //hämtar användare
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

        [HttpPost]
        //redigera projekt metod
        public async Task<IActionResult> EditProjectInfo(User updatedUser)
        {
            //hämtar användare som finns i db (nuvarande)
            var currentUser = await _context.Users
                .Include(u => u.Projects)
            .FirstOrDefaultAsync(u => u.Id == updatedUser.Id);

            if (currentUser == null)
            {
                return Content("Hittar ej användare i databas");
            }

            //tar bort alla keys som inte har att göra med projektets model state
            var projKey = ModelState.Keys.Where(k => k.StartsWith("Projects")
                                                && !k.EndsWith(".Creator")
                                                && !k.EndsWith(".CreatorId"))
                                                .ToList();
            var allKeys = ModelState.Keys.ToList();

            foreach (var key in allKeys)
            {
                if (!projKey.Contains(key))
                {
                    ModelState.Remove(key);
                }
            }

            //kontrollerar om modelstate är valid och kopierar den data som användare skrivit in så den inte försvinner
            if (!ModelState.IsValid)
            {

                currentUser.Projects.Clear();

                if (updatedUser.Projects != null)
                {
                    foreach (var p in updatedUser.Projects)
                    {
                        currentUser.Projects.Add(new Project
                        {
                            Title = p.Title,
                            Language = p.Language,
                            GithubLink = p.GithubLink,
                            Year = p.Year,
                            Description = p.Description,
                            CreatorId = currentUser.Id
                        });
                    }
                }

                return View("EditProjects", currentUser);
            }

            currentUser.Projects.Clear();

            if (updatedUser.Projects != null)
            {
                foreach (var p in updatedUser.Projects)
                {
                    if (!string.IsNullOrWhiteSpace(p.Title) && !string.IsNullOrWhiteSpace(p.Language) && p.Year != null)
                    {
                        currentUser.Projects.Add(new Project
                        {
                            Title = p.Title,
                            Language = p.Language,
                            GithubLink = p.GithubLink,
                            Year = p.Year,
                            Description = p.Description,
                            CreatorId = currentUser.Id
                        });
                    }
                }
            }

            await _context.SaveChangesAsync();

            //skickar meddelande för lyckad sparning
            TempData["SuccessMessage"] = "Projekt sparat";
            return RedirectToAction("EditProjects");

        }
    }
}