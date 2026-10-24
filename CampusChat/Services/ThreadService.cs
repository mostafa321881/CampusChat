
using CampusChat.Models;

namespace CampusChat.Services;

public class ThreadService
{
    private readonly CampusChatStore _store;

    public ThreadService(CampusChatStore store)
    {
        _store = store;
    }

    /// <summary>
    /// Returns all available threads.
    /// </summary>
    public List<CampusChat.Models.Thread> GetAllThreads()
    {
        return _store.Threads;
    }

    /// <summary>
    /// Searches for threads by name.
    /// </summary>
    public List<CampusChat.Models.Thread> SearchThreads(string searchText)
    {
        return _store.Threads
            .Where(thread => thread.Name.Contains(
                searchText,
                StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    /// <summary>
    /// Attempts to join a thread.
    /// </summary>
    public bool JoinThread(int threadId, int userId, string accessCode)
    {
        var thread = _store.Threads
            .FirstOrDefault(t => t.Id == threadId);

        if (thread == null)
        {
            return false;
        }

        if (!_store.Users.Any(user => user.Id == userId))
        {
            return false;
        }

        if (thread.IsPrivate && thread.AccessCode != accessCode)
        {
            return false;
        }

        if (thread.MemberUserIds.Contains(userId))
        {
            return false;
        }

        thread.MemberUserIds.Add(userId);

        return true;
    }

    /// <summary>
    /// Removes a user from a thread.
    /// </summary>
    public bool LeaveThread(int threadId, int userId)
    {
        var thread = _store.Threads
            .FirstOrDefault(t => t.Id == threadId);

        if (thread == null)
        {
            return false;
        }

        return thread.MemberUserIds.Remove(userId);
    }

    /// <summary>
    /// Creates a new public or private discussion thread.
    /// </summary>
    public bool CreateThread(
        string name,
        string description,
        int createdByUserId,
        bool isPrivate,
        string accessCode)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }

        if (!_store.Users.Any(user => user.Id == createdByUserId))
        {
            return false;
        }

        if (isPrivate && string.IsNullOrWhiteSpace(accessCode))
        {
            return false;
        }

        int newId = _store.Threads.Count == 0
            ? 1
            : _store.Threads.Max(thread => thread.Id) + 1;

        var newThread = new CampusChat.Models.Thread
        {
            Id = newId,
            Name = name.Trim(),
            Description = description,
            CreatedByUserId = createdByUserId,
            CreatedAt = DateTime.UtcNow,
            IsPrivate = isPrivate,
            AccessCode = isPrivate ? accessCode : ""
        };

        newThread.MemberUserIds.Add(createdByUserId);

        _store.Threads.Add(newThread);

        return true;
    }
}
