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
            var usersQuery = _context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                usersQuery = usersQuery.Where(u =>
                    u.FirstName.Contains(search) ||
                    u.LastName.Contains(search)
                );
            }

            var users = await usersQuery.ToListAsync();

            return View(users);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Index()
        {
           
            var latestProject = await _context.Projects
                .OrderByDescending(p => p.Id)
                .FirstOrDefaultAsync();

            
            var users = await _context.Users.ToListAsync();

            var model = new HomeViewModel
            {
                Users = users,
                LatestProject = latestProject
            };

            return View(model);
        }
    }
}
