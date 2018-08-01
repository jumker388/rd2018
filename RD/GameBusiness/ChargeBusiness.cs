using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web365Domain;

namespace GameBusiness
{
    public class ChargeBusiness
    {
        /// <summary>
        /// Danh sách user nạp thẻ, sms
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public ChargeResult GetCharged(DateTime from, DateTime to, int skip, int take)
        {
            var rs = new ChargeResult();
            var data = new List<ChargeItem>();
            int totalRecord = 0;
            string sql = "SELECT b.id AS 'user_id', b.username,b.fullname,'card' as 'type', cardNumber, cardSerial, cp,cardPrice, refNo, tranNo, source, dateCreated FROM portal.a_paycard a inner join portal.user b on a.username=b.id where a.cardPrice > 0 and a.dateCreated >= @date1 and dateCreated <= @date2 union all SELECT b.id AS 'user_id', b.username, b.fullname, 'sms' AS 'type', '' as 'cardNumber','' as 'cardSerial', telco as 'cp',amount as 'cardPrice', '' as 'refNo', '' as 'tranNo', '' as 'source' , responeTime as 'dateCreated' FROM portal.a_smsplus a INNER JOIN portal.user b ON a.targetUser=b.username WHERE amount > 0 AND a.responeTime >= @date3 AND a.responeTime <= @date4 ORDER BY dateCreated DESC LIMIT @skip, @take";
            MySqlConnection conn = new MySqlConnection(Constants.DBConnection);
            MySqlCommand MyCommand = new MySqlCommand(sql, conn);
            MyCommand.Parameters.AddWithValue("date1", from.ToString(Constants.DateFormat));
            MyCommand.Parameters.AddWithValue("date2", to.ToString(Constants.DateFormat));
            MyCommand.Parameters.AddWithValue("date3", from.ToString(Constants.DateFormat));
            MyCommand.Parameters.AddWithValue("date4", to.ToString(Constants.DateFormat));
            MyCommand.Parameters.AddWithValue("skip", skip);
            MyCommand.Parameters.AddWithValue("take", take);
            MySqlDataReader MyReader;
            conn.Open();
            MyReader = MyCommand.ExecuteReader();
            int stt = 1;
            while (MyReader.Read())
            {
                ChargeItem item1 = new ChargeItem();
                item1.Stt = stt;
                item1.uid = MyReader.GetInt64("user_id");
                item1.username = MyReader.GetString("username");
                item1.fullname = MyReader.GetString("fullname");
                item1.type = MyReader.GetString("type");
                item1.cardNumber = MyReader.GetString("cardNumber");
                item1.cardSerial = MyReader.GetString("cardSerial");
                item1.telco = MyReader.GetString("cp");
                item1.Price = MyReader.GetInt64("cardPrice");
                item1.refNo = MyReader.GetString("refNo");
                item1.tranNo = MyReader.GetString("tranNo");
                item1.source = MyReader.GetString("source");
                item1.time = MyReader.GetDateTime("dateCreated");
                item1.timeString = item1.time.ToString("dd-MM-yyyy");

                data.Add(item1);
                stt++;
            }
            MyReader.Close();
            sql = "Select count(*) as totalRecord from portal.a_paycard where cardPrice > 0 and dateCreated >= @date1 and dateCreated <= @date2";
            MyCommand = new MySqlCommand(sql, conn);
            MyCommand.Parameters.AddWithValue("date1", from.ToString(Constants.DateFormat));
            MyCommand.Parameters.AddWithValue("date2", to.ToString(Constants.DateFormat));
            MyReader = MyCommand.ExecuteReader();
            while (MyReader.Read())
            {
                totalRecord += MyReader.GetInt32("totalRecord");
            }
            MyReader.Close();
            sql = "Select count(*) as totalRecord from portal.a_smsplus WHERE amount > 0 AND responeTime >= @date3 AND responeTime <= @date4";
            MyCommand = new MySqlCommand(sql, conn);
            MyCommand.Parameters.AddWithValue("date3", from.ToString(Constants.DateFormat));
            MyCommand.Parameters.AddWithValue("date4", to.ToString(Constants.DateFormat));
            MyReader = MyCommand.ExecuteReader();
            while (MyReader.Read())
            {
                totalRecord += MyReader.GetInt32("totalRecord");
            }
            MyReader.Close();
            conn.Close();
            rs.data = data;
            rs.totalRecord = totalRecord;
            return rs;
        }
    }
}
