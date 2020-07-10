using System;
using System.Runtime.Caching;

namespace CrmDynamics.Library.Extensions
{
    public static class MemoryCacheHelper
    {
        public static T GetValue<T>(string key)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            return (T)memoryCache.Get(key);
        }

        public static bool AddValue(string key, object value, DateTimeOffset absExpiration)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            return memoryCache.Add(key, value, absExpiration);
        }
    }
}
