using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smart_Delivery___Fleet_Management_System.DTOs.Order;
using Smart_Delivery___Fleet_Management_System.Enums;
using Smart_Delivery___Fleet_Management_System.Interface;
using Smart_Delivery___Fleet_Management_System.Models;
using Smart_Delivery___Fleet_Management_System.Services;
using System.Threading.Tasks;

namespace Smart_Delivery___Fleet_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersRepository _ordersRepo;
        private readonly IDriversRepository _driversRepo;
        private readonly OpenStreetMapService _maps;

        public OrdersController(IOrdersRepository ordersRepo, IDriversRepository driversRepo, OpenStreetMapService maps)
        {
            _ordersRepo = ordersRepo;
            _driversRepo = driversRepo;
            _maps = maps;
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDto dto)
        {
            // 1) Geocode
            var (pickupLat, pickupLng) = await _maps.GeocodeAddress(dto.PickupAddress);
            var (dropLat, dropLng) = await _maps.GeocodeAddress(dto.DropoffAddress);

            // 2) Create Order
            var order = new Order
            {
                CustomerName = dto.CustomerName,
                CustomerPhone = dto.CustomerPhone,
                PickupAddress = dto.PickupAddress,
                PickupLat = pickupLat,
                PickupLng = pickupLng,
                DropoffAddress = dto.DropoffAddress,
                DropoffLat = dropLat,
                DropoffLng = dropLng,
                Status = OrderStatus.Created,
                CreatedAt = DateTime.UtcNow
            };
            await _ordersRepo.AddAsync(order);
            await _ordersRepo.SaveAsync();

            // 3) History: Created
            await _ordersRepo.AddStatusHistoryAsync(order.Id, OrderStatus.Created, null);

            // 4) Auto Assign
            var drivers = await _driversRepo.GetAvailableWithLocationAsync();

            Driver? bestDriver = null;
            int bestTime = int.MaxValue;

            foreach (var driver in drivers)
            {
                var time = await _maps.GetTravelTimeInSeconds(
                    driver.CurrentLat,
                    driver.CurrentLng,
                    pickupLat,
                    pickupLng);

                if (time < bestTime)
                {
                    bestTime = time;
                    bestDriver = driver;
                }
            }

            if (bestDriver != null)
            {
                order.AssignedDriverId = bestDriver.Id;
                order.Status = OrderStatus.Assigned;

                await _ordersRepo.UpdateAsync(order);

                // History: Assigned
                await _ordersRepo.AddStatusHistoryAsync(order.Id, OrderStatus.Assigned, bestDriver.Id);

                
                var activeOrdersCount = await _driversRepo.GetActiveOrdersCountAsync(bestDriver.Id);

                const int MaxOrdersPerDriver = 5;

                if (activeOrdersCount >= MaxOrdersPerDriver)
                {
                    bestDriver.Status = DriverStatus.Busy;
                    await _driversRepo.UpdateAsync(bestDriver);
                }

                await _ordersRepo.SaveAsync();
            }


            return Ok(order.Id);
        }
    }
}
