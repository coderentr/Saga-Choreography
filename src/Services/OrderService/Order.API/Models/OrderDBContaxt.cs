using Microsoft.EntityFrameworkCore;
using Order.API.Models.Entities;

namespace Order.API.Models
{
    public class OrderDBContext:DbContext
    {
        public OrderDBContext(DbContextOptions optons):base(optons)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Entities.Order>()
                .Property(p => p.TotalPrice)
                .HasColumnType("decimal(18,4)");
            modelBuilder.Entity<OrderItem>()
             .Property(p => p.Price)
             .HasColumnType("decimal(18,4)");
        }
        public DbSet<Entities.Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
    }
}
