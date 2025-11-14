namespace InnoShop.Core.Models
{
    public class User : BaseEntity<int>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public User() { }
    }
}
