namespace OnlineRetailStoreV01.Models
{
    public class VendorProduct
    {
        public int VendorId {  get; set; }
        public Vendor Vendor { get; set; }
        public int ProductId { get; set; }  
        public Product Product { get; set; }
    }
}