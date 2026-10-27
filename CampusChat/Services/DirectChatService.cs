
using CampusChat.Models;

namespace CampusChat.Services;

/// <summary>
/// Manages private conversations between two students.
/// Auditors may read all conversations, but only
/// participants may send messages.
/// </summary>
public class DirectChatService
{
    private readonly CampusChatStore _store;

    private readonly List<DirectChat> _chats = new();
    private readonly List<DirectMessage> _messages = new();

    public DirectChatService(CampusChatStore store)
    {
        _store = store;
    }

    /// <summary>
    /// Returns conversations that the user can see.
    /// Auditors can see every conversation.
    /// </summary>
    public List<DirectChat> GetVisibleChats(int userId)
    {
        var user = _store.Users.FirstOrDefault(
            item => item.Id == userId);

        if (user == null)
        {
            return new List<DirectChat>();
        }

        if (user.IsAuditor)
        {
            return _chats.ToList();
        }

        return _chats
            .Where(chat =>
                chat.User1Id == userId ||
                chat.User2Id == userId)
            .ToList();
    }

    /// <summary>
    /// Returns a conversation only when the user
    /// is a participant or an auditor.
    /// </summary>
    public DirectChat? GetChatById(int chatId, int userId)
    {
        if (!CanReadChat(chatId, userId))
        {
            return null;
        }

        return _chats.FirstOrDefault(
            chat => chat.Id == chatId);
    }

    /// <summary>
    /// Creates or returns an existing conversation
    /// between two different students.
    /// </summary>
    public DirectChat? CreateChat(int user1Id, int user2Id)
    {
        if (user1Id == user2Id)
        {
            return null;
        }

        bool firstUserExists = _store.Users.Any(
            user => user.Id == user1Id);

        bool secondUserExists = _store.Users.Any(
            user => user.Id == user2Id);

        if (!firstUserExists || !secondUserExists)
        {
            return null;
        }

        var existingChat = _chats.FirstOrDefault(chat =>
            (chat.User1Id == user1Id &&
             chat.User2Id == user2Id) ||
            (chat.User1Id == user2Id &&
             chat.User2Id == user1Id));

        if (existingChat != null)
        {
            return existingChat;
        }

        int nextId = _chats.Count == 0
            ? 1
            : _chats.Max(chat => chat.Id) + 1;

        var newChat = new DirectChat
        {
            Id = nextId,
            User1Id = user1Id,
            User2Id = user2Id
        };

        _chats.Add(newChat);

        return newChat;
    }

    /// <summary>
    /// Returns messages newest first.
    /// Only participants and auditors can read them.
    /// </summary>
    public List<DirectMessage> GetMessages(int chatId, int userId)
    {
        if (!CanReadChat(chatId, userId))
        {
            return new List<DirectMessage>();
        }

        return _messages
            .Where(message => message.ChatId == chatId)
            .OrderByDescending(message => message.CreatedAt)
            .ThenByDescending(message => message.Id)
            .ToList();
    }

    /// <summary>
    /// Sends a message.
    /// Only the two participants may send messages.
    /// An auditor who is not a participant cannot send.
    /// </summary>
    public DirectMessage? SendMessage(
        int chatId,
        int senderUserId,
        string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return null;
        }

        if (!CanSendMessage(chatId, senderUserId))
        {
            return null;
        }

        int nextId = _messages.Count == 0
            ? 1
            : _messages.Max(message => message.Id) + 1;

        var newMessage = new DirectMessage
        {
            Id = nextId,
            ChatId = chatId,
            SenderUserId = senderUserId,
            Content = content.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        _messages.Add(newMessage);

        return newMessage;
    }

    /// <summary>
    /// Checks whether a student may read a conversation.
    /// Auditors can read all conversations.
    /// </summary>
    public bool CanReadChat(int chatId, int userId)
    {
        var user = _store.Users.FirstOrDefault(
            item => item.Id == userId);

        var chat = _chats.FirstOrDefault(
            item => item.Id == chatId);

        if (user == null || chat == null)
        {
            return false;
        }

        return user.IsAuditor ||
               chat.User1Id == userId ||
               chat.User2Id == userId;
    }

    /// <summary>
    /// Checks whether a student may send a message.
    /// Only conversation participants can send.
    /// </summary>
    public bool CanSendMessage(int chatId, int userId)
    {
        bool userExists = _store.Users.Any(
            user => user.Id == userId);

        if (!userExists)
        {
            return false;
        }

        var chat = _chats.FirstOrDefault(
            item => item.Id == chatId);

        if (chat == null)
        {
            return false;
        }

        return chat.User1Id == userId ||
               chat.User2Id == userId;
    }
}
