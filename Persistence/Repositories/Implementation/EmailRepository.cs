using NotificationService.Domain;
using NotificationService.Persistence.Repositories.Interfaces;

namespace NotificationService.Persistence.Repositories.Implementation;

public class EmailRepository : IEmailRepository
{
    private readonly AppDbContext _context;

    public EmailRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task Add(Email email)
    {
        await _context.AddAsync(email);
    }

    public async Task Save()
    {
        await _context.SaveChangesAsync();
    }
}