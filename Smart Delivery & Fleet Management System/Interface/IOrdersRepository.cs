using Smart_Delivery___Fleet_Management_System.Enums;
using Smart_Delivery___Fleet_Management_System.Models;

namespace Smart_Delivery___Fleet_Management_System.Interface
{
    public interface IOrdersRepository
    {
        Task AddAsync(Order order);
        Task DeleteAsync(Order order);
        Task UpdateAsync(Order order);
        Task<Order?> GetByIdAsync(int id);
        Task<IEnumerable<Order>> GetAllAsync();
        Task<IEnumerable<Order>> GetUnassignedAsync();
        Task<Order?> GetForAssignmentAsync(int id);
        Task AddStatusHistoryAsync(int orderId, OrderStatus status, int? driverId);
        Task SaveAsync();

    }
}
