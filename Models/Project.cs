using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
	public class Project
	{
		[Key]
		public int Id { get; set; }

		[Required(ErrorMessage = "Du måste skriva titel")]
        [StringLength(50, ErrorMessage = "Max 50 tecken")]
		public string Title { get; set; }

		[Required(ErrorMessage = "Du måste skriva språk")]
        [StringLength(50, ErrorMessage = "Max 50 tecken")]
		public string Language { get; set; }

        [RegularExpression(@"^https?://(www\.)?github\.com/[A-Za-z0-9_.-]+/[A-Za-z0-9_.-]+(?:\.git)?/?$",
			ErrorMessage = "Enter a valid GitHub repository URL (https://github.com/owner/repo).")]
        public string? GithubLink { get; set; }

		[Required(ErrorMessage = "Du måste skriva årtal")]
		[RegularExpression(@"^\d{4}$", 
            ErrorMessage = "Årtalet måste innehålla fyra siffror")]
		public int Year { get; set; }

		[StringLength(200, ErrorMessage = "Max 200 tecken")]
		public string? Description { get; set; }

		public string CreatorId { get; set; }
        [ForeignKey(nameof(CreatorId))]
        public virtual User Creator { get; set; }

		public virtual IEnumerable<ProjectMembers> ProjectMembers {get; set;} = new List<ProjectMembers>();


	}
}