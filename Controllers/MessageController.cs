using CvProjekt.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CvProjekt.Controllers;
//[Authorize]
public class MessageController : Controller
{
    private readonly CvContext context;
    private readonly UserManager<User> _userManager;
    
    public MessageController(CvContext context, UserManager<User> user)
    {
        this.context = context;
        _userManager = user;
    }
    
    [HttpPost]
    public async Task<IActionResult> SendMessage(Message mess)
    {
        mess.Date= DateTime.Now;
        var nowUser = await _userManager.GetUserAsync(User);

        //detta måste göras iordning sen när allt dunkar med inlogg
        if (nowUser == null)
        {
            mess.FromUserId = "user-1"; 
        }
        else
        {
            mess.FromUserId = nowUser.Id;
        }
        
        context.Messages.Add(mess);
        await context.SaveChangesAsync();
        return RedirectToAction("SeeMessages");
    }

//borttaget för testing, när allt funkar, återsätll denna och ta bort den andra
    [HttpGet]
/*     public async Task<IActionResult> SeeMessages()
    {
        var nowUser = await _userManager.GetUserAsync(User);
        if (nowUser != null)
        {
            //Lägg in att användaren ska tillbaka till hemskärmen när den är klar
        }

        var messages = await context.Messages
            .Include(m => m.FromUser)
            .Where(m => m.ToUserId == nowUser.Id)
            .OrderByDescending(m => m.Date)
            .ToListAsync();
        return View(messages);
    } */
    public async Task<IActionResult> SeeMessages()
    {
        // 1. Försök hämta riktig användare
        var user = await _userManager.GetUserAsync(User);

        // 2. TEMPORÄR FIX: Om ingen är inloggad, hämta Erik manuellt
        if (user == null)
        {
            // Vi hämtar användaren med ID "user-1" från din Seed Data
            user = await context.Users.FirstOrDefaultAsync(u => u.Id == "user-1");
        }

        // Säkerhetskoll: Om databasen är tom
        if (user == null)
        {
            return Content("Fel: Inga användare hittades i databasen. Har du kört seed data?");
        }

        // 3. Nu har vi garanterat en 'user' (antingen riktig eller Erik).
        // Hämta meddelanden TILL denna användare.
        var messages = await context.Messages
            .Include(m => m.FromUser) // Viktigt för att visa namn/bild
            .Where(m => m.ToUserId == user.Id)
            .OrderByDescending(m => m.Date)
            .ToListAsync();

        return View(messages);
    }
    [HttpPost]
    public async Task<IActionResult> ReadMessage(int id)
    {
        var mess = await context.Messages.FindAsync(id);
        if (mess != null)
        {
            mess.Read=true;
            context.Messages.Update(mess);
            await context.SaveChangesAsync();
        }
        return RedirectToAction("SeeMessages");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteMessage(int id)
    {
        var mess = await context.Messages.FindAsync(id);
        if (mess != null)
        {
            context.Messages.Remove(mess);
            await context.SaveChangesAsync();
        }
        return RedirectToAction("SeeMessages");
    }

}