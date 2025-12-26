using ASP;
using CvProjekt.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CvProjekt.Controllers;

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
        if (nowUser != null)
        {
            mess.FromUserId = nowUser.Id;
        }
        
        context.Messages.Add(mess);
        await context.SaveChangesAsync();
        return RedirectToAction("SeeMessages");
    }

    [HttpGet]
    public async Task<IActionResult> SeeMessages()
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
    }
    
    [HttpPost]
    public async Task<IActionResult> ReadMessage(Message mess)
    {
        mess.Read=true;
        context.Messages.Update(mess);
        await context.SaveChangesAsync();
        return RedirectToAction("SeeMessages");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteMessage(Message mess)
    {
        context.Messages.Remove(mess);
        await context.SaveChangesAsync();
        return RedirectToAction("SeeMessages");
    }

}