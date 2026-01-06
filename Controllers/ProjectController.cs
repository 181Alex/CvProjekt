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
                .Where(u => u.ResumeId != null && u.IsPrivate == false && u.IsActive == true)
                .OrderByDescending(u => u.ResumeId)
                .AsSplitQuery()
                .AsQueryable();

            var users = await usersQuery.ToListAsync();

            var allProjects = await context.Projects
                .Include(p => p.Creator)
                .OrderByDescending(p => p.Id)
                .ToListAsync();

            var membersList = await context.ProjectMembers
                .Include(PM => PM.user)
                .Include(PM => PM.project)
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
        public async Task<IActionResult> JoinProject(int Pid)
        {
            var user= await _userManager.GetUserAsync(User);

            var project = await context.Projects.FindAsync(Pid);
            if (project == null)
            {
                ModelState.AddModelError("sender","Projektet hittades inte.");
                return RedirectToAction("AllProjects");
            }
            // kontroll för att se om de redan är en medlem.
            var exists = await context.ProjectMembers
               .AnyAsync(pm => pm.MemberId == user.Id && pm.MProjectId == Pid);

            if (exists)
            {
                ModelState.AddModelError("sender","Du är redan medlem i projektet.");
                return RedirectToAction("AllProjects");
            }

            var membership = new ProjectMembers
            {
                MemberId = user.Id,
                MProjectId = Pid
            };
            context.ProjectMembers.Add(membership);
            await context.SaveChangesAsync();
            return RedirectToAction("AllProjects");
        }

        [HttpPost]
        public async Task<IActionResult> LeaveProject(int Pid)
        {
            var user = await _userManager.GetUserAsync(User);

            var membership = await context.ProjectMembers
                .FirstOrDefaultAsync(pm => pm.MemberId == user.Id && pm.MProjectId == Pid);

            if (membership == null)
            {
                ModelState.AddModelError("sender", "Du är inte medlem i projektet.");
                return RedirectToAction("AllProjects");
            }

            context.ProjectMembers.Remove(membership);
            await context.SaveChangesAsync();

            return RedirectToAction("AllProjects");
        }


    }
}
