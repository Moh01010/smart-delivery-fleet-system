using Microsoft.EntityFrameworkCore;
using Smart_Delivery___Fleet_Management_System.Data;
using Smart_Delivery___Fleet_Management_System.DTOs.Dispatcher;
using Smart_Delivery___Fleet_Management_System.Enums;
using Smart_Delivery___Fleet_Management_System.Interface;
using Smart_Delivery___Fleet_Management_System.Models;
using Smart_Delivery___Fleet_Management_System.Services;

namespace Smart_Delivery___Fleet_Management_System.Repository
{
    public class DispatcherService: IDispatcherService
    {
        private readonly DeliveryDbContext _context;
        private readonly IDriversRepository _driversRepo;
        private readonly IOrdersRepository _ordersRepo;
        private readonly OpenStreetMapService _maps;

        public DispatcherService(DeliveryDbContext context, IDriversRepository driversRepo, IOrdersRepository ordersRepo, OpenStreetMapService maps)
        {
            _context = context;
            _driversRepo = driversRepo;
            _ordersRepo = ordersRepo;
            _maps = maps;
        }

        public async Task<IEnumerable<ActiveOrderDto>> GetActiveOrdersAsync()
        {
            return await _context.Orders
                .Where(o => o.Status != OrderStatus.Delivered &&
                            o.Status != OrderStatus.Failed)
                .Select(o => new ActiveOrderDto
                {
                    Id = o.Id,
                    PickupAddress = o.PickupAddress,
                    DropoffAddress = o.DropoffAddress,
                    Status = o.Status,
                    AssignedDriverId = o.AssignedDriverId
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<LiveDriverDto>> GetLiveDriversAsync()
        {
            var drivers = await _context.Drivers.Include(d => d.User).ToListAsync();


            var result = new List<LiveDriverDto>();

            foreach (var d in drivers)
            {
                var activeOrders = await _driversRepo.GetActiveOrdersCountAsync(d.Id);

                result.Add(new LiveDriverDto
                {
                    DriverId = d.Id,
                    Name = d.User.Name,
                    Lat = d.CurrentLat,
                    Lng = d.CurrentLng,
                    Status = d.Status,
                    ActiveOrders = activeOrders
                });
            }

            return result;
        }

        public async Task ReassignOrderAsync(int orderId)
        {
            //var order = await _ordersRepo.GetForAssignmentAsync(orderId);
            var order = await _ordersRepo.GetByIdAsync(orderId);
            if (order == null) return;

            var drivers = await _driversRepo.GetAvailableWithLocationAsync();

            Driver? bestDriver = null;
            int bestTime = int.MaxValue;

            foreach (var driver in drivers)
            {
                var time = await _maps.GetTravelTimeInSeconds(
                    driver.CurrentLat,
                    driver.CurrentLng,
                    order.PickupLat,
                    order.PickupLng);

                if (time < bestTime)
                {
                    bestTime = time;
                    bestDriver = driver;
                }
            }

            if (bestDriver == null) return;

            order.AssignedDriverId = bestDriver.Id;
            order.Status = OrderStatus.Assigned;

            await _ordersRepo.UpdateAsync(order);
            await _ordersRepo.AddStatusHistoryAsync(order.Id, OrderStatus.Assigned, bestDriver.Id);

            await _ordersRepo.SaveAsync();
        }
    }
}
