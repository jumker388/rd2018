using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web365Domain.Game;

namespace Web365Business.Game
{
    public class RoomBusiness
    {
        public List<RoomItem> GetAll(int gameId)
        {
            var lst = new List<RoomItem>();
            var sql = "";
            if (gameId > 0)
            {
                sql = "Select r.*, g.name as 'game_name' from [portal].[dbo].room r inner join [portal].[dbo].game g on g.id = r.game_id WHERE r.game_id = @game_id";
            }
            else
            {
                sql = "Select r.*, g.name as 'game_name' from [portal].[dbo].room r inner join [portal].[dbo].game g on g.id = r.game_id order by id desc";
            }
            var conn = new SqlConnection(Constants.DBConnection);
            var myCommand = new SqlCommand(sql, conn);
            myCommand.Parameters.AddWithValue("game_id", gameId);
            SqlDataReader MyReader;
            conn.Open();
            MyReader = myCommand.ExecuteReader();
            while (MyReader.Read())
            {
                var item = new RoomItem
                {
                    id = MyReader.GetInt32(MyReader.GetOrdinal("id")),
                    name = MyReader.GetString(MyReader.GetOrdinal("name")),
                    state = MyReader.GetInt32(MyReader.GetOrdinal("state")),
                    game_id = MyReader.GetInt32(MyReader.GetOrdinal("game_id")),
                    min_bet = MyReader.GetInt32(MyReader.GetOrdinal("min_bet")),
                    game_name = MyReader.GetString(MyReader.GetOrdinal("game_name"))
                };
                lst.Add(item);
            }
            return lst;
        }

        public RoomItem GetDetailRoom(int roomId)
        {
            var lst = new RoomItem();
            var sql = "";
            sql = "Select * from [portal].[dbo].room r WHERE r.Id = @roomId";

            var conn = new SqlConnection(Constants.DBConnection);
            var myCommand = new SqlCommand(sql, conn);
            myCommand.Parameters.AddWithValue("roomId", roomId);
            SqlDataReader MyReader;
            conn.Open();
            MyReader = myCommand.ExecuteReader();
            while (MyReader.Read())
            {
                var item = new RoomItem
                {
                    id = MyReader.GetInt32(MyReader.GetOrdinal("id")),
                    name = MyReader.GetString(MyReader.GetOrdinal("name")),
                    state = MyReader.GetInt32(MyReader.GetOrdinal("state")),
                    game_id = MyReader.GetInt32(MyReader.GetOrdinal("game_id")),
                    min_bet = MyReader.GetInt32(MyReader.GetOrdinal("min_bet")),
                };
                lst = item;
            }
            return lst;
        }

        public int Update(RoomItem room)
        {
            var rowAffected = 0;
            const string sql = "update [portal].[dbo].room set name=@name, min_bet = @min_bet, state = @state where id = @id";
            var conn = new SqlConnection(Constants.DBConnection);
            var myCommand = new SqlCommand(sql, conn);
            myCommand.Parameters.AddWithValue("name", room.name);
            myCommand.Parameters.AddWithValue("min_bet", room.min_bet);
            myCommand.Parameters.AddWithValue("state", room.state);
            myCommand.Parameters.AddWithValue("id", room.id);
            conn.Open();
            rowAffected = myCommand.ExecuteNonQuery();
            conn.Close();
            return rowAffected;
        }

        public int Insert(RoomItem room)
        {
            var rowAffected = 0;
            var sql = "INSERT [portal].[dbo].room (name, min_cash, min_level, game_id, room_number, state, min_bet, tax, end_time, start_time) values (@name, 0, 0, @game_id, 50, @state, @min_bet, 5, -1, -1)";
            var conn = new SqlConnection(Constants.DBConnection);
            var myCommand = new SqlCommand(sql, conn);
            myCommand.Parameters.AddWithValue("name", room.name);
            myCommand.Parameters.AddWithValue("min_bet", room.min_bet);
            myCommand.Parameters.AddWithValue("state", room.state);
            myCommand.Parameters.AddWithValue("game_id", room.game_id);
            conn.Open();
            rowAffected = myCommand.ExecuteNonQuery();
            conn.Close();
            return rowAffected;
        }
    }
}
