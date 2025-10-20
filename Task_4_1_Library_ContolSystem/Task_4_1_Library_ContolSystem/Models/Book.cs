namespace Task_4_1_Library_ControlSystem.Models
{
    public class Book : BaseEntity<int>
    {
        public string? Title { get; set; }
        public int PublishedYear { get; set; }
        public int AuthorId { get; set; }

        public Book() { }
        public Book(Book book)
        {
            Id = book.Id;
            Title = book.Title;
            PublishedYear = book.PublishedYear;
            AuthorId = book.AuthorId;
        }
    }
}
