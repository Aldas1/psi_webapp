namespace QuizAppApi.Models.Flashcards;

public class Flashcard
{
    public int Id { get; set; }
    public string Question { get; set; }
    public string Answer { get; set; }

    public virtual FlashcardCollection FlashcardCollection { get; set; }
    public int FlashcardCollectionId { get; set; }
}