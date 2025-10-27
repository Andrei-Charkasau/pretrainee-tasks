namespace Task_4_1_Library_ControlSystem.DtoModels
{
    public class AuthorDto
    {
        public string? Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public List<BookDto> Books { get; set; } = new();
    }
}
