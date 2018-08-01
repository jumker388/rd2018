using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web365Domain
{

    public class GiftTurnItem
    {
        public int ID { get; set; }
        public int Quantity { get; set; }
        public string Prefix { get; set; }
        public string DateExpiredString { get; set; }
        public DateTime? DateExpired { get; set; }
        public DateTime? DateCreated { get; set; }
        public string DateCreatedString { get; set; }
        public bool? IsShow { get; set; }
        public bool? IsDelete { get; set; }
        public int TypeID { get; set; }
        public decimal? Money { get; set; }
        public string Note { get; set; }
        public string TypeName { get; set; }
        public string Name { get; set; }
        public string NoteFix { get; set; }

    }

    public class GiftCodeTypeItem
    {
        public int ID { get; set; }
        public string name { get; set; }
        public DateTime? DateCreated { get; set; }
        public string DateCreatedString { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public string DateStartString { get; set; }
        public string DateEndString { get; set; }
        public bool? IsShow { get; set; }
        public bool? IsDelete { get; set; }
        public decimal? Money { get; set; }

    }

    public class GiftCodeItem2
    {
        public int ID { get; set; }
        public string username { get; set; }
        public string fullname { get; set; }
        public string Code { get; set; }
        public decimal Values { get; set; }
        public string TypeName { get; set; }
        public DateTime? DateUsed { get; set; }
        public string DateUsedString { get; set; }
        public DateTime? DateCreated { get; set; }
        public string DateCreatedString { get; set; }
        public string DateExpiredString { get; set; }
        public DateTime? DateExpired { get; set; }
        public int? user_id { get; set; }
        public int? TurnID { get; set; }
        public bool? IsShowTurn { get; set; }
        public decimal? Money { get; set; }
    }

    public class CardOptionItem
    {
        public int id { get; set; }
        public int? telcoid { get; set; }
        public int? isShow { get; set; }
        public bool? IsShowd { get; set; }
        public int? isDeleted { get; set; }
        public int? DisplayOrder { get; set; }
        public string name { get; set; }
        public decimal? value { get; set; }
        public decimal? rate { get; set; }
    }

    public class CardTelcoItem
    {
        public int ID { get; set; }
        public int? UserID { get; set; }
        public int? MenhGia { get; set; }
        public int? MenhGiaResponse { get; set; }
        public int? StatusID { get; set; }
        public int? TelcoID { get; set; }
        public string Code { get; set; }
        public string Seria { get; set; }
        public string IDNPH { get; set; }
        public bool? IsUsed { get; set; }
        public DateTime? DateCreated { get; set; }
        public string DateCreatedString { get; set; }
        public string DateUsedString { get; set; }
        public string username { get; set; }
        public string fullname { get; set; }
        public DateTime? DateUsed { get; set; }
    }


    public class GiftCodeItem
    {
        public int ID { get; set; }
        public string code { get; set; }
        public int value { get; set; }
        public string name { get; set; }
        public DateTime dateCreated { get; set; }
        public DateTime dateExpired { get; set; }
        public string sdateCreated { get; set; }
        public string sdateExpired { get; set; }
        public bool used { get; set; }
        public int user_id { get; set; }
        public int isVCash { get; set; }
        public string username { get; set; }
        public string fullname { get; set; }
    }
}
