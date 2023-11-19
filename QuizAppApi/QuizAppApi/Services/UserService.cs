using QuizAppApi.Exceptions;
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

    public async Task<UserResponseDto?> CreateUserAsync(UserRequestDto request)
    {
        if (await _userRepository.GetUserAsync(request.Username) != null) return null;
        var user = new User(request.Username, BC.HashPassword(request.Password));
        await _userRepository.AddUserAsync(user);
        return MapUserToResponse(user);
    }

    public async Task<UserResponseDto?> GetUserAsync(string username)
    {
        var user = await _userRepository.GetUserAsync(username);
        return user == null ? null : MapUserToResponse(user);
    }
}