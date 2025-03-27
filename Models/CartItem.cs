namespace Cart_biblio.Models
{
    public class CartItem
    {
        public int cartItemId { get; set; }
        public string book_name { get; set; }
        public int Quantity { get; set; }
        public int totalPrice { get; set; }
        public string userNationalId { get; set;}
    }
}
