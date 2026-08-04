using Microsoft.AspNetCore.Mvc;
using backend.Services;
using backend.DTOs;

namespace backend.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto req)
        {
            var result = await _authService.LoginAsync(req);
            
            if (!result.Success)
                return Unauthorized(new { Message = result.Message });

            return Ok(new 
            { 
                Token = result.Token, 
                Message = result.Message,
                User = result.UserDetails
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto req)
        {
            var result = await _authService.RegisterCustomerAsync(req);

            if (!result.Success)
                return BadRequest(new { Message = result.Message });

            return Ok(new { Message = result.Message });
        }
    }
}