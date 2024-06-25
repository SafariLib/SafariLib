using SafariLib.Repositories.Models;

namespace Test.Utils.Data.Models;

public class User : Entity
{
    public User()
    {
        Username = RandomUtils.GenerateRandomUsername();
        Password = RandomUtils.GenerateRandomPassword();
        CreatedAt = DateTime.Now;
    }

    public User(string username)
    {
        Username = username;
        Password = RandomUtils.GenerateRandomPassword();
        CreatedAt = DateTime.Now;
    }

    public string Username { get; set; }
    public string Password { get; set; }
}