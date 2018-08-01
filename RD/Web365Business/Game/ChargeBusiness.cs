using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web365Domain;

namespace Web365Business.Game
{
    public class ChargeBusiness
    {

        public ChargeResult GetAll(string type = "", string username = "", int currentRecord = 0, int numberRecord = 10)
        {
            var rs = new ChargeResult();
            var data = new List<ChargeItem>();
            var totalRecord = 0;
            var sql = "";
            if (string.IsNullOrEmpty(type) || type == "card")
            {
                sql += "SELECT b.id AS 'user_id', b.username,b.fullname,'card' as 'type', cardNumber, cardSerial, cp,cardPrice, refNo, tranNo, source, dateCreated FROM [portal].[dbo].a_paycard a inner join [portal].[dbo].user b on a.username=b.id where a.cardPrice > 0";
                if (!string.IsNullOrEmpty(username))
                {
                    sql += " and b.username like '%" + username + "%' ";
                }
            }

            if (string.IsNullOrEmpty(type))
            {
                sql += " union all ";
            }

            if (string.IsNullOrEmpty(type) || type == "sms")
            {
                sql += "SELECT b.id AS 'user_id', b.username, b.fullname, 'sms' AS 'type', '' as 'cardNumber','' as 'cardSerial', telco as 'cp',amount as 'cardPrice', '' as 'refNo', '' as 'tranNo', '' as 'source' , responeTime as 'dateCreated' FROM [portal].[dbo].a_smsplus a INNER JOIN [portal].[dbo].user b ON a.targetUser=b.username WHERE amount > 0";
                if (!string.IsNullOrEmpty(username))
                {
                    sql += " and b.username like '%" + username + "%' ";
                }
            }

            sql += " ORDER BY dateCreated DESC OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY";

            var conn = new SqlConnection(Constants.DBConnection);
            var myCommand = new SqlCommand(sql, conn);
            myCommand.Parameters.AddWithValue("skip", currentRecord);
            myCommand.Parameters.AddWithValue("take", numberRecord);
            conn.Open();
            var MyReader = myCommand.ExecuteReader();
            var stt = 1;
            while (MyReader.Read())
            {
                var item1 = new ChargeItem
                {
                    Stt = stt,
                    uid = MyReader.GetInt64(MyReader.GetOrdinal("user_id")),
                    username = MyReader.GetString(MyReader.GetOrdinal("username")),
                    fullname = MyReader.GetString(MyReader.GetOrdinal("fullname")),
                    type = MyReader.GetString(MyReader.GetOrdinal("type")),
                    cardNumber = MyReader.GetString(MyReader.GetOrdinal("cardNumber")),
                    cardSerial = MyReader.GetString(MyReader.GetOrdinal("cardSerial")),
                    telco = MyReader.GetString(MyReader.GetOrdinal("cp")),
                    Price = MyReader.GetInt64(MyReader.GetOrdinal("cardPrice")),
                    refNo = MyReader.GetString(MyReader.GetOrdinal("refNo")),
                    tranNo = MyReader.GetString(MyReader.GetOrdinal("tranNo")),
                    source = MyReader.GetString(MyReader.GetOrdinal("source")),
                    time = MyReader.GetDateTime(MyReader.GetOrdinal("dateCreated"))
                };
                item1.timeString = item1.time.ToString("dd-MM-yyyy");

                data.Add(item1);
                stt++;
            }
            MyReader.Close();
            sql = "Select count(*) as totalRecord from [portal].[dbo].a_paycard where cardPrice > 0";
            if (!String.IsNullOrEmpty(username))
            {
                sql += " and username like '%" + username + "%' ";
            }
            myCommand = new SqlCommand(sql, conn);
            MyReader = myCommand.ExecuteReader();
            while (MyReader.Read())
            {
                totalRecord += MyReader.GetInt32(MyReader.GetOrdinal("totalRecord"));
            }
            MyReader.Close();
            sql = "Select count(*) as totalRecord from [portal].[dbo].a_smsplus WHERE amount > 0";
            if (!String.IsNullOrEmpty(username))
            {
                sql += " and targetUser like '%" + username + "%' ";
            }
            myCommand = new SqlCommand(sql, conn);
            MyReader = myCommand.ExecuteReader();
            while (MyReader.Read())
            {
                totalRecord += MyReader.GetInt32(MyReader.GetOrdinal("totalRecord"));
            }
            MyReader.Close();
            conn.Close();
            rs.data = data;
            rs.totalRecord = totalRecord;
            return rs;
        }

