using Microsoft.EntityFrameworkCore;
using QuizAppApi.Data;
using QuizAppApi.Interfaces;
using QuizAppApi.Models.Questions;
using QuizAppApi.Repositories;
using QuizAppApi.Services;
using QuizAppApi.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition =
        System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddScoped<IQuizService, QuizService>();
builder.Services
    .AddScoped<IQuestionDTOConverterService<SingleChoiceQuestion>, SingleChoiceQuestionDTOConverterService>();
builder.Services
    .AddScoped<IQuestionDTOConverterService<MultipleChoiceQuestion>, MultipleChoiceQuestionDTOConverterService>();
builder.Services.AddScoped<IQuestionDTOConverterService<OpenTextQuestion>, OpenTextQuestionDTOConverterService>();
builder.Services.AddSingleton<IQuizRepository, QuizRepository>();
builder.Services.AddScoped<IExplanationService, ExplanationService>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var openAiApiKey = configuration["OpenAIApiKey"];
    if (openAiApiKey != null) return new ExplanationService(openAiApiKey);
    throw new Exception("OpenAI API key not found.");
});
builder.Services.AddSingleton<EventPublisher>();

builder.Services.AddDbContext<QuizContext>(options =>
    options
        .UseLazyLoadingProxies()
        .UseSqlServer(builder.Configuration["ConnectionString"]), ServiceLifetime.Singleton);
builder.Services.AddScoped<IAnswerCheckerService, AnswerCheckerService>();
builder.Services.AddScoped<IUserActionsService, UserActionsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
