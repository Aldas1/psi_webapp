using QuizAppApi.Dtos;

namespace QuizAppApi.Interfaces;

public interface IUserService
{
    UserResponseDto? CreateUser(UserRequestDto request);
    UserResponseDto? GetUser(string username);
}