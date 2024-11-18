using Microsoft.EntityFrameworkCore;
using WebShop.Entities;

namespace WebShop;

public class DbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DbSet<ProductEntity> ProductEntities { get; set; }
    
    public DbContext(DbContextOptions options) : base(options)
    {
        
    }
    
}