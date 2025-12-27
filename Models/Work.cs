using System.ComponentModel.DataAnnotations.Schema;

namespace CvProjekt.Models
{
    public class Work
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Position { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string Description { get; set; }
        public int ResumeId { get; set; }

        [ForeignKey(nameof(ResumeId))]
        public virtual Resume Resume { get; set; }
    }
}
