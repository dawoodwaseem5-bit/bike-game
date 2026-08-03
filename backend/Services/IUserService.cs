using backend.Controllers;

namespace backend.Services
{
    public interface IUserService
    {
        Task<(bool Success, string Message, int? UserId)> CreateUserAsync(CreateUserRequest req, string adminEmail);
    }
}
