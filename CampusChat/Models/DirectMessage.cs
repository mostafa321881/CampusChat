namespace CampusChat.Models;

public class DirectMessage
{
    public int Id { get; set; }

    public int DirectChatId { get; set; }

    public int SenderUserId { get; set; }

    public string Content { get; set; } = "";

    public DateTime SentAt { get; set; } = DateTime.UtcNow;
}