using backend.Models;

namespace backend.Services
{
    public interface ICustomerService
    {
        Task<object> GetCustomersPaginatedAsync(int page, int pageSize, string search);
        Task<Customer> CreateCustomerAsync(Customer customer, string username);
        Task<(bool Success, string Message, Customer? UpdatedCustomer)> UpdateCustomerAsync(int id, Customer updateModel, string username);
        Task<(bool Success, string Message)> DeleteCustomerAsync(int id, string username);
    }
}
