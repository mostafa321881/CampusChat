
using CampusChat.Models;

namespace CampusChat.Services;

/// <summary>
/// Tracks unread discussion posts for each student.
/// Each student has a separate read position for each thread.
/// </summary>
public class UnreadService
{
    private readonly CampusChatStore _store;

    // Key: (UserId, ThreadId)
    // Value: IDs of posts the student has already read.
    private readonly Dictionary<(int UserId, int ThreadId), HashSet<int>>
        _readPosts = new();

    private readonly object _lock = new();

    public UnreadService(CampusChatStore store)
    {
        _store = store;
    }

    /// <summary>
    /// Returns the number of unread posts in a joined thread.
    /// A student's own posts are not counted as unread.
    /// </summary>
    public int GetUnreadCount(int threadId, int userId)
    {
        lock (_lock)
        {
            var thread = _store.Threads.FirstOrDefault(
                item => item.Id == threadId);

            if (thread == null ||
                !_store.Users.Any(user => user.Id == userId) ||
                !thread.MemberUserIds.Contains(userId))
            {
                return 0;
            }

            var key = (UserId: userId, ThreadId: threadId);

            _readPosts.TryGetValue(key, out var readIds);

            return _store.Posts.Count(post =>
                post.ThreadId == threadId &&
                post.AuthorUserId != userId &&
                (readIds == null || !readIds.Contains(post.Id)));
        }
    }

    /// <summary>
    /// Marks all current posts in a thread as read for one student.
    /// The student must be a member of the thread.
    /// </summary>
    public void MarkThreadAsRead(int threadId, int userId)
    {
        lock (_lock)
        {
            var thread = _store.Threads.FirstOrDefault(
                item => item.Id == threadId);

            if (thread == null ||
                !_store.Users.Any(user => user.Id == userId) ||
                !thread.MemberUserIds.Contains(userId))
            {
                return;
            }

            var key = (UserId: userId, ThreadId: threadId);

            if (!_readPosts.TryGetValue(key, out var readIds))
            {
                readIds = new HashSet<int>();
                _readPosts[key] = readIds;
            }

            foreach (var post in _store.Posts.Where(
                         post => post.ThreadId == threadId))
            {
                readIds.Add(post.Id);
            }
        }
    }

    /// <summary>
    /// Initializes the read position when a student joins a thread.
    /// Existing posts are treated as read when joining.
    /// </summary>
    public void InitializeThreadForUser(int threadId, int userId)
    {
        MarkThreadAsRead(threadId, userId);
    }

    /// <summary>
    /// Clears the read history when a student leaves a thread.
    /// </summary>
    public void ClearThreadForUser(int threadId, int userId)
    {
        lock (_lock)
        {
            _readPosts.Remove(
                (UserId: userId, ThreadId: threadId));
        }
    }
}
