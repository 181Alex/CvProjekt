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

        [HttpGet]
        public async Task<IActionResult> Profile(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Ingen användar-id angivet.");
            }

            // Hämtar profilen med alla projekt användaren skapat samt projekt de deltar i
            var user = await _context.Users
                .Include(u => u.Projects) // Projekt användaren har skapat
                .Include(u => u.ProjectMembers) // Projekt där användaren är medlem
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

            // Öka profilbesök (räknare för varje gång någon klickar in på profilen)
            try
            {
                user.ProfileVisits++;
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            catch { /* Felsäkert */ }

            // LOGIK: Hämta liknande profiler baserat på delade kompetenser (Qualifications)
            List<User> similarProfiles = new List<User>();
            // Skapar en lista med namnen på användarens kompetenser, rensade från mellanslag och i gemener
            var qualNames = user.Resume?.Qualifications?
                .Select(q => q.Name.Replace(" ", "").ToLower())
                .Where(n => !string.IsNullOrWhiteSpace(n))
                .Distinct().ToList() ?? new List<string>();

            if (qualNames.Any())
            {
                // Hittar andra användare som har minst en kompetens som matchar
                var candidates = await _context.Users
                    .Include(u => u.Resume)
                        .ThenInclude(r => r.Qualifications)
                    .Where(u => u.Id != id && u.Resume != null &&
                                u.Resume.Qualifications.Any(q => qualNames.Contains(q.Name.Replace(" ", "").ToLower())))
                    .ToListAsync();

                // Rangordnar resultaten efter hur många kompetenser som faktiskt delas
                similarProfiles = candidates
                    .Select(c => new
                    {
                        User = c,
                        SharedCount = c.Resume?.Qualifications?
                            .Count(q => qualNames.Contains(q.Name.Replace(" ", "").ToLower())) ?? 0
                    })
                    .OrderByDescending(x => x.SharedCount) // Flest matchningar först
                    .ThenBy(x => x.User.FirstName)
                    .Select(x => x.User)
                    .Take(6) // Begränsar till de 6 mest relevanta
                    .ToList();
            }

            ViewBag.SimilarProfiles = similarProfiles;
            return View("Profile", user);
        }
    }
}