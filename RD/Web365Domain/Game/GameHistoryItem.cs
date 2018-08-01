using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web365Domain
{
    public class GameHistoryItem
    {
        public int? id { get; set; }
        public int? user_id { get; set; }
        public decimal? cash { get; set; }
        public decimal? current_cash { get; set; }
        public string description { get; set; }
        public int? game_id { get; set; }
        public int? matchId { get; set; }


        public string sourceDevice { get; set; }

        public int? matchNum { get; set; }
        public int? betTaiXiu { get; set; }
        public int? valueCard { get; set; }
        public decimal? cashPlay { get; set; }
        public decimal? CashFinal { get; set; }
        
        public int? taxPercent { get; set; }
        public int? tax { get; set; }
        
        public int? trans_type { get; set; }
        public int? ShopLevel { get; set; }
        public DateTime? time { get; set; }
        public decimal? before_cash { get; set; }
        public string username { get; set; }
        public string timestring { get; set; }
        public string fullname { get; set; }
        public string gamename { get; set; }

        public string gamedetail { get; set; }
    }

    public class TaiXiuGameHistoryItem
    {
        public int? id { get; set; }
        public int? user_id { get; set; }
        public decimal? cash { get; set; }
        public decimal? current_cash { get; set; }
        public string description { get; set; }
        public int? game_id { get; set; }
        public int? matchId { get; set; }


        public string sourceDevice { get; set; }

        public int? matchNum { get; set; }
        public decimal? betTaiXiu { get; set; }
        public decimal? TienHoan { get; set; }
        public int? CuaDat { get; set; }
        public decimal? TienThang { get; set; }
        public int? valueCard { get; set; }
        public decimal? cashPlay { get; set; }
        public decimal? CashFinal { get; set; }

        public int? taxPercent { get; set; }
        public int? tax { get; set; }

        public int? trans_type { get; set; }
        public DateTime? time { get; set; }
        public decimal? before_cash { get; set; }
        public string username { get; set; }
        public string timestring { get; set; }
        public string fullname { get; set; }
        public string gamename { get; set; }

        public string gamedetail { get; set; }
    }

    public class HopThuItem
    {
        public int id { get; set; }
        public int? userIDSend { get; set; }
        public int? userIDReceive { get; set; }
        public string mes { get; set; }
        public DateTime datetimeSend { get; set; }
        public string datetimeSendString { get; set; }
        public string SendName { get; set; }
        public string ReceiveName { get; set; }

    }

}
