using QuizAppApi.DTOs;

namespace QuizAppApi.Interfaces;

public interface IUserService
{
    UserResponseDTO? CreateUser(UserRequestDTO request);
    UserResponseDTO? GetUser(string username);
}