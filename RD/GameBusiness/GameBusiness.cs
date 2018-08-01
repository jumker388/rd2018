using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web365Domain;

namespace GameBusiness
{
    public class GameBusiness
    {
        /// <summary>
        /// Lấy ra Danh sách các Game
        /// </summary>
        /// <returns></returns>
        public List<GameItem> GetAll()
        {
            var rs = new List<GameItem>();
            string sql = "select * from newDB.game";
            MySqlConnection conn = new MySqlConnection(Constants.DBConnection);
            MySqlCommand MyCommand = new MySqlCommand(sql, conn);
            MySqlDataReader MyReader;
            conn.Open();
            MyReader = MyCommand.ExecuteReader();
            while (MyReader.Read())
            {
                GameItem item = new GameItem();
                item.zoneID = MyReader.GetInt32("id");
                item.name = MyReader.GetString("name");
                item.displayStatus = MyReader.GetInt32("displayStatus");
                item.gameOrder = MyReader.GetInt32("gameOrder");
                rs.Add(item);
            }
            conn.Close();
            return rs;
        }

        /// <summary>
        /// Cập nhật thông tin Game, on/off theo game Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="status"></param>
        /// <param name="gameOrder"></param>
        /// <returns></returns>
        public int Update(int id, string name, int status, int gameOrder)
        {
            int rowAffected = 0;
            string sql = "update newDB.game set name=@name, displayStatus = @displayStatus, gameOrder = @gameOrder where id = @id";
            MySqlConnection conn = new MySqlConnection(Constants.DBConnection);
            MySqlCommand MyCommand = new MySqlCommand(sql, conn);
            MyCommand.Parameters.AddWithValue("name", name);
            MyCommand.Parameters.AddWithValue("displayStatus", status);
            MyCommand.Parameters.AddWithValue("gameOrder", gameOrder);
            MyCommand.Parameters.AddWithValue("id", id);
            conn.Open();
            rowAffected = MyCommand.ExecuteNonQuery();
            conn.Close();
            return rowAffected;
        }

        /// <summary>
        /// Danh sách lịch sử các trận đã chơi theo game Id
        /// </summary>
        /// <param name="zoneId"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public GameHistoryResult GetGameHistoryByZoneID(int zoneId, int skip, int take)
        {            
            var rs = new GameHistoryResult();
            var lst = new List<GameHistoryItem>();
            int totalRecord = 0;
            string sql = "SELECT a.*, b.username, b.fullname FROM newDB.game_history a inner join newDB.view_user_info b on a.user_id = b.id WHERE game_id = @zoneId ORDER BY id DESC LIMIT @skip, @take";
            string sqlTotalQuery = "SELECT count(*) as totalRecord FROM newDB.game_history WHERE game_id = @zoneId";
            MySqlConnection conn = new MySqlConnection(Constants.DBConnection);
            MySqlCommand MyCommand = new MySqlCommand(sql, conn);
            MyCommand.Parameters.AddWithValue("zoneId", zoneId);
            MyCommand.Parameters.AddWithValue("skip", skip);
            MyCommand.Parameters.AddWithValue("take", take);
            MySqlDataReader MyReader;
            conn.Open();
            MyReader = MyCommand.ExecuteReader();
            while (MyReader.Read())
            {
                GameHistoryItem item1 = new GameHistoryItem();
                item1.id = MyReader.GetInt64("id");
                item1.user_id = MyReader.GetInt64("user_id");
                item1.cash = MyReader.GetInt64("cash");
                item1.current_cash = MyReader.GetInt64("current_cash");
                item1.description = MyReader.GetString("description");
                item1.game_id = MyReader.GetInt32("game_id");
                item1.trans_type = MyReader.GetInt32("trans_type");
                item1.time = MyReader.GetDateTime("time");
                item1.before_cash = MyReader.GetInt64("before_cash");
                item1.username = MyReader.GetString("username");
                item1.fullname = MyReader.GetString("fullname");
                lst.Add(item1);
            }
            //close reader cũ
            MyReader.Close();
            MyCommand = new MySqlCommand(sqlTotalQuery, conn);
            MyCommand.Parameters.AddWithValue("zoneId", zoneId);
            MyReader = MyCommand.ExecuteReader();
            while (MyReader.Read())
            {
                totalRecord = MyReader.GetInt32("totalRecord");
            }
            conn.Close();
            rs.data = lst;
            rs.totalRecord = totalRecord;
            return rs;
        }

        /// <summary>
        /// Danh sách top user thắng nhiều (top) theo từng Game Id
        /// </summary>
        /// <param name="zoneId"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public List<UserInfo> GetTopUserByZoneID(int zoneId, int top)
        {
            var rs = new List<UserInfo>();
            var sql = "select b.id,b.username, b.fullname,sum(a.cash) as total_cash_win from newDB.game_history a inner join newDB.view_user_info b on a.user_id = b.id where a.cash > 0 AND (trans_type=1 or trans_type=18) AND game_id = @zoneId" +
                        " group by a.user_id order by total_cash_win desc" +
                        " limit 0, @top";
            MySqlConnection conn = new MySqlConnection(Constants.DBConnection);
            MySqlCommand MyCommand = new MySqlCommand(sql, conn);
            MyCommand.Parameters.AddWithValue("zoneId", zoneId);
            MyCommand.Parameters.AddWithValue("top", top);            
            MySqlDataReader MyReader;
            conn.Open();
            MyReader = MyCommand.ExecuteReader();
            while (MyReader.Read())
            {
                UserInfo u = new UserInfo();
                u.uid = MyReader.GetInt64("id");
                u.userName = MyReader.GetString("username");
                u.fullName = MyReader.GetString("fullname");
                u.cashWin = MyReader.GetInt64("total_cash_win");
                rs.Add(u);
            }
            MyReader.Close();
            conn.Close();
            return rs;
        }
    }
}
