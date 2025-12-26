using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CvProjekt.Models
{
    public class CvContext : IdentityDbContext<User>
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
        public DbSet<Qualification> Qualifications {get; set;}

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
                .OnDelete(DeleteBehavior.Restrict);

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



            // ==========================================
            // SEED DATA (STATISK DATA)
            // ==========================================

                string u1 = "user-1"; 
                string u2 = "user-2"; 
                string u3 = "user-3"; 
                string u4 = "user-4"; 
                string u5 = "user-5"; 

                modelBuilder.Entity<Resume>().HasData(
                    new Resume { Id = 1 }, new Resume { Id = 2 }, new Resume { Id = 3 }, new Resume { Id = 4 }, new Resume { Id = 5 }
                );

                string staticPasswordHash = "AQAAAAIAAYagAAAAELg7Xy0k9/8Q7k6Xy0k9/8Q7k6Xy0k9/8Q7k6Xy0k9/8Q7k6Xy==";

                modelBuilder.Entity<User>().HasData(
                    CreateUser(u1, "erik@mail.com", "Erik", "Svensson", "Storgatan 1", true, 1, staticPasswordHash, 
                        "https://images.unsplash.com/photo-1500648767791-00dcc994a43e?w=200"),
                    
                    CreateUser(u2, "anna@mail.com", "Anna", "Lind", "Sveavägen 10", true, 2, staticPasswordHash, 
                        "https://images.unsplash.com/photo-1494790108377-be9c29b29330?w=200"),
                    
                    CreateUser(u3, "johan@mail.com", "Johan", "Ek", "Hamngatan 4", true, 3, staticPasswordHash, 
                        "https://images.unsplash.com/photo-1560250097-0b93528c311a?w=200"),
                    
                    CreateUser(u4, "sara@mail.com", "Sara", "Berg", "Skolgatan 55", true, 4, staticPasswordHash, 
                        "https://images.unsplash.com/photo-1573496359-0933d2768d98?w=200"),
                    
                    CreateUser(u5, "david@mail.com", "David", "Nordin", "Studentvägen 3", false, 5, staticPasswordHash, 
                        "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=200")
                );

                modelBuilder.Entity<Qualification>().HasData(
                    new Qualification { Id = 1, Name = "C#", ResumeId = 1 },
                    new Qualification { Id = 2, Name = "SQL Server", ResumeId = 1 },
                    new Qualification { Id = 3, Name = "JavaScript", ResumeId = 2 },
                    new Qualification { Id = 4, Name = "React", ResumeId = 2 },
                    new Qualification { Id = 5, Name = "Scrum", ResumeId = 3 },
                    new Qualification { Id = 6, Name = "Python", ResumeId = 4 },
                    new Qualification { Id = 7, Name = "Machine Learning", ResumeId = 4 },
                    new Qualification { Id = 8, Name = "HTML", ResumeId = 5 }
                );

                modelBuilder.Entity<Work>().HasData(
                    new Work { Id = 1, CompanyName = "Volvo", Position = "Utvecklare", StartDate = new DateTime(2020, 1, 1), EndDate = new DateTime(2022, 1, 1), Description = "Backend C#", ResumeId = 1 },
                    new Work { Id = 2, CompanyName = "Spotify", Position = "Frontend Dev", StartDate = new DateTime(2021, 5, 1), Description = "Jobbar med webbspelaren", ResumeId = 2 },
                    new Work { Id = 3, CompanyName = "IKEA", Position = "Chef", StartDate = new DateTime(2018, 1, 1), EndDate = new DateTime(2023, 1, 1), Description = "Ledde IT-team", ResumeId = 3 },
                    new Work { Id = 4, CompanyName = "Google", Position = "Data Analyst", StartDate = new DateTime(2019, 1, 1), Description = "AI forskning", ResumeId = 4 }
                );

                modelBuilder.Entity<Education>().HasData(
                    new Education { Id = 1, SchoolName = "KTH", DegreeName = "Civilingenjör", StartYear = 2015, EndYear = 2020, Description = "Datateknik", ResumeId = 1 },
                    new Education { Id = 2, SchoolName = "Nackademin", DegreeName = "Frontend", StartYear = 2019, EndYear = 2021, Description = "YH-utbildning", ResumeId = 2 },
                    new Education { Id = 3, SchoolName = "Handels", DegreeName = "Ekonomi", StartYear = 2010, EndYear = 2014, Description = "Master", ResumeId = 3 },
                    new Education { Id = 4, SchoolName = "MIT", DegreeName = "PhD CS", StartYear = 2015, EndYear = 2019, Description = "Forskning", ResumeId = 4 },
                    new Education { Id = 5, SchoolName = "Gymnasiet", DegreeName = "Teknik", StartYear = 2021, EndYear = 2024, Description = "Student", ResumeId = 5 }
                );

                modelBuilder.Entity<Project>().HasData(
                    new Project { Id = 1, Title = "E-handel", Language = "C#", GithubLink = "github.com/erik/shop", Year = 2023, Description = "Byggde en butik", UserId = u1 },
                    new Project { Id = 2, Title = "Portfolio", Language = "React", GithubLink = "github.com/anna/me", Year = 2024, Description = "Min hemsida", UserId = u2 },
                    new Project { Id = 3, Title = "BudgetApp", Language = "Python", GithubLink = "github.com/sara/cash", Year = 2022, Description = "AI budgetering", UserId = u4 }
                );

                modelBuilder.Entity<Message>().HasData(
                    new Message { Id = 1, Text = "Tjena Anna! Snygg frontend du byggde.", Date = new DateTime(2024, 02, 20), Read = true, FromUserId = u1, ToUserId = u2 },
                    new Message { Id = 2, Text = "Tack Erik! Behöver hjälp med API:et dock.", Date = new DateTime(2024, 02, 21), Read = false, FromUserId = u2, ToUserId = u1 },
                    new Message { Id = 3, Text = "Hej David, söker du jobb?", Date = new DateTime(2024, 03, 01), Read = false, FromUserId = u3, ToUserId = u5 }
                ); 


        }

private User CreateUser(string id, string email, string fName, string lName, string adress, bool active, int resumeId, string passwordHash, string imagePath)
{
    return new User
    {
        Id = id,
        UserName = email,
        NormalizedUserName = email.ToUpper(),
        Email = email,
        NormalizedEmail = email.ToUpper(), // Viktigt för inloggning
        EmailConfirmed = true,             // Viktigt för inloggning
        FirstName = fName,
        LastName = lName,
        Adress = adress,
        IsActive = active,
        ProfileVisits = 0,
        ResumeId = resumeId,
        PasswordHash = passwordHash,
        SecurityStamp = "static-security-stamp-" + id,
        ConcurrencyStamp = "static-concurrency-stamp-" + id,
        ImgUrl = imagePath // Här sparas bildlänken
    };
} 
    }
}
