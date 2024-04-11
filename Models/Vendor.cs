using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineRetailStoreV01.Models
{
    public class Vendor
    {
        [Key]
        public int VendorId {  get; set; }

        //Navigation Property
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<Product> Products { get; set; }  
        public ICollection<VendorProduct> VendorProducts { get; set; }

    }
}