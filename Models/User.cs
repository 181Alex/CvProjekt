using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace CvProjekt.Models
{
    //Inherits: Id, Username, Email och PasswordHash
    public class User:IdentityUser
    {
        [Required(ErrorMessage = "Du måste skriva förnamn")]
        [StringLength(30, ErrorMessage = "Max 30 tecken")]
        [RegularExpression("^[a-zA-ZåäöÅÄÖ]+$",
            ErrorMessage = "Du får inte ha specialtecken eller siffror")]
        public string FirstName;
        
        [Required(ErrorMessage = "Du måste skriva efternamn")]
        [StringLength(50, ErrorMessage = "Max 50 tecken")]
        [RegularExpression("^[a-zA-ZåäöÅÄÖ]+$",
            ErrorMessage = "Du får inte ha specialtecken eller siffror")]
        public string LastName;

        [Required(ErrorMessage = "Du måste skriva adress")]
        [StringLength(100, ErrorMessage = "Max 100 tecken")]
        [RegularExpression("^[a-zA-Z0-9]+$",
            ErrorMessage = "Du får inte ha specialtecken")]
        public string Adress;

        public bool isActive;

        public int ProfileVisits;

        public virtual ICollection<Message> Messages {get; set;} = new List<Message>();
        public virtual ICollection<Project> Projects {get; set;} = new List<Project>();

        public int ResumeId;

        [ForeignKey(nameof(ResumeId))]
        public virtual Resume Resume {get; set;}


    }
}
