using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineRetailStoreV01.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; } 

        [Required(ErrorMessage ="Full name is required")]
        [StringLength(100)]
        [DisplayName("Full Name")]
        [Column("Full Name")]
        public string FullName {  get; set; }

        [Required(ErrorMessage ="Email address is required")]
        [EmailAddress(ErrorMessage ="Invalid email address")]
        [StringLength (100)]
        [DisplayName("Email Address")]
        [Column("Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage="Password is required")]
        [StringLength(100,MinimumLength =6,ErrorMessage ="Password must be atleast 6 characters")]
        [DisplayName("Password")]
        [Column("Hashed Password")]
        public string Password { get; set; }

        [Required(ErrorMessage ="User type is required")]
        [DisplayName("User Type")]
        [Column("User Type")]
        public UserType UserType { get; set; }
    }
}
