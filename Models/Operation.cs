using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bibliotheque.Models
{
	public class Operation
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int operation_id { get; set; }

		[ForeignKey("Client")]
		public string national_id { get; set; }

		[ForeignKey("Book")]
		public string book_name	 { get; set; }

		//[ForeignKey("Book")]
		public int quantity { get; set; }

		public string type { get; set; }

		public DateTime date { get; set; }

		public virtual Book Book { get; set; }

		public virtual Client Client { get; set; }
		
	}
}
