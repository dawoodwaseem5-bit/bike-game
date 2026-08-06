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

        public async Task<DashboardSummaryResponseDto> GetDashboardSummaryAsync(string email = "", string role = "")
        {
            var totalCustomers = await _dashboardRepository.GetTotalCustomersAsync();
            var totalProducts = await _dashboardRepository.GetTotalProductsAsync();
            var totalQuotations = await _dashboardRepository.GetTotalQuotationsAsync(email, role);
            var totalRevenue = await _dashboardRepository.GetTotalRevenueAsync(email, role);
            var quotationsByStatus = await _dashboardRepository.GetQuotationsByStatusAsync(email, role);

            return new DashboardSummaryResponseDto
            {
                TotalCustomers = totalCustomers,
                TotalProducts = totalProducts,
                TotalQuotations = totalQuotations,
                TotalRevenue = totalRevenue,
                QuotationsByStatus = quotationsByStatus
            };
        }
    }
}
