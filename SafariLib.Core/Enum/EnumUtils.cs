using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SafariLib.Core.Enum;

public static class EnumUtils
{
    /// <summary>
    ///     Get the display name of an enum value that has a Display Attribute.
    /// </summary>
    public static string GetDisplayName(this System.Enum enumValue) =>
        enumValue
            .GetType()
            .GetMember(enumValue.ToString())
            .First()
            .GetCustomAttribute<DisplayAttribute>()?
            .Name ?? string.Empty;
}