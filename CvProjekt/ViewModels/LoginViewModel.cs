using System.ComponentModel.DataAnnotations;

namespace CvProjekt.ViewModels
{
	public class LoginViewModel
	{
		[Required(ErrorMessage = "Vänligen ange e-post")]
		[EmailAddress(ErrorMessage = "Ogiltig e-postadress")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Vänligen ange lösenord")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}
}