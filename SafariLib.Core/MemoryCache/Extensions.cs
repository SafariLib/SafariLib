using Microsoft.Extensions.Caching.Memory;

namespace SafariLib.Core.MemoryCache;

public static class Extensions
{
    public static T? TryGetValue<T>(this IMemoryCache memoryCache, string key)
    {
        memoryCache.TryGetValue(key, out T? value);
        return value;
    }
}