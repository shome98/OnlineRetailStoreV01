using Microsoft.EntityFrameworkCore;
using OnlineRetailStoreV01.Models;

namespace OnlineRetailStoreV01.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Product> Products { get; set; }    
        public DbSet<Customer> Customers { get; set; }  
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems {  get; set; }   
        public DbSet<Courier> Couriers { get; set; }    
        public DbSet<Delivery> Deliveries { get; set;}
        public DbSet<VendorProduct> VendorProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VendorProduct>()
                .HasKey(vp => new { vp.VendorId, vp.ProductId });

            modelBuilder.Entity<OrderItem>()
                .HasKey(oi => new {oi.OrderId, oi.ProductId});

            modelBuilder.Entity<OrderItem>()
                .HasOne(o => o.Order)
                .WithMany(oi => oi.OrderItems)
                .HasForeignKey(oi => oi.OrderId);
            modelBuilder.Entity<Delivery>()
                .HasOne(d=>d.Order)
                .WithMany(o=>o.Deliveries)
                .HasForeignKey(d=>d.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Delivery>()
                .HasOne(d=>d.Courier)
                .WithMany(c=>c.Deliveries)
                .HasForeignKey(d=>d.OrderId) 
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<VendorProduct>()
                .HasOne(vp=>vp.Product)
                .WithMany(p=>p.Vendors)
                .HasForeignKey(vp=>vp.ProductId);
            modelBuilder.Entity<VendorProduct>()
                .HasOne(vp=>vp.Vendor)
                .WithMany(v=>v.VendorProducts)
                .HasForeignKey(vp=>vp.ProductId);

        }
    }
    
}
