using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web365Domain;

namespace Web365Models
{
    public class ListArticleModel
    {
        public ArticleTypeItem Type { get; set; }
        public ArticleGroupItem Group { get; set; }
        public List<ArticleItem> List { get; set; }        
        public int total { get; set; }
    }
}
