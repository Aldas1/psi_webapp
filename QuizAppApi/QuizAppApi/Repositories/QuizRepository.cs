using Microsoft.EntityFrameworkCore;
using QuizAppApi.Data;
using QuizAppApi.Interfaces;
using QuizAppApi.Models;

namespace QuizAppApi.Repositories
{
    public class QuizRepository : IQuizRepository
    {
        private readonly QuizContext _context;

        public QuizRepository(QuizContext context)
        {
            _context = context;
        }

        public Quiz? AddQuiz(Quiz quiz)
        {
            var entity = _context.Quizzes.Add(quiz).Entity;
            _context.SaveChanges();
            return entity;
        }

        public Quiz? UpdateQuiz(int id, Quiz quiz)
        {
            Quiz oldQuiz = GetQuizById(id);
            if (oldQuiz is not null)
            {
                _context.Quizzes.Entry(oldQuiz).CurrentValues.SetValues(quiz);
                int changes = _context.SaveChanges();

                if (changes > 0)
                {
                    return quiz;
                }
            }
            return null;
        }

        public IEnumerable<Quiz> GetQuizzes()
        {
            return _context.Quizzes;
        }

        public Quiz? GetQuizById(int id)
        {
            return _context.Quizzes.FirstOrDefault(q => q.Id == id);
        }

        public void DeleteQuiz(int id)
        {
            var quiz = GetQuizById(id);
            if (quiz is not null)
            {
                _context.Quizzes.Remove(quiz);
                _context.SaveChanges();
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}