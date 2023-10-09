using QuizAppApi.DTOs;
using QuizAppApi.Enums;
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

                switch (QuestionTypeConverter.FromString(question.QuestionType))
                {
                    case QuestionType.SingleChoiceQuestion:
                        var newSingleChoiceQuestion = new SingleChoiceQuestion
                        {
                            Text = question.QuestionText
                        };

                        newSingleChoiceQuestion.Options = new List<Option>();
                        foreach (var option in question.QuestionParameters.Options)
                        {
                            Option newOption = new Option
                            {
                                Name = option
                            };
                            newSingleChoiceQuestion.Options.Add(newOption);
                        }

                        int correctOptionIndex = (int)question.QuestionParameters.CorrectOptionIndex;
                        if (correctOptionIndex < 0 || correctOptionIndex >= question.QuestionParameters.Options.Count)
                        {
                            return new QuizCreationResponseDTO { Status = "Correct option index out of options list bounds" };
                        }
                        newSingleChoiceQuestion.CorrectOption = new Option
                        {
                            Name = question.QuestionParameters.Options[correctOptionIndex]
                        };

                        newQuiz.Questions.Add(newSingleChoiceQuestion);
                        break;

                    case QuestionType.MultipleChoiceQuestion:
                        var newMultipleChoiceQuestion = new MultipleChoiceQuestion
                        {
                            Text = question.QuestionText
                        };

                        newMultipleChoiceQuestion.Options = new List<Option>();
                        foreach (var option in question.QuestionParameters.Options)
                        {
                            Option newOption = new Option
                            {
                                Name = option
                            };
                            newMultipleChoiceQuestion.Options.Add(newOption);
                        }

                        newMultipleChoiceQuestion.CorrectOptions = new List<Option>();
                        int optionCount = question.QuestionParameters.Options.Count;
                        foreach (var index in question.QuestionParameters.CorrectOptionIndexes)
                        {
                            if (index < 0 || index >= optionCount)
                            {
                                return new QuizCreationResponseDTO { Status = "Correct option index out of options list bounds" };
                            }

                            Option newCorrectOption = new Option
                            {
                                Name = question.QuestionParameters.Options.ElementAt(index)
                            };
                            newMultipleChoiceQuestion.CorrectOptions.Add(newCorrectOption);
                        }

                        newQuiz.Questions.Add(newMultipleChoiceQuestion);
                        break;

                    case QuestionType.OpenTextQuestion:
                        var newOpenTextQuestion = new OpenTextQuestion
                        {
                            Text = question.QuestionText
                        };

                        newOpenTextQuestion.Text = question.QuestionText;
                        newQuiz.Questions.Add(newOpenTextQuestion);
                        break;

                    default:
                        return new QuizCreationResponseDTO { Status = "Question type not found" };
                }
            }

            Quiz createdQuiz = _quizRepository.AddQuiz(newQuiz);

            if (createdQuiz == null)
            {
                return new QuizCreationResponseDTO { Status = "failed" };
            }

            int createdQuizId = createdQuiz.Id;

            return new QuizCreationResponseDTO { Status = "success", Id = createdQuizId };
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
                { Id = question.Id, QuestionText = question.Text, QuestionType = QuestionTypeConverter.ToString(question.Type) };
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
            var quizzes = _quizRepository.GetQuizzes();

            return quizzes.Select(quiz => new QuizResponseDTO { Name = quiz.Name, Id = quiz.Id });
        }

        public QuizResponseDTO? GetQuiz(int id)
        {
            var quiz = _quizRepository.GetQuizById(id);
            if (quiz == null)
            {
                return null;
            }

            return new QuizResponseDTO { Name = quiz.Name, Id = quiz.Id };
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
            }
            else
            {
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
                        var selectedOptionSingle = singleChoiceQuestion.Options.FirstOrDefault(o => o.Name == answer.OptionName);
                        if (selectedOptionSingle != null && SingleChoiceAnswerChecker.IsCorrect(singleChoiceQuestion, selectedOptionSingle))
                        {
                            correctAnswers++;
                        }
                        break;

                    case MultipleChoiceQuestion multipleChoiceQuestion:
                        if (answer.OptionNames != null)
                        {
                            List<Option> newOptions = new List<Option>();
                            foreach(var selectedOptionMulti in answer.OptionNames)
                            {
                                newOptions.Add(multipleChoiceQuestion.Options.FirstOrDefault(o => o.Name == selectedOptionMulti));
                            }
                            MultipleChoiceAnswerChecker.IsCorrect(multipleChoiceQuestion, newOptions);
                            correctAnswers++;
                        }
                        break;

                    case OpenTextQuestion openTextQuestion:
                        var answerText = answer.AnswerText;
                        if (answerText != null && OpenTextAnswerChecker.IsCorrect(openTextQuestion, answerText))
                        {
                            correctAnswers++;
                        }
                        break;
                }
            }

            response.CorrectlyAnswered = correctAnswers;
            response.Score = quiz.Questions.Count == 0 ? 0 : (correctAnswers * 100 / quiz.Questions.Count);

            return response;
        }

        public bool DeleteQuiz(int id)
        {
            var quiz = _quizRepository.GetQuizById(id);
            if (quiz == null)
            {
                return false;
            }

            _quizRepository.DeleteQuiz(id);
            return true;
        }
    }
}