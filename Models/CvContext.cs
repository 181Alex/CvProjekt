using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CvProjekt.Models
{
    public class CvContext: IdentityDbContext<User>
    {
        public CvContext(DbContextOptions<CvContext> options)
            : base(options)
        {
        }

        public DbSet<Resume> Resumes { get; set; }
        public DbSet<Project> Projects { get; set; }

        public DbSet<Education> Education { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Work> Works { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Relation för CV: Om CV:t raderas, töm bara ResumeId hos User (ingen radering av User)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Resume)
                .WithOne(r => r.User)
                .HasForeignKey<User>(u => u.ResumeId)
                .OnDelete(DeleteBehavior.SetNull);

            //Relation för PROJEKT: Om en User raderas, radera även alla dennes projekt (Cascade)
            modelBuilder.Entity<Project>()
                .HasOne(p => p.User)
                .WithMany(u => u.Projects)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            //Relation för MEDDELANDEN (Mottagare): Om mottagaren raderas, radera meddelandet
            modelBuilder.Entity<Message>()
                .HasOne(m => m.ToUser)
                .WithMany(u => u.Messages)
                .HasForeignKey(m => m.ToUserId)
                .OnDelete(DeleteBehavior.Cascade);

            //Relation för MEDDELANDEN (Avsändare): Om avsändaren raderas, sätt FromUserId till NULL
            modelBuilder.Entity<Message>()
                .HasOne(m => m.FromUser)
                .WithMany()
                .HasForeignKey(m => m.FromUserId)
                .OnDelete(DeleteBehavior.SetNull);

            //Relation för UTBILDNING/JOBB/kvalifikationer: Om CV:t raderas, radera alla tillhörande erfarenheter
            modelBuilder.Entity<Education>()
                .HasOne(e => e.Resume)
                .WithMany(r => r.EducationList)
                .HasForeignKey(e => e.ResumeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Work>()
                .HasOne(w => w.Resume)
                .WithMany(r => r.WorkList)
                .HasForeignKey(w => w.ResumeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Qualification>()
                .HasOne(q => q.Resume)
                .WithMany(r => r.Qualifications)
                .HasForeignKey(q => q.ResumeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
