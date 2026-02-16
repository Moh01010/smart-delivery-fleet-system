using Smart_Delivery___Fleet_Management_System.Models;

namespace Smart_Delivery___Fleet_Management_System.Interface
{
    public interface IUserRepository
    {
        Task<User?> GetByPhoneAsync(string phone);
        Task AddAsync(User user);
        Task SaveAsync();
    }
}
