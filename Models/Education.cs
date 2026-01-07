using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CvProjekt.Models
{
    public class Education
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Du måste skriva skolnamn")]
        [StringLength(50, ErrorMessage = "Max 50 tecken")]
        public string SchoolName { get; set; }

        [Required(ErrorMessage = "Du måste skriva utbildningsnamn")]
        [StringLength(50, ErrorMessage = "Max 50 tecken")]
        public string DegreeName { get; set; }

        [Required(ErrorMessage = "Du måste ange startår")]
        [RegularExpression(@"^\d{4}$", 
            ErrorMessage = "Årtalet måste innehålla fyra siffror")]
        public int StartYear { get; set; }

        [RegularExpression(@"^\d{4}$", 
            ErrorMessage = "Årtalet måste innehålla fyra siffror")]
        public int? EndYear { get; set; }

        [StringLength(200, ErrorMessage = "Max 200 tecken")]
        public string? Description { get; set; }
        public int ResumeId { get; set; }

        [ForeignKey(nameof(ResumeId))]
        public virtual Resume Resume { get; set; }
    }
}
