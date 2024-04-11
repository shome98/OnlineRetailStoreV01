using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
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

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public double Price { get; set; }

        [Required(ErrorMessage ="Product Image is required")]
        public string ImageUrl {  get; set; }

        [Required(ErrorMessage = "Inventory count is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Inventory count must be non-negative")]
        public int Inventory { get; set; }

        [Required(ErrorMessage = "Product category is required")]
        [Display(Name = "Product Category")]
        public ProductCategory ProductCategory { get; set; }

        //Navigation Properties
        public ICollection<VendorProduct> Vendors { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
