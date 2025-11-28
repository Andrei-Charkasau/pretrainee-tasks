namespace InnoShop.Auth.Services.Services
{
    public interface IEmailService
    {
        Task<bool> SendConfirmationEmailAsync(string toEmail, string mailSubject);
        Task<bool> SendPasswordResetEmailAsync(string email, string resetToken);
    }
}
