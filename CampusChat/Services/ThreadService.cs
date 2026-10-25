
using CampusChat.Models;

namespace CampusChat.Services;

/// <summary>
/// Manages discussion threads and their access rules.
/// </summary>
public class ThreadService
{
    private readonly CampusChatStore _store;

    public ThreadService(CampusChatStore store)
    {
        _store = store;
    }

    /// <summary>
    /// Returns all threads, including public names of private threads.
    /// </summary>
    public List<CampusChat.Models.Thread> GetAllThreads()
    {
        return _store.Threads;
    }

    /// <summary>
    /// Finds a thread by its ID.
    /// </summary>
    public CampusChat.Models.Thread? GetThreadById(int threadId)
    {
        return _store.Threads
            .FirstOrDefault(thread => thread.Id == threadId);
    }

    /// <summary>
    /// Searches for threads by name.
    /// </summary>
    public List<CampusChat.Models.Thread> SearchThreads(string searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
        {
            return GetAllThreads();
        }

        return _store.Threads
            .Where(thread => thread.Name.Contains(
                searchText,
                StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    /// <summary>
    /// Checks whether a user can read a thread.
    /// Public threads are readable by students.
    /// Private threads require membership, except for auditors.
    /// </summary>
    public bool CanReadThread(int threadId, int userId)
    {
        var thread = GetThreadById(threadId);

        var user = _store.Users
            .FirstOrDefault(student => student.Id == userId);

        if (thread == null || user == null)
        {
            return false;
        }

        if (!thread.IsPrivate)
        {
            return true;
        }

        if (user.IsAuditor)
        {
            return true;
        }

        return thread.MemberUserIds.Contains(userId);
    }

    /// <summary>
    /// Joins a thread. Private threads require the correct code,
    /// unless the user is an auditor.
    /// </summary>
    public bool JoinThread(int threadId, int userId, string accessCode)
    {
        var thread = GetThreadById(threadId);

        var user = _store.Users
            .FirstOrDefault(student => student.Id == userId);

        if (thread == null || user == null)
        {
            return false;
        }

        if (thread.MemberUserIds.Contains(userId))
        {
            return false;
        }

        if (thread.IsPrivate &&
            !user.IsAuditor &&
            thread.AccessCode != accessCode)
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
        var thread = GetThreadById(threadId);

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
