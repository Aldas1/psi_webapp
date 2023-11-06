using QuizAppApi.Dtos;
using QuizAppApi.Interfaces;
using QuizAppApi.Models;
using BC = BCrypt.Net.BCrypt;

namespace QuizAppApi.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    private static UserResponseDto MapUserToResponse(User user)
    {
        return new UserResponseDto
            { Username = user.Username, TotalScore = user.TotalScore, NumberOfSubmissions = user.NumberOfSubmissions };
    }

    public UserResponseDto? CreateUser(UserRequestDto request)
    {
        if (_userRepository.GetUser(request.Username) != null) return null;
        var user = new User(request.Username, BC.HashPassword(request.Password));
        _userRepository.AddUser(user);
        return MapUserToResponse(user);
    }

    public UserResponseDto? GetUser(string username)
    {
        var user = _userRepository.GetUser(username);
        return user == null ? null : MapUserToResponse(user);
    }
}