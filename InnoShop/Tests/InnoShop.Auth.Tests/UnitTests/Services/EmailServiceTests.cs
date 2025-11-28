using FluentAssertions;
using InnoShop.Auth.Services.Services;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Moq;

namespace InnoShop.Auth.Tests.UnitTests.Services
{
    public class EmailServiceTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IConfigurationSection> _mockEmailSettings;
        private readonly EmailService _emailService;

        public EmailServiceTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockEmailSettings = new Mock<IConfigurationSection>();

            _mockEmailSettings.Setup(x => x["SmtpServer"]).Returns("smtp.test.com");
            _mockEmailSettings.Setup(x => x["Port"]).Returns("587");
            _mockEmailSettings.Setup(x => x["Username"]).Returns("test@test.com");
            _mockEmailSettings.Setup(x => x["Password"]).Returns("testpassword");

            _mockConfiguration.Setup(x => x.GetSection("EmailSettings"))
                .Returns(_mockEmailSettings.Object);

            _emailService = new EmailService(_mockConfiguration.Object);
        }

        [Fact]
        public async Task SendConfirmationEmailAsync_WithValidData_SendsEmail()
        {
            // Arrange
            var email = "user@example.com";
            var confirmationToken = "test-token-123";
            var mockSmtpClient = new Mock<ISmtpClient>();

            // Act
            var action = async () => await _emailService.SendConfirmationEmailAsync(email, confirmationToken);

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task SendPasswordResetEmailAsync_WithValidData_SendsEmail()
        {
            // Arrange
            var email = "user@example.com";
            var resetToken = "reset-token-456";

            // Act
            var action = async () => await _emailService.SendPasswordResetEmailAsync(email, resetToken);

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task SendConfirmationEmailAsync_WithSmtpError_ReturnsFalse()
        {
            // Arrange
            var email = "user@example.com";
            var confirmationToken = "test-token-123";

            _mockEmailSettings.Setup(x => x["SmtpServer"]).Returns("invalid-server");

            // Act
            var result = await _emailService.SendConfirmationEmailAsync(email, confirmationToken);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task SendPasswordResetEmailAsync_WithSmtpError_ReturnsFalse()
        {
            // Arrange
            var email = "user@example.com";
            var resetToken = "reset-token-456";

            _mockEmailSettings.Setup(x => x["SmtpServer"]).Returns("invalid-server");

            // Act
            var result = await _emailService.SendPasswordResetEmailAsync(email, resetToken);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task SendConfirmationEmailAsync_WithInvalidPort_ReturnsFalse()
        {
            // Arrange
            var email = "user@example.com";
            var confirmationToken = "test-token-123";

            _mockEmailSettings.Setup(x => x["Port"]).Returns("not-a-number");

            // Act
            var result = await _emailService.SendConfirmationEmailAsync(email, confirmationToken);

            // Assert
            result.Should().BeFalse();
        }
    }
}