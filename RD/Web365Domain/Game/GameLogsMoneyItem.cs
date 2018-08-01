using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web365Domain
{
    public class GameLogsMoneyItem
    {
        public int ID { get; set; }
        public int LogID { get; set; }
        public int? ShopLevel { get; set; }
        public int? ShopLevelSend { get; set; }
        public int? ShopLevelRecive { get; set; }
        public DateTime? DateCreated { get; set; }
        public string DateCreatedString { get; set; }
        public string ContentTransaction { get; set; }
        public string UserSend { get; set; }
        public string UserRecive { get; set; }
        public double? MoneySend { get; set; }
        public double? MoneyRecive { get; set; }
        public double? TaxMoneySystem { get; set; }
        public double? TaxMoneyParent { get; set; }
        public double? TaxMoney { get; set; }
        public double? Tax { get; set; }
        public double? TaxParent { get; set; }
        public double? TaxSystem { get; set; }
        public decimal? SoDuNguoiNhan { get; set; }
        public decimal? KetNguoiNhan { get; set; }
        public string Note { get; set; }
        public int? StatusID { get; set; }
        public DateTime? DateUpdate { get; set; }

    }
}
