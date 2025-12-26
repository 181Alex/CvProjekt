using System.ComponentModel.DataAnnotations;

namespace CvProjekt.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Vänligen ange förnamn")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Vänligen ange efternamn")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Vänligen ange adress")]
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