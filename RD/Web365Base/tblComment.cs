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
    
    public partial class tblComment
    {
        public int ID { get; set; }
        public Nullable<int> JoinID { get; set; }
        public Nullable<int> AccountID { get; set; }
        public string Detail { get; set; }
        public Nullable<bool> IsShow { get; set; }
        public Nullable<int> AdminId { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<int> CateJoinID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Link { get; set; }
        public Nullable<int> CommentID { get; set; }
    
        public virtual tblTypeComment tblTypeComment { get; set; }
    }
}
