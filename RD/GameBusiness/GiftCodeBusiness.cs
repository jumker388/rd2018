using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web365Domain;

namespace GameBusiness
{
    public class GiftCodeBusiness
    {
        public List<GiftCodeItem> Generator(int total, int value, string name, DateTime dateExpired)
        {
            var lst = new List<GiftCodeItem>();
            string sql = "insert into newDB.giftcode(code, value, dateCreated, dateExpired, used, name) values(@code, @value, @dateCreated, @dateExpired, @used, @name";
            DateTime dateCreated = DateTime.Now;
            MySqlConnection conn = new MySqlConnection(Constants.DBConnection);
            MySqlCommand MyCommand = new MySqlCommand(sql, conn);
            conn.Open();
            for (int i = 1; i <= total; i++)
            {
                String code = GenGiftCode();
                MyCommand.Parameters.Clear();
                MyCommand.Parameters.AddWithValue("code", code);
                MyCommand.Parameters.AddWithValue("value", value);
                MyCommand.Parameters.AddWithValue("dateCreated", dateCreated.ToString(Constants.DateFormat));
                MyCommand.Parameters.AddWithValue("dateExpired", dateExpired.ToString(Constants.DateFormat));
                MyCommand.Parameters.AddWithValue("used", 0);
                MyCommand.Parameters.AddWithValue("name", name);
                int inserted = MyCommand.ExecuteNonQuery();
                if(inserted > 0)
                {
                    GiftCodeItem item = new GiftCodeItem();
                    item.code = code;
                    item.ID = (int)MyCommand.LastInsertedId;
                    item.dateCreated = dateCreated;
                    item.dateExpired = dateExpired;
                    item.used = false;
                    item.name = name;
                    item.value = value;
                    item.user_id = 0;
                    item.username = "";
                    item.fullname = "";
                    lst.Add(item);
                }
            }
            conn.Close();
            return lst;
        }

        private string GenGiftCode()
        {
            var chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 8)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
        }

        public GiftCodeResult GetAll(bool used, int skip, int take)
        {
            var rs = new GiftCodeResult();
            List<GiftCodeItem> data = new List<GiftCodeItem>();
            int totalRecord = 0;
            string sql = "";
            if(used)
            {
                sql = "SELECT u.username, u.fullname, gc.* FROM newDB.giftcode gc inner join newDB.view_user_info u on gc.user_id = u.id WHERE used = 1 ORDER BY gc.id DESC LIMIT @skip, @take";
                MySqlConnection conn = new MySqlConnection(Constants.DBConnection);
                MySqlCommand MyCommand = new MySqlCommand(sql, conn);
                MyCommand.Parameters.AddWithValue("skip", skip);
                MyCommand.Parameters.AddWithValue("take", take);
                MySqlDataReader MyReader;
                conn.Open();
                MyReader = MyCommand.ExecuteReader();
                while (MyReader.Read())
                {
                    GiftCodeItem item = new GiftCodeItem();
                    item.code = MyReader.GetString("code");
                    item.ID = MyReader.GetInt32("id");
                    item.dateCreated = MyReader.GetDateTime("dateCreated");
                    item.dateExpired = MyReader.GetDateTime("dateExpired"); ;
                    item.used = false;
                    item.name = MyReader.GetString("name");
                    item.value = MyReader.GetInt32("value");
                    item.user_id = MyReader.GetInt32("user_id");
                    item.username = MyReader.GetString("username");
                    item.fullname = MyReader.GetString("fullname");
                    data.Add(item);
                }
                MyReader.Close();
                sql = "Select count(*) as totalRecord from newDB.giftcode WHERE used = 1";
                MyCommand = new MySqlCommand(sql, conn);
                MyReader = MyCommand.ExecuteReader();
                while (MyReader.Read())
                {
                    totalRecord = MyReader.GetInt32("totalRecord");
                }
                MyReader.Close();
                conn.Close();
            }
            else
            {
                sql = "SELECT * FROM newDB.giftcode WHERE used = 0 ORDER BY id DESC LIMIT @skip, @take";
                MySqlConnection conn = new MySqlConnection(Constants.DBConnection);
                MySqlCommand MyCommand = new MySqlCommand(sql, conn);                
                MyCommand.Parameters.AddWithValue("skip", skip);
                MyCommand.Parameters.AddWithValue("take", take);
                MySqlDataReader MyReader;
                conn.Open();
                MyReader = MyCommand.ExecuteReader();
                while(MyReader.Read())
                {
                    GiftCodeItem item = new GiftCodeItem();
                    item.code = MyReader.GetString("code");
                    item.ID = MyReader.GetInt32("id");
                    item.dateCreated = MyReader.GetDateTime("dateCreated");
                    item.dateExpired = MyReader.GetDateTime("dateExpired"); ;
                    item.used = false;
                    item.name = MyReader.GetString("name");
                    item.value = MyReader.GetInt32("value");
                    item.user_id = 0;
                    item.username = "";
                    item.fullname = "";
                    data.Add(item);
                }
                MyReader.Close();
                sql = "Select count(*) as totalRecord from newDB.giftcode WHERE used = 0";
                MyCommand = new MySqlCommand(sql, conn);
                MyReader = MyCommand.ExecuteReader();
                while (MyReader.Read())
                {
                    totalRecord = MyReader.GetInt32("totalRecord");
                }
                MyReader.Close();
                conn.Close();
            }
            rs.totalRecord = totalRecord;
            rs.data = data;
            return rs;
        }
    }
}
