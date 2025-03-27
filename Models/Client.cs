using System.ComponentModel.DataAnnotations;

namespace bibliotheque.Models
{
	public class Client
	{
		[Key]
		[StringLength(14)]
		[Required(ErrorMessage = "National id value is required")]
		public string national_id { get; set; }

		[Required(ErrorMessage = "Client name value is required")]
		public string client_name { get; set; }


		[Phone]
		[Required(ErrorMessage = "Telephone value is required")]
		[StringLength(11)]
		public string telephone { get; set; }


		[EmailAddress]
		[Required(ErrorMessage = "Email address value is required")]
		public string email_address { get; set; }


		[Required(ErrorMessage = "Home address value is required")]
		public string home_address { get; set; }

		public virtual ICollection<Operation> Operations { get; set; }

	}
}

