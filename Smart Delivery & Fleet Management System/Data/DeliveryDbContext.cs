using Microsoft.EntityFrameworkCore;
using Smart_Delivery___Fleet_Management_System.Models;

namespace Smart_Delivery___Fleet_Management_System.Data
{
    public class DeliveryDbContext: DbContext
    {
        public DeliveryDbContext(DbContextOptions<DeliveryDbContext> options)
        : base(options)
        {
        }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderStatusHistory> OrderStatusHistories { get; set; }
        public DbSet<DriverLocation> DriverLocations { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
