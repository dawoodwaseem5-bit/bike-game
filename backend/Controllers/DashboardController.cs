using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.Services;

using System.Security.Claims;

namespace backend.Controllers
{
    [Route("api/dashboard")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value ?? "";
            var role = User.FindFirst(ClaimTypes.Role)?.Value ?? "";

            var result = await _dashboardService.GetDashboardSummaryAsync(email, role);
            return Ok(result);
        }
    }
}
