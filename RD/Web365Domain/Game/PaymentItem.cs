using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web365Domain.Game
{
    public class PaymentItem
    {
        public int id { get; set; }
        public int buyer_uid { get; set; }
        public string buyer_fullname { get; set; }
        public string username { get; set; }
        public string fullname { get; set; }
        public string buyer_mobile { get; set; }
        public int total_amount { get; set; }
        public string order_code { get; set; }
        public string payment_method { get; set; }
        public string bank_code { get; set; }
        public string transaction_id { get; set; }
        public string payment_type { get; set; }
        public string transaction_status { get; set; }
        public string token { get; set; }

        public string time_request_string { get; set; }
        public string time_receive_string { get; set; }
        public DateTime time_request { get; set; }
        public DateTime time_receive { get; set; }
    }
}
