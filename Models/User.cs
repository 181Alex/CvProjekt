using Microsoft.AspNetCore.Identity;

namespace CvProjekt.Models
{
    //Inherits: Id, Username, Email och PasswordHash
    public class User:IdentityUser
    {
        public string FirstName;
        public string LastName;

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
