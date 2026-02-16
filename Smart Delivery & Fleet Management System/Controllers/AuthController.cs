using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Smart_Delivery___Fleet_Management_System.DTOs.User;
using Smart_Delivery___Fleet_Management_System.Interface;

namespace Smart_Delivery___Fleet_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var token = await _authService.LoginAsync(dto.Phone, dto.Password);

            if (token == null)
                return Unauthorized("Invalid credentials");

            return Ok(new { token });
        }
    }
}
