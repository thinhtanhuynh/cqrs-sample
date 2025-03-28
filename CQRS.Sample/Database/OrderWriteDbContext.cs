using Microsoft.EntityFrameworkCore;
using CQRS.Sample.Database.Entities;

namespace CQRS.Sample.Database;
public class OrderWriteDbContext : DbContext
{
    public OrderWriteDbContext(DbContextOptions<OrderWriteDbContext> options) : base(options) { }

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>().HasKey(o => o.Id);
        modelBuilder.Entity<OrderItem>().HasKey(oi => oi.Id);
        modelBuilder.Entity<OrderItem>()
            .HasOne<Order>()
            .WithMany(o => o.Items)
            .HasForeignKey(oi => oi.OrderId);
    }
}