using backend.Controllers;
using backend.Models;
using backend.Repositories;

namespace backend.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<(bool Success, string Message, int? UserId)> CreateUserAsync(CreateUserRequest req, string adminEmail)
        {
            if (await _userRepository.EmailExistsAsync(req.Email))
                return (false, "Email already exists.", null);

            if (await _userRepository.UsernameExistsAsync(req.Username))
                return (false, "Username already exists.", null);

            if (req.Role != "Manager" && req.Role != "SalesRep" && req.Role != "Customer")
                return (false, "Invalid Role. Must be Manager, SalesRep, or Customer.", null);

            var newUser = new User
            {
                Username = req.Username,
                Email = req.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password),
                Role = req.Role,
                CreatedBy = adminEmail,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                IsDeleted = false
            };

            await _userRepository.AddAsync(newUser);
            return (true, $"{req.Role} created successfully!", newUser.UserId);
        }
    }
}
