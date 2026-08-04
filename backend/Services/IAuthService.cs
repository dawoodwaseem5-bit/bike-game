using backend.DTOs;

namespace backend.Services
{
    public interface IAuthService
    {
        Task<(bool Success, string Message, string Token, object? UserDetails)> LoginAsync(LoginRequestDto req);
        Task<(bool Success, string Message)> RegisterCustomerAsync(RegisterRequestDto req);
    }
}
