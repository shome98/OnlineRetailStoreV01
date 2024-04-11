using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineRetailStoreV01.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }

        //Navigation Property
        [ForeignKey("Order")]
        public int OrderId { get; set; }    
        public Order Order { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}