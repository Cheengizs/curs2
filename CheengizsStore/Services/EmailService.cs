using System.Net;
using System.Net.Mail;

namespace CheengizsStore.Services;

public interface IEmailService
{
    Task SendEmailAsync(string email, string subject, string message);
}

public class EmailService: IEmailService
{
    private readonly IConfiguration _configuration;
    public EmailService(IConfiguration config)
    {
        this._configuration = config;
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var sender = _configuration.GetValue<string>("EmailService:Email");
        var password = _configuration.GetValue<string>("EmailService:Password");
        var host = _configuration.GetValue<string>("EmailService:Host");
        var port = _configuration.GetValue<int>("EmailService:Port");

        var client = new SmtpClient()
        {
            Host = host,
            Port = port,
            UseDefaultCredentials = false,
            EnableSsl = true,
            Credentials = new NetworkCredential(sender, password),
        };

        var msg = new MailMessage(from: sender, to: email, subject, message);

        await client.SendMailAsync(msg);
    }
}