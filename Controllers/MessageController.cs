using System.Diagnostics;
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



    [HttpGet]
    public IActionResult SendMessages(string receiverId)
    {
    var receiver = context.Users.FirstOrDefault(u => u.Id == receiverId);
    var model = new Message
    {
        ToUserId = receiver.Id,
        ToUser = receiver   
    };
    return View(model);
    }
    
    [HttpPost]
    public async Task<IActionResult> SendMessage(Message mess)
    {
    mess.Date = DateTime.Now;
    mess.Read = false;
    
    var nowUser = await _userManager.GetUserAsync(User);
        if (nowUser != null)
        {
            mess.FromUserId = nowUser.Id;
            mess.SenderName = $"{nowUser.FirstName} {nowUser.LastName}";
            ModelState.Remove("SenderName");
        }
        else
        {
            mess.FromUserId = null;
            if (string.IsNullOrWhiteSpace(mess.SenderName))
                {
                    ModelState.AddModelError("SenderName", "Du måste ange ditt namn för att skicka meddelande.");
                }
        }
    ModelState.Remove("ToUser");
    ModelState.Remove("FromUser");
    ModelState.Remove("FromUserId");
    if (ModelState.IsValid)
    {
        context.Messages.Add(mess);
        await context.SaveChangesAsync();
        return RedirectToAction("SeeMessages");
        }

    mess.ToUser = context.Users.Find(mess.ToUserId);
    return View("SendMessages", mess);
    }



    [HttpGet]
    public async Task<IActionResult> SeeMessages()
    {
        var nowUser = await _userManager.GetUserAsync(User);
        if (nowUser == null)
        {
            return Content("Fel: Inga användare hittades i databasen.");

        }        

        var messages = await context.Messages
            .Include(m => m.FromUser)
            .Where(m => m.ToUserId == nowUser.Id)
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

    [HttpGet]
    public async Task<IActionResult> DeleteMessage(int id)
    {
        var message = await context.Messages
            .Include(m => m.FromUser)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (message == null)
        {
            return NotFound();
        }
        return View(message);
    }
    [HttpPost, ActionName("DeleteMessage")]
    public async Task<IActionResult> DeleteMessageConfirmed(int id)
    {
        var message = await context.Messages.FindAsync(id);
        
        var currentUser = await _userManager.GetUserAsync(User);
        
        if (message != null && currentUser != null && message.ToUserId == currentUser.Id)
        {
            context.Messages.Remove(message);
            await context.SaveChangesAsync();
        }

        return RedirectToAction("SeeMessages");
    }

}