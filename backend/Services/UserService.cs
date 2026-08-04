using backend.DTOs;
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

        public async Task<(bool Success, string Message, UserResponseDto? User)> CreateUserAsync(CreateUserRequestDto req, string adminEmail)
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
            
            var userResponse = new UserResponseDto
            {
                UserId = newUser.UserId,
                Username = newUser.Username,
                Email = newUser.Email,
                Role = newUser.Role,
                IsActive = newUser.IsActive,
                CreatedAt = newUser.CreatedAt
            };

            return (true, $"{req.Role} created successfully!", userResponse);
        }
    }
}
