using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web365Domain
{
    public class ChargeItem
    {
        public int Stt { get; set; }
        public long uid { get;set; }
        public string username { get; set; }
        public string fullname { get; set; }
        public string type { get; set; }

        public string cardNumber { get; set; }
        public string cardSerial { get; set; }
        public string telco { get; set; }
        public long Price { get; set; }
        public string refNo { get; set; }
        public string tranNo { get; set; }
        public string source { get; set; }

        public DateTime time { get; set; }

        public string timeString { get; set; }
    }
}
