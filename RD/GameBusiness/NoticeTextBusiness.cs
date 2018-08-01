using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBusiness
{
    public class NoticeTextBusiness
    {
        public string GetText()
        {
            var rs = "";
            string sql = "Select noticeText from newDB.cp where id = 1";
            MySqlConnection conn = new MySqlConnection(Constants.DBConnection);
            MySqlCommand MyCommand = new MySqlCommand(sql, conn);
            MySqlDataReader MyReader;
            conn.Open();
            MyReader = MyCommand.ExecuteReader();
            while (MyReader.Read())
            {
                rs = MyReader.GetString("noticeText");
            }
            MyReader.Close();
            conn.Close();
            return rs;
        }

        public int Update(string noticeText)
        {
            var rs = 0;
            string sql = "Update newDB.cp set noticeText=@noticeText  WHERE id=1;";
            MySqlConnection conn = new MySqlConnection(Constants.DBConnection);
            MySqlCommand MyCommand = new MySqlCommand(sql, conn);
            MyCommand.Parameters.AddWithValue("noticeText", noticeText);            
            conn.Open();
            rs = MyCommand.ExecuteNonQuery();
            conn.Close();
            return rs;
        }
    }
}
