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
    
    public partial class tblContact
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<bool> IsViewed { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
}