using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web365Domain
{
    public class ReportOnlineItem : BaseModel
    {
        public string CcuNow { get; set; }
        public string CcuMaxToday { get; set; }
        public string CcuMaxyesterday { get; set; }

        public string RevenueToday { get; set; }
        public string RevenueWeek { get; set; }
        public string RevenueMonth { get; set; }
        public string RevenueLastMonth { get; set; }

        public string TcoiTon { get; set; }
        public string TcoiPhe { get; set; }
        public string TcoiChanged { get; set; }

        public int TotalNguoiChoiTraPhi { get; set; }

        public int TotalNguoiChoi { get; set; }

        public int TotalNguoiChoiTraPhiHomNay { get; set; }

        public int TongDoanhThu { get; set; }

        public int TotalYcDoiThuongHomNay { get; set; }

        public string AvgAll { get; set; }
        public string AvgTraPhi { get; set; }

        public List<GameTaxItem> GameTaxList { get; set; }
    }

    public class GameTaxItem
    {
        public string name { get; set; }
        public int game_id { get; set; }
        public string tax { get; set; }
        public decimal taxMonney { get; set; }

        public string cash { get; set; }
        public decimal cashMonney { get; set; }
    }

    public class GameCashItem
    {
        public string name { get; set; }
        public int game_id { get; set; }

        public string cash4h { get; set; }
        public decimal cashMonney4h { get; set; }

        public string cash8h { get; set; }
        public decimal cashMonney8h { get; set; }

        public string cash12h { get; set; }
        public decimal cashMonney12h { get; set; }

        public string cash16h { get; set; }
        public decimal cashMonney16h { get; set; }

        public string cash20h { get; set; }
        public decimal cashMonney20h { get; set; }

        public string cash24h { get; set; }
        public decimal cashMonney24h { get; set; }

        public string cashAll { get; set; }
        public decimal cashMonneyAll { get; set; }
    }
}
