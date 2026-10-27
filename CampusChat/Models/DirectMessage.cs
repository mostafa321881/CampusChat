namespace CampusChat.Models;

/// <summary>
/// Represents one message inside a direct chat
/// between two students.
/// </summary>
public class DirectMessage
{
    public int Id { get; set; }

    public int ChatId { get; set; }

    public int SenderUserId { get; set; }

    public string Content { get; set; } = "";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}