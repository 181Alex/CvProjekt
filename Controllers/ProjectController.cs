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

        // Metod för att gå med i ett projekt
        [HttpPost]
        public async Task<IActionResult> JoinProject(int projectId)
        {
            // Hämtar den inloggade användaren
            var user = await _userManager.GetUserAsync(User);

            var project = await context.Projects.FindAsync(projectId);
            if (project == null)
            {
                TempData["Info"] = "Projektet hittades inte.";
                return RedirectToAction("AllProjects");
            }

            // Kontrollerar i kopplingstabellen ProjectMembers om användaren redan är medlem
            var exists = await context.ProjectMembers
               .AnyAsync(pm => pm.MemberId == user.Id && pm.MProjectId == projectId);

            if (exists)
            {
                TempData["Info"] = "Du är redan medlem i projektet.";
                return RedirectToAction("AllProjects");
            }

            // Skapar en ny rad i kopplingstabellen för att registrera medlemskapet
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

        // Metod för att lämna ett projekt
        [HttpPost]
        public async Task<IActionResult> LeaveProject(int projectId, string source)
        {
            var user = await _userManager.GetUserAsync(User);

            // Letar upp raden i kopplingstabellen som matchar användaren och projektet
            var membership = await context.ProjectMembers
                .FirstOrDefaultAsync(pm => pm.MemberId == user.Id && pm.MProjectId == projectId);

            if (membership == null)
            {
                TempData["Info"] = "Du är inte medlem i projektet.";
                return RedirectToAction("AllProjects");
            }

            // Tar bort raden från kopplingstabellen (avslutar medlemskapet)
            context.ProjectMembers.Remove(membership);
            await context.SaveChangesAsync();

            // Skickar användaren tillbaka till antingen sin profil eller projektlistan beroende på varifrån de klickade
            if (source == "Profile")
            {
                return RedirectToAction("MyProfile", "Profile");
            }
            TempData["Success"] = "Du lämnade projektet.";
            return RedirectToAction("AllProjects");
        }
    }
}