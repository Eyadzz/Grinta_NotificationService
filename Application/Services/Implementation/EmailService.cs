using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using NotificationService.Application.Services.Interfaces;
using NotificationService.Domain;
using NotificationService.Persistence.Repositories.Interfaces;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace NotificationService.Application.Services.Implementation;

public class EmailService: IEmailService
{
    private readonly IEmailRepository _repository;
    private readonly MailSettings _mailSettings;

    public EmailService(IEmailRepository repository, IOptions<MailSettings> mailSettings)
    {
        _repository = repository;
        _mailSettings = mailSettings.Value;
    }

    public async Task Add(Email email)
    {
        await _repository.Add(email);
        await _repository.Save();
    }
    

    public async Task Send(Email email)
    {
        var mimeMessage = new MimeMessage();
        mimeMessage.From.Add(new MailboxAddress("Vote System", _mailSettings.Mail));
        mimeMessage.To.Add(MailboxAddress.Parse(email.EmailAddress));
        mimeMessage.Subject = email.Subject;

        var htmlView = new TextPart("html") { Text = email.Body };
        mimeMessage.Body = htmlView;
        
        using var smtp = new SmtpClient();
        smtp.CheckCertificateRevocation = false;
        await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port,false);
        await smtp.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password);
        await smtp.SendAsync(mimeMessage);
        await smtp.DisconnectAsync(true);
        
        Thread.Sleep(3000);
    }
}