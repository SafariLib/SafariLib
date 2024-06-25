using SafariLib.Repositories.Models;

namespace Test.Utils.Data.Models;

public class UserWithGuid : EntityWithGuid
{
    public UserWithGuid()
    {
        Username = RandomUtils.GenerateRandomUsername();
        Password = RandomUtils.GenerateRandomPassword();
        CreatedAt = DateTime.Now;
    }

    public UserWithGuid(string username)
    {
        Username = username;
        Password = RandomUtils.GenerateRandomPassword();
        CreatedAt = DateTime.Now;
    }

    public string Username { get; set; }
    public string Password { get; set; }
}