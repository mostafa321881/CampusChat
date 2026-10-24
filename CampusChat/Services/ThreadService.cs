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
}