using Microsoft.AspNetCore.Mvc;
using CvProjekt.Models;
using Microsoft.AspNetCore.Identity;

namespace CvProjekt.Controllers
{
    public class AccountController : Controller
    {
        // UserManager hanterar användardata (skapa, hitta, uppdatera användare)
        private readonly UserManager<User> _userManager;
        // SignInManager hanterar själva inloggningsprocessen och cookies
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

        // Hanterar registrering av nya användare
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Skapar ett nytt User-objekt baserat på registreringsformuläret
                var user = new User
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Adress = model.Adress,
                    IsActive = true
                };

                // Sparar användaren i databasen med det angivna lösenordet (krypteras automatiskt)
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Vid lyckad registrering skickas användaren till inloggningssidan
                    return RedirectToAction("Login");
                }

                // Om något gick fel (t.ex. för svagt lösenord) läggs felen till i formuläret
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        // Hanterar inloggning
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Försöker logga in användaren med e-post och lösenord
                // isPersistent: false betyder att sessionen inte sparas när webbläsaren stängs
                var result = await _signInManager.PasswordSignInAsync(
                    model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    // Skickar användaren till startsidan vid lyckad inloggning
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Felaktig e-postadress eller lösenord.");
            }
            return View(model);
        }

        // Hanterar utloggning
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // Rensar inloggnings-cookien
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}