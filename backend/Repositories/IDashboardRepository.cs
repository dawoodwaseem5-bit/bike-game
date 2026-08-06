namespace backend.Repositories
{
    public interface IDashboardRepository
    {
        Task<int> GetTotalCustomersAsync();
        Task<int> GetTotalProductsAsync();
        Task<int> GetTotalQuotationsAsync(string email = "", string role = "");
        Task<decimal> GetTotalRevenueAsync(string email = "", string role = "");
        Task<Dictionary<string, int>> GetQuotationsByStatusAsync(string email = "", string role = "");
    }
}
