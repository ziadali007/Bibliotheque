    using Cart_biblio.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bibliotheque.Models
{
    public class RegisterModel
    {
        [Key]
        //[StringLength(14)]
        public string nationalId { get; set; }

        [Required(ErrorMessage = "Name value is required")]
        public string fullName { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool isAdmin { get; set; }

        [Phone]
        [Required(ErrorMessage = "Telephone value is required")]
        [StringLength(11)]
        public string telephone { get; set; }

        [Required(ErrorMessage = "Email address value is required")]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Home address value is required")]
        public string homeAddress { get; set; }

        [Required(ErrorMessage = "Password value is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Password not match")]
        public string confirmPassword { get; set; }

        public virtual ICollection<Bridge>? Bridge { get; set; }

        public ICollection<Carts>? Carts { get; set; }
        

    }
}
