using Microsoft.AspNetCore.Identity.UI.Services;

namespace Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Task.CompletedTask; // Implement your email sending logic here
        }
    }
}
