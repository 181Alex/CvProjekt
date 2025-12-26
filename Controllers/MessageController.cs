using System.Diagnostics;
using CvProjekt.Models;
using Microsoft.AspNetCore.Mvc;
namespace CvProjekt.Controllers
{
    public class MessageController : Controller
    {
        private readonly ILogger<MessageController> _logger;

        public MessageController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
    }
}