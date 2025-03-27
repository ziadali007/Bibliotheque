using bibliotheque.Models;
using System.ComponentModel.DataAnnotations;

namespace Cart_biblio.Models
{
    public class Carts
    {
        [Key]
        public int cartId { get; set; }

        public string book_name { get; set; }

        public Book? book { get; set; }

        public List<CartItem>? Items { get; set; }

}
}
