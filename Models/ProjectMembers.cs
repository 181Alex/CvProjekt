using System.ComponentModel.DataAnnotations.Schema;

namespace CvProjekt.Models
{
    public class ProjectMembers
    {
        public string UserId {  get; set; }
        public int ProjectId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User user { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public Project project { get; set; }
    }
}
