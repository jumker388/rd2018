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
    
    public partial class tblProductType_Group_Map
    {
        public int ID { get; set; }
        public Nullable<int> ProductTypeID { get; set; }
        public Nullable<int> GroupTypeID { get; set; }
        public Nullable<int> DisplayOrder { get; set; }
    
        public virtual tblProductTypeGroup tblProductTypeGroup { get; set; }
        public virtual tblTypeProduct tblTypeProduct { get; set; }
    }
}