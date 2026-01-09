using System.Diagnostics;
using CvProjekt.Models;
using Microsoft.AspNetCore.Mvc;
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
            var usersQuery = _context.Users
                .Include(u => u.Resume).ThenInclude(r => r.Qualifications)
                .Include(u => u.Resume).ThenInclude(r => r.WorkList)
                .Include(u => u.Resume).ThenInclude(r => r.EducationList)
                .Where(u => u.ResumeId != null)
                .OrderByDescending(u => u.ResumeId)
                .AsSplitQuery()
                .AsQueryable();

            if (!User.Identity?.IsAuthenticated == true)
            {
                usersQuery = usersQuery.Where(u => u.IsPrivate == false && u.IsActive == true);
            }
            else
            {
                usersQuery = usersQuery.Where(u => u.IsActive == true);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchWords = search.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                foreach (var word in searchWords)
                {
                    var tempWord = word;
                    usersQuery = usersQuery.Where(u =>
                        u.FirstName.Contains(tempWord) ||
                        u.LastName.Contains(tempWord) ||
                        u.UserName.Contains(tempWord) ||
                        u.Resume.Qualifications.Any(q => q.Name.Contains(tempWord))
                    );
                }

                if (!User.Identity?.IsAuthenticated == true)
                {
                    usersQuery = usersQuery.Where(u => u.IsPrivate == false && u.IsActive == true);
                }
                else
                {

                    usersQuery = usersQuery.Where(u => u.IsActive == true);
                }
            }



            // ÄNDRING: Döpte om från 'users' till 'latestUsers' för att matcha din ViewModel
            var latestUsers = await usersQuery.Take(5).ToListAsync();

            // ÄNDRING: Lade till denna rad eftersom den saknades i den pullade koden men krävs av din ViewModel
            var allUsers = await usersQuery.ToListAsync();


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