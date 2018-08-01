using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web365Domain;

namespace GameBusiness
{
    public class AnnouncementBusiness
    {
        public List<AnnouncementItem> GetAll()
        {
            var lst = new List<AnnouncementItem>();
            string sql = "SELECT * FROM portal.announcement ORDER BY id DESC";
            MySqlConnection conn = new MySqlConnection(Constants.DBConnection);
            MySqlCommand MyCommand = new MySqlCommand(sql, conn);
            MySqlDataReader MyReader;
            conn.Open();
            MyReader = MyCommand.ExecuteReader();
            while (MyReader.Read())
            {
                var item = new AnnouncementItem();
                item.ID = MyReader.GetInt32("id");
                byte[] datasubject = Convert.FromBase64String(MyReader.GetString("subject"));                
                item.Subject = Encoding.UTF8.GetString(datasubject);
                byte[] dataContent = Convert.FromBase64String(MyReader.GetString("content"));
                item.Content = Encoding.UTF8.GetString(dataContent);
                lst.Add(item);
            }
            MyReader.Close();
            conn.Close();
            return lst;
        }

        public int Insert(string subject, string content)
        {
            var rs = 0;
            string sql = "INSERT INTO portal.announcement(cp, subject, content) VALUES('0', @subject, @content);";
            MySqlConnection conn = new MySqlConnection(Constants.DBConnection);
            MySqlCommand MyCommand = new MySqlCommand(sql, conn);
            MyCommand.Parameters.AddWithValue("subject", Convert.ToBase64String(Encoding.UTF8.GetBytes(subject)));
            MyCommand.Parameters.AddWithValue("content", Convert.ToBase64String(Encoding.UTF8.GetBytes(content)));            
            conn.Open();
            rs = MyCommand.ExecuteNonQuery();
            conn.Close();
            return rs;
        }

        public int Update(int ID, string subject, string content)
        {
            var rs = 0;
            string sql = "UPDATE portal.announcement set subject = @subject, content = @content where id = @id";
            MySqlConnection conn = new MySqlConnection(Constants.DBConnection);
            MySqlCommand MyCommand = new MySqlCommand(sql, conn);
            MyCommand.Parameters.AddWithValue("subject", Convert.ToBase64String(Encoding.UTF8.GetBytes(subject)));
            MyCommand.Parameters.AddWithValue("content", Convert.ToBase64String(Encoding.UTF8.GetBytes(content)));
            MyCommand.Parameters.AddWithValue("id", ID);            
            conn.Open();
            rs = MyCommand.ExecuteNonQuery();
            conn.Close();
            return rs;
        }

        public int Delete(int ID)
        {
            var rs = 0;
            string sql = "DELETE from portal.announcement where id = @id";
            MySqlConnection conn = new MySqlConnection(Constants.DBConnection);
            MySqlCommand MyCommand = new MySqlCommand(sql, conn);            
            MyCommand.Parameters.AddWithValue("id", ID);
            conn.Open();
            rs = MyCommand.ExecuteNonQuery();
            conn.Close();
            return rs;
        }

    }
}
