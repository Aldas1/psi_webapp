using System.Security.Claims;
using QuizAppApi.Interfaces;

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
        if (username != null)
        {
            var userRepo = context.RequestServices.GetService<IUserRepository>();
            if (userRepo == null)
            {
                await _next(context);
                return;
            }
            var user = await userRepo.GetUserAsync(username);
            if (user == null)
            {
                context.Items["UserName"] = null;
            }

            context.Items["User"] = user;
        }

        await _next(context);
    }
}