using Microsoft.EntityFrameworkCore;
using QuizAppApi.Data;
using QuizAppApi.Interfaces;
using QuizAppApi.Models;
using System.Reflection.Metadata.Ecma335;

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
            var oldQuiz = GetQuizById(id);
            if (oldQuiz is null)
            {
                return null;
            }

            oldQuiz.Questions.Clear();
            _context.Entry(oldQuiz).State = EntityState.Detached;

            _context.Quizzes.Update(quiz);
        
            _context.SaveChanges();
            return GetQuizById(id);
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

        public bool Save()
        {
            var changes = _context.SaveChanges();
            return changes > 0;
        }
    }
}