using NotificationService.Domain;

namespace NotificationService.Application.Services.Interfaces;

public interface IEmailService
{
    Task Send(Email email);
    Task Add(Email email);
}