using System.ComponentModel.DataAnnotations.Schema;

namespace NotificationService.Domain;

[Table("Emails", Schema = "Notification")]
public class Email
{
    public int EmailId { get; set; }
    public string EmailAddress { get; set; } = string.Empty;
    public int ReceiverId { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public DateTime? SentAt { get; set; } = DateTime.Now;
}