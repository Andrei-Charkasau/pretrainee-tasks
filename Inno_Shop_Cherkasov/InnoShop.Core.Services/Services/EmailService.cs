namespace InnoShop.Core.Services.Services
{
    using MailKit.Net.Smtp;
    using MailKit.Security;
    using Microsoft.Extensions.Configuration;
    using MimeKit;

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendConfirmationEmailAsync(string email, string confirmationToken)
        {
            try
            {
                var smtpServer = _configuration["EmailSettings:SmtpServer"];
                var port = int.Parse(_configuration["EmailSettings:Port"]);
                var fromMail = _configuration["EmailSettings:Username"];
                var password = _configuration["EmailSettings:Password"];

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("INNOSHOP & Co.", "noreply@innoshopco.com"));
                message.To.Add(new MailboxAddress("", email));
                message.Subject = "Подтвердите ваш email";

                var confirmationLink = $"https://localhost:7019/api/Auth/confirm-email?token={confirmationToken}";

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = $@"
                    <h3>Подтверждение email</h3>
                    <p>Для завершения регистрации нажмите на ссылку:</p>
                    <a href='{confirmationLink}'>Подтвердить email</a>
                    <p><small>Ссылка действительна 24 часа</small></p>
                ",
                    TextBody = $"Подтвердите email: {confirmationLink}"
                };

                message.Body = bodyBuilder.ToMessageBody();

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(smtpServer, port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(fromMail, password);
                await smtp.SendAsync(message);
                await smtp.DisconnectAsync(true);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}

