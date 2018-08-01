using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web365Domain
{
    public class ChuyenKhoanItem
    {
        public string UserSend { get; set; }
        public string UserRecive { get; set; }
        public DateTime DateCreated { get; set; }
        public float MoneySend { get; set; }
        public float MoneyRecive { get; set; }
        public float Tax { get; set; }
        public float TaxParent { get; set; }
        public float TaxSystem { get; set; }
        public float TaxMoneySystem { get; set; }
        public float TaxMoneyParent { get; set; }
        public float TaxMoney { get; set; }
        public string Note { get; set; }
        public int StatusID { get; set; }
        public int ID { get; set; }
        public int LogID { get; set; }
    }
}
