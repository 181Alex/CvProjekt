using CvProjekt.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CvProjekt.Controllers
{
    public class ProjectController : Controller
    {
        private readonly CvContext context;
        private readonly UserManager<User> _userManager;

        public ProjectController(CvContext context, UserManager<User> user)
        {
            this.context = context;
            _userManager = user;
        }

        [HttpGet]
        public async Task<IActionResult> AllProjects()
        {
            var usersQuery = context.Users
                .Include(u => u.Resume).ThenInclude(r => r.Qualifications)
                .Include(u => u.Resume).ThenInclude(r => r.WorkList)
                .Include(u => u.Resume).ThenInclude(r => r.EducationList)
                .Where(u => u.ResumeId != null)
                .OrderByDescending(u => u.ResumeId)
                .AsSplitQuery()
                .AsQueryable();
            // om man inte är inloggad kan man inte se privata
            if (!User.Identity?.IsAuthenticated == true)
            {
                usersQuery = usersQuery.Where(u => u.IsPrivate == false && u.IsActive == true);
            }
            else
            {
                usersQuery = usersQuery.Where(u => u.IsActive == true);
            }

            var users = await usersQuery.ToListAsync();

            var projectsQuery = context.Projects
                .Include(p => p.Creator)
                .AsQueryable();
            // samma som ovan
            if (!User.Identity?.IsAuthenticated == true)
            {
                projectsQuery = projectsQuery
                    .Where(p => p.Creator.IsPrivate == false && p.Creator.IsActive == true);
            }
            else
            {
                projectsQuery = projectsQuery.Where(p => p.Creator.IsActive == true);
            }

            var allProjects = await projectsQuery
                .OrderByDescending(p => p.Id)
                .ToListAsync();

            var membersList = await context.ProjectMembers
                .Include(pm => pm.user)
                .Include(pm => pm.project)
                .ToListAsync();

            var model = new HomeViewModel
            {
                Users = users,
                AllProjects = allProjects,
                ProjectMemberList = membersList
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> JoinProject(int projectId)
        {
            var user= await _userManager.GetUserAsync(User);

            var project = await context.Projects.FindAsync(projectId);
            if (project == null)
            {
                TempData["Info"] = "Projektet hittades inte.";
                return RedirectToAction("AllProjects");
            }
            // kontroll för att se om de redan är en medlem.
            var exists = await context.ProjectMembers
               .AnyAsync(pm => pm.MemberId == user.Id && pm.MProjectId == projectId);

            if (exists)
            {
                TempData["Info"] = "Du är redan medlem i projektet.";
                return RedirectToAction("AllProjects");
            }

            var membership = new ProjectMembers
            {
                MemberId = user.Id,
                MProjectId = projectId
            };
            context.ProjectMembers.Add(membership);
            await context.SaveChangesAsync();
            TempData["Success"] = "Du har gått med i projektet.";
            return RedirectToAction("AllProjects");
        }

        [HttpPost]
        public async Task<IActionResult> LeaveProject(int projectId, string source)
        {
            var user = await _userManager.GetUserAsync(User);

            var membership = await context.ProjectMembers
                .FirstOrDefaultAsync(pm => pm.MemberId == user.Id && pm.MProjectId == projectId);

            if (membership == null)
            {
                TempData["Info"] = "Du är inte medlem i projektet.";
                return RedirectToAction("AllProjects");
            }

            context.ProjectMembers.Remove(membership);
            await context.SaveChangesAsync();

            if (source == "Profile")
            {
                return RedirectToAction("MyProfile", "Profile");
            }
            TempData["Success"] = "Du lämnade projektet.";
            return RedirectToAction("AllProjects");
        }


    }
}
