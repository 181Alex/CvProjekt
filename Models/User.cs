using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace CvProjekt.Models
{
    public class User:IdentityUser
    {
        public int Id {get; set; }

        public string Name {get; set;}

        public string Adress {get; set;}

        public string Email {get; set;}

        
    }
}
