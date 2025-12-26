using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace CvProjekt.Models
{
    //Inherits: Id, Username, Email och PasswordHash
    public class User:IdentityUser
    {
        [Required(ErrorMessage = "Du m�ste skriva f�rnamn")]
        [StringLength(30, ErrorMessage = "Max 30 tecken")]
        [RegularExpression("^[a-zA-Z������]+$",
            ErrorMessage = "Du f�r inte ha specialtecken eller siffror")]
        public string FirstName;
        
        [Required(ErrorMessage = "Du m�ste skriva efternamn")]
        [StringLength(50, ErrorMessage = "Max 50 tecken")]
        [RegularExpression("^[a-zA-Z������]+$",
            ErrorMessage = "Du f�r inte ha specialtecken eller siffror")]
        public string LastName;

        [Required(ErrorMessage = "Du m�ste skriva adress")]
        [StringLength(100, ErrorMessage = "Max 100 tecken")]
        [RegularExpression("^[a-zA-Z0-9]+$",
            ErrorMessage = "Du f�r inte ha specialtecken")]
        public string Adress;

        public bool isActive;

        public int ProfileVisits;

        public virtual ICollection<Message> Messages {get; set;} = new List<Message>();
        public virtual ICollection<Project> Projects {get; set;} = new List<Project>();

        public int? ResumeId {get; set;}

        [ForeignKey(nameof(ResumeId))]
        public virtual Resume? Resume {get; set;}


    }
}
