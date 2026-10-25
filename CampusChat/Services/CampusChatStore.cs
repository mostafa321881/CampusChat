using CampusChat.Models;

namespace CampusChat.Services;

public class CampusChatStore
{
    public List<User> Users { get; } = new();

    public List<CampusChat.Models.Thread> Threads { get; } = new();

    // Stores posts and replies in discussion threads.
    public List<Post> Posts { get; } = new();

    public CampusChatStore()
    {
        Users.Add(new User
        {
            Id = 1,
            DisplayName = "Mostafa",
            Bio = "Computer science student",
            StudyYear = 1,
            AvatarColor = "#4F46E5",
            IsAuditor = false
        });

        Users.Add(new User
        {
            Id = 2,
            DisplayName = "Sara",
            Bio = "Software development student",
            StudyYear = 2,
            AvatarColor = "#16A34A",
            IsAuditor = false
        });

        Users.Add(new User
        {
            Id = 3,
            DisplayName = "Ahmed",
            Bio = "Computer science student",
            StudyYear = 3,
            AvatarColor = "#EA580C",
            IsAuditor = false
        });
    }
}