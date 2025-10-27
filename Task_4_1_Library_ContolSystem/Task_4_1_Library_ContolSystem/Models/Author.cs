namespace Task_4_1_Library_ControlSystem.Models
{
    public class Author : BaseEntity<int>
    {
        public string? Name { get; set; }
        public DateTime DateOfBirth { get; set; }

        public virtual ICollection<Book> Books { get; set; } = new List<Book>();

        public Author() { }
    }
}
