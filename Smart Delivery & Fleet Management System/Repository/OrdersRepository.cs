using Microsoft.EntityFrameworkCore;
using Smart_Delivery___Fleet_Management_System.Data;
using Smart_Delivery___Fleet_Management_System.Interface;
using Smart_Delivery___Fleet_Management_System.Models;
using Smart_Delivery___Fleet_Management_System.Enums;
namespace Smart_Delivery___Fleet_Management_System.Repository
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly DeliveryDbContext _context;

        public OrdersRepository(DeliveryDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order); 
        }

        public async Task AddStatusHistoryAsync(int orderId, OrderStatus status, int? driverId)
        {
            var history = new OrderStatusHistory
            {
                OrderId = orderId,
                Status = status,
                Timestamp = DateTime.UtcNow,
                UpdatedByDriverId = driverId
            };

            await _context.OrderStatusHistories.AddAsync(history);
        }

        public async Task DeleteAsync(Order order)
        {
            _context.Orders.Remove(order);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.AssignedDriver)
                .Include(o => o.StatusHistory)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Order?> GetForAssignmentAsync(int id)
        {
            return await _context.Orders
                .Where(o => o.Id == id)
                .Select(o => new Order
                {
                    Id = o.Id,
                    PickupLat = o.PickupLat,
                    PickupLng = o.PickupLng,
                    Status = o.Status
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Order>> GetUnassignedAsync()
        {
            return await _context.Orders
                .Where(o => o.Status == OrderStatus.Created).ToListAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Order order)
        {
            _context.Update(order);
            await Task.CompletedTask;
        }
    }
}
