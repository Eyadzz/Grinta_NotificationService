using AutoMapper;
using MassTransit;
using NotificationService.Application.Services.Interfaces;
using NotificationService.Domain;
using SharedLibrary;

namespace NotificationService.Application.Consumers;

public class EmailConsumer : IConsumer<EmailMessage>
{
    private readonly IEmailService _emailService;
    private readonly IMapper _mapper;

    public EmailConsumer(IEmailService emailService, IMapper mapper)
    {
        _emailService = emailService;
        _mapper = mapper;
    }
    public async Task Consume(ConsumeContext<EmailMessage> context)
    {
        var email = _mapper.Map<Email>(context.Message);

        try
        {
            await _emailService.Send(email);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine("Could not send the email. Sent to notifications_error queue");
            throw;
        }
        
        await _emailService.Add(email);
    }
}