
using CampusChat.Models;

namespace CampusChat.Services;

/// <summary>
/// Manages student profiles using the shared CampusChatStore.
/// </summary>
public class ProfileService
{
    private readonly CampusChatStore _store;

    public ProfileService(CampusChatStore store)
    {
        _store = store;
    }

    /// <summary>
    /// Returns all student profiles.
    /// </summary>
    public List<User> GetProfiles()
    {
        return _store.Users.ToList();
    }

    /// <summary>
    /// Returns one student profile by ID.
    /// </summary>
    public User? GetProfileById(int userId)
    {
        return _store.Users.FirstOrDefault(
            user => user.Id == userId
        );
    }

    /// <summary>
    /// Creates a new student account and profile.
    /// New students never receive auditor permissions.
    /// </summary>
    public User? CreateProfile(
        string displayName,
        string bio,
        int studyYear,
        string avatarColor)
    {
        if (string.IsNullOrWhiteSpace(displayName))
        {
            return null;
        }

        if (studyYear < 1 || studyYear > 6)
        {
            return null;
        }

        if (!IsValidAvatarColor(avatarColor))
        {
            return null;
        }

        string cleanName = displayName.Trim();

        bool nameExists = _store.Users.Any(user =>
            string.Equals(
                user.DisplayName,
                cleanName,
                StringComparison.OrdinalIgnoreCase));

        if (nameExists)
        {
            return null;
        }

        int nextId = _store.Users.Count == 0
            ? 1
            : _store.Users.Max(user => user.Id) + 1;

        var newUser = new User
        {
            Id = nextId,
            DisplayName = cleanName,
            Bio = bio?.Trim() ?? "",
            StudyYear = studyYear,
            AvatarColor = avatarColor,
            IsAuditor = false
        };

        _store.Users.Add(newUser);

        return newUser;
    }

    /// <summary>
    /// Updates a student's own profile.
    /// Students cannot edit another student's profile.
    /// </summary>
    public bool UpdateProfile(
        int userId,
        int requestingUserId,
        string displayName,
        string bio,
        int studyYear,
        string avatarColor)
    {
        if (userId != requestingUserId)
        {
            return false;
        }

        var user = GetProfileById(userId);

        if (user == null ||
            string.IsNullOrWhiteSpace(displayName) ||
            studyYear < 1 ||
            studyYear > 6 ||
            !IsValidAvatarColor(avatarColor))
        {
            return false;
        }

        string cleanName = displayName.Trim();

        bool nameExists = _store.Users.Any(other =>
            other.Id != userId &&
            string.Equals(
                other.DisplayName,
                cleanName,
                StringComparison.OrdinalIgnoreCase));

        if (nameExists)
        {
            return false;
        }

        user.DisplayName = cleanName;
        user.Bio = bio?.Trim() ?? "";
        user.StudyYear = studyYear;
        user.AvatarColor = avatarColor;

        return true;
    }

    /// <summary>
    /// Returns the threads a student has joined.
    /// </summary>
    public List<CampusChat.Models.Thread> GetJoinedThreads(int userId)
    {
        if (GetProfileById(userId) == null)
        {
            return new List<CampusChat.Models.Thread>();
        }

        return _store.Threads
            .Where(thread => thread.MemberUserIds.Contains(userId))
            .ToList();
    }

    /// <summary>
    /// Validates a six-digit hexadecimal avatar color.
    /// </summary>
    private bool IsValidAvatarColor(string avatarColor)
    {
        return !string.IsNullOrWhiteSpace(avatarColor) &&
               System.Text.RegularExpressions.Regex.IsMatch(
                   avatarColor,
                   "^#[0-9A-Fa-f]{6}$");
    }
}
