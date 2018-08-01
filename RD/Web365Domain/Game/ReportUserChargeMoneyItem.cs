using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web365Domain
{
    public class ReportUserChargeMoneyItem
    {
        public int user_id { get; set; }
        public string username { get; set; }
        public string fullname { get; set; }
        public string ip { get; set; }        
        public long gameCash { get; set; }
        public long vCash { get; set; }

        public long moenyCharged { get; set; }
    }
}
