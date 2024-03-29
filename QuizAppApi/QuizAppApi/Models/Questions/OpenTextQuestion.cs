﻿using QuizAppApi.Enums;

namespace QuizAppApi.Models.Questions;

public class OpenTextQuestion : Question
{
    public string CorrectAnswer { get; set; }
    public override QuestionType Type { get => QuestionType.OpenTextQuestion; }
}