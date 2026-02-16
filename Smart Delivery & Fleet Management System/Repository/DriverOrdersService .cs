using Microsoft.EntityFrameworkCore;
using Smart_Delivery___Fleet_Management_System.Data;
using Smart_Delivery___Fleet_Management_System.DTOs.DriverOrder;
using Smart_Delivery___Fleet_Management_System.Enums;
using Smart_Delivery___Fleet_Management_System.Interface;
using Smart_Delivery___Fleet_Management_System.Models;

namespace Smart_Delivery___Fleet_Management_System.Repository
{
    public class DriverOrdersService : IDriverOrdersService
    {
        private readonly IOrdersRepository _ordersRepo;
        private readonly IDriversRepository _driversRepo;
        private readonly DeliveryDbContext _context;

        public DriverOrdersService(IOrdersRepository ordersRepo, IDriversRepository driversRepo, DeliveryDbContext context)
        {
            _ordersRepo = ordersRepo;
            _driversRepo = driversRepo;
            _context = context;
        }

        public async Task<IEnumerable<DriverOrderDto>> GetDriverOrdersAsync(int driverId)
        {
            return await _context.Orders
                .Where(o =>
                    o.AssignedDriverId == driverId &&
                    o.Status != OrderStatus.Delivered &&
                    o.Status != OrderStatus.Failed)
                .Select(o => new DriverOrderDto
                {
                    Id = o.Id,
                    PickupAddress = o.PickupAddress,
                    DropoffAddress = o.DropoffAddress,
                    Status = o.Status,
                    CustomerName = o.CustomerName,
                    CustomerPhone = o.CustomerPhone
                })
                .ToListAsync();
        }

        public async Task UpdateOrderStatusAsync(int orderId, int driverId, OrderStatus newStatus)
        {
            var order=await _ordersRepo.GetByIdAsync(orderId);
            if (order == null)
                throw new Exception("Order not found");

            if (order.AssignedDriverId != driverId)
                throw new Exception("This order is not assigned to this driver");

            order.Status = newStatus;
            await _ordersRepo.UpdateAsync(order);

            await _ordersRepo.AddStatusHistoryAsync(orderId, newStatus, driverId);

            //if (newStatus == OrderStatus.Delivered || newStatus == OrderStatus.Failed)
            //{
            //    var activeOrdersCount = await _driversRepo.GetActiveOrdersCountAsync(driverId);

            //    if (activeOrdersCount < 5)
            //    {
            //        var driver = await _driversRepo.GetByIdAsync(driverId);
            //        driver.Status = DriverStatus.Available;
            //        await _driversRepo.UpdateAsync(driver);
            //    }
            //}

            await _ordersRepo.SaveAsync();
        }
    }
    
}
