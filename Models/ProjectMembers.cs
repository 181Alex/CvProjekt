using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class ProjectMembers
    {
        public string MemberId {  get; set; }
        public int MProjectId { get; set; }

        [ForeignKey(nameof(MemberId))]
        public User user { get; set; }
        [ForeignKey(nameof(MProjectId))]
        public Project project { get; set; }
    }
}
