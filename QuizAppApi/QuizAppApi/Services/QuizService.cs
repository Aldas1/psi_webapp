using QuizAppApi.DTOs;
using QuizAppApi.Interfaces;
using QuizAppApi.Models.Questions;
using QuizAppApi.Utils;

namespace QuizAppApi.Services
{
    public class QuizService : IQuizService
    {
        private IQuizRepository _quizRepository;

        public QuizService(IQuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }

        public QuizCreationResponseDTO CreateQuiz(QuizCreationRequestDTO request)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<QuestionResponseDTO> GetQuestions(int id)
        {
            throw new NotImplementedException();
            // var quiz = _quizRepository.GetQuizById(id);
            // if (quiz == null)
            // {
            //     return null;
            // }
            // // TODO: Return type
            // return quiz.Questions.Select(quiz => new QuestionResponseDTO { Text = quiz.Text });
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
            response.Score = quiz.Questions.Count == 0 ? 0 :correctAnswers * 100 / quiz.Questions.Count;

            return response;
        }
    }
}
