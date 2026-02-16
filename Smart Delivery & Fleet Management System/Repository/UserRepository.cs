using Microsoft.EntityFrameworkCore;
using Smart_Delivery___Fleet_Management_System.Data;
using Smart_Delivery___Fleet_Management_System.Interface;
using Smart_Delivery___Fleet_Management_System.Models;

namespace Smart_Delivery___Fleet_Management_System.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DeliveryDbContext _context;

        public UserRepository(DeliveryDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task<User?> GetByPhoneAsync(string phone)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Phone == phone);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

}
