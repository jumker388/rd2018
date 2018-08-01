using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web365Base;
using Web365Utility;
using Web365Cached;
using System.Web;

namespace Web365Business.Front_End
{
    public class BaseFE : CacheController
    {
        protected Entities365 web365db = new Entities365();
        public Entities365 Web365DB
        {
            get
            {
                return web365db;
            }
        }

        public int LanguageId
        {
            get
            {
                return HttpContext.Current.Request.Url.Segments.Length > 1 && (HttpContext.Current.Request.Url.Segments[1] == "en" || HttpContext.Current.Request.Url.Segments[1] == "en/") ? 2 : 1;
            }
        } 
    }
}
