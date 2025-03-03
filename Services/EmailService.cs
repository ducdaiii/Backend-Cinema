using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

public class EmailService
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _env;

    public EmailService(IConfiguration configuration, IWebHostEnvironment env)
    {
        _configuration = configuration;
        _env = env;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var smtpClient = new SmtpClient(_configuration["EmailSettings:SmtpServer"])
        {
            Port = int.Parse(_configuration["EmailSettings:SmtpPort"]),
            Credentials = new NetworkCredential(
                _configuration["EmailSettings:SenderEmail"],
                _configuration["EmailSettings:SenderPassword"]
            ),
            EnableSsl = true
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_configuration["EmailSettings:SenderEmail"]),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        mailMessage.To.Add(toEmail);
        await smtpClient.SendMailAsync(mailMessage);
    }

    public async Task<string> GetEmailTemplate(string templateName, Dictionary<string, string> placeholders)
    {
        string templatePath = Path.Combine(_env.ContentRootPath, "Templates", $"{templateName}.html");

        if (!File.Exists(templatePath))
            return "Template không tồn tại.";

        string emailBody = await File.ReadAllTextAsync(templatePath);

        foreach (var placeholder in placeholders)
        {
            emailBody = emailBody.Replace($"{{{placeholder.Key}}}", placeholder.Value);
        }

        return emailBody;
    }
}