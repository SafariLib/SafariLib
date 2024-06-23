using System.ComponentModel.DataAnnotations;
using SafariLib.Core.Enum;

namespace SafariLib.Core.Tests.Enum;

public class ExtensionsTest
{
    private enum ETest
    {
        [Display(Name = "Test of very simple case")]
        Test,
        Test2
    }

    [Fact]
    public void GetDisplayName_ShouldReturnEnumDisplayName() =>
        Assert.Equal("Test of very simple case", ETest.Test.GetDisplayName());

    [Fact]
    public void GetDisplayName_ShouldReturnEmptyString() =>
        Assert.Equal(string.Empty, ETest.Test2.GetDisplayName());
}