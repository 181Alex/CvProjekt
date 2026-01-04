using System.ComponentModel.DataAnnotations.Schema;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CvProjekt.Models
{
    public class Qualification
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Kompetensen måste ha ett namn")]
        [StringLength(50, ErrorMessage = "Max 50 tecken")]
        public string Name { get; set; }

        // Koppling till Resume
        public int ResumeId { get; set; }

        [ForeignKey(nameof(ResumeId))]
        public virtual Resume Resume { get; set; }
    }
}