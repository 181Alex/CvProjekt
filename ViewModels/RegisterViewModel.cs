using Microsoft.JSInterop.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace CvProjekt.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Vänligen ange förnamn")]
        [StringLength(30, ErrorMessage = "Max 30 tecken")]
        // Tillåter stora och små bokstäver samt å, ä, ö
        [RegularExpression(@"^[a-zA-ZåäöÅÄÖ]+$",
            ErrorMessage = "Förnamnet får endast innehålla bokstäver")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Vänligen ange efternamn")]
        [StringLength(50, ErrorMessage = "Max 50 tecken")]
        // Tillåter stora och små bokstäver samt å, ä, ö
        [RegularExpression(@"^[a-zA-ZåäöÅÄÖ]+$",
            ErrorMessage = "Efternamnet får endast innehålla bokstäver")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Vänligen ange adress")]
        [StringLength(100, ErrorMessage = "Max 100 tecken")]
        [RegularExpression(@"^[a-zA-Z0-9åäöÅÄÖ\s]+$",
            ErrorMessage = "Adressen får inte innehålla specialtecken")]
        public string Adress { get; set; }

        [Required(ErrorMessage = "Vänligen ange e-post")]
        [EmailAddress(ErrorMessage = "Ogiltig e-postadress")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vänligen ange lösenord")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Lösenorden matchar inte")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}