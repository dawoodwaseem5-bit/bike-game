using backend.DTOs;
using backend.Models;
using backend.Repositories;

namespace backend.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUserRepository _userRepository;

        public CustomerService(ICustomerRepository customerRepository, IUserRepository userRepository)
        {
            _customerRepository = customerRepository;
            _userRepository = userRepository;
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

        public async Task<CustomerResponseDto> CreateCustomerAsync(CustomerCreateDto dto, string username)
        {
            var customer = new Customer
            {
                Name = dto.Name,
                Email = dto.Email,
                Address = dto.Address,
                Company = dto.Company,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = username,
                IsActive = true,
                IsDeleted = false
            };

            await _customerRepository.AddAsync(customer);
            await _customerRepository.SaveChangesAsync();

            return new CustomerResponseDto
            {
                CustomerId = customer.CustomerId,
                Name = customer.Name,
                Email = customer.Email,
                Address = customer.Address,
                Company = customer.Company,
                IsActive = customer.IsActive,
                CreatedAt = customer.CreatedAt
            };
        }

        public async Task<(bool Success, string Message, CustomerResponseDto? UpdatedCustomer)> UpdateCustomerAsync(int id, CustomerUpdateDto dto, string username)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
                return (false, "Customer not found.", null);

            customer.Name = dto.Name;
            customer.Email = dto.Email;
            customer.Address = dto.Address;
            customer.Company = dto.Company;
            customer.IsActive = dto.IsActive;

            customer.UpdatedAt = DateTime.UtcNow;
            customer.UpdatedBy = username;

            await _customerRepository.SaveChangesAsync();

            var responseDto = new CustomerResponseDto
            {
                CustomerId = customer.CustomerId,
                Name = customer.Name,
                Email = customer.Email,
                Address = customer.Address,
                Company = customer.Company,
                IsActive = customer.IsActive,
                CreatedAt = customer.CreatedAt
            };

            return (true, "Customer updated successfully.", responseDto);
        }

        public async Task<(bool Success, string Message, CustomerResponseDto? Profile)> GetMyProfileAsync(string email)
        {
            var customer = await _customerRepository.GetByEmailAsync(email);
            if (customer == null)
                return (false, "Customer profile not found.", null);

            return (true, "OK", new CustomerResponseDto
            {
                CustomerId = customer.CustomerId,
                Name = customer.Name,
                Email = customer.Email,
                Address = customer.Address,
                Company = customer.Company,
                IsActive = customer.IsActive,
                CreatedAt = customer.CreatedAt
            });
        }

        public async Task<(bool Success, string Message, CustomerResponseDto? UpdatedCustomer)> UpdateMyProfileAsync(string email, CustomerProfileUpdateDto dto)
        {
            var customer = await _customerRepository.GetByEmailAsync(email);
            if (customer == null)
                return (false, "Customer profile not found.", null);

            customer.Name = dto.Name;
            customer.Address = dto.Address;
            customer.Company = dto.Company;
            customer.UpdatedAt = DateTime.UtcNow;
            customer.UpdatedBy = email;

            await _customerRepository.SaveChangesAsync();

            return (true, "Profile updated successfully.", new CustomerResponseDto
            {
                CustomerId = customer.CustomerId,
                Name = customer.Name,
                Email = customer.Email,
                Address = customer.Address,
                Company = customer.Company,
                IsActive = customer.IsActive,
                CreatedAt = customer.CreatedAt
            });
        }

        public async Task<(bool Success, string Message)> DeleteCustomerAsync(int id, string username)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
                return (false, "Customer not found.");

            customer.IsDeleted = true;
            customer.UpdatedAt = DateTime.UtcNow;
            customer.UpdatedBy = username;

            var user = await _userRepository.GetByEmailAsync(customer.Email);
            if (user != null)
            {
                user.IsDeleted = true;
                user.IsActive = false;
                user.UpdatedAt = DateTime.UtcNow;
                user.UpdatedBy = username;
            }

            await _customerRepository.SaveChangesAsync();

            return (true, "Customer deleted successfully.");
        }
    }
}
