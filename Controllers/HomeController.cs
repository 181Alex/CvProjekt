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
                .Where(u => u.ResumeId != null && u.IsPrivate==false && u.IsActive==true)
                .OrderByDescending(u => u.ResumeId) 
                .AsSplitQuery()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                usersQuery = usersQuery.Where(u =>
                    u.FirstName.Contains(search) || u.LastName.Contains(search));
            }

            var users = await usersQuery.Take(5).ToListAsync(); 

            
            var latestProjects = await _context.Projects
                .Include(p => p.Creator)
                .OrderByDescending(p => p.Id) 
                .Take(5)                      
                .ToListAsync();
            var allProjects = await _context.Projects
                .Include(p => p.Creator)
                .OrderByDescending(p => p.Id)
                .ToListAsync();

            var model = new HomeViewModel
            {
                Users = users,
                LatestProjects = latestProjects, 
                AllProjects = allProjects
            };

            return View(model);
        }
    }








}