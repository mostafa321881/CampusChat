using CampusChat.Models;

namespace CampusChat.Services;

public class UserService
{
    private readonly CampusChatStore _store;

    public UserService(CampusChatStore store)
    {
        _store = store;
    }

    /// <summary>
    /// Returns all users in CampusChat.
    /// </summary>
    public List<User> GetAllUsers()
    {
        return _store.Users;
    }

    /// <summary>
    /// Finds a user by their ID.
    /// </summary>
    public User? GetUserById(int id)
    {
        return _store.Users.FirstOrDefault(user => user.Id == id);
    }
}