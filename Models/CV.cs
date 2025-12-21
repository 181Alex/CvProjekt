using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CvProjekt.Models
{
    public class CV
    {
        [Key]
        public int Id { get; set; }
        public List<String> qualifications { get; set; }

        public List<Work> workList { get; set; }
        public List<Projekt> projektList { get; set; }
        public List<Education> educationList { get; set; }




    }
}
