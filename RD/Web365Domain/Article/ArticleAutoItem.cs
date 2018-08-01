using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web365Domain
{
    public class ArticleAutoItem : BaseModel
    {
        public string Name { get; set; }
        public string Link { get; set; }
        public int? PictureID { get; set; }
        public int? Parent { get; set; }
        public bool? IsShow { get; set; }
        public bool? IsDeleted { get; set; }
        public int? Number { get; set; }
        public string Xpath { get; set; }
        public string XpathTitle { get; set; }
        public string XpathSummary { get; set; }
        public string ListGroupArticle { get; set; }
        public string XpathDetail { get; set; }
        public string XpathImage { get; set; }
        public int? TypeID { get; set; }
        public string Xpage { get; set; }
        public string XpathPaging { get; set; }
    }
}
