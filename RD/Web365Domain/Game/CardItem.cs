using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web365Domain
{
    public class CardItem
    {
        public int id { get; set; }
        public string serial { get; set; }
        public string cardNo { get; set; }
        public DateTime dateExpired { get; set; }
        public DateTime dateInput { get; set; }
        public DateTime? dateUse { get; set; }

        public string dateExpiredString { get; set; }
        public string dateInputString { get; set; }
        public string dateUseString { get; set; }
        public Byte used { get; set; }
        public int value { get; set; }
        public int telcoId { get; set; }
    }

    public class DaiLyItem
    {
        public int id { get; set; }
        public string username { get; set; }
        public string parent { get; set; }
        public int ShopLevel { get; set; }

    }

    public class DaiLyShowItem
    {
        public int ID { get; set; }
        public string DiaChi { get; set; }
        public string FB { get; set; }
        public string NickName { get; set; }
        public string TenDaiLy { get; set; }
        public string Phone { get; set; }
        public bool? IsDelete { get; set; }
        public bool? IsShow { get; set; }

    }


    public class GameBankItem
    {
        public decimal Hu1 { get; set; }
        public decimal Hu2 { get; set; }
        public decimal Hu3 { get; set; }
        public decimal Hu4 { get; set; }
        public decimal Hu5 { get; set; }
        public decimal Cash_Public1 { get; set; }
        public decimal Cash_Public2 { get; set; }
        public decimal Cash_Public3 { get; set; }
        public decimal Cash_Public4 { get; set; }
        public decimal Cash_Public5 { get; set; }
        public string Game { get; set; }


    }

}
