using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web365Utility
{
    public static class StaticEnum
    {
        /// <summary>
        /// Enum about type file when upload of download
        /// </summary>
        public enum FileType
        {
            //type file jpg, jpeg, png, gif...
            Image = 0,
            //thumb type file jpg, jpeg, png, gif...
            ImageThumb = 1,
            //type file doc, docx, xls, xlsx, pdf...
            Document = 2,
            //type file mp3, mp4, avi, mkv...
            Multimedia = 3,
            //type file rar, zip, dll, exe...
            File = 4
        }
    }
}
