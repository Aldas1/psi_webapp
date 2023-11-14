using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using QuizAppApi.Interfaces;
using BC = BCrypt.Net.BCrypt;

namespace QuizAppApi.Services;

public class LoginService : ILoginService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public LoginService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public string? Login(string username, string password)
    {
        var user = _userRepository.GetUserAsync(username);
        if (user == null) return null;
        if (!BC.Verify(password, user.PasswordHash)) return null;

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.NameId, username)
        };

        var creds = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT_SECRET"])), SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = creds,
            Expires = new DateTime(2032, 1, 1)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}