using Smart_Delivery___Fleet_Management_System.Models;

namespace Smart_Delivery___Fleet_Management_System.Interface
{
    public interface IDriversRepository
    {
        Task AddAsync(Driver driver);
        Task DeleteAsync(Driver driver);
        Task UpdateAsync(Driver driver);
        Task<Driver?> GetByIdAsync(int id);
        Task<Driver?> GetByUserIdAsync(int userId);
        Task<IEnumerable<Driver>> GetAllAsync();
        Task<IEnumerable<Driver>> GetAvailableWithLocationAsync();
        Task UpdateDriverLocationAsync(int driverId, double lat, double lng);
        Task<int> GetActiveOrdersCountAsync(int driverId);
        Task SaveAsync();
        
    }
}
