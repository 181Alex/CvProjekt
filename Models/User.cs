using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Models
{
    //Inherits: Id, Username, Email och PasswordHash
    public class User:IdentityUser
    {
        [Required(ErrorMessage = "Du måste skriva förnamn")]
        [StringLength(30, ErrorMessage = "Max 30 tecken")]
        // Tillåter stora och små bokstäver samt å, ä, ö
        [RegularExpression(@"^[a-zA-ZåäöÅÄÖ]+$", 
            ErrorMessage = "Förnamnet får endast innehålla bokstäver")]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "Du måste skriva efternamn")]
        [StringLength(50, ErrorMessage = "Max 50 tecken")]
        // Tillåter stora och små bokstäver samt å, ä, ö
        [RegularExpression(@"^[a-zA-ZåäöÅÄÖ]+$", 
            ErrorMessage = "Efternamnet får endast innehålla bokstäver")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Du måste skriva adress")]
        [StringLength(100, ErrorMessage = "Max 100 tecken")]
        [RegularExpression(@"^[a-zA-Z0-9åäöÅÄÖ\s]+$", 
            ErrorMessage = "Adressen får inte innehålla specialtecken")]
        public string Adress{get;set;}
        [Required(ErrorMessage = "Du måste skriva epost")]
        [StringLength(100, ErrorMessage = "Max 100 tecken")]

        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            ErrorMessage = "Ogiltig epost")]
        public override string? Email { get => base.Email; set => base.Email = value; }

        public bool IsActive{get;set;}
        public bool IsPrivate { get;set;} = false;

        public int ProfileVisits{get;set;}

        public string? ImgUrl {get; set;}

        public virtual ICollection<Message> Messages {get; set;} = new List<Message>();
        public virtual List<Project> Projects {get; set;} = new List<Project>();

        public virtual List<ProjectMembers> ProjectMembers {get; set;} = new List<ProjectMembers>();

        public int? ResumeId{get;set;}

        [ForeignKey(nameof(ResumeId))]
        public virtual Resume? Resume {get; set;}


    }
}
