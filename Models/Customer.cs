using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineRetailStoreV01.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        //Navigation property
        [ForeignKey("User")]
        public int UserId {  get; set; }
        public virtual User User { get; set; }  
        public ICollection<Order> Orders { get; set;}
    }
}
