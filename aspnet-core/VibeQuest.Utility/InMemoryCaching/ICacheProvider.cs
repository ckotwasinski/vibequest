using Microsoft.Extensions.Caching.Memory;
using VibeQuest.Utility.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Utility.InMemoryCaching
{
    public interface ICacheProvider : IScopedDependency
    {
        T GetFromCache<T>(string key) where T : class;
        void SetCache<T>(string key, T value) where T : class;
        void SetCache<T>(string key, T value, DateTimeOffset duration) where T : class;
        void SetCache<T>(string key, T value, MemoryCacheEntryOptions options) where T : class;
        void ClearCache(string key);
    }
}
