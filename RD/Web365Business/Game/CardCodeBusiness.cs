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
    public class CardCodeBusiness
    {
        public int InsertItem(CardItem obj)
        {
            var result = 0;
            var sql = "insert into [portal].[dbo].exchangeCardInfo(serial,CardNo,dateExpired,dateInput,used,value, telcoId) OUTPUT INSERTED.id values('" + obj.serial + "','" + obj.cardNo + "','" + obj.dateExpired.ToString("yyyy-MM-dd HH:mm:ss") + "','" + obj.dateInput.ToString("yyyy-MM-dd HH:mm:ss") + "','0','" + obj.value + "','" + obj.telcoId + "');";
            var conn = new SqlConnection(Constants.DBConnection);
            var MyCommand = new SqlCommand(sql, conn);
            conn.Open();
            var inserted = (int)MyCommand.ExecuteScalar();
            if (inserted > 0)
            {
                result = inserted;
            }

            conn.Close();
            return result;
        }

        public List<CardItem> GetAllCard(out int total, int skip, int take, int used = 0, int telcoId = 0, int value = 0, string seri = "")
        {
            var rs = new List<CardItem>();
            var sql = "SELECT * FROM [portal].[dbo].exchangeCardInfo ";

            var sqlTotalQuery = "SELECT Count(id) as totalRecord FROM [portal].[dbo].exchangeCardInfo";

            sql += " where used = " + used;
            sqlTotalQuery += " where used = " + used;

            if (telcoId > 0)
            {
                sql += " and telcoId = " + telcoId;
                sqlTotalQuery += " and telcoId = " + telcoId;
            }

            if (value > 0)
            {
                sql += " and value = " + value;
                sqlTotalQuery += " and value = " + value;
            }

            if (!string.IsNullOrEmpty(seri))
            {
                sql += " and serial = '" + seri + "'";
                sqlTotalQuery += " and serial = '" + value + "'";
            }

            sql += " ORDER BY id desc OFFSET " + skip + " ROWS FETCH NEXT "+ take +" ROWS ONLY ";

            var conn = new SqlConnection(Constants.DBConnection);
            var MyCommand = new SqlCommand(sql, conn);
            SqlDataReader MyReader;
            conn.Open();
            MyReader = MyCommand.ExecuteReader();
            while (MyReader.Read())
            {
                var u = new CardItem
                {
                    id = MyReader.GetInt32(MyReader.GetOrdinal("id")),
                    serial = MyReader.GetString(MyReader.GetOrdinal("serial")),
                    //cardNo = MyReader.GetString(MyReader.GetOrdinal("cardNo"),
                    dateInput = MyReader.GetDateTime(MyReader.GetOrdinal("dateInput")),
                    dateInputString = MyReader.GetDateTime(MyReader.GetOrdinal("dateInput")).ToShortDateString(),
                    dateExpiredString = MyReader.GetDateTime(MyReader.GetOrdinal("dateExpired")).ToShortDateString(),
                    dateExpired = MyReader.GetDateTime(MyReader.GetOrdinal("dateExpired")),
                    used = MyReader.GetByte(MyReader.GetOrdinal("used")),
                    value = MyReader.GetInt32(MyReader.GetOrdinal("value")),
                    telcoId = MyReader.GetInt32(MyReader.GetOrdinal("telcoId"))

                };

                if (used > 0)
                {
                    u.dateUse = MyReader.GetDateTime(MyReader.GetOrdinal("dateUse"));
                    u.dateUseString = MyReader.GetDateTime(MyReader.GetOrdinal("dateUse")).ToShortDateString();
                }
                rs.Add(u);
            }
            MyReader.Close();

            MyCommand = new SqlCommand(sqlTotalQuery, conn);
            MyReader = MyCommand.ExecuteReader();
            total = 0;
            while (MyReader.Read())
            {
                total = MyReader.GetInt32(MyReader.GetOrdinal("totalRecord"));
            }
            MyReader.Close();
            conn.Close();
            return rs;
        }

        public List<ConfigMiniPokerItem> GetConfigMiniPoker()
        {
            var rs = new List<ConfigMiniPokerItem>();
            var sql = "SELECT top 1 * FROM [newDB].[dbo].configMiniPoker ORDER BY id DESC";

            var conn = new SqlConnection(Constants.DBConnection);
            var MyCommand = new SqlCommand(sql, conn);
            SqlDataReader MyReader;
            conn.Open();
            MyReader = MyCommand.ExecuteReader();
            while (MyReader.Read())
            {
                var u = new ConfigMiniPokerItem
                {
                    Id = MyReader.GetInt32(MyReader.GetOrdinal("Id")),
                    a = MyReader.GetInt32(MyReader.GetOrdinal("a")),
                    a1 = MyReader.GetInt32(MyReader.GetOrdinal("a1")),
                    a2 = MyReader.GetInt32(MyReader.GetOrdinal("a2")),
                    a3 = MyReader.GetInt32(MyReader.GetOrdinal("a3")),
                    a4 = MyReader.GetInt32(MyReader.GetOrdinal("a4")),
                    a5 = MyReader.GetInt32(MyReader.GetOrdinal("a5")),
                    a6 = MyReader.GetInt32(MyReader.GetOrdinal("a6")),
                    a7 = MyReader.GetInt32(MyReader.GetOrdinal("a7")),
                    a8 = MyReader.GetInt32(MyReader.GetOrdinal("a8")),
                    a9 = MyReader.GetInt32(MyReader.GetOrdinal("a9")),
                    a10 = MyReader.GetInt32(MyReader.GetOrdinal("a10"))
                };
                rs.Add(u);
            }
            MyReader.Close();

            conn.Close();
            return rs;
        }

        public ConfigMiniPokerItem GetConfigMiniPokerDetail(int id)
        {
            var rs = new ConfigMiniPokerItem();
            var sql = "SELECT * FROM [newDB].[dbo].configMiniPoker where Id = " + id;

            var conn = new SqlConnection(Constants.DBConnection);
            var MyCommand = new SqlCommand(sql, conn);
            SqlDataReader MyReader;
            conn.Open();
            MyReader = MyCommand.ExecuteReader();
            while (MyReader.Read())
            {
                rs = new ConfigMiniPokerItem
                {
                    Id = MyReader.GetInt32(MyReader.GetOrdinal("Id")),
                    a = MyReader.GetInt32(MyReader.GetOrdinal("a")),
                    a1 = MyReader.GetInt32(MyReader.GetOrdinal("a1")),
                    a2 = MyReader.GetInt32(MyReader.GetOrdinal("a2")),
                    a3 = MyReader.GetInt32(MyReader.GetOrdinal("a3")),
                    a4 = MyReader.GetInt32(MyReader.GetOrdinal("a4")),
                    a5 = MyReader.GetInt32(MyReader.GetOrdinal("a5")),
                    a6 = MyReader.GetInt32(MyReader.GetOrdinal("a6")),
                    a7 = MyReader.GetInt32(MyReader.GetOrdinal("a7")),
                    a8 = MyReader.GetInt32(MyReader.GetOrdinal("a8")),
                    a9 = MyReader.GetInt32(MyReader.GetOrdinal("a9")),
                    a10 = MyReader.GetInt32(MyReader.GetOrdinal("a10"))
                };
            }
            MyReader.Close();

            conn.Close();
            return rs;
        }
        public int UpdateConfigMiniPoker(ConfigMiniPokerItem obj)
        {
            var rowAffected = 0;
            const string sql = "update [newDB].[dbo].configMiniPoker set a=@a, a1 = @a1, a2 = @a2, a3=@a3, a4=@a4, a5=@a5, a6=@a6, a7=@a7, a8=@a8, a9=@a9, a10=@a10 where Id = @id";
            var conn = new SqlConnection(Constants.DBConnection);
            var myCommand = new SqlCommand(sql, conn);
            myCommand.Parameters.AddWithValue("a", obj.a);
            myCommand.Parameters.AddWithValue("a1", obj.a1);
            myCommand.Parameters.AddWithValue("a2", obj.a2);
            myCommand.Parameters.AddWithValue("a3", obj.a3);
            myCommand.Parameters.AddWithValue("a4", obj.a4);
            myCommand.Parameters.AddWithValue("a5", obj.a5);
            myCommand.Parameters.AddWithValue("a6", obj.a6);
            myCommand.Parameters.AddWithValue("a7", obj.a7);
            myCommand.Parameters.AddWithValue("a8", obj.a8);
            myCommand.Parameters.AddWithValue("a9", obj.a9);
            myCommand.Parameters.AddWithValue("a10", obj.a10);
            myCommand.Parameters.AddWithValue("id", obj.Id);
            conn.Open();
            rowAffected = myCommand.ExecuteNonQuery();
            conn.Close();
            return rowAffected;
        }


        #region config Xeng

        public List<ConfigMiniPokerItem> GetConfigXeng()
        {
            var rs = new List<ConfigMiniPokerItem>();
            var sql = "SELECT top 1 * FROM [newDB].[dbo].configQuayXeng ORDER BY id DESC";

            var conn = new SqlConnection(Constants.DBConnection);
            var MyCommand = new SqlCommand(sql, conn);
            SqlDataReader MyReader;
            conn.Open();
            MyReader = MyCommand.ExecuteReader();
            while (MyReader.Read())
            {
                var u = new ConfigMiniPokerItem
                {
                    Id = MyReader.GetInt32(MyReader.GetOrdinal("id")),
                    a = MyReader.GetInt32(MyReader.GetOrdinal("a")),
                    a1 = MyReader.GetInt32(MyReader.GetOrdinal("a1")),
                    a2 = MyReader.GetInt32(MyReader.GetOrdinal("a2")),
                    a3 = MyReader.GetInt32(MyReader.GetOrdinal("a3")),
                    a4 = MyReader.GetInt32(MyReader.GetOrdinal("a4")),
                    a5 = MyReader.GetInt32(MyReader.GetOrdinal("a5")),
                    a6 = MyReader.GetInt32(MyReader.GetOrdinal("a6"))
                };
                rs.Add(u);
            }
            MyReader.Close();

            conn.Close();
            return rs;
        }

        public ConfigMiniPokerItem GetConfigXengDetail(int id)
        {
            var rs = new ConfigMiniPokerItem();
            var sql = "SELECT * FROM [newDB].[dbo].configQuayXeng where id = " + id;

            var conn = new SqlConnection(Constants.DBConnection);
            var MyCommand = new SqlCommand(sql, conn);
            SqlDataReader MyReader;
            conn.Open();
            MyReader = MyCommand.ExecuteReader();
            while (MyReader.Read())
            {
                rs = new ConfigMiniPokerItem
                {
                    Id = MyReader.GetInt32(MyReader.GetOrdinal("id")),
                    a = MyReader.GetInt32(MyReader.GetOrdinal("a")),
                    a1 = MyReader.GetInt32(MyReader.GetOrdinal("a1")),
                    a2 = MyReader.GetInt32(MyReader.GetOrdinal("a2")),
                    a3 = MyReader.GetInt32(MyReader.GetOrdinal("a3")),
                    a4 = MyReader.GetInt32(MyReader.GetOrdinal("a4")),
                    a5 = MyReader.GetInt32(MyReader.GetOrdinal("a5")),
                    a6 = MyReader.GetInt32(MyReader.GetOrdinal("a6"))
                };
            }
            MyReader.Close();

            conn.Close();
            return rs;
        }
        public int UpdateConfigXeng(ConfigMiniPokerItem obj)
        {
            var rowAffected = 0;
            const string sql = "update [newDB].[dbo].configQuayXeng set a=@a, a1 = @a1, a2 = @a2, a3=@a3, a4=@a4, a5=@a5, a6=@a6 where id = @id";
            var conn = new SqlConnection(Constants.DBConnection);
            var myCommand = new SqlCommand(sql, conn);
            myCommand.Parameters.AddWithValue("a", obj.a);
            myCommand.Parameters.AddWithValue("a1", obj.a1);
            myCommand.Parameters.AddWithValue("a2", obj.a2);
            myCommand.Parameters.AddWithValue("a3", obj.a3);
            myCommand.Parameters.AddWithValue("a4", obj.a4);
            myCommand.Parameters.AddWithValue("a5", obj.a5);
            myCommand.Parameters.AddWithValue("a6", obj.a6);
            myCommand.Parameters.AddWithValue("id", obj.Id);
            conn.Open();
            rowAffected = myCommand.ExecuteNonQuery();
            conn.Close();
            return rowAffected;
        }
            
        #endregion

        #region config Tai Xiu

        public List<ConfigMiniPokerItem> GetConfigTaiXiu()
        {
            var rs = new List<ConfigMiniPokerItem>();
            var sql = "SELECT * FROM [newDB].[dbo].configBotTaixiu";

            var conn = new SqlConnection(Constants.DBConnection);
            var MyCommand = new SqlCommand(sql, conn);
            SqlDataReader MyReader;
            conn.Open();
            MyReader = MyCommand.ExecuteReader();
            while (MyReader.Read())
            {
                var u = new ConfigMiniPokerItem
                {
                    a1 = MyReader.GetInt32(MyReader.GetOrdinal("a1")),
                    a2 = MyReader.GetInt32(MyReader.GetOrdinal("a2")),
                    a3 = MyReader.GetInt32(MyReader.GetOrdinal("a3")),
                    a4 = MyReader.GetInt32(MyReader.GetOrdinal("a4")),
                    a5 = MyReader.GetInt32(MyReader.GetOrdinal("a5")),
                    a6 = MyReader.GetInt32(MyReader.GetOrdinal("a6")),
                    a7 = MyReader.GetInt32(MyReader.GetOrdinal("a7")),
                    a8 = MyReader.GetInt32(MyReader.GetOrdinal("a8")),
                    a9 = MyReader.GetInt32(MyReader.GetOrdinal("a9")),
                    a10 = MyReader.GetInt32(MyReader.GetOrdinal("a10")),
                    a11 = MyReader.GetInt32(MyReader.GetOrdinal("a11")),
                    a12 = MyReader.GetInt32(MyReader.GetOrdinal("a12")),
                    a13 = MyReader.GetInt32(MyReader.GetOrdinal("a13")),
                    a14 = MyReader.GetInt32(MyReader.GetOrdinal("a14")),
                    a15 = MyReader.GetInt32(MyReader.GetOrdinal("a15")),
                    a16 = MyReader.GetInt32(MyReader.GetOrdinal("a16")),
                    a17 = MyReader.GetInt32(MyReader.GetOrdinal("a17")),
                    a18 = MyReader.GetInt32(MyReader.GetOrdinal("a18")),
                    a19 = MyReader.GetInt32(MyReader.GetOrdinal("a19")),
                };
                rs.Add(u);
            }
            MyReader.Close();

            conn.Close();
            return rs;
        }

        public ConfigMiniPokerItem GetConfigTaiXiuDetail(int id)
        {
            var rs = new ConfigMiniPokerItem();
            var sql = "SELECT top 1 * FROM [newDB].[dbo].configBotTaixiu";

            var conn = new SqlConnection(Constants.DBConnection);
            var MyCommand = new SqlCommand(sql, conn);
            SqlDataReader MyReader;
            conn.Open();
            MyReader = MyCommand.ExecuteReader();
            while (MyReader.Read())
            {
                rs = new ConfigMiniPokerItem
                {
                    a1 = MyReader.GetInt32(MyReader.GetOrdinal("a1")),
                    a2 = MyReader.GetInt32(MyReader.GetOrdinal("a2")),
                    a3 = MyReader.GetInt32(MyReader.GetOrdinal("a3")),
                    a4 = MyReader.GetInt32(MyReader.GetOrdinal("a4")),
                    a5 = MyReader.GetInt32(MyReader.GetOrdinal("a5")),
                    a6 = MyReader.GetInt32(MyReader.GetOrdinal("a6")),
                    a7 = MyReader.GetInt32(MyReader.GetOrdinal("a7")),
                    a8 = MyReader.GetInt32(MyReader.GetOrdinal("a8")),
                    a9 = MyReader.GetInt32(MyReader.GetOrdinal("a9")),
                    a10 = MyReader.GetInt32(MyReader.GetOrdinal("a10")),
                    a11 = MyReader.GetInt32(MyReader.GetOrdinal("a11")),
                    a12 = MyReader.GetInt32(MyReader.GetOrdinal("a12")),
                    a13 = MyReader.GetInt32(MyReader.GetOrdinal("a13")),
                    a14 = MyReader.GetInt32(MyReader.GetOrdinal("a14")),
                    a15 = MyReader.GetInt32(MyReader.GetOrdinal("a15")),
                    a16 = MyReader.GetInt32(MyReader.GetOrdinal("a16")),
                    a17 = MyReader.GetInt32(MyReader.GetOrdinal("a17")),
                    a18 = MyReader.GetInt32(MyReader.GetOrdinal("a18")),
                    a19 = MyReader.GetInt32(MyReader.GetOrdinal("a19")),
                };
            }
            MyReader.Close();

            conn.Close();
            return rs;
        }
        public int UpdateConfigTaiXiu(ConfigMiniPokerItem obj)
        {
            var rowAffected = 0;
            const string sql = "update [newDB].[dbo].configBotTaixiu set a1 = @a1, a2 = @a2, a3=@a3, a4=@a4, a5=@a5, a6=@a6 , a7=@a7 ,a8=@a8 ,a9=@a9 ,a10=@a10 ,a11=@a11 ,a12=@a12 ,a13=@a13 ,a14=@a14 ,a15=@a15 ,a16=@a16 ,a17=@a17 ,a18=@a18 ,a19=@a19";
            var conn = new SqlConnection(Constants.DBConnection);
            var myCommand = new SqlCommand(sql, conn);
            myCommand.Parameters.AddWithValue("a1", obj.a1);
            myCommand.Parameters.AddWithValue("a2", obj.a2);
            myCommand.Parameters.AddWithValue("a3", obj.a3);
            myCommand.Parameters.AddWithValue("a4", obj.a4);
            myCommand.Parameters.AddWithValue("a5", obj.a5);
            myCommand.Parameters.AddWithValue("a6", obj.a6);
            myCommand.Parameters.AddWithValue("a7", obj.a7);
            myCommand.Parameters.AddWithValue("a8", obj.a8);
            myCommand.Parameters.AddWithValue("a9", obj.a9);
            myCommand.Parameters.AddWithValue("a10", obj.a10);
            myCommand.Parameters.AddWithValue("a11", obj.a11);
            myCommand.Parameters.AddWithValue("a12", obj.a12);
            myCommand.Parameters.AddWithValue("a13", obj.a13);
            myCommand.Parameters.AddWithValue("a14", obj.a14);
            myCommand.Parameters.AddWithValue("a15", obj.a15);
            myCommand.Parameters.AddWithValue("a16", obj.a16);
            myCommand.Parameters.AddWithValue("a17", obj.a17);
            myCommand.Parameters.AddWithValue("a18", obj.a18);
            myCommand.Parameters.AddWithValue("a19", obj.a19);
            conn.Open();
            rowAffected = myCommand.ExecuteNonQuery();
            conn.Close();
            return rowAffected;
        }

        #endregion
    }
}

