namespace QuizAppApi.Dtos;

public class QuestionParametersDto
{
    public List<string>? Options { get; set; }
    public int? CorrectOptionIndex { get; set; }
    public List<int>? CorrectOptionIndexes { get; set; }
    public string? CorrectText { get; set;}
}