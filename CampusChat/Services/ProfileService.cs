using CampusChat.Models;

namespace CampusChat.Services;

public class ProfileService
{
    private readonly List<StudentProfile> _profiles = new();

    public ProfileService()
    {
        _profiles.Add(new StudentProfile
        {
            DisplayName = "Mostafa",
            Bio = "Computer science student",
            StudyYear = 1
        });

        _profiles.Add(new StudentProfile
        {
            DisplayName = "Sara",
            Bio = "Software development student",
            StudyYear = 2
        });

        _profiles.Add(new StudentProfile
        {
            DisplayName = "Ahmed",
            Bio = "IT student",
            StudyYear = 3
        });
    }

    public List<StudentProfile> GetProfiles()
    {
        return _profiles;
    }

    public void AddProfile(StudentProfile profile)
    {
        _profiles.Add(profile);
    }
}