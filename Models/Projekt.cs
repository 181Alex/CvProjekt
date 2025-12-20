using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CvSida.Models
{
	public class Projekt
	{
		[Key]
		public int Id { get; set; }

		public string Titel { get; set; }

		public string Språk { get; set; }

		public string GithubLänk { get; set; }

		public int År { get; set; }

		public string Beskrivning { get; set; }


	}
}