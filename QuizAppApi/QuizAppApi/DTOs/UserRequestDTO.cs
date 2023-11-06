using System.ComponentModel.DataAnnotations;

namespace QuizAppApi.DTOs;

public class UserRequestDTO
{
    [Required]
    public string Username { get; init; }

    [Required]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$",
        ErrorMessage =
            "Password must contain at least 8 characters including at least one digit, lowercase and uppercase letter")]
    public string Password { get; init; }
}