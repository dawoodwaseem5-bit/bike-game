using backend.DTOs;

namespace backend.Services
{
    public interface IUserService
    {
        Task<(bool Success, string Message, UserResponseDto? User)> CreateUserAsync(CreateUserRequestDto req, string adminEmail);
    }
}
