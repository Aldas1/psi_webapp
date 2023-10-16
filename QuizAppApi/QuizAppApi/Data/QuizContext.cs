using Microsoft.EntityFrameworkCore;
using QuizAppApi.Models;
using QuizAppApi.Models.Questions;

namespace QuizAppApi.Data
{
    public class QuizContext : DbContext
    {
        public QuizContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SingleChoiceOption>().HasKey(o => new
            {
                o.Name,
                o.SingleChoiceQuestionId
            });
            modelBuilder.Entity<MultipleChoiceOption>().HasKey(o => new
            {
                o.Name,
                o.MultipleChoiceQuestionId
            });
        }

        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<SingleChoiceQuestion> SingleChoiceQuestions { get; set; }
        public DbSet<MultipleChoiceQuestion> MultipleChoiceQuestions { get; set; }
        public DbSet<OpenTextQuestion> OpenTextQuestions { get; set; }
        public DbSet<SingleChoiceOption> SingleChoiceOptions { get; set; }
        public DbSet<MultipleChoiceOption> MultipleChoiceOptions { get; set; }
    }
}