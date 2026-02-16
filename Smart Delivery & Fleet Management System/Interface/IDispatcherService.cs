using Smart_Delivery___Fleet_Management_System.DTOs.Dispatcher;

namespace Smart_Delivery___Fleet_Management_System.Interface
{
    public interface IDispatcherService
    {
        Task<IEnumerable<ActiveOrderDto>> GetActiveOrdersAsync();
        Task<IEnumerable<LiveDriverDto>> GetLiveDriversAsync();
        Task ReassignOrderAsync(int orderId);
    }
}
