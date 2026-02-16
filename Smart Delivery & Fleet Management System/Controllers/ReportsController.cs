using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smart_Delivery___Fleet_Management_System.Interface;

namespace Smart_Delivery___Fleet_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportsService _service;

        public ReportsController(IReportsService service)
        {
            _service = service;
        }

        [HttpGet("drivers-performance")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DriversPerformance()
        {
            return Ok(await _service.GetDriversPerformanceAsync());
        }

        [HttpGet("average-delivery-time")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AverageDeliveryTime()
        {
            return Ok(await _service.GetAverageDeliveryTimeAsync());
        }

        [HttpGet("failure-rate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> FailureRate()
        {
            return Ok(await _service.GetFailureRateAsync());
        }
    }
}
