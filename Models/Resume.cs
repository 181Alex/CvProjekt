using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CvProjekt.Models
{
    public class Resume
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Vänligen lägg in minst en kompetens")]
        public List<String> Qualifications { get; set; }

        public virtual ICollection <Work> WorkList { get; set; } = new List<Work>();
        public virtual ICollection<Education> EducationList { get; set; } = new List<Education>(); 
        public virtual User User { get; set; } = null!;
    }
}
