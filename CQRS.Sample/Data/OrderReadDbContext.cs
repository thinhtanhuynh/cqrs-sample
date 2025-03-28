using Microsoft.EntityFrameworkCore;
using CQRS.Sample.Models;

namespace CQRS.Sample.Data;
public class OrderReadDbContext : DbContext
{
    public OrderReadDbContext(DbContextOptions<OrderReadDbContext> options) : base(options) { }

    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure OrderDto for reading.  Use fluent API.
        modelBuilder.Entity<Order>().HasKey(o => o.Id); // Define PK
        modelBuilder.Entity<Order>().ToTable("Orders"); // Map to a view or table
        modelBuilder.Entity<Order>().Property(o => o.Id).HasColumnName("id");
        modelBuilder.Entity<Order>().Property(o => o.CustomerName).HasColumnName("customerName");
        modelBuilder.Entity<Order>().Property(o => o.OrderDate).HasColumnName("orderDate");
        modelBuilder.Entity<Order>().Property(o => o.Status).HasColumnName("status");
        modelBuilder.Entity<Order>().Property(o => o.Total).HasColumnName("total");

        modelBuilder.Entity<OrderItem>().HasKey(oi => new { oi.OrderId, oi.ProductId });  // Composite Key
        modelBuilder.Entity<OrderItem>().ToTable("OrderItems");
        modelBuilder.Entity<OrderItem>().Property(oi => oi.OrderId).HasColumnName("orderId");
        modelBuilder.Entity<OrderItem>().Property(oi => oi.ProductId).HasColumnName("productId");
        modelBuilder.Entity<OrderItem>().Property(oi => oi.ProductName).HasColumnName("productName");
        modelBuilder.Entity<OrderItem>().Property(oi => oi.Quantity).HasColumnName("quantity");
        modelBuilder.Entity<OrderItem>().Property(oi => oi.Price).HasColumnName("price");
        modelBuilder.Entity<OrderItem>()
            .HasOne<Order>()
            .WithMany(o => o.Items)
            .HasForeignKey(oi => oi.OrderId);
    }

}