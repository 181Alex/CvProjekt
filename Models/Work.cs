using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Work
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Du måste ange företagsnamn")]
        [StringLength(50, ErrorMessage = "Max 50 tecken")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Du måste ange position")]
        [StringLength(50, ErrorMessage = "Max 50 tecken")]
        public string Position { get; set; }

        [Required(ErrorMessage = "Du måste ange startdatum")]
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }

        [StringLength(200, ErrorMessage = "Max 200 tecken")]
        public string? Description { get; set; }
        public int ResumeId { get; set; }

        [ForeignKey(nameof(ResumeId))]
        public virtual Resume Resume { get; set; }
    }
}
