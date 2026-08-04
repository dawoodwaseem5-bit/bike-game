using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.Services;
using backend.DTOs;
using System.Security.Claims;

namespace backend.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize(Roles = "Manager")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDto req)
        {
            var adminEmail = User.FindFirstValue(ClaimTypes.Email) ?? "Admin";

            var result = await _userService.CreateUserAsync(req, adminEmail);

            if (!result.Success)
                return BadRequest(new { Message = result.Message });

            return Ok(new { Message = result.Message, User = result.User });
        }
    }
}
