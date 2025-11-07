namespace Task_4_1_Library_ControlSystem.DtoModels
{
    public class AuthorWithBooksCountDto
    {
        public int Id { get; }
        public string? Name { get; }
        public int BookCount { get; }

        public AuthorWithBooksCountDto(int id, string? name, int bookCount)
        {
            Id = id;
            Name = name;
            BookCount = bookCount;
        }
    }
}
