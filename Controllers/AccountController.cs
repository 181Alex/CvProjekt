using Microsoft.AspNetCore.Mvc;
using CvProjekt.Models;
using Microsoft.AspNetCore.Identity;

namespace CvProjekt.Controllers
{
    public class AccountController : Controller
    {
        // Vi hämtar in Identitys inbyggda tjänster
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Skapa ett nytt User-objekt med data från formuläret.
                // Vi använder model.Email som UserName eftersom model.Username saknas.
                var user = new User
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Adress = model.Adress,
                    IsActive = true // Sätt som aktiv som standard
                };

                // Skapa användaren i databasen (Identity sköter hashing av lösenordet här)
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Om det lyckas, skicka användaren till login-sidan
                    return RedirectToAction("Login");
                }

                // Om det blir fel, visa felen för användaren
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Försök logga in användaren. 
                // Vi använder model.Email istället för model.Username och sätter RememberMe till false.
                var result = await _signInManager.PasswordSignInAsync(
                    model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                // Om inloggningen misslyckas
                ModelState.AddModelError("", "Felaktig e-postadress eller lösenord.");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}