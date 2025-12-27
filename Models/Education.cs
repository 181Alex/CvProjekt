using System.ComponentModel.DataAnnotations.Schema;

namespace CvProjekt.Models
{
    public class Education
    {
        public int Id { get; set; }
        public string SchoolName { get; set; }
        public string DegreeName { get; set; }
        public int StartYear { get; set; }
        public int? EndYear { get; set; }
        public string Description { get; set; }
        public int ResumeId { get; set; }

        [ForeignKey(nameof(ResumeId))]
        public virtual Resume Resume { get; set; }
    }
}
