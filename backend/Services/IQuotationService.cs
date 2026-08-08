using backend.DTOs;

namespace backend.Services
{
    public interface IQuotationService
    {
        Task<object> GetQuotationsPaginatedAsync(int page, int pageSize, string search, string email = "", string role = "");
        Task<(bool Success, string Message, QuotationResponseDto? Quotation)> CreateQuotationAsync(QuotationCreateDto dto, string email, string role);
        Task<(bool Success, string Message, QuotationResponseDto? UpdatedQuotation)> FinalizeQuotationAsync(int id, QuotationFinalizeDto dto, string email);
        Task<(bool Success, string Message, QuotationResponseDto? UpdatedQuotation)> UpdateQuotationStatusAsync(int id, string status, string username);
        Task<(bool Success, string Message)> DeleteQuotationAsync(int id, string username);
    }
}
