namespace QuizAppApi.Models.Flashcards;

public class FlashcardCollection
{
    public int Id { get; set; }
    public string Name { get; set; }

    public virtual ICollection<Flashcard> Flashcards { get; set; }
}