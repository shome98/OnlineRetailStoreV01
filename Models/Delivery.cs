using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineRetailStoreV01.Models
{
    public class Delivery
    {
        [Key]
        public int DeliveryId { get; set; }

        //navigation property
        [ForeignKey("Courier")]
        public int CourierId {  get; set; } 
        public Courier Courier {  get; set; }
        [ForeignKey("Order")]
        public int OrderId {  get; set; }
        public Order Order { get; set; }
    }
}