using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CvProjekt.Models
{
    public class Meddelande
    {
        [Key]
        public int Id { get; set; }

        // DateTime.now hämtar datumet när meddelande skickades 
        public DateTime Date { get; set; } = DateTime.Now;

        public string Text { get; set; }

        public bool Read { get; set; } = false;

        public string FromUserId { get; set; }


        // Virtual gör lazy loading möjligt  
        [ForeignKey(nameof(FromUserId))]
        public virtual User FromUser { get; set; }

        public string ToUserId { get; set; }

        [ForeignKey(nameof(ToUserId))]
        public virtual User ToUser { get; set; }
    }
}