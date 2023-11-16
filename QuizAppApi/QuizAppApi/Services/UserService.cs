using QuizAppApi.DTOs;
using QuizAppApi.Exceptions;
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

    private static UserResponseDTO MapUserToResponse(User user)
    {
        return new UserResponseDTO
            { Username = user.Username, TotalScore = user.TotalScore, NumberOfSubmissions = user.NumberOfSubmissions };
    }

    public UserResponseDTO? CreateUser(UserRequestDTO request)
    {
        if (_userRepository.GetUser(request.Username) != null)
        {
            throw new CustomException("User already exists.", "USER_EXISTS");
        }

        var user = new User(request.Username, BC.HashPassword(request.Password));
        _userRepository.AddUser(user);
        return MapUserToResponse(user);
    }

    public UserResponseDTO? GetUser(string username)
    {
        var user = _userRepository.GetUser(username);
        return user == null ? null : MapUserToResponse(user);
    }
}