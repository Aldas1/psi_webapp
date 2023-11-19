using QuizAppApi.Dtos;

namespace QuizAppApi.Interfaces;

public interface IUserService
{
    Task<UserResponseDto?> CreateUserAsync(UserRequestDto request);
    Task<UserResponseDto?> GetUserAsync(string username);
}