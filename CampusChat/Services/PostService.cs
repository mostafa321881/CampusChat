
using CampusChat.Models;

namespace CampusChat.Services;

/// <summary>
/// Manages posts and replies inside discussion threads.
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
    /// Private thread posts are visible only to members.
    /// </summary>
    public List<Post> GetPostsByThread(int threadId, int userId)
    {
        if (!CanAccessThread(threadId, userId))
        {
            return new List<Post>();
        }

        return _store.Posts
            .Where(post => post.ThreadId == threadId)
            .OrderBy(post => post.CreatedAt)
            .ToList();
    }

    /// <summary>
    /// Creates a post or a reply inside a thread.
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

        if (!CanAccessThread(threadId, authorUserId))
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
    /// Deletes a post only if the requesting user wrote it.
    /// Replies to the deleted post are also removed.
    /// </summary>
    public bool DeletePost(int postId, int userId)
    {
        var post = _store.Posts.FirstOrDefault(
            item => item.Id == postId);

        if (post == null || post.AuthorUserId != userId)
        {
            return false;
        }

        DeletePostAndReplies(postId);

        return true;
    }

    /// <summary>
    /// Checks whether a student can access a thread.
    /// </summary>
    private bool CanAccessThread(int threadId, int userId)
    {
        bool userExists = _store.Users.Any(
            user => user.Id == userId);

        if (!userExists)
        {
            return false;
        }

        var thread = _store.Threads.FirstOrDefault(
            item => item.Id == threadId);

        if (thread == null)
        {
            return false;
        }

        return !thread.IsPrivate ||
               thread.MemberUserIds.Contains(userId);
    }

    /// <summary>
    /// Removes a post and its replies recursively.
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
