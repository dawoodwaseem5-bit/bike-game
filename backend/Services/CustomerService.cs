using backend.Models;
using backend.Repositories;

namespace backend.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<object> GetCustomersPaginatedAsync(int page, int pageSize, string search)
        {
            var (totalItems, items) = await _customerRepository.GetPaginatedAsync(page, pageSize, search);

            return new
            {
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                Data = items
            };
        }

        public async Task<Customer> CreateCustomerAsync(Customer customer, string username)
        {
            customer.CreatedAt = DateTime.UtcNow;
            customer.CreatedBy = username;

            await _customerRepository.AddAsync(customer);
            await _customerRepository.SaveChangesAsync();

            return customer;
        }

        public async Task<(bool Success, string Message, Customer? UpdatedCustomer)> UpdateCustomerAsync(int id, Customer updateModel, string username)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
                return (false, "Customer not found.", null);

            customer.Name = updateModel.Name;
            customer.Email = updateModel.Email;
            customer.Address = updateModel.Address;
            customer.Company = updateModel.Company;
            customer.IsActive = updateModel.IsActive;

            customer.UpdatedAt = DateTime.UtcNow;
            customer.UpdatedBy = username;

            await _customerRepository.SaveChangesAsync();

            return (true, "Customer updated successfully.", customer);
        }

        public async Task<(bool Success, string Message)> DeleteCustomerAsync(int id, string username)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
                return (false, "Customer not found.");

            customer.IsDeleted = true;
            customer.UpdatedAt = DateTime.UtcNow;
            customer.UpdatedBy = username;

            await _customerRepository.SaveChangesAsync();

            return (true, "Customer deleted successfully.");
        }
    }
}
