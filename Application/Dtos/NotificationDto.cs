namespace NotificationService.Application.Dtos;

public class NotificationDto
{
    public string? Message { get; set; }
    public string? Url { get; set; }
    public string? Section { get; set; }
    public int ReceiverId { get; set; }
}