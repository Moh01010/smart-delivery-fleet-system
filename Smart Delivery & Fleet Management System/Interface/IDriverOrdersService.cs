using Smart_Delivery___Fleet_Management_System.DTOs.DriverOrder;
using Smart_Delivery___Fleet_Management_System.Enums;
using Smart_Delivery___Fleet_Management_System.Models;

namespace Smart_Delivery___Fleet_Management_System.Interface
{
    public interface IDriverOrdersService
    {
        Task<IEnumerable<DriverOrderDto>> GetDriverOrdersAsync(int driverId);
        Task UpdateOrderStatusAsync(int orderId, int driverId, OrderStatus newStatus);
    }
}
