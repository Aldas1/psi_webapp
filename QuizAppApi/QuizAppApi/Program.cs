using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuizAppApi.Data;
using QuizAppApi.Events;
using QuizAppApi.Extensions;
using QuizAppApi.Hubs;
using QuizAppApi.Interfaces;
using QuizAppApi.Middleware;
using QuizAppApi.Models.Questions;
using QuizAppApi.Repositories;
using QuizAppApi.Services;
using QuizAppApi.Workers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "DevClient",
        policy  =>
        {
            policy.WithOrigins("http://localhost:5173")
                .AllowCredentials()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});


builder.Services.AddSignalR();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition =
        System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo {
        Title = "Quiz App Api", Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme() {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddScoped<IQuizService, QuizService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services
    .AddScoped<IQuestionDtoConverterService<SingleChoiceQuestion>, SingleChoiceQuestionDtoConverterService>();
builder.Services
    .AddScoped<IQuestionDtoConverterService<MultipleChoiceQuestion>, MultipleChoiceQuestionDtoConverterService>();
builder.Services.AddScoped<IQuestionDtoConverterService<OpenTextQuestion>, OpenTextQuestionDtoConverterService>();
builder.Services.AddScoped<IQuizRepository, QuizRepository>();
builder.Services.AddTransient<ICommentRepository, CommentRepository>();
builder.Services.AddSingleton<ICacheRepository, CacheRepository>();
builder.Services.AddScoped<IExplanationService, ExplanationService>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var openAiApiKey = configuration["OpenAIApiKey"];
    if (openAiApiKey != null) return new ExplanationService(openAiApiKey);
    throw new Exception("OpenAI API key not found.");
});

builder.Services.AddDbContext<QuizContext>(options =>
    options
        .UseLazyLoadingProxies()
        .UseSqlServer(builder.Configuration["ConnectionString"]), ServiceLifetime.Transient, ServiceLifetime.Transient);
builder.Services.AddScoped<IAnswerCheckerService, AnswerCheckerService>();
builder.Services.AddScoped<IAnswerService, AnswerService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<ILeaderboardService, LeaderboardService>();
builder.Services.AddScoped<IQuizDiscussionService, QuizDiscussionService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT_SECRET"])),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddEventHandler<AnswerSubmittedEventHandlerQuizUpdater>();
builder.Services.AddEventHandler<AnswerSubmittedEventHandlerUserUpdater>();

builder.Services.AddSingleton<DiscussionCacheDbSyncWorker>();

var app = builder.Build();

app.UseEventHandlers();
app.UseCors("DevClient");
app.MapHub<DiscussionHub>("/discussionHub");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseMiddleware<JwtUserMapperMiddleware>();
app.Services.GetService<DiscussionCacheDbSyncWorker>()?.Start();
app.Run();
