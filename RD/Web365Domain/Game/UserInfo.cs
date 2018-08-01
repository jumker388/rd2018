using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web365Domain
{
    public class UserInfo
    {
        public long uid { get; set; }
        public long id { get; set; }
        public string userName { get; set; }
        public string mobile { get; set; }
        public string device { get; set; }
        public DateTime? register_date { get; set; }
        public string register_date_string { get; set; }
        public string last_login_string { get; set; }
        public decimal? gameCash { get; set; }
        public decimal? cashPlay { get; set; }
        public DateTime? last_login { get; set; }
        public bool? is_block { get; set; }
        public bool? IsChat { get; set; }


        public bool? sex { get; set; }
        public string passWord { get; set; }
        public string ip { get; set; }
        public string fullName { get; set; }
        public string dateRegister { get; set; }

        public bool isMale { get; set; }
        public int level { get; set; }
        public int levelName { get; set; }
        public long? cash { get; set; }
        public long vcash { get; set; }
        public int playsNumber { get; set; }
        public int typyPlay { get; set; }
        public int playsWin { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string cmnd { get; set; }
        public string ipAddress { get; set; }
        public int isMobile { get; set; }

        public int is_active { get; set; }
        public string lastLogin { get; set; }
        public string description { get; set; }

        public long cashWin { get; set; }

    }
    public class OtpItem
    {
        public string Otp_Plus { get; set; }
        public string username { get; set; }
        public string temp { get; set; }
        public string smsTemp { get; set; }
        public int ID { get; set; }
        public int? User_ID { get; set; }
        public int Type { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class UserInfo2
    {
        public string device { get; set; }
        public decimal? cashPlay { get; set; }
        public decimal? Ket { get; set; }
        public int id { get; set; }
        public int? totalPlay { get; set; }
        public string username { get; set; }
        public int? sex { get; set; }
        public string fullname { get; set; }
        public string ip { get; set; }
        public string cmnd { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public DateTime? register_date { get; set; }
        public string register_date_string { get; set; }
        public string last_login_string { get; set; }
        public int? is_active { get; set; }
        public decimal? gameCash { get; set; }
        public decimal? cash { get; set; }
        public DateTime? last_login { get; set; }
        public int? is_block { get; set; }
        public bool? IsChat { get; set; }

    }
    public class GameSlotCheckItem
    {
        public int? id { get; set; }
        public int? game_id { get; set; }
        public int? host_id { get; set; }
        public decimal? cash { get; set; }
        public string room_name { get; set; }
        public int? slotMachineTotalLine { get; set; }
        public string slotMachineWinLine { get; set; }
        public int? slotMachineWinCash { get; set; }
        public string slotMachineNumber { get; set; }
        public string miniPokerCards { get; set; }
        public string miniPokerResult { get; set; }
        public string miniPokerString { get; set; }
        public decimal? miniPokerWinCash { get; set; }
        public DateTime? time { get; set; }
    }

    public class RetetionItem
    {
        public int? Nru { get; set; }
        public int? Pu { get; set; }
    }

    public class UserInfoNew
    {
        public int user_id { get; set; }
        public string username { get; set; }
        public double gameCash { get; set; }
        public string gameCashFormat { get; set; }
        public string parent { get; set; }
        public string password { get; set; }
        public int? ShopLevel { get; set; }
        public int? UserId { get; set; }
        public decimal? Cash { get; set; }
    }

    public class UserInfoNew2
    {
        public long indexRow { get; set; }
        public int user_id { get; set; }
        //public int id { get; set; }
        public string username { get; set; }
        public string fullname { get; set; }
        public string Note { get; set; }
        public double gameCash { get; set; }
        public decimal? gameCashN { get; set; }
        public decimal? CashKet { get; set; }
        public decimal gameCashD { get; set; }
        public string gameCashFormat { get; set; }
        public string parent { get; set; }
        public string password { get; set; }
        public int? ShopLevel { get; set; }
        public decimal? totalTax { get; set; }
        public double? totalSendToUser { get; set; }
        public double? totalRecivefromUser { get; set; }
        public int? UserId { get; set; }

    }
    public class UserInfoNew3
    {
        public long indexRow { get; set; }
        public int user_id { get; set; }
        public string username { get; set; }
        public string fullname { get; set; }
        public string Note { get; set; }
        public double gameCash { get; set; }
        public decimal gameCashD { get; set; }
        public decimal? gameCashN { get; set; }
        public decimal? CashKet { get; set; }
        public string gameCashFormat { get; set; }
        public string parent { get; set; }
        public string password { get; set; }
        public int? ShopLevel { get; set; }
        public decimal? totalTax { get; set; }
        public decimal? totalSendToUser { get; set; }
        public decimal? total { get; set; }
        public decimal? totalRecivefromUser { get; set; }

        public decimal? totalTaxCap1 { get; set; }
        public decimal? totalSendToUserCap1 { get; set; }
        public decimal? totalCap1 { get; set; }
        public decimal? totalRecivefromUserCap1 { get; set; }

        public int? UserId { get; set; }

    }
}
