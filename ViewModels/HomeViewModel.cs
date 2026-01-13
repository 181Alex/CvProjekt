using Models;
namespace CvProjekt.ViewModels
{
    public class HomeViewModel
    {
        public List<User> Users { get; set; } = new List<User>();
        public List<User> AllUsers { get; set; } = new List<User>();
        public List<Project> LatestProjects { get; set; } = new List<Project>();
        public List<Project> AllProjects { get; set; } = new List<Project>();
        public List<ProjectMembers> ProjectMemberList { get; set; } = new List<ProjectMembers>();
    }
}