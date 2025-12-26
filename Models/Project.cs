using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CvProjekt.Models
{
	public class Project
	{
		[Key]
		public int Id { get; set; }

		public string Title { get; set; }

		public string Language { get; set; }

		public string GithubLink { get; set; }

		public int Year { get; set; }

		public string Description { get; set; }

		public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }


	}
}