using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bibliotheque.Models
{
    public class LoginModel
    {
        [DefaultValue(0)]
        public int Id { get; set; }

        [Required (ErrorMessage ="Please enter valid email address")]
        [EmailAddress]
        [Display (Name ="Email Address")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Please enter a password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