        public ChargeResult ExportExcel(DateTime? from, DateTime? to, string type = "")
        {
            var rs = new ChargeResult();
            var data = new List<ChargeItem>();
            var sql = "";
            if (string.IsNullOrEmpty(type) || type == "card")
            {
                sql += "SELECT b.id AS 'user_id', b.username,b.fullname,'card' as 'type', cardNumber, cardSerial, cp,cardPrice, refNo, tranNo, source, dateCreated FROM [portal].[dbo].a_paycard a inner join [portal].[dbo].user b on a.username=b.id where a.cardPrice > 0";
                if (from != null && to != null)
                {
                    sql += " and a.dateCreated >= '" + from.Value.ToString("yyyy-MM-dd HH:mm:ss") + "' and a.dateCreated <= '" + to.Value.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                }
            }
            

            if (string.IsNullOrEmpty(type))
            {
                sql += " union all ";
            }

            if (string.IsNullOrEmpty(type) || type == "sms")
            {
                sql += "SELECT b.id AS 'user_id', b.username, b.fullname, 'sms' AS 'type', '' as 'cardNumber','' as 'cardSerial', telco as 'cp',amount as 'cardPrice', '' as 'refNo', '' as 'tranNo', '' as 'source' , responeTime as 'dateCreated' FROM [portal].[dbo].a_smsplus a INNER JOIN [portal].[dbo].user b ON a.targetUser=b.username WHERE amount > 0";
                if (from != null && to != null)
                {
                    sql += " and responeTime >= '" + from.Value.ToString("yyyy-MM-dd HH:mm:ss") + "' and responeTime <= '" + to.Value.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                }
            }
            

            sql += " ORDER BY dateCreated DESC";

            var conn = new SqlConnection(Constants.DBConnection);
            var myCommand = new SqlCommand(sql, conn);
            conn.Open();
            var MyReader = myCommand.ExecuteReader();
            var stt = 1;
            while (MyReader.Read())
            {
                var item1 = new ChargeItem
                {
                    Stt = stt,
                    uid = MyReader.GetInt64(MyReader.GetOrdinal("user_id")),
                    username = MyReader.GetString(MyReader.GetOrdinal("username")),
                    fullname = MyReader.GetString(MyReader.GetOrdinal("fullname")),
                    type = MyReader.GetString(MyReader.GetOrdinal("type")),
                    cardNumber = MyReader.GetString(MyReader.GetOrdinal("cardNumber")),
                    cardSerial = MyReader.GetString(MyReader.GetOrdinal("cardSerial")),
                    telco = MyReader.GetString(MyReader.GetOrdinal("cp")),
                    Price = MyReader.GetInt64(MyReader.GetOrdinal("cardPrice")),
                    refNo = MyReader.GetString(MyReader.GetOrdinal("refNo")),
                    tranNo = MyReader.GetString(MyReader.GetOrdinal("tranNo")),
                    source = MyReader.GetString(MyReader.GetOrdinal("source")),
                    time = MyReader.GetDateTime(MyReader.GetOrdinal("dateCreated"))
                };
                item1.timeString = item1.time.ToString("dd-MM-yyyy");

                data.Add(item1);
                stt++;
            }
            MyReader.Close();

            rs.data = data;
            return rs;
        }

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
            string sql = "SELECT b.id AS 'user_id', b.username,b.fullname,'card' as 'type', cardNumber, cardSerial, cp,cardPrice, refNo, tranNo, source, dateCreated FROM [portal].[dbo].a_paycard a inner join [portal].[dbo].user b on a.username=b.id where a.cardPrice > 0 and a.dateCreated >= @date1 and dateCreated <= @date2 union all SELECT b.id AS 'user_id', b.username, b.fullname, 'sms' AS 'type', '' as 'cardNumber','' as 'cardSerial', telco as 'cp',amount as 'cardPrice', '' as 'refNo', '' as 'tranNo', '' as 'source' , responeTime as 'dateCreated' FROM [portal].[dbo].a_smsplus a INNER JOIN [portal].[dbo].user b ON a.targetUser=b.username WHERE amount > 0 AND a.responeTime >= @date3 AND a.responeTime <= @date4 ORDER BY dateCreated DESC OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY";
            SqlConnection conn = new SqlConnection(Constants.DBConnection);
            SqlCommand MyCommand = new SqlCommand(sql, conn);
            MyCommand.Parameters.AddWithValue("date1", from.ToString(Constants.DateFormat));
            MyCommand.Parameters.AddWithValue("date2", to.ToString(Constants.DateFormat));
            MyCommand.Parameters.AddWithValue("date3", from.ToString(Constants.DateFormat));
            MyCommand.Parameters.AddWithValue("date4", to.ToString(Constants.DateFormat));
            MyCommand.Parameters.AddWithValue("skip", skip);
            MyCommand.Parameters.AddWithValue("take", take);
            SqlDataReader MyReader;
            conn.Open();
            MyReader = MyCommand.ExecuteReader();
            int stt = 1;
            while (MyReader.Read())
            {
                ChargeItem item1 = new ChargeItem();
                item1.Stt = stt;
                item1.uid = MyReader.GetInt64(MyReader.GetOrdinal("user_id"));
                item1.username = MyReader.GetString(MyReader.GetOrdinal("username"));
                item1.fullname = MyReader.GetString(MyReader.GetOrdinal("fullname"));
                item1.type = MyReader.GetString(MyReader.GetOrdinal("type"));
                item1.cardNumber = MyReader.GetString(MyReader.GetOrdinal("cardNumber"));
                item1.cardSerial = MyReader.GetString(MyReader.GetOrdinal("cardSerial"));
                item1.telco = MyReader.GetString(MyReader.GetOrdinal("cp"));
                item1.Price = MyReader.GetInt64(MyReader.GetOrdinal("cardPrice"));
                item1.refNo = MyReader.GetString(MyReader.GetOrdinal("refNo"));
                item1.tranNo = MyReader.GetString(MyReader.GetOrdinal("tranNo"));
                item1.source = MyReader.GetString(MyReader.GetOrdinal("source"));
                item1.time = MyReader.GetDateTime(MyReader.GetOrdinal("dateCreated"));
                item1.timeString = item1.time.ToString("dd-MM-yyyy");

                data.Add(item1);
                stt++;
            }
            MyReader.Close();
            sql = "Select count(*) as totalRecord from [portal].[dbo].a_paycard where cardPrice > 0 and dateCreated >= @date1 and dateCreated <= @date2";
            MyCommand = new SqlCommand(sql, conn);
            MyCommand.Parameters.AddWithValue("date1", from.ToString(Constants.DateFormat));
            MyCommand.Parameters.AddWithValue("date2", to.ToString(Constants.DateFormat));
            MyReader = MyCommand.ExecuteReader();
            while (MyReader.Read())
            {
                totalRecord += MyReader.GetInt32(MyReader.GetOrdinal("totalRecord"));
            }
            MyReader.Close();
            sql = "Select count(*) as totalRecord from [portal].[dbo].a_smsplus WHERE amount > 0 AND responeTime >= @date3 AND responeTime <= @date4";
            MyCommand = new SqlCommand(sql, conn);
            MyCommand.Parameters.AddWithValue("date3", from.ToString(Constants.DateFormat));
            MyCommand.Parameters.AddWithValue("date4", to.ToString(Constants.DateFormat));
            MyReader = MyCommand.ExecuteReader();
            while (MyReader.Read())
            {
                totalRecord += MyReader.GetInt32(MyReader.GetOrdinal("totalRecord"));
            }
            MyReader.Close();
            conn.Close();
            rs.data = data;
            rs.totalRecord = totalRecord;
            return rs;
        }
    }
}
