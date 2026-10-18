using CampusChat.Models;

namespace CampusChat.Services;

public class ProfileService
{
    private readonly List<StudentProfile> _profiles = new();

    public List<StudentProfile> GetProfiles()
    {
        return _profiles;
    }

    public void AddProfile(StudentProfile profile)
    {
        _profiles.Add(profile);
    }
}