using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web365Domain
{
    public class AnnouncementItem
    {
        public int ID { get; set; }

        public string Subject { get; set; }
        public string DoiTuong { get; set; }  
        public string ThoiGian { get; set; }
        public int GameID { get; set; }
        public int DisplayOrder { get; set; }
        

        public string Content { get; set; }

        public DateTime begin_time { get; set; }
        public DateTime end_time { get; set; }

        public string UrlImage { get; set; }
        public string begin_timestring { get; set; }
        public string end_timestring { get; set; }
    }

    public class RuleEventItem
    {
        public string gamename { get; set; }
        public int ID { get; set; }

        public int? GameID { get; set; }

        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public string DateStartString { get; set; }
        public string DateEndString { get; set; }
        public bool? IsShow { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? DateCreated { get; set; }

        public int? CountHu { get; set; }
        public int? Nhan { get; set; }
        public int? LevelRoom { get; set; }
        public int? MaxHu { get; set; }
        public decimal? MaxCashHu { get; set; }
    }

}
