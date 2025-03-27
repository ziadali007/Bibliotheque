using bibliotheque.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cart_biblio.Models
{
    [PrimaryKey(nameof(book_name),nameof(nationalId))]
    public class Bridge
    {
        [Key]
        public string book_name { get; set; }
        public string nationalId { get; set; }

        [ForeignKey("book_name")]
        public Book book { get; set; }

        [ForeignKey("nationalId")]
        public RegisterModel userNationalId { get; set; }

    }
}
