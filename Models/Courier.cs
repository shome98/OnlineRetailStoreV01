using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineRetailStoreV01.Models
{
    public class Courier
    {
        [Key]
        public int CourierId { get; set; }

        //Navigation Property
        [ForeignKey("User")]
        public int UserId {  get; set; }
        public User User { get; set; }
        public ICollection<Delivery> Deliveries { get; set; }
    }
}
