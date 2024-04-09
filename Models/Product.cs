using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OnlineRetailStoreV01.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage ="Product name is required")]
        [StringLength(50)]
        [DisplayName("Product Name")]
        public string ProductName {  get; set; }

        [Required(ErrorMessage ="Product description is required")]
        [StringLength(200)]
        [DisplayName("Product Description")]
        public string ProductDescription { get; set; }
        
    }
}
