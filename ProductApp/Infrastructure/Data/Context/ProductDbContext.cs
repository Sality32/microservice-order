using Microsoft.EntityFrameworkCore;
using ProductApp.Core.Domain.Entities;

namespace ProductApp.Infrastructure.Data.Context;

public class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
}