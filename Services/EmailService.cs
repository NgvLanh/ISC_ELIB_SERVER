using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

public class EmailService : IEmailService
{
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _smtpUser;
    private readonly string _smtpPass;

    public EmailService(IConfiguration configuration)
    {
        _smtpServer = configuration["Smtp:Server"];
        _smtpPort = int.Parse(configuration["Smtp:Port"]);
        _smtpUser = configuration["Smtp:User"];
        _smtpPass = configuration["Smtp:Pass"];
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("ISC LMS", _smtpUser));
        message.To.Add(new MailboxAddress("", to));
        message.Subject = subject;

        var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "EmailTemplate.html");
        var template = await File.ReadAllTextAsync(templatePath);
        var htmlBody = template.Replace("{{subject}}", subject).Replace("{{body}}", body);

        message.Body = new TextPart("html")
        {
            Text = htmlBody
        };

        using (var client = new SmtpClient())
        {
            try
            {
                await client.ConnectAsync(_smtpServer, _smtpPort, false);
                await client.AuthenticateAsync(_smtpUser, _smtpPass);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw;
            }
        }
    }
}
