using System.Runtime.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web365Cached
{
    public class InitCaching : ICachingReposity
    {
        private ObjectCache Cache { get { return MemoryCache.Default; } }

        public void Set(string key, object data, int cacheTime)
        {
            CacheItemPolicy policy = new CacheItemPolicy();

            policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime);

            if (data != null)
            {
                Cache.Add(new CacheItem(key, data), policy);
            }
                
        }

        public T Get<T>(string key)
        {
            return (T)Cache[key];
        }

        public void Delete(string key)
        {
            try
            {
                Cache.Remove(key);
            }
            catch (Exception)
            {
            }

        }

        public bool Exists(string key)
        {
            try
            {
                return Cache.Get(key) != null;
            }
            catch (Exception)
            {
                return false;
            }            
        }
    }
}
