using backend.DTOs;

namespace backend.Services
{
    public interface IDashboardService
    {
        Task<DashboardSummaryResponseDto> GetDashboardSummaryAsync();
    }
}
