namespace Task_4_1_Library_ControlSystem.Models
{
    public class Author : BaseEntity<int>
    {
        public string? Name { get; set; }
        public DateTime DateOfBirth { get; set; }

        public Author() { }

        public Author (Author author)
        {
            Id = author.Id;
            Name = author.Name;
            DateOfBirth = author.DateOfBirth;
        }
    }
}
