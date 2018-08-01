using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web365Domain
{
    public class ProductItem : BaseModel
    {
        public string Name { get; set; }
        public string NameAscii { get; set; }
        public string Serial { get; set; }
        public int? TypeID { get; set; }
        public int? Manufacturer { get; set; }
        public int? Distributor { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
        public int? PictureID { get; set; }
        public string HighLights { get; set; }
        public string Summary { get; set; }
        public string Detail { get; set; }
        public int? Number { get; set; }
        public int? Viewed { get; set; }
        public string SEOTitle { get; set; }
        public string SEODescription { get; set; }
        public string SEOKeyword { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string UpdateBy { get; set; }
        public string CreateBy { get; set; }
        public bool? IsShow { get; set; }
        public bool? IsDeleted { get; set; }
        public int[] ListStatusId { get; set; }
        public int[] ListLabelId { get; set; }
        public int[] ListFilterId { get; set; }
        public int[] ListPictureID { get; set; }
        public PictureItem Picture { get; set; }
        public ProductTypeItem ProductType { get; set; }
        public List<ProductVariantItem> ListProductVariant { get; set; }
        public List<PictureItem> ListPicture { get; set; }
    }
}
