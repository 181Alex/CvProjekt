using Microsoft.AspNetCore.Mvc;
using CvProjekt.Models;

namespace CvProjekt.Controllers
{
    public class AccountController : Controller
    {
        // visar Login sidan
        public IActionResult Login()
        {
            return View();
        }

        // visar Register-sidan
        public IActionResult Register()
        {
            return View();
        }
    }
}