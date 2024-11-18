using Microsoft.EntityFrameworkCore;

namespace WebShop.DataAccess;

public class DbContext(DbContextOptions options) : Microsoft.EntityFrameworkCore.DbContext(options)
{
    public DbSet<Entities.Product> Products { get; set; }
    public DbSet<Entities.Customer> Customers { get; set; }
    public DbSet<Entities.Order> Orders { get; set; }
    public DbSet<Entities.OrderProducts> OrderProducts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Entities.Product>().HasKey(p => p.Id);
        modelBuilder.Entity<Entities.Order>().HasKey(o => o.Id);
        modelBuilder.Entity<Entities.Customer>().HasKey(c => c.Id);
        
        modelBuilder.Entity<Entities.Order>(entity =>
        {
            entity.HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(c => c.Id) 
                .IsRequired();
        });

        modelBuilder.Entity<Entities.OrderProducts>(entity =>
        {
            entity.HasKey(op => new { op.OrderId, op.ProductId }); 

            entity.HasOne(op => op.Order)
                .WithMany(o => o.Products) 
                .HasForeignKey(op => op.OrderId);

            entity.HasOne(op => op.Product)
                .WithMany() 
                .HasForeignKey(op => op.ProductId);
        });
        
        modelBuilder.Entity<Entities.Product>(entity =>
        {
            entity.Property(p => p.Price).IsRequired();
            entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<Entities.Customer>(entity =>
        {
            entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
            entity.Property(c => c.Email).IsRequired().HasMaxLength(100);
        });
    }
}