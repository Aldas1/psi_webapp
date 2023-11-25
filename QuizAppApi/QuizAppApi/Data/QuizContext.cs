using Microsoft.EntityFrameworkCore;
using QuizAppApi.Models;
using QuizAppApi.Models.Questions;

namespace QuizAppApi.Data;

public class QuizContext : DbContext
{
    public QuizContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Option>().HasKey(o => new
        {
            o.Name,
            o.QuestionId
        });

        modelBuilder.Entity<User>()
            .HasMany(u => u.Quizzes)
            .WithOne(q => q.User)
            .HasForeignKey(q => q.Username);
    }

    public DbSet<Quiz> Quizzes { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<OptionQuestion> OptionQuestions { get; set; }
    public DbSet<SingleChoiceQuestion> SingleChoiceQuestions { get; set; }
    public DbSet<MultipleChoiceQuestion> MultipleChoiceQuestions { get; set; }
    public DbSet<OpenTextQuestion> OpenTextQuestions { get; set; }
    public DbSet<Option> Options { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Comment> Comments { get; set; }
}