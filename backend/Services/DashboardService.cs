using backend.DTOs;
using backend.Repositories;

namespace backend.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<DashboardSummaryResponseDto> GetDashboardSummaryAsync()
        {
            var totalCustomers = await _dashboardRepository.GetTotalCustomersAsync();
            var totalProducts = await _dashboardRepository.GetTotalProductsAsync();
            var totalQuotations = await _dashboardRepository.GetTotalQuotationsAsync();
            var totalRevenue = await _dashboardRepository.GetTotalRevenueAsync();

            return new DashboardSummaryResponseDto
            {
                TotalCustomers = totalCustomers,
                TotalProducts = totalProducts,
                TotalQuotations = totalQuotations,
                TotalRevenue = totalRevenue
            };
        }
    }
}
