using Azure.Core;
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


        public QuizManipulationResponseDTO CreateQuiz(QuizManipulationRequestDTO request)
        {
            Quiz? generatedQuiz = GenerateQuiz(request);

            if (generatedQuiz is null)
            {
                return new QuizManipulationResponseDTO { Status = "Quiz creation failed" };
            }

            Quiz? addedQuiz = _quizRepository.AddQuiz(generatedQuiz);

            if (addedQuiz is null)
            {
                return new QuizManipulationResponseDTO { Status = "failed" };
            }

            int createdQuizId = addedQuiz.Id;

            return new QuizManipulationResponseDTO { Status = "success", Id = createdQuizId };
        }

        public QuizManipulationResponseDTO UpdateQuiz(int id, QuizManipulationRequestDTO updateRequest)
        {
            Quiz? generatedQuiz = GenerateQuiz(updateRequest);

            if (generatedQuiz is null)
            {
                return new QuizManipulationResponseDTO { Status = "Quiz creation failed" };
            }

            generatedQuiz.Id = id;
            Quiz? updatedQuiz = _quizRepository.UpdateQuiz(id, generatedQuiz);

            if (updatedQuiz is null)
            {
                return new QuizManipulationResponseDTO { Status = "failed" };
            }

            return new QuizManipulationResponseDTO { Status = "success", Id = id };
        }

        public IEnumerable<QuestionResponseDTO>? GetQuestions(int id)
        {
            var quiz = _quizRepository.GetQuizById(id);
            if (quiz is null)
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
            if (quiz is null)
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

            if (quiz is null)
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

                if (question is null)
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
            if (quiz is null)
            {
                return false;
            }

            _quizRepository.DeleteQuiz(id);
            return true;
        }

        private Quiz? GenerateQuiz(QuizManipulationRequestDTO request)
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

                if (generatedQuestion is null)
                {
                    return null;
                }

                generatedQuestion.Text = question.QuestionText;
                newQuiz.Questions.Add(generatedQuestion);
            }
            return newQuiz;
        }
    }
}