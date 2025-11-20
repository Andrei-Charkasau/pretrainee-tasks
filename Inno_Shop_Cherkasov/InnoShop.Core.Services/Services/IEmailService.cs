namespace InnoShop.Core.Services.Services
{
    public interface IEmailService
    {
        Task<bool> SendConfirmationEmailAsync(string toEmail, string mailSubject);
    }
}
