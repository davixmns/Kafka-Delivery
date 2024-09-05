using KafkaDelivery.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KafkaDelivery.Infra.Context;

public class DeliveryDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }

    public DeliveryDbContext(DbContextOptions<DeliveryDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConvertLongTextToVarchar(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }
    
    private void ConvertLongTextToVarchar(ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entity.GetProperties())
            {
                if (property.ClrType == typeof(string))
                {
                    // Define o tipo varchar com tamanho 255 por padrão
                    property.SetColumnType("varchar(255)"); 
                }
            }
        }
    }
}