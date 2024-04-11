using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mail;

namespace OnlineRetailStoreV01.Models
{
    public class Order
    {
        public int OrderId {  get; set; }

        //Navigation Property
        [ForeignKey("Customer")]
        public int CustomerId {  get; set; }    
        public Customer Customer { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }   
        public ICollection<Delivery> Deliveries { get; set; }   
    }
}