using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Web365.Controllers
{
    public class CacheAttribute : OutputCacheAttribute
    {
        public CacheAttribute(string cacheProfileName)
        {
            Duration = 1;

            VaryByParam = "none";

            if (Web365Utility.ConfigWeb.UseOutputCache)
            {

                var cacheSection = (OutputCacheSettingsSection)WebConfigurationManager.GetSection("system.web/caching/outputCacheSettings");

                var cacheProfile = cacheSection.OutputCacheProfiles[cacheProfileName];

                Duration = cacheProfile.Duration;

                VaryByParam = cacheProfile.VaryByParam;

            }
        }

    }
}