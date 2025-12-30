namespace CvProjekt.Models
{
	public class HomeViewModel
	{
		public List<User> Users { get; set; } = new List<User>();
		public Project? LatestProject { get; set; }
	}
}