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
    
    public partial class tblGroup_Article_Map
    {
        public int ID { get; set; }
        public Nullable<int> NewsID { get; set; }
        public Nullable<int> GroupID { get; set; }
        public Nullable<int> DisplayOrder { get; set; }
    
        public virtual tblArticle tblArticle { get; set; }
        public virtual tblGroupArticle tblGroupArticle { get; set; }
    }
}
