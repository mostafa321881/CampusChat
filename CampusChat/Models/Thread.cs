namespace CampusChat.Models;

public class Thread
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public string Description { get; set; } = "";

    public int CreatedByUserId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsPrivate { get; set; } = false;

    public string AccessCode { get; set; } = "";

    public List<int> MemberUserIds { get; set; } = new();
}