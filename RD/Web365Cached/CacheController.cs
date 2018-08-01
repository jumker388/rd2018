using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web365Utility;

namespace Web365Cached
{
    public class CacheController
    {
        ICachingReposity cache;

        public CacheController()
        {
            cache = new InitCaching();
        }

        public bool TryGetCache<T>(out T obj, string key) where T : new()
        {
            if (ConfigWeb.UseCache && cache.Exists(key))
            {
                obj = cache.Get<T>(key);
                return true;
            }            

            obj = new T();

            return false;
        }

        public void SetCache(string key, object obj, int minutes)
        {
            if (ConfigWeb.UseCache)
            {
                if (cache.Exists(key))
                {
                    cache.Delete(key);
                }

                cache.Set(key, obj, minutes);
            }            
        }
        
    }
}
