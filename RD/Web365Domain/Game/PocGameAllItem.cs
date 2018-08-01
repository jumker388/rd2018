using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web365Domain
{

    public class FilterItem
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public string Source { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Key { get; set; }
        public int CurrentRecord { get; set; }
        public int numberRecord { get; set; }
        public int currentRecord { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }

    }

    public class GameReportItemNew
    {
        public int ID { get; set; }
        public string DateCreated { get; set; }
        public int? UserAction { get; set; }
        public int? NRU { get; set; }
        public int? ExchangeNumber { get; set; }
        public decimal? CashFlow { get; set; }
        public decimal? Profit { get; set; }
        public int? TotalUser { get; set; }
        public int? NPUShop { get; set; }
        public int? NPUUser { get; set; }
        public int? PU { get; set; }

    }

    public class GameReportItem
    {
        public DateTime DateCreate { get; set; }
        public string DateCreateString { get; set; }
        public string game { get; set; }
        public int? totalUser { get; set; }
        public int? id { get; set; }
        public int? game_id { get; set; }

        public decimal? cashPlay { get; set; }

        public int? tax { get; set; }
        public int? user_id { get; set; }
        
        public DateTime? time { get; set; }
        public int? totalTransaction { get; set; }
        public decimal? totalCash { get; set; }
        public decimal? cash { get; set; }
        public decimal? totalTax { get; set; }

        public int? maxccu { get; set; }
        public int? minccu { get; set; }
        public int? avgccu { get; set; }
        public int? totalAccountRegiter { get; set; }
        public int? totalTrans { get; set; }
        public int? totalCashPlay { get; set; }

    }

    public class ModelCcu
    {
        public List<TimeCcuItem2> Day0 { get; set; }
        public List<TimeCcuItem2> Day1 { get; set; }
        public List<TimeCcuItem2> Day7 { get; set; }

    }
    public class modelreturnCCitem
    {        
        public List<returnCCitem> data { get; set; }
    }

    public class returnCCitem
    {
        public string label { get; set; }
        public List<TimeCcuItem2> data { get; set; }
    }

    public class TimeCcuItem
    {
        public int? id { get; set; }
        public string Time { get; set; }
        public int? Ccu { get; set; }
    }
    public class TimeCcuItem2
    {
        public string Time { get; set; }
        public int? Ccu { get; set; }
    }
    public class ReportCcuItem
    {
        public int? id { get; set; }
        public DateTime DateCreate { get; set; }
        public string Time { get; set; }
        public int? Day1 { get; set; }
        public int? Day0 { get; set; }
        public int? Day7 { get; set; }
    }

    public class SumPocItem
    {
        public int? ID { get; set; }
        public decimal? TongTienUser { get; set; }
        public decimal? TongTienKetSatUser { get; set; }
        public decimal? TongTienKetSat { get; set; }
        public decimal? TongTienKetSatNickTongPoc { get; set; }
        public int? TongUser { get; set; }
        public decimal? TongGiftCodeDaNap { get; set; }
        public decimal? LoiNhuan { get; set; }

        public decimal? TongTienNickTongPoc { get; set; }
        public decimal? TongHu { get; set; }
        public decimal? TongGameBank { get; set; }
        public decimal? TongTienDaiLyCap1 { get; set; }
        public decimal? TongTienKetSatDaiLyCap1 { get; set; }
        public decimal? TongTienDaiLyCap2 { get; set; }
        public decimal? TongTienKetSatDaiLyCap2 { get; set; }
        public decimal? TongTienGiftCodeChuaNap { get; set; }
        public DateTime? DateCreated { get; set; }
        public string DateCreatedString { get; set; }

    }
}
