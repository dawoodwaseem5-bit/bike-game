using backend.Controllers;

namespace backend.Services
{
    public interface IAuthService
    {
        Task<(bool Success, string Message, string Token, object? UserDetails)> LoginAsync(LoginRequest req);
        Task<(bool Success, string Message)> RegisterCustomerAsync(RegisterRequest req);
    }
}
