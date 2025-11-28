namespace InnoShop.Shared.Domain.Models
{
    public class User : BaseEntity<int>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string PasswordHash { get; set; }
        public bool IsEmailConfirmed { get; set; } = false;
        public string? EmailConfirmationToken { get; set; }
        public DateTime? EmailConfirmationTokenExpires { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpires { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? DeactivatedAt { get; set; }

        public User() { }
    }
}
