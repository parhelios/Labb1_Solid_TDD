using Microsoft.EntityFrameworkCore;
using WebShop.Domain.Entities;

namespace WebShop.Infrastructure.DataAccess;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
        
    }
    public DbSet<Product> Products { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderProducts> OrderProducts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>().HasKey(p => p.Id);
        modelBuilder.Entity<Order>().HasKey(o => o.Id);
        modelBuilder.Entity<Customer>().HasKey(c => c.Id);
        
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(c => c.Id) 
                .IsRequired();
        });

        modelBuilder.Entity<OrderProducts>(entity =>
        {
            entity.HasKey(op => new { op.Id, op.ProductId }); 

            entity.HasOne(op => op.Order)
                .WithMany(o => o.Products) 
                .HasForeignKey(op => op.Id);

            entity.HasOne(op => op.Product)
                .WithMany() 
                .HasForeignKey(op => op.ProductId);
        });
        
        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(p => p.Price).IsRequired();
            entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
            entity.Property(c => c.Email).IsRequired().HasMaxLength(100);
        });
    }
}