using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CvProjekt.Models
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Vänligen ange ditt nuvarande lösenord")]
        [DataType(DataType.Password)]
        [Display(Name = "Nuvarande lösenord")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Vänligen ange ett nytt lösenord")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Lösenordet måste vara minst {2} tecken")]
        [DataType(DataType.Password)]
        [Display(Name = "Nytt lösenord")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Vänligen bekräfta det nya lösenordet")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Lösenorden matchar inte")]
        [Display(Name = "Bekräfta nytt lösenord")]
        public string ConfirmPassword { get; set; }
    }
}