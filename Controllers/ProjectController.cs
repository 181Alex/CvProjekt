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
                .Include(PM => PM.MemberId)
                .Include(PM => PM.MProjectId)
                .ToListAsync();

            var model = new HomeViewModel
            {
                Users = users,
                AllProjects = allProjects,
                ProjectMemberList = membersList
            };

            return View(model);
        }





    }
}
