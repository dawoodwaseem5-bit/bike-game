using backend.DTOs;
using backend.Models;
using backend.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;

        public AuthService(IUserRepository userRepository, IConfiguration config)
        {
            _userRepository = userRepository;
            _config = config;
        }

        public async Task<(bool Success, string Message, string Token, object? UserDetails)> LoginAsync(LoginRequestDto req)
        {
            var user = await _userRepository.GetByEmailAsync(req.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
            {
                return (false, "Invalid email or password.", "", null);
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            var userDetails = new
            {
                UserId = user.UserId,
                Email = user.Email,
                Role = user.Role,
                Username = user.Username
            };

            return (true, "Login successful!", tokenString, userDetails);
        }

        public async Task<(bool Success, string Message)> RegisterCustomerAsync(RegisterRequestDto req)
        {
            if (await _userRepository.EmailExistsAsync(req.Email))
                return (false, "Email already exists.");

            if (await _userRepository.UsernameExistsAsync(req.Username))
                return (false, "Username already exists.");

            var newUser = new User
            {
                Username = req.Username,
                Email = req.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password),
                Role = "Customer",
                CreatedBy = "Self",
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                IsDeleted = false
            };

            await _userRepository.AddAsync(newUser);
            return (true, "Customer registered successfully!");
        }
    }
}
