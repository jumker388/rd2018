using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web365Domain
{
    public class ArticleItem : BaseModel
    {
        public int? TypeID { get; set; }
        public string Title { get; set; }
        public string TitleAscii { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }
        public string SEOTitle { get; set; }
        public string SEODescription { get; set; }
        public string SEOKeyword { get; set; }
        public int? PictureID { get; set; }
        public int? Viewed { get; set; }
        public string Summary { get; set; }
        public string Detail { get; set; }
        public string Tags { get; set; }
        public string LinkReference { get; set; }
        public string RelatedArticles { get; set; }
        public string LinkReferenPicture { get; set; }
        public int? Number { get; set; }
        public bool? IsShow { get; set; }
        public bool? IsDeleted { get; set; }
        public int[] ListGroupID { get; set; }
        public PictureItem Picture { get; set; }
        public ArticleTypeItem ArticleType { get; set; }
        public ArticleGroupItem ArticleGroup { get; set; }
    }
}
