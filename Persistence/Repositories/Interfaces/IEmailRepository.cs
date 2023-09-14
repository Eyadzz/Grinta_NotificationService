using NotificationService.Domain;

namespace NotificationService.Persistence.Repositories.Interfaces;

public interface IEmailRepository
{
    Task Add(Email email);
    Task Save();
}