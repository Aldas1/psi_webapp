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
        private readonly IQuestionDTOConverterService<SingleChoiceQuestion> _singleChoiceDTOConverter;
        private readonly IQuestionDTOConverterService<MultipleChoiceQuestion> _multipleChoiceDTOConverter;
        private readonly IQuestionDTOConverterService<OpenTextQuestion> _openTextDTOConverter;

        public QuizService(
            IQuizRepository quizRepository,
            IQuestionDTOConverterService<SingleChoiceQuestion> singleChoiceDTOConverter,
            IQuestionDTOConverterService<MultipleChoiceQuestion> multipleChoiceDTOConverter,
            IQuestionDTOConverterService<OpenTextQuestion> openTextDTOConverter)
        {
            _quizRepository = quizRepository;
            _singleChoiceDTOConverter = singleChoiceDTOConverter;
            _multipleChoiceDTOConverter = multipleChoiceDTOConverter;
            _openTextDTOConverter = openTextDTOConverter;
        }


        public QuizCreationResponseDTO CreateQuiz(QuizCreationRequestDTO request)
        {
            var newQuiz = new Quiz { Name = request.Name };
            foreach (var question in request.Questions)
            {
                Question? generatedQuestion = null;
                switch (QuestionTypeConverter.FromString(question.QuestionType))
                {
                    case QuestionType.SingleChoiceQuestion:
                        generatedQuestion = _singleChoiceDTOConverter.CreateFromParameters(question.QuestionParameters);
                        break;
                    case QuestionType.MultipleChoiceQuestion:
                        generatedQuestion = _multipleChoiceDTOConverter.CreateFromParameters(question.QuestionParameters);
                        break;
                    case QuestionType.OpenTextQuestion:
                        generatedQuestion = _openTextDTOConverter.CreateFromParameters(question.QuestionParameters);
                        break;
                }
                if (generatedQuestion == null)
                {
                    return new QuizCreationResponseDTO { Status = "Invalid question data" };
                }
                generatedQuestion.Text = question.QuestionText;
                newQuiz.Questions.Add(generatedQuestion);
            }
            Quiz? createdQuiz = _quizRepository.AddQuiz(newQuiz);

            if (createdQuiz == null)
            {
                return new QuizCreationResponseDTO { Status = "failed" };
            }

            int createdQuizId = createdQuiz.Id;

            return new QuizCreationResponseDTO { Status = "success", Id = createdQuizId };
        }

        public QuizCreationResponseDTO UpdateQuiz(int id, QuizCreationRequestDTO editRequest)
        {
            var newQuiz = _quizRepository.GetQuizById(id);
            if (newQuiz == null)
            {
                return new QuizCreationResponseDTO { Status = "Quiz not found" };
            }

            newQuiz.Name = editRequest.Name;
            newQuiz.Questions.Clear();

            foreach (var question in editRequest.Questions)
            {
                Question? generatedQuestion = null;
                switch (QuestionTypeConverter.FromString(question.QuestionType))
                {
                    case QuestionType.SingleChoiceQuestion:
                        generatedQuestion = _singleChoiceDTOConverter.CreateFromParameters(question.QuestionParameters);
                        break;
                    case QuestionType.MultipleChoiceQuestion:
                        generatedQuestion = _multipleChoiceDTOConverter.CreateFromParameters(question.QuestionParameters);
                        break;
                    case QuestionType.OpenTextQuestion:
                        generatedQuestion = _openTextDTOConverter.CreateFromParameters(question.QuestionParameters);
                        break;
                }

                if (generatedQuestion == null)
                {
                    return new QuizCreationResponseDTO { Status = "Invalid question data" };
                }

                generatedQuestion.Text = question.QuestionText;
                newQuiz.Questions.Add(generatedQuestion);
            }

            Quiz? updatedQuiz = _quizRepository.UpdateQuiz(id, newQuiz);

            if (updatedQuiz == null)
            {
                return new QuizCreationResponseDTO { Status = "failed" };
            }

            return new QuizCreationResponseDTO { Status = "success", Id = id };
        }

        public IEnumerable<QuestionResponseDTO>? GetQuestions(int id)
        {
            var quiz = _quizRepository.GetQuizById(id);
            if (quiz == null)
            {
                return null;
            }
            return quiz.Questions.Select(question =>
            {
                QuestionParametersDTO generatedParameters;
                switch (question)
                {
                    case SingleChoiceQuestion singleChoiceQuestion:
                        generatedParameters = _singleChoiceDTOConverter.GenerateParameters(singleChoiceQuestion);
                        break;
                    case MultipleChoiceQuestion multipleChoiceQuestion:
                        generatedParameters = _multipleChoiceDTOConverter.GenerateParameters(multipleChoiceQuestion);
                        break;
                    case OpenTextQuestion openTextQuestion:
                        generatedParameters = _openTextDTOConverter.GenerateParameters(openTextQuestion);
                        break;
                    default:
                        throw new NotImplementedException();
                }
                return new QuestionResponseDTO
                {
                    QuestionText = question.Text,
                    Id = question.Id,
                    QuestionType = QuestionTypeConverter.ToString(question.Type),
                    QuestionParameters = generatedParameters
                };

            });
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
                        if (answer.OptionName != null && SingleChoiceAnswerChecker.IsCorrect(singleChoiceQuestion, answer.OptionName))
                        {
                            correctAnswers++;
                        }
                        break;

                    case MultipleChoiceQuestion multipleChoiceQuestion:
                        if (answer.OptionNames != null)
                        {
                            if (MultipleChoiceAnswerChecker.IsCorrect(multipleChoiceQuestion, answer.OptionNames.Select(opt => new MultipleChoiceOption { Name = opt }).ToList()))
                            {
                                correctAnswers++;
                            }
                        }
                        break;

                    case OpenTextQuestion openTextQuestion:
                        var answerText = answer.AnswerText;
                        if (answerText != null && OpenTextAnswerChecker.IsCorrect(openTextQuestion, answerText, trimWhitespace: true))
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