using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        // DateTime.now h�mtar datumet n�r meddelande skickades 
        public DateTime Date { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Du måste skriva text")]
        // Tillåter bokstäver, siffror, mellanslag (viktigt!) och åäö
        [RegularExpression(@"^[a-zA-Z0-9åäöÅÄÖ\s]+$", ErrorMessage = "texten får inte innehålla specialtecken")]
        public string Text { get; set; }

        public bool Read { get; set; } = false;
        public string SenderName {get; set;}

        public string? FromUserId { get; set; }


        // Virtual g�r lazy loading m�jligt  
        [ForeignKey(nameof(FromUserId))]
        public virtual User? FromUser { get; set; }

        public string ToUserId { get; set; }

        [ForeignKey(nameof(ToUserId))]
        public virtual User ToUser { get; set; }
        
    }
}