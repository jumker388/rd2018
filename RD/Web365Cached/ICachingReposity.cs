using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web365Cached
{
    interface ICachingReposity
    {
        void Delete(string key);
        bool Exists(string key);
        T Get<T>(string key);
        void Set(string key, object data, int cacheTime);
    }
}
