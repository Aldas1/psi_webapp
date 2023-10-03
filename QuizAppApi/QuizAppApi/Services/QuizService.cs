using QuizAppApi.DTOs;
using QuizAppApi.Interfaces;
using QuizAppApi.Models;
using QuizAppApi.Models.Questions;
using QuizAppApi.Utils;

namespace QuizAppApi.Services
{
    public class QuizService : IQuizService
    {
        private readonly IQuizRepository _quizRepository;

        public QuizService(IQuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }

        public QuizCreationResponseDTO CreateQuiz(QuizCreationRequestDTO request)
        {
            Quiz newQuiz = new Quiz();
            newQuiz.Name = request.Name;
            newQuiz.Questions = new List<Question>();

            foreach (var question in request.Questions)
            {
                switch (question.QuestionType)
                {
                    case "singleChoiceQuestion":
                        var newQuestion = new SingleChoiceQuestion
                        {
                            Text = question.QuestionText
                        };

                        int correctOptionIndex = (int)question.QuestionParameters.CorrectOptionIndex;
                        if (correctOptionIndex < 0 || correctOptionIndex >= question.QuestionParameters.Options.Count)
                        {
                            return new QuizCreationResponseDTO { Status = "Correct option index out of Options list bounds" };
                        }
                        newQuestion.CorrectOption = new Option
                        {
                            Name = question.QuestionParameters.Options[correctOptionIndex]
                        };

                        newQuestion.Options = new List<Option>();

                        foreach (var option in question.QuestionParameters.Options)
                        {
                            Option newOption = new Option();
                            newOption.Name = option;
                            newQuestion.Options.Add(newOption);                            
                        }

                        newQuiz.Questions.Add(newQuestion);
                        break;
                    case "multipleChoiceQuestion":

                        break;
                    case "openTextQuestion":

                        break;
                    default:
                        return new QuizCreationResponseDTO { Status = "Question type not found"};
                }
            }

            int newId = _quizRepository.AddQuiz(newQuiz).Id;
            
            if (newQuiz == null)
            {
                return new QuizCreationResponseDTO { Status = "failed" };
            }

            return new QuizCreationResponseDTO { Status = "success", Id = newId};
        }

        public IEnumerable<QuestionResponseDTO>? GetQuestions(int id)
        {
            var quiz = _quizRepository.GetQuizById(id);
            if (quiz == null)
            {
                return null;
            }
            var questions = new List<QuestionResponseDTO>();
            foreach (var question in quiz.Questions)
            {
                var questionResponse = new QuestionResponseDTO
                    { Id = question.Id, QuestionText = question.Text, QuestionType = question.Type};
                switch (question)
                {
                    case SingleChoiceQuestion singleChoiceQuestion:
                        questionResponse.QuestionParameters = new QuestionParametersDTO
                        {
                            Options = singleChoiceQuestion.Options.Select(opt => opt.Name).ToList(),
                        };
                        questions.Add(questionResponse);
                        break;
                }
            }
            return questions;
        }

        public IEnumerable<QuizResponseDTO> GetQuizzes()
        {
            _quizRepository.GetQuizzes();
            return _quizRepository.GetQuizzes()
                .Select(
                    quiz => new QuizResponseDTO { Name = quiz.Name, Id = quiz.Id }
                );
        }

        public AnswerSubmitResponseDTO SubmitAnswers(int id, List<AnswerSubmitRequestDTO> request)
        {
            var response = new AnswerSubmitResponseDTO();
            var quiz = _quizRepository.GetQuizById(id);
            var correctAnswers = 0;

            if (quiz == null)
            {
                response.Status = "failed";
                return response;
            } else {
                response.Status = "success";
            }

            foreach (var answer in request)
            {
                var question = quiz.Questions.FirstOrDefault(q => q.Id == answer.QuestionId);

                if (question == null)
                {
                    continue;
                }

                switch (question)
                {
                    case SingleChoiceQuestion singleChoiceQuestion:
                        var selectedOption = singleChoiceQuestion.Options.FirstOrDefault(o => o.Name == answer.OptionName);
                        if (selectedOption != null)
                        {
                            var checker = new SingleChoiceAnswerChecker();
                            var isCorrect = checker.IsCorrect(singleChoiceQuestion, selectedOption);

                            if (isCorrect)
                            {
                                correctAnswers++;
                            }
                        }
                        break;

                    case MultipleChoiceQuestion multipleChoiceQuestion:
                        // TODO: Implement multiple choice answer checking
                        break;

                    case OpenTextQuestion openTextQuestion:
                        // TODO: Implement text answer checking
                        break;
                }
            }

            response.CorrectlyAnswered = correctAnswers;
            response.Score = quiz.Questions.Count == 0 ? 0 : (correctAnswers * 100 / quiz.Questions.Count);

            return response;
        }
    }
}