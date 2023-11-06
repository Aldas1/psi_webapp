using System.Security.Claims;

namespace QuizAppApi.Middleware;

public class JwtUserMapperMiddleware
{
    private readonly RequestDelegate _next;

    public JwtUserMapperMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var username = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        context.Items["UserName"] = username;
        await _next(context);
    }
}