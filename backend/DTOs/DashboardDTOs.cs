namespace backend.DTOs
{
    public class DashboardSummaryResponseDto
    {
        public int TotalCustomers { get; set; }
        public int TotalProducts { get; set; }
        public int TotalQuotations { get; set; }
        public decimal TotalRevenue { get; set; }
        public Dictionary<string, int> QuotationsByStatus { get; set; } = new();
    }
}
