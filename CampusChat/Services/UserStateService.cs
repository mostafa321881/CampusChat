namespace CampusChat.Services;

public class UserStateService
{
    public int? CurrentUserId { get; private set; }

    public void SetCurrentUser(int userId)
    {
        CurrentUserId = userId;
    }

    public void ClearCurrentUser()
    {
        CurrentUserId = null;
    }
}