
using CampusChat.Models;

namespace CampusChat.Services;

/// <summary>
/// Manages posts and replies inside discussion threads.
/// Enforces separate reading and writing permissions.
/// </summary>
public class PostService
{
    private readonly CampusChatStore _store;

    public PostService(CampusChatStore store)
    {
        _store = store;
    }

    /// <summary>
    /// Returns posts from a thread in chronological order.
    /// Public posts are readable by all valid users.
    /// Private posts are readable by members and auditors.
    /// </summary>
    public List<Post> GetPostsByThread(int threadId, int userId)
    {
        if (!CanReadThread(threadId, userId))
        {
            return new List<Post>();
        }

        return _store.Posts
            .Where(post => post.ThreadId == threadId)
            .OrderBy(post => post.CreatedAt)
            .ToList();
    }

    /// <summary>
    /// Creates a post or reply inside a thread.
    /// Private threads require membership, including for auditors.
    /// </summary>
    public Post? CreatePost(
        int threadId,
        int authorUserId,
        string content,
        int? parentPostId = null)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return null;
        }

        if (!CanWriteThread(threadId, authorUserId))
        {
            return null;
        }

        if (parentPostId.HasValue)
        {
            bool parentExists = _store.Posts.Any(post =>
                post.Id == parentPostId.Value &&
                post.ThreadId == threadId);

            if (!parentExists)
            {
                return null;
            }
        }

        int nextId = _store.Posts.Count == 0
            ? 1
            : _store.Posts.Max(post => post.Id) + 1;

        var newPost = new Post
        {
            Id = nextId,
            ThreadId = threadId,
            AuthorUserId = authorUserId,
            Content = content.Trim(),
            CreatedAt = DateTime.UtcNow,
            ParentPostId = parentPostId
        };

        _store.Posts.Add(newPost);

        return newPost;
    }

    /// <summary>
    /// Deletes a post only if the requesting user wrote it
    /// and still has permission to write in that thread.
    /// Replies to the deleted post are also removed.
    /// </summary>
    public bool DeletePost(int postId, int userId)
    {
        var post = _store.Posts.FirstOrDefault(
            item => item.Id == postId);

        if (post == null)
        {
            return false;
        }

        if (post.AuthorUserId != userId)
        {
            return false;
        }

        if (!CanWriteThread(post.ThreadId, userId))
        {
            return false;
        }

        DeletePostAndReplies(postId);

        return true;
    }

    /// <summary>
    /// Checks whether a user can read a thread.
    /// Auditors can read private threads without joining.
    /// </summary>
    private bool CanReadThread(int threadId, int userId)
    {
        var user = _store.Users.FirstOrDefault(
            item => item.Id == userId);

        var thread = _store.Threads.FirstOrDefault(
            item => item.Id == threadId);

        if (user == null || thread == null)
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
    /// Checks whether a user can write posts or replies.
    /// Auditors must also be members to write in private threads.
    /// </summary>
    private bool CanWriteThread(int threadId, int userId)
    {
        var user = _store.Users.FirstOrDefault(
            item => item.Id == userId);

        var thread = _store.Threads.FirstOrDefault(
            item => item.Id == threadId);

        if (user == null || thread == null)
        {
            return false;
        }

        if (!thread.IsPrivate)
        {
            return true;
        }

        return thread.MemberUserIds.Contains(userId);
    }

    /// <summary>
    /// Removes a post and all its replies recursively.
    /// </summary>
    private void DeletePostAndReplies(int postId)
    {
        var replies = _store.Posts
            .Where(post => post.ParentPostId == postId)
            .Select(post => post.Id)
            .ToList();

        foreach (int replyId in replies)
        {
            DeletePostAndReplies(replyId);
        }

        var post = _store.Posts.FirstOrDefault(
            item => item.Id == postId);

        if (post != null)
        {
            _store.Posts.Remove(post);
        }
    }
}
