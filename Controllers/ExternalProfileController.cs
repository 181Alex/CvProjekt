using CvProjekt.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Serialization;
using System.Linq;

namespace CvProjekt.Controllers
{
    public class ExternalProfileController : Controller
    {
        private readonly CvContext _context;
        private readonly UserManager<User> _userManager;

        public ExternalProfileController(CvContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /ExternalProfile/Profile/{id}
        [HttpGet]
        public async Task<IActionResult> Profile(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Ingen användar-id angivet.");
            }

            var user = await _context.Users
                .Include(u => u.Projects)

                .Include(u => u.ProjectMembers)
                    .ThenInclude(pm => pm.project)

                .Include(u => u.Resume)
                    .ThenInclude(r => r.Qualifications)
                .Include(u => u.Resume)
                    .ThenInclude(r => r.WorkList)
                .Include(u => u.Resume)
                    .ThenInclude(r => r.EducationList)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound($"Hittar ingen användare med id '{id}'.");
            }

            // Öka profilbesök 
            try
            {
                user.ProfileVisits++;
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            catch
            {
                // Felsäkert – om save misslyckas ska inte vyn krascha.
            }

            // Hämta liknande profiler baserat på delade kompetenser (Qualifications)
            List<User> similarProfiles = new List<User>();
            var qualNames = user.Resume?.Qualifications?.Select(q => q.Name.Replace(" ", "").ToLower()).Where(n => !string.IsNullOrWhiteSpace(n)).Distinct().ToList() ?? new List<string>();

            if (qualNames.Any())
            {
                // Hitta andra användare som har minst en av dessa kompetenser
                var candidates = await _context.Users
                    .Include(u => u.Resume)
                        .ThenInclude(r => r.Qualifications)
                    .Include(u => u.Projects)
                    .Where(u => u.Id != id && u.Resume != null && u.Resume.Qualifications.Any(q => qualNames.Contains(q.Name.Replace(" ", "").ToLower())))
                    .ToListAsync();

                // Rangordna efter antal delade kompetenser och ta max 6
                similarProfiles = candidates
                    .Select(c => new
                    {
                        User = c,
                        SharedCount = c.Resume?.Qualifications?.Count(q => qualNames.Contains(q.Name.Replace(" ", "").ToLower())) ?? 0
                    })
                    .OrderByDescending(x => x.SharedCount)
                    .ThenBy(x => x.User.FirstName)
                    .Select(x => x.User)
                    .Take(6)
                    .ToList();
            }

            ViewBag.SimilarProfiles = similarProfiles;

            return View("Profile", user);
        }

        // GET: /ExternalProfile/DownloadData/{id}
        [HttpGet]
        public async Task<IActionResult> DownloadData(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Ingen användar-id angivet.");
            }

            var userObject = await _context.Users
                .Include(u => u.Projects)
                .Include(u => u.Resume)
                    .ThenInclude(r => r.WorkList)
                .Include(u => u.Resume)
                    .ThenInclude(r => r.Qualifications)
                .Include(u => u.Resume)
                    .ThenInclude(r => r.EducationList)
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();

            if (userObject == null)
            {
                return NotFound("Användaren hittades inte.");
            }
            var exporten = new UserExportDto
            {
                FirstName = userObject.FirstName,
                LastName = userObject.LastName,
                Email = userObject.Email,
                Adress = userObject.Adress,
                ImgUrl = userObject.ImgUrl,
                Projects = userObject.Projects?.Select(p => new ProjectExportDto
                {
                    Title = p.Title,
                    Language = p.Language,
                    Description = p.Description,
                    GithubLink = p.GithubLink,
                    Year = p.Year
                }).ToList() ?? new System.Collections.Generic.List<ProjectExportDto>()
            };

            exporten.Resume = new ResumeExportDto
            {
                Qualifications = userObject.Resume?.Qualifications?.Select(q => new QualificationExportDto
                {
                    Name = q.Name
                }).ToList() ?? new System.Collections.Generic.List<QualificationExportDto>(),

                WorkList = userObject.Resume?.WorkList?.Select(w => new WorkExportDto
                {
                    CompanyName = w.CompanyName,
                    Position = w.Position,
                    Description = w.Description,
                    StartDate = w.StartDate.ToString("yyyy-MM-dd"),
                    EndDate = w.EndDate.HasValue ? w.EndDate.Value.ToString("yyyy-MM-dd") : "Pågående"
                }).ToList() ?? new System.Collections.Generic.List<WorkExportDto>(),

                EducationList = userObject.Resume?.EducationList?.Select(e => new EducationExportDto
                {
                    SchoolName = e.SchoolName,
                    DegreeName = e.DegreeName,
                    StartYear = e.StartYear,
                    EndYear = e.EndYear.HasValue ? e.EndYear.Value.ToString() : "Pågående",
                    Description = e.Description ?? ""
                }).ToList() ?? new System.Collections.Generic.List<EducationExportDto>()
            };

            var serializer = new XmlSerializer(typeof(UserExportDto));
            using (var streaming = new MemoryStream())
            {
                serializer.Serialize(streaming, exporten);
                var fileName = $"profil_{userObject.FirstName}_{userObject.LastName}.xml";
                return File(streaming.ToArray(), "application/xml", fileName);
            }
        }
    }
}