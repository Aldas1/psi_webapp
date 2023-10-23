﻿using QuizAppApi.Enums;

namespace QuizAppApi.Models;

public abstract class Question
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public abstract QuestionType Type { get; }
        
    public virtual Quiz Quiz { get; set; }
}