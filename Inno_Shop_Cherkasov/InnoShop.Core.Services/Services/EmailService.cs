namespace InnoShop.Core.Services.Services
{
    using MailKit.Net.Smtp;
    using MimeKit;
    using MailKit.Security;

    public class EmailService : IEmailService
    {
        public async Task<bool> SendConfirmationEmailAsync(string email, string confirmationToken)
        {
            try
            {
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

                using var client = new SmtpClient();

                await client.ConnectAsync("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync("kyla.greenholt@ethereal.email", "BAHTu9KgBBX47hRUWM");

                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                Console.WriteLine($"Email sent to: {email}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email failed: {ex.Message}");
                return false;
            }
        }
    }
}
