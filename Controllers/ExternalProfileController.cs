using CvProjekt.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        // GET: /ExternalProfile/Profile/{id}
        [HttpGet]
        public async Task<IActionResult> Profile(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Ingen användar-id angivet.");
            }

            var user = await _context.Users
                .Include(u => u.Projects)
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

            // Öka profilbesök (valfritt)
            try
            {
                user.ProfileVisits++;
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            catch
            {
                // Felsäkert — om save misslyckas ska inte vyn krascha.
            }

            return View("Profile", user);
        }
    }
}