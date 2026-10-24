using CampusChat.Models;

namespace CampusChat.Services;

public class CampusChatStore
{
    public List<User> Users { get; } = new();

    public List<CampusChat.Models.Thread> Threads { get; } = new();
}