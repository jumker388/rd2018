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
    
    public partial class tblProduct_Status_Map
    {
        public int ID { get; set; }
        public Nullable<int> ProductID { get; set; }
        public Nullable<int> ProductStatusID { get; set; }
        public Nullable<int> DisplayOrder { get; set; }
    
        public virtual tblProduct tblProduct { get; set; }
        public virtual tblProductStatus tblProductStatus { get; set; }
    }
}
