using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Smart_Delivery___Fleet_Management_System.DTOs.Deiver;
using Smart_Delivery___Fleet_Management_System.Enums;
using Smart_Delivery___Fleet_Management_System.Hubs;
using Smart_Delivery___Fleet_Management_System.Interface;
using Smart_Delivery___Fleet_Management_System.Models;
using System.Threading.Tasks;
using Smart_Delivery___Fleet_Management_System.Services;
using System.Security.Claims;
namespace Smart_Delivery___Fleet_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriversController : ControllerBase
    {
        private readonly IDriversRepository _driversRepo;
        private readonly IUserRepository _userRepo;
        private readonly IHubContext<TrackingHub> _hub;
        private readonly PasswordHasher _hasher;

        public DriversController(IDriversRepository driversRepo, IUserRepository userRepo, IHubContext<TrackingHub> hub, PasswordHasher hasher)
        {
            _driversRepo = driversRepo;
            _userRepo = userRepo;
            _hub = hub;
            _hasher = hasher;
        }


        [Authorize(Roles ="Admin")]
        [HttpPost]
        public async Task<IActionResult> AddDriver(CreateDriverDto dto)
        {
            var user = new User
            {
                Name = dto.Name,
                Phone = dto.Phone,
                PasswordHash = _hasher.Hash(dto.Password),
                Role = UserRole.Driver
            };
            await _userRepo.AddAsync(user);
            await _userRepo.SaveAsync();

            var driver = new Driver
            {
                UserId = user.Id,
                Status = DriverStatus.Offline,
                CreatedAt = DateTime.UtcNow,
                Rating = 5,
                CurrentLat = 0,
                CurrentLng = 0
            };
            await _driversRepo.AddAsync(driver);
            await _driversRepo.SaveAsync();

            return Ok(driver.Id);
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Dispatcher")]
        public async Task<IActionResult> GetAllDrivers()
        {
            var drivers = await _driversRepo.GetAllAsync();
            return Ok(drivers);
        }
        [HttpGet("available")]
        [Authorize(Roles = "Dispatcher,Admin")]
        public async Task<IActionResult> GetAvailableDrivers()
        {
            var drivers = await _driversRepo.GetAvailableWithLocationAsync();
            return Ok(drivers);
        }
        [HttpPost("my-location")]
        [Authorize(Roles = "Driver")]
        public async Task<IActionResult> UpdateLocation(UpdateLocationDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var driver = await _driversRepo.GetByUserIdAsync(userId);

            if (driver == null)
                return NotFound();

            await _driversRepo.UpdateDriverLocationAsync(driver.Id, dto.Lat,dto.Lng);
            await _driversRepo.SaveAsync();

            await _hub.Clients.All.SendAsync("ReceiveLocation", driver.Id, dto.Lat,dto.Lng);

            return Ok();
        }
        [HttpPut("status")]
        [Authorize(Roles = "Driver")]
        public async Task<IActionResult> UpdateStatus(UpdateDriverStatusDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var driver = await _driversRepo.GetByUserIdAsync(userId);

            if (driver == null)
                return NotFound();

            driver.Status = dto.Status;

            await _driversRepo.UpdateAsync(driver);
            await _driversRepo.SaveAsync();

            return Ok();

        }
    }
}
