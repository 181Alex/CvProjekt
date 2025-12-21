using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CvProjekt.Models
{
    public class Resume
    {
        [Key]
        public int Id { get; set; }
        public List<String> qualifications { get; set; }

        public virtual ICollection <Work> workList { get; set; } = new List<Work>();
        public virtual ICollection<Education> educationList { get; set; } = new List<Education>();  
    }
}
