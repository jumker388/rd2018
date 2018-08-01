using System.Runtime.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web365Cached
{
    public class InitMemcached : ICachingReposity
    {
        private ObjectCache cache { get { return MemoryCache.Default; } }

        public void Set(string key, object data, int cacheTime)
        {
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime);
            if (data != null)
                cache.Add(new CacheItem(key, data), policy);
        }

        public T Get<T>(string key)
        {
            return (T)cache[key];
        }

        public void Delete(string key)
        {
            try
            {
                cache.Remove(key);
            }
            catch (Exception)
            {
            }

        }

        public bool Exists(string key)
        {
            try
            {
                return cache.Get(key) != null;
            }
            catch (Exception)
            {
                return false;
            }            
        }
    }
}
