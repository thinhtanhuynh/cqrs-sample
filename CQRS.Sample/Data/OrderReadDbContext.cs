using Microsoft.EntityFrameworkCore;
using CQRS.Sample.DTOs;

namespace CQRS.Sample.Data;
public class OrderReadDbContext : DbContext
{
    public OrderReadDbContext(DbContextOptions<OrderReadDbContext> options) : base(options) { }

    public DbSet<OrderDto> Orders { get; set; } // Using DTO as a DbSet

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure OrderDto for reading.  Use fluent API.
        modelBuilder.Entity<OrderDto>().HasKey(o => o.Id); // Define PK
        modelBuilder.Entity<OrderDto>().ToTable("OrdersRead"); // Map to a view or table
        modelBuilder.Entity<OrderDto>().Property(o => o.Id).HasColumnName("Id");
        modelBuilder.Entity<OrderDto>().Property(o => o.CustomerName).HasColumnName("CustomerName");
        modelBuilder.Entity<OrderDto>().Property(o => o.OrderDate).HasColumnName("OrderDate");
        modelBuilder.Entity<OrderDto>().Property(o => o.Status).HasColumnName("Status");
        modelBuilder.Entity<OrderDto>().Property(o => o.Total).HasColumnName("Total");

        modelBuilder.Entity<OrderItemDto>().HasKey(oi => new { oi.OrderId, oi.ProductId });  // Composite Key
        modelBuilder.Entity<OrderItemDto>().ToTable("OrderItemsRead");
        modelBuilder.Entity<OrderItemDto>().Property(oi => oi.OrderId).HasColumnName("OrderId");
        modelBuilder.Entity<OrderItemDto>().Property(oi => oi.ProductId).HasColumnName("ProductId");
        modelBuilder.Entity<OrderItemDto>().Property(oi => oi.ProductName).HasColumnName("ProductName");
        modelBuilder.Entity<OrderItemDto>().Property(oi => oi.Quantity).HasColumnName("Quantity");
        modelBuilder.Entity<OrderItemDto>().Property(oi => oi.Price).HasColumnName("Price");
    }

}