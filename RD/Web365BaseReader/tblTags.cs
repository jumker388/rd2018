//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Web365BaseReader
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblTags
    {
        public tblTags()
        {
            this.tblTags_Mapping_News = new HashSet<tblTags_Mapping_News>();
        }
    
        public int ID { get; set; }
        public string Title { get; set; }
        public string TitleAcsii { get; set; }
    
        public virtual ICollection<tblTags_Mapping_News> tblTags_Mapping_News { get; set; }
    }
}
