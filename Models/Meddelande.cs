using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CvSida.Models
{
    public class Meddelande
    {
        [Key]
        public int Id { get; set; }

        // DateTime.now hämtar datumet när meddelande skickades 
        public DateTime Datum { get; set; } = DateTime.Now;

        public string Text { get; set; }

        public bool Läst { get; set; } = false;

        public string FrånAnvändareId { get; set; }


        // Virtual gör lazy loading möjligt  
        [ForeignKey(nameof(FrånAnvändareId))]
        public virtual Användare FrånAnvändare { get; set; }

        public string TillAnvändareId { get; set; }

        [ForeignKey(nameof(TillAnvändareId))]
        public virtual Användare TillAnvändare { get; set; }
    }
}