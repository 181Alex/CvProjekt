using System.Diagnostics;
using CvProjekt.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL;
using Models;

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
            // Grundfråga för att hämta användare och inkludera all data i deras CV (Resume)
            // Include och ThenInclude används för att hämta relaterad data som kompetenser, jobb och utbildning
            var usersQuery = _context.Users
                .Include(u => u.Resume).ThenInclude(r => r.Qualifications)
                .Include(u => u.Resume).ThenInclude(r => r.WorkList)
                .Include(u => u.Resume).ThenInclude(r => r.EducationList)
                .Where(u => u.ResumeId != null) // Endast användare som faktiskt har skapat ett CV visas
                .OrderByDescending(u => u.ResumeId)
                .AsSplitQuery() // Optimerar frågan när många tabeller inkluderas
                .AsQueryable();

            // Säkerhetsfilter: 
            // Om besökaren inte är inloggad visas endast offentliga profiler (IsPrivate == false)
            if (!User.Identity?.IsAuthenticated == true)
            {
                usersQuery = usersQuery.Where(u => u.IsPrivate == false && u.IsActive == true);
            }
            else
            {
                // Inloggade användare kan se alla aktiva användare, även de med privata profiler
                usersQuery = usersQuery.Where(u => u.IsActive == true);
            }

            // Hanterar sökfunktionen
            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchWords = search.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                foreach (var word in searchWords)
                {
                    var tempWord = word;
                    // Söker i förnamn, efternamn, användarnamn eller kompetenser i CV:t
                    usersQuery = usersQuery.Where(u =>
                        u.FirstName.Contains(tempWord) ||
                        u.LastName.Contains(tempWord) ||
                        u.UserName.Contains(tempWord) ||
                        u.Resume.Qualifications.Any(q => q.Name.Contains(tempWord))
                    );
                }
            }

            // Hämtar de 5 senaste användarna för en "senaste"-lista
            var latestUsers = await usersQuery.Take(5).ToListAsync();
            // Hämtar alla matchande användare för den fullständiga listan
            var allUsers = await usersQuery.ToListAsync();

            // Samma logik appliceras på projekt (inkludera skapare och filtrera efter privat-status)
            var projectsQuery = _context.Projects
                .Include(p => p.Creator)
                .AsQueryable();

            if (!User.Identity?.IsAuthenticated == true)
            {
                projectsQuery = projectsQuery
                    .Where(p => p.Creator.IsPrivate == false && p.Creator.IsActive == true);
            }
            else
            {
                projectsQuery = projectsQuery.Where(p => p.Creator.IsActive == true);
            }

            var latestProjects = await projectsQuery
                .Where(p => p.Creator.IsActive)
                .OrderByDescending(p => p.Id)
                .Take(5)
                .ToListAsync();

            var allProjects = await projectsQuery
                .Where(p => p.Creator.IsActive)
                .OrderByDescending(p => p.Id)
                .ToListAsync();

            var model = new HomeViewModel
            {
                Users = latestUsers,
                AllUsers = allUsers,
                LatestProjects = latestProjects,
                AllProjects = allProjects
            };

            return View(model);
        }
    }
}