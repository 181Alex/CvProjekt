using System.Xml.Serialization;

namespace CvProjekt.Models

    //DTO klass (data transfer objekt), behövs skapas för att inte serieliserareren ska snurra runt bland alla referenser och ordentligt hitta allt.
    //listor i kalsserna blir en lsita av den specefika dtokalssen, t.ex lsita av projekt blir lista av projektdto
    // jag har inte hller lagt in meddelanden då jag inte tycker att det är profil information lol
{
    [XmlRoot("UserProfile")]
    public class UserExportDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Adress { get; set; }
        public string ImgUrl { get; set; }

        [XmlArray("Projects")]
        [XmlArrayItem("Project")]
        public List<ProjectExportDto> Projects { get; set; } = new List<ProjectExportDto>();
        public ResumeExportDto Resume { get; set; }
        public List<ProjectMembersDto> ProjectMember { get; set; } = new List<ProjectMembersDto>();
    }
    public class ProjectExportDto
    {
        public string Title { get; set; }
        public string Language { get; set; }
        public string Description { get; set; }
        public string GithubLink { get; set; }
        public int Year { get; set; }
    }

    public class ResumeExportDto
    {
        [XmlArray("WorkExperience")]
        [XmlArrayItem("Work")]
        public List<WorkExportDto> WorkList { get; set; } = new List<WorkExportDto>();

        [XmlArray("Qualifications")]
        [XmlArrayItem("Qualification")]
        public List<QualificationExportDto> Qualifications { get; set; } = new List<QualificationExportDto>();

        [XmlArray("Educations")]
        [XmlArrayItem("Education")]
        public List<EducationExportDto> EducationList { get; set; } = new List<EducationExportDto>();
    }

    public class WorkExportDto
    {
        public string CompanyName { get; set; }
        public string Position { get; set; }
        public string Description { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; } //Gjorde den till string så att man kan sätta pågående som alternativy
    }

    public class QualificationExportDto
    {
        public string Name { get; set; }
    }

    public class EducationExportDto
    {
        public string SchoolName { get; set; }
        public string DegreeName { get; set; }
        public int StartYear { get; set; }
        public string EndYear { get; set; }//samma som ovan
        public string Description { get; set; }
    }

    public class ProjectMembersDto
    {
        public string Title { get; set; }
        public string Language { get; set; }
        public string Description { get; set; }
        public string GithubLink { get; set; }
        public int Year { get; set; }
    }
}
