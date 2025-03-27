using Cart_biblio.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bibliotheque.Models
{
	public class Book
	{
		[Key]
		[Required(ErrorMessage = "Book name is required")]
		public string? book_name { get; set; }


		[Required(ErrorMessage = "Author name is required")]
		public string? author { get; set; }

		[Required(ErrorMessage = "Quantity value is required")]
		[Range(1, int.MaxValue, ErrorMessage = "Invalid Input")]
		public int quantity { get; set; }

		[Required(ErrorMessage = "Price value is required")]
		[Range(1, int.MaxValue, ErrorMessage = "Invalid Input")]
		public int price { get; set; }

		[Required(ErrorMessage = "Enter Yes or No")]
		[RegularExpression("^(Yes|No|yes|no|YES|NO)$", ErrorMessage = "Your condition must be 'Yes' or 'No'.")]
		public string borrowable { get; set; }

		[Display(Name = "Image")]
		[DefaultValue("default.jpg")]
		public string? book_pic { get; set; }	

		public virtual ICollection<Operation>? Operations { get; set; }

        public virtual ICollection<Bridge>? Bridge { get; set; }



    }
}

