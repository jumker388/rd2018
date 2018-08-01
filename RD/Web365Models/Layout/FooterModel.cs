using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web365Domain;

namespace Web365Models
{
    public class FooterModel
    {
        public LayoutContentItem Social { get; set; }
        public LayoutContentItem Content { get; set; }
        public List<MenuItem> ListMenu { get; set; }

        public ArticleTypeItem TypeDSCT;
        public ArticleItem FooterInfo;
        public ArticleItem FooterHotline;
        
    }
}
