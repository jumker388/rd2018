//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Web365Base
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblOrder_Status
    {
        public tblOrder_Status()
        {
            this.tblOrder = new HashSet<tblOrder>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<tblOrder> tblOrder { get; set; }
    }
}
