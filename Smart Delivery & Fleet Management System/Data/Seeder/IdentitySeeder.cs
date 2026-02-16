using Smart_Delivery___Fleet_Management_System.Enums;
using Smart_Delivery___Fleet_Management_System.Models;
using Smart_Delivery___Fleet_Management_System.Services;
namespace Smart_Delivery___Fleet_Management_System.Data.Seeder
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(DeliveryDbContext context, PasswordHasher hasher)
        {
            if (!context.Users.Any())
            {
                var users = new List<User>
        {
            new User
            {
                Name = "System Admin",
                Phone = "1000",
                PasswordHash = hasher.Hash("admin123"),
                Role = UserRole.Admin
            },
            new User
            {
                Name = "Main Dispatcher",
                Phone = "2000",
                PasswordHash = hasher.Hash("dispatch123"),
                Role = UserRole.Dispatcher
            }
        };

                await context.Users.AddRangeAsync(users);
                await context.SaveChangesAsync();
            }
        }

    }

}
