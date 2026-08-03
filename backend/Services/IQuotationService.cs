using backend.Controllers;
using backend.Models;

namespace backend.Services
{
    public interface IQuotationService
    {
        Task<object> GetQuotationsPaginatedAsync(int page, int pageSize, string search);
        Task<(bool Success, string Message, Quotation? Quotation)> CreateQuotationAsync(QuotationsController.QuotationCreateDto dto, string username);
        Task<(bool Success, string Message, Quotation? UpdatedQuotation)> UpdateQuotationStatusAsync(int id, string status, string username);
        Task<(bool Success, string Message)> DeleteQuotationAsync(int id, string username);
    }
}
