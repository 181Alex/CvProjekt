using Microsoft.EntityFrameworkCore;

namespace CvProjekt.Models
{
    public class CvContext:DbContext
    {
        public CvContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Resume> Resumes { get; set; }
        public DbSet<Project> Projects { get; set; }

        public DbSet<Education> Education { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Work> Works { get; set; }
    }
}
