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
    public class AnnouncementBusiness
    {
        public List<AnnouncementItem> GetAll(int currentRecord = 0, int numberRecord = 10)
        {
            var lst = new List<AnnouncementItem>();
            string sql = "SELECT * FROM [newDB].[dbo].[event] where [IsDelete] = 0 order by id desc OFFSET " + currentRecord + " ROWS FETCH NEXT " + numberRecord + "ROWS ONLY ";
            SqlConnection conn = new SqlConnection(Constants.DBConnection);
            SqlCommand MyCommand = new SqlCommand(sql, conn);
            SqlDataReader MyReader;
            conn.Open();
            MyReader = MyCommand.ExecuteReader();
            while (MyReader.Read())
            {
                var item = new AnnouncementItem
                {
                    ID = MyReader.GetInt32(MyReader.GetOrdinal("id")),
                    DisplayOrder = MyReader.GetInt32(MyReader.GetOrdinal("DisplayOrder")),
                    begin_time = MyReader.GetDateTime(MyReader.GetOrdinal("DateStart")),
                    end_time = MyReader.GetDateTime(MyReader.GetOrdinal("DateEnd")),
                    Subject = MyReader.GetString(MyReader.GetOrdinal("name")),
                    Content = MyReader.GetString(MyReader.GetOrdinal("content")),
                };
                lst.Add(item);
            }
            MyReader.Close();
            conn.Close();
            return lst;
        }

        public AnnouncementItem GetOne(int ID)
        {
            var item = new AnnouncementItem();
            string sql = "SELECT * FROM [newDB].[dbo].[event] WHERE id = @id";
            SqlConnection conn = new SqlConnection(Constants.DBConnection);
            SqlCommand MyCommand = new SqlCommand(sql, conn);
            MyCommand.Parameters.AddWithValue("id", ID);
            SqlDataReader MyReader;
            conn.Open();
            MyReader = MyCommand.ExecuteReader();
            while (MyReader.Read())
            {
                item.ID = MyReader.GetInt32(MyReader.GetOrdinal("id"));
                item.begin_time = MyReader.GetDateTime(MyReader.GetOrdinal("DateStart"));
                item.end_time = MyReader.GetDateTime(MyReader.GetOrdinal("DateEnd"));
                item.Subject = MyReader.GetString(MyReader.GetOrdinal("name"));
                item.UrlImage = MyReader.GetString(MyReader.GetOrdinal("UrlImage"));
                item.Content = MyReader.GetString(MyReader.GetOrdinal("content"));
                item.DoiTuong = MyReader.GetString(MyReader.GetOrdinal("DoiTuong"));
                item.ThoiGian = MyReader.GetString(MyReader.GetOrdinal("ThoiGian"));
                item.GameID = MyReader.GetInt32(MyReader.GetOrdinal("GameID"));
                item.DisplayOrder = MyReader.GetInt32(MyReader.GetOrdinal("DisplayOrder"));
                item.begin_timestring = item.begin_time.ToString(Constants.DateFormat);
                item.end_timestring = item.end_time.ToString(Constants.DateFormat);
            }
            MyReader.Close();
            conn.Close();
            return item;
        }

        public int Insert(string subject, string content, DateTime begin_time, DateTime end_time, string UrlImage, int gameid, string doituong, string thoigian, int DisplayOrder)
        {
            var rs = 0;
            string sql = "INSERT INTO [newDB].[dbo].[event]([name],[content], UrlImage,[DateStart],[DateEnd],[DateCreated],[IsShow],[IsDelete], GameID, DoiTuong, ThoiGian, DisplayOrder) VALUES(@subject, @content, @UrlImage, @begin_time ,@end_time, GETDATE(), 1, 0, @gameid, @doituong, @thoigian, @DisplayOrder);";
            SqlConnection conn = new SqlConnection(Constants.DBConnection);
            SqlCommand MyCommand = new SqlCommand(sql, conn);
            MyCommand.Parameters.AddWithValue("subject", subject);
            MyCommand.Parameters.AddWithValue("content", content);
            MyCommand.Parameters.AddWithValue("UrlImage", UrlImage);
            MyCommand.Parameters.AddWithValue("begin_time", begin_time.ToString(Constants.DateFormat));
            MyCommand.Parameters.AddWithValue("end_time", end_time.ToString(Constants.DateFormat));
            MyCommand.Parameters.AddWithValue("gameid", gameid);
            MyCommand.Parameters.AddWithValue("doituong", doituong);
            MyCommand.Parameters.AddWithValue("thoigian", thoigian);
            MyCommand.Parameters.AddWithValue("DisplayOrder", DisplayOrder);
            conn.Open();
            rs = MyCommand.ExecuteNonQuery();
            conn.Close();
            return rs;
        }

        public int Update(int ID, string subject, string content, DateTime begin_time, DateTime end_time, string UrlImage, int gameid, string doituong, string thoigian, int DisplayOrder)
        {
            var rs = 0;
            string sql = "UPDATE [newDB].[dbo].[event] set name = @subject, content = @content, UrlImage = @UrlImage, DateStart = @begin_time , DateEnd = @end_time, GameID = @gameid, DoiTuong = @doituong, ThoiGian = @thoigian, DisplayOrder = @DisplayOrder where id = @id";
            SqlConnection conn = new SqlConnection(Constants.DBConnection);
            SqlCommand MyCommand = new SqlCommand(sql, conn);
            MyCommand.Parameters.AddWithValue("subject", subject);
            MyCommand.Parameters.AddWithValue("content", content);
            MyCommand.Parameters.AddWithValue("UrlImage", UrlImage);
            MyCommand.Parameters.AddWithValue("begin_time", begin_time.ToString(Constants.DateFormat));
            MyCommand.Parameters.AddWithValue("end_time", end_time.ToString(Constants.DateFormat));
            MyCommand.Parameters.AddWithValue("gameid", gameid);
            MyCommand.Parameters.AddWithValue("doituong", doituong);
            MyCommand.Parameters.AddWithValue("thoigian", thoigian);
            MyCommand.Parameters.AddWithValue("DisplayOrder", DisplayOrder);
            MyCommand.Parameters.AddWithValue("id", ID);
            conn.Open();
            rs = MyCommand.ExecuteNonQuery();
            conn.Close();
            return rs;
        }

        public int Delete(int ID)
        {
            var rs = 0;
            string sql = "update [newDB].[dbo].[event] set IsDelete = 1 where id = @id";
            SqlConnection conn = new SqlConnection(Constants.DBConnection);
            SqlCommand MyCommand = new SqlCommand(sql, conn);
            MyCommand.Parameters.AddWithValue("id", ID);
            conn.Open();
            rs = MyCommand.ExecuteNonQuery();
            conn.Close();
            return rs;
        }

    }
}
