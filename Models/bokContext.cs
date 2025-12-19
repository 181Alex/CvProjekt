using Microsoft.EntityFrameworkCore;
namespace CvProjekt.Models
{
    public class bokContext : DbContext
    {
        public bokContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Bok> Bocker { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Bok>().HasData(
                new Bok
                {
                    Id = 1,
                    Name = "Soppa"
                });
        }

    }
}
