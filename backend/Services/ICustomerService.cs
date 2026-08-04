using backend.DTOs;

namespace backend.Services
{
    public interface ICustomerService
    {
        Task<object> GetCustomersPaginatedAsync(int page, int pageSize, string search);
        Task<CustomerResponseDto> CreateCustomerAsync(CustomerCreateDto dto, string username);
        Task<(bool Success, string Message, CustomerResponseDto? UpdatedCustomer)> UpdateCustomerAsync(int id, CustomerUpdateDto dto, string username);
        Task<(bool Success, string Message)> DeleteCustomerAsync(int id, string username);
    }
}
