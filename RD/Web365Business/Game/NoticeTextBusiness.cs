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
    public class NoticeTextBusiness
    {
        public string GetText()
        {
            var rs = "";
            string sql = "Select noticeText from [portal].[dbo].cp where id = 1";
            SqlConnection conn = new SqlConnection(Constants.DBConnection);
            SqlCommand MyCommand = new SqlCommand(sql, conn);
            SqlDataReader MyReader;
            conn.Open();
            MyReader = MyCommand.ExecuteReader();
            while (MyReader.Read())
            {
                rs = MyReader.GetString(MyReader.GetOrdinal("noticeText"));
            }
            MyReader.Close();
            conn.Close();
            return rs;
        }

        public int Update(string noticeText)
        {
            var rs = 0;
            string sql = "Update [portal].[dbo].cp set noticeText=@noticeText  WHERE id=1;";
            SqlConnection conn = new SqlConnection(Constants.DBConnection);
            SqlCommand MyCommand = new SqlCommand(sql, conn);
            MyCommand.Parameters.AddWithValue("noticeText", noticeText);            
            conn.Open();
            rs = MyCommand.ExecuteNonQuery();
            conn.Close();
            return rs;
        }


        /*update 24/03*/

        public List<GameTextItem> GetAll()
        {
            var lst = new List<GameTextItem>();
            string sql = "SELECT * FROM [portal].[dbo].chuchaychay ORDER BY id DESC";
            SqlConnection conn = new SqlConnection(Constants.DBConnection);
            SqlCommand MyCommand = new SqlCommand(sql, conn);
            SqlDataReader MyReader;
            conn.Open();
            MyReader = MyCommand.ExecuteReader();
            while (MyReader.Read())
            {
                var item = new GameTextItem
                {
                    ID = MyReader.GetInt32(MyReader.GetOrdinal("id")),
                    Title = MyReader.GetString(MyReader.GetOrdinal("Title")),
                    Link = MyReader.GetString(MyReader.GetOrdinal("Link")),
                    DataStart = MyReader.GetDateTime(MyReader.GetOrdinal("DataStart")),
                    DateEnd = MyReader.GetDateTime(MyReader.GetOrdinal("DateEnd")),
                    IsDelete = MyReader.GetBoolean(MyReader.GetOrdinal("IsDelete")),
                    Order = MyReader.GetInt32(MyReader.GetOrdinal("Orders")),
                    DataStartstring = MyReader.GetDateTime(MyReader.GetOrdinal("DataStart")).ToShortDateString(),
                    DateEndstring = MyReader.GetDateTime(MyReader.GetOrdinal("DateEnd")).ToShortDateString()
                };

                lst.Add(item);
            }
            MyReader.Close();
            conn.Close();
            return lst;
        }

        public GameTextItem GetOne(int ID)
        {
            var item = new GameTextItem();
            string sql = "SELECT * FROM [portal].[dbo].chuchaychay WHERE id = @id";
            SqlConnection conn = new SqlConnection(Constants.DBConnection);
            SqlCommand MyCommand = new SqlCommand(sql, conn);
            MyCommand.Parameters.AddWithValue("id", ID);
            SqlDataReader MyReader;
            conn.Open();
            MyReader = MyCommand.ExecuteReader();
            while (MyReader.Read())
            {
                 item = new GameTextItem
                {
                    ID = MyReader.GetInt32(MyReader.GetOrdinal("id")),
                    Title = MyReader.GetString(MyReader.GetOrdinal("Title")),
                    Link = MyReader.GetString(MyReader.GetOrdinal("Link")),
                    DataStart = MyReader.GetDateTime(MyReader.GetOrdinal("DataStart")),
                    DateEnd = MyReader.GetDateTime(MyReader.GetOrdinal("DateEnd")),
                    IsDelete = MyReader.GetBoolean(MyReader.GetOrdinal("IsDelete")),
                    Order = MyReader.GetInt32(MyReader.GetOrdinal("Orders")),
                };
            }
            MyReader.Close();
            conn.Close();
            return item;
        }

        public int Insert(string Title, string Link, DateTime DataStart, DateTime DateEnd, bool IsDelete, int Order)
        {
            if (string.IsNullOrEmpty(Link))
            {
                Link += " ";   
            }
            var rs = 0;
            string sql = "INSERT INTO [portal].[dbo].chuchaychay(Title, Link, DataStart, DateEnd, IsDelete, Orders) VALUES(@Title, @Link, @DataStart, @DateEnd , 0, @Order)";
            SqlConnection conn = new SqlConnection(Constants.DBConnection);
            SqlCommand MyCommand = new SqlCommand(sql, conn);
            MyCommand.Parameters.AddWithValue("Title", Title);
            MyCommand.Parameters.AddWithValue("Link", Link);
            MyCommand.Parameters.AddWithValue("DataStart", DataStart.ToString(Constants.DateFormat));
            MyCommand.Parameters.AddWithValue("DateEnd", DateEnd.ToString(Constants.DateFormat));
            MyCommand.Parameters.AddWithValue("Order", Order);
            conn.Open();
            rs = MyCommand.ExecuteNonQuery();
            conn.Close();
            return rs;
        }

        public int Update(int ID, string Title, string Link, DateTime DataStart, DateTime DateEnd, bool IsDelete, int Order)
        {
            var rs = 0;
            string sql = "UPDATE [portal].[dbo].chuchaychay set Title = @Title, Link = @Link, DataStart = @DataStart , DateEnd = @DateEnd, Orders= @Order  where id = @id";
            SqlConnection conn = new SqlConnection(Constants.DBConnection);
            SqlCommand MyCommand = new SqlCommand(sql, conn);
            MyCommand.Parameters.AddWithValue("Title", Title);
            MyCommand.Parameters.AddWithValue("Link",Link);
            MyCommand.Parameters.AddWithValue("DataStart", DataStart.ToString(Constants.DateFormat));
            MyCommand.Parameters.AddWithValue("DateEnd", DateEnd.ToString(Constants.DateFormat));
            MyCommand.Parameters.AddWithValue("Order", Order);
            MyCommand.Parameters.AddWithValue("id", ID);
            conn.Open();
            rs = MyCommand.ExecuteNonQuery();
            conn.Close();
            return rs;
        }

        public int Delete(int ID)
        {
            var rs = 0;
            string sql = "DELETE from [portal].[dbo].chuchaychay where id = @id";
            SqlConnection conn = new SqlConnection(Constants.DBConnection);
            SqlCommand MyCommand = new SqlCommand(sql, conn);
            MyCommand.Parameters.AddWithValue("id", ID);
            conn.Open();
            rs = MyCommand.ExecuteNonQuery();
            conn.Close();
            return rs;
        }

        /*end*/

        #region game guide

        public int GameGuideUpdate(GameGuideItem obj)
        {
            var rs = 0;
            string sql = "UPDATE [portal].[dbo].gameGuide set description = @description, game_id = @game_id where id = @id";
            SqlConnection conn = new SqlConnection(Constants.DBConnection);
            SqlCommand MyCommand = new SqlCommand(sql, conn);
            MyCommand.Parameters.AddWithValue("description", obj.description.Replace("'","\""));
            MyCommand.Parameters.AddWithValue("game_id", obj.game_id);
            MyCommand.Parameters.AddWithValue("id", obj.id);
            conn.Open();
            rs = MyCommand.ExecuteNonQuery();
            conn.Close();
            return rs;
        }

        public int GameGuideInsert(GameGuideItem obj)
        {
            var rs = 0;
            string sql = "INSERT INTO [portal].[dbo].gameGuide(description, game_id) VALUES(@description, @game_id)";
            SqlConnection conn = new SqlConnection(Constants.DBConnection);
            SqlCommand MyCommand = new SqlCommand(sql, conn);
            MyCommand.Parameters.AddWithValue("description", obj.description);
            MyCommand.Parameters.AddWithValue("game_id", obj.game_id);
            conn.Open();
            rs = MyCommand.ExecuteNonQuery();
            conn.Close();
            return rs;
        }

        public int GameGuideDelete(int id)
        {
            var rs = 0;
            string sql = "Delete from [portal].[dbo].gameGuide WHERE id= @id;";
            SqlConnection conn = new SqlConnection(Constants.DBConnection);
            SqlCommand MyCommand = new SqlCommand(sql, conn);
            MyCommand.Parameters.AddWithValue("id", id);
            conn.Open();
            rs = MyCommand.ExecuteNonQuery();
            conn.Close();
            return rs;
        }
        public List<GameGuideItem> GameGuideGetAll()
        {
            var lst = new List<GameGuideItem>();
            string sql = "SELECT gui.*, ga.Name FROM [portal].[dbo].gameGuide gui LEFT JOIN [portal].[dbo].game ga ON ga.ID = gui.game_id ORDER BY gui.id DESC";
            SqlConnection conn = new SqlConnection(Constants.DBConnection);
            SqlCommand MyCommand = new SqlCommand(sql, conn);
            SqlDataReader MyReader;
            conn.Open();
            MyReader = MyCommand.ExecuteReader();
            while (MyReader.Read())
            {
                var item = new GameGuideItem
                {
                    id = MyReader.GetInt32(MyReader.GetOrdinal("id")),
                    game = MyReader.GetString(MyReader.GetOrdinal("Name")),
                    description = MyReader.GetString(MyReader.GetOrdinal("description")),
                    game_id = MyReader.GetInt32(MyReader.GetOrdinal("game_id")),
                };

                lst.Add(item);
            }
            MyReader.Close();
            conn.Close();
            return lst;
        }
        public GameGuideItem GameGuideGetOne(int id)
        {
            var item = new GameGuideItem();
            string sql = "SELECT * FROM [portal].[dbo].gameGuide WHERE id = @id";
            SqlConnection conn = new SqlConnection(Constants.DBConnection);
            SqlCommand MyCommand = new SqlCommand(sql, conn);
            MyCommand.Parameters.AddWithValue("id", id);
            SqlDataReader MyReader;
            conn.Open();
            MyReader = MyCommand.ExecuteReader();
            while (MyReader.Read())
            {
                item = new GameGuideItem
                {
                    id = MyReader.GetInt32(MyReader.GetOrdinal("id")),
                    description = MyReader.GetString(MyReader.GetOrdinal("description")),
                    game_id = MyReader.GetInt32(MyReader.GetOrdinal("game_id")),
                };
            }
            MyReader.Close();
            conn.Close();
            return item;
        }
        #endregion
    }
}

