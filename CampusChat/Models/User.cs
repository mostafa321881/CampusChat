namespace CampusChat.Models;

public class User
{
    public int Id { get; set; }

    public string DisplayName { get; set; } = "";

    public string Bio { get; set; } = "";

    public int StudyYear { get; set; }
    
    public string AvatarColor { get; set; } = "#4F46E5";

    public bool IsAuditor { get; set; } = false;

    // TODO: Add an AvatarColor string property.
    // Give it a default colour, such as "#4F46E5".

    // TODO: Add an IsAuditor bool property.
    // Its default value should be false.
}