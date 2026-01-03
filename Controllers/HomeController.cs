using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CvProjekt.Models;
using Microsoft.EntityFrameworkCore;

namespace CvProjekt.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CvContext _context;

        public HomeController(ILogger<HomeController> logger, CvContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string search)
        {
            // 1. Hämta CV:n direkt från Resumes-tabellen.
            // Vi Includerar all data som behövs för att visa profilen (Qualifications, WorkList, etc.)
            var resumeQuery = _context.Resumes
                .Include(r => r.User)
                .Include(r => r.Qualifications)
                .Include(r => r.WorkList)
                .Include(r => r.EducationList)
                .OrderByDescending(r => r.Id) // Nyaste CV:t först
                .AsQueryable();

            // Sökfunktion: Filtrera på användarens namn via kopplingen i CV:t
            if (!string.IsNullOrWhiteSpace(search))
            {
                resumeQuery = resumeQuery.Where(r =>
                    r.User.FirstName.Contains(search) || r.User.LastName.Contains(search));
            }

            // Hämta de 5 senaste CV:na
            var latestResumes = await resumeQuery.Take(5).ToListAsync();

            // 2. Skapa listan med Users som Viewn förväntar sig
            var users = new List<User>();

            foreach (var resume in latestResumes)
            {
                if (resume.User != null)
                {
                    // --- VIKTIGT ---
                    // Vi måste manuellt koppla CV:t till User-objektet här.
                    // Annars blir 'user.Resume' null i Viewn, och då syns inga kompetenser.
                    resume.User.Resume = resume;

                    users.Add(resume.User);
                }
            }

            // 3. Hämta de 5 senaste projekten (precis som förut)
            var latestProjects = await _context.Projects
                .Include(p => p.User)
                .OrderByDescending(p => p.Id)
                .Take(5)
                .ToListAsync();

            // 4. Skapa ViewModel
            var model = new HomeViewModel
            {
                Users = users,
                LatestProjects = latestProjects
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}