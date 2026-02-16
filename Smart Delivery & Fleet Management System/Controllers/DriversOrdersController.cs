using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smart_Delivery___Fleet_Management_System.Enums;
using Smart_Delivery___Fleet_Management_System.Interface;
using System.Security.Claims;

namespace Smart_Delivery___Fleet_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriversOrdersController : ControllerBase
    {
        private readonly IDriverOrdersService _service;
        private readonly IDriversRepository _driversRepo;

        public DriversOrdersController(IDriverOrdersService service, IDriversRepository driversRepo)
        {
            _service = service;
            _driversRepo = driversRepo;
        }

        [HttpGet("my-orders")]
        [Authorize(Roles = "Driver")]
        public async Task<IActionResult> GetMyOrders()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var driver = await _driversRepo.GetByUserIdAsync(userId);

            if (driver == null)
                return NotFound();

            var orders = await _service.GetDriverOrdersAsync(driver.Id);
            return Ok(orders);
        }
        [HttpPut("orders/status")]
        [Authorize(Roles = "Driver")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId,[FromQuery] OrderStatus status)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var driver = await _driversRepo.GetByUserIdAsync(userId);

            if (driver == null)
                return NotFound();
            await _service.UpdateOrderStatusAsync(orderId, driver.Id, status);
            return Ok();
        }
    }
}
