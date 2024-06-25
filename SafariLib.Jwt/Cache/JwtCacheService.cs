using Microsoft.Extensions.Caching.Memory;
using SafariLib.Core.MemoryCache;

namespace SafariLib.Jwt.Cache;

public class JwtCacheService(IMemoryCache memoryCache, int maxTokenAllowed)
    : IJwtCacheService
{
    public bool IsTokenRegistered(Guid userId, string userAgent, string token)
    {
        var tokenList = memoryCache.TryGetValue<List<(string, string)>>(userId.ToString());
        return tokenList?.Any(t => t.Item1 == userAgent && t.Item2 == token) ?? false;
    }

    public void RegisterToken(Guid userId, string userAgent, string token)
    {
        var key = userId.ToString();
        var tokenList = memoryCache.TryGetValue<List<(string, string)>>(key) ?? [];
        var existingTokenIndex = tokenList.FindIndex(t => t.Item1 == userAgent);

        if (existingTokenIndex != -1)
        {
            tokenList[existingTokenIndex] = (userAgent, token);
        }
        else if (tokenList?.Count >= (maxTokenAllowed < 1 ? 1 : maxTokenAllowed))
        {
            tokenList.RemoveAt(0);
            tokenList.Add((userAgent, token));
        }
        else
        {
            tokenList?.Add((userAgent, token));
        }

        memoryCache.Set(key, tokenList);
    }

    public void RevokeAllTokens(Guid userId) => memoryCache.Remove(userId.ToString());

    public void RevokeToken(Guid userId, string userAgent, string token)
    {
        var key = userId.ToString();
        var tokenList = memoryCache.TryGetValue<List<(string, string)>>(key) ?? [];
        var existingTokenIndex = tokenList.FindIndex(t => t.Item1 == userAgent && t.Item2 == token);

        if (existingTokenIndex == -1) return;
        tokenList.RemoveAt(existingTokenIndex);
        memoryCache.Set(key, tokenList);
    }
}