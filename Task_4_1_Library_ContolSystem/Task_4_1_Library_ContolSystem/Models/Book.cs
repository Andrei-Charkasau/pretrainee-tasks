namespace Task_4_1_Library_ControlSystem.Models
{
    public class Book : BaseEntity<int>
    {
        public string? Title { get; set; }
        public int PublishedYear { get; set; }
        public int AuthorId { get; set; }

        public Book() { }
    }
}
