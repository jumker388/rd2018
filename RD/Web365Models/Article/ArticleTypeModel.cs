using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web365Domain;

namespace Web365Models
{
    public class ArticleTypeModel
    {
        public ArticleTypeItem ArticleTypeRoot { get; set; }
        public List<ArticleTypeItem> ListArticleType { get; set; }
        
    }
}
