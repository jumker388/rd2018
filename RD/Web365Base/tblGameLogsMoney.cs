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
    
    public partial class tblGameLogsMoney
    {
        public int ID { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public string UserSend { get; set; }
        public string UserRecive { get; set; }
        public Nullable<double> MoneySend { get; set; }
        public Nullable<double> MoneyRecive { get; set; }
        public Nullable<double> Tax { get; set; }
        public Nullable<double> TaxParent { get; set; }
        public Nullable<double> TaxSystem { get; set; }
        public Nullable<double> TaxMoney { get; set; }
        public Nullable<double> TaxMoneyParent { get; set; }
        public Nullable<double> TaxMoneySystem { get; set; }
        public string ContentTransaction { get; set; }
        public Nullable<bool> IsTruyThu { get; set; }
    }
}