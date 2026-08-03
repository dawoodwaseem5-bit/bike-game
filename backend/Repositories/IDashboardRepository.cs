namespace backend.Repositories
{
    public interface IDashboardRepository
    {
        Task<int> GetTotalCustomersAsync();
        Task<int> GetTotalProductsAsync();
        Task<int> GetTotalQuotationsAsync();
        Task<decimal> GetTotalRevenueAsync();
    }
}
