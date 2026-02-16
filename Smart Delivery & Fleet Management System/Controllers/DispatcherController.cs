using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smart_Delivery___Fleet_Management_System.Interface;

namespace Smart_Delivery___Fleet_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DispatcherController : ControllerBase
    {
        private readonly IDispatcherService _service;

        public DispatcherController(IDispatcherService service)
        {
            _service = service;
        }

        [HttpGet("orders/active")]
        [Authorize(Roles = "Dispatcher")]
        public async Task<IActionResult> GetActiveOrders()
        {
            return Ok(await _service.GetActiveOrdersAsync());
        }

        [HttpGet("drivers/live")]
        [Authorize(Roles = "Dispatcher")]
        public async Task<IActionResult> GetLiveDrivers()
        {
            return Ok(await _service.GetLiveDriversAsync());
        }

        [HttpPut("orders/{orderId}/reassign")]
        [Authorize(Roles = "Dispatcher")]
        public async Task<IActionResult> Reassign(int orderId)
        {
            await _service.ReassignOrderAsync(orderId);
            return Ok();
        }
    }
}
