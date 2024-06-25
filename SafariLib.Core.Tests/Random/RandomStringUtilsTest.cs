using SafariLib.Core.Random;

namespace SafariLib.Core.Tests.Random;

public class RandomStringUtilsTest
{
    [Fact]
    public void GenerateRandomSecret_Should_return_a_random_string()
    {
        // Act
        var secret = RandomStringUtils.GenerateRandomSecret();

        // Assert
        Assert.NotNull(secret);
        Assert.NotEmpty(secret);
        Assert.Equal(128, secret.Length);
    }
}