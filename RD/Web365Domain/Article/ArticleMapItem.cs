using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web365Domain
{
    public class ArticleMapItem : BaseModel
    {
        public string Title { get; set; }
        public string TitleAscii { get; set; }
        public DateTime? DateCreated { get; set; }        
        public string Summary { get; set; }
        public string PictureURL { get; set; }
        public string TypeName { get; set; }
        public string Detail { get; set; }
        public string TypeNameAscii { get; set; }
        public string TypeParentName { get; set; }
        public string TypeParentNameAscii { get; set; }
        public string LinkReference { get; set; }
        public string RelatedArticles { get; set; }
        public string LinkReferenPicture { get; set; }
    }
}
