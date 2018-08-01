using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web365Domain;
using Web365Domain.Game;

namespace Web365Business.Game
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
            const string sql = "select * from [newDB].[dbo].[game]";
            var conn = new SqlConnection(Constants.DBConnection);
            var myCommand = new SqlCommand(sql, conn);
            SqlDataReader MyReader;
            conn.Open();
            MyReader = myCommand.ExecuteReader();
            while (MyReader.Read())
            {
                var item = new GameItem
                {
                    zoneID = MyReader.GetInt32(MyReader.GetOrdinal("id")),
                    name = MyReader.GetString(MyReader.GetOrdinal("name")),
                    displayStatus = MyReader.GetInt32(MyReader.GetOrdinal("displayStatus")),
                    gameOrder = MyReader.GetInt32(MyReader.GetOrdinal("gameOrder"))
                };
                rs.Add(item);
            }
            conn.Close();
            return rs;
        }

        public GameItem SelectOne(int id)
        {
            var user = new GameItem();
            var sql = "select * from [portal].[dbo].game where id = " + id;
            var conn = new SqlConnection(Constants.DBConnection);
            var myCommand = new SqlCommand(sql, conn);
            SqlDataReader MyReader;
            conn.Open();
            MyReader = myCommand.ExecuteReader();
            while (MyReader.Read())
            {
                user.zoneID = MyReader.GetInt32(MyReader.GetOrdinal("id"));
                user.name = MyReader.GetString(MyReader.GetOrdinal("name"));
                user.displayStatus = MyReader.GetInt32(MyReader.GetOrdinal("displayStatus"));
                user.gameOrder = MyReader.GetInt32(MyReader.GetOrdinal("gameOrder"));
            }
            //close reader cũ
            MyReader.Close();
            conn.Close();

            return user;
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
            var rowAffected = 0;
            var sql = "update [portal].[dbo].game set name=@name, displayStatus = @displayStatus, gameOrder = @gameOrder where id = @id";
            var conn = new SqlConnection(Constants.DBConnection);
            var MyCommand = new SqlCommand(sql, conn);
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
            var totalRecord = 0;
            var sql = "SELECT a.*, b.username, b.fullname FROM [portal].[dbo].game_history a inner join [portal].[dbo].view_user_info b on a.user_id = b.id WHERE game_id = @zoneId ORDER BY id DESC OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY";
            var sqlTotalQuery = "SELECT count(*) as totalRecord FROM [portal].[dbo].game_history WHERE game_id = @zoneId";
            var conn = new SqlConnection(Constants.DBConnection);
            var MyCommand = new SqlCommand(sql, conn);
            MyCommand.Parameters.AddWithValue("zoneId", zoneId);
            MyCommand.Parameters.AddWithValue("skip", skip);
            MyCommand.Parameters.AddWithValue("take", take);
            SqlDataReader MyReader;
            conn.Open();
            MyReader = MyCommand.ExecuteReader();
            while (MyReader.Read())
            {
                var item1 = new GameHistoryItem
                {
                    id = MyReader.GetInt32(MyReader.GetOrdinal("id")),
                    user_id = MyReader.GetInt32(MyReader.GetOrdinal("user_id")),
                    cash = MyReader.GetInt64(MyReader.GetOrdinal("cash")),
                    current_cash = MyReader.GetInt64(MyReader.GetOrdinal("current_cash")),
                    description = MyReader.GetString(MyReader.GetOrdinal("description")),
                    game_id = MyReader.GetInt32(MyReader.GetOrdinal("game_id")),
                    trans_type = MyReader.GetInt32(MyReader.GetOrdinal("trans_type")),
                    time = MyReader.GetDateTime(MyReader.GetOrdinal("time")),
                    before_cash = MyReader.GetInt64(MyReader.GetOrdinal("before_cash")),
                    username = MyReader.GetString(MyReader.GetOrdinal("username")),
                    fullname = MyReader.GetString(MyReader.GetOrdinal("fullname"))
                };
                lst.Add(item1);
            }
            //close reader cũ
            MyReader.Close();
            MyCommand = new SqlCommand(sqlTotalQuery, conn);
            MyCommand.Parameters.AddWithValue("zoneId", zoneId);
            MyReader = MyCommand.ExecuteReader();
            while (MyReader.Read())
            {
                totalRecord = MyReader.GetInt32(MyReader.GetOrdinal("totalRecord"));
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
            var sql = "select top " + top + " b.id,b.username, b.fullname,sum(a.cash) as total_cash_win from [portal].[dbo].game_history a inner join [portal].[dbo].view_user_info b on a.user_id = b.id where a.cash > 0 AND (trans_type=1 or trans_type=18) AND game_id = @zoneId" +
                        " group by a.user_id order by total_cash_win desc";
            var conn = new SqlConnection(Constants.DBConnection);
            var MyCommand = new SqlCommand(sql, conn);
            MyCommand.Parameters.AddWithValue("zoneId", zoneId);
            MyCommand.Parameters.AddWithValue("top", top);
            SqlDataReader MyReader;
            conn.Open();
            MyReader = MyCommand.ExecuteReader();
            while (MyReader.Read())
            {
                var u = new UserInfo();
                u.uid = MyReader.GetInt64(MyReader.GetOrdinal("id"));
                u.userName = MyReader.GetString(MyReader.GetOrdinal("username"));
                u.fullName = MyReader.GetString(MyReader.GetOrdinal("fullname"));
                u.cashWin = MyReader.GetInt64(MyReader.GetOrdinal("total_cash_win"));
                rs.Add(u);
            }
            MyReader.Close();
            conn.Close();
            return rs;
        }

        public List<UserInfo> RealTimeAccPlayingByGameId(out int total, int gameId, int top)
        {
            total = 0;
            var rs = new List<UserInfo>();
            var sql = "select top "+ top +" b.username, b.fullname from [portal].[dbo].play_log a inner join [portal].[dbo].view_user_info b on a.user_id = b.id where is_play = 1 and a.game_id = @gameId" +
                        " group by b.username, b.fullname";
            var sqlTotalQuery =
                "select count(a.id) as totalRecord from [portal].[dbo].play_log a inner join [portal].[dbo].view_user_info b on a.user_id = b.id where is_play = 1 and a.game_id = @gameId group by a.user_id";

            SqlConnection conn = new SqlConnection(Constants.DBConnection);
            SqlCommand MyCommand = new SqlCommand(sql, conn);
            MyCommand.Parameters.AddWithValue("gameId", gameId);
            MyCommand.Parameters.AddWithValue("top", top);
            SqlDataReader MyReader;
            conn.Open();
            MyReader = MyCommand.ExecuteReader();
            while (MyReader.Read())
            {
                var u = new UserInfo
                {
                    userName = MyReader.GetString(MyReader.GetOrdinal("username")),
                    fullName = MyReader.GetString(MyReader.GetOrdinal("fullname"))
                };
                rs.Add(u);
            }
            MyReader.Close();

            MyCommand = new SqlCommand(sqlTotalQuery, conn);
            MyCommand.Parameters.AddWithValue("gameId", gameId);
            MyReader = MyCommand.ExecuteReader();
            while (MyReader.Read())
            {
                total = MyReader.GetInt32(MyReader.GetOrdinal("totalRecord"));
            }
            conn.Close();
            conn.Close();
            return rs;
        }

        public List<UserInfo> GetTopGem()
        {
            var rs = new List<UserInfo>();
            var sql = "SELECT top 100 * FROM [portal].[dbo].view_user_info ORDER BY gameCash DESC";
            SqlConnection conn = new SqlConnection(Constants.DBConnection);
            SqlCommand MyCommand = new SqlCommand(sql, conn);
            SqlDataReader MyReader;
            conn.Open();
            MyReader = MyCommand.ExecuteReader();
            while (MyReader.Read())
            {
                var u = new UserInfo
                {
                    uid = MyReader.GetInt64(MyReader.GetOrdinal("id")),
                    userName = MyReader.GetString(MyReader.GetOrdinal("username")),
                    fullName = MyReader.GetString(MyReader.GetOrdinal("fullname")),
                    cashWin = MyReader.GetInt64(MyReader.GetOrdinal("gameCash"))
                };
                rs.Add(u);
            }
            MyReader.Close();
            conn.Close();
            return rs;
        }

        public List<UserInfo> GetTopXu()
        {
            var rs = new List<UserInfo>();
            var sql = "SELECT top 100 * FROM [portal].[dbo].view_user_info ORDER BY vCash DESC";
            SqlConnection conn = new SqlConnection(Constants.DBConnection);
            SqlCommand MyCommand = new SqlCommand(sql, conn);
            SqlDataReader MyReader;
            conn.Open();
            MyReader = MyCommand.ExecuteReader();
            while (MyReader.Read())
            {
                var u = new UserInfo
                {
                    uid = MyReader.GetInt64(MyReader.GetOrdinal("id")),
                    userName = MyReader.GetString(MyReader.GetOrdinal("username")),
                    fullName = MyReader.GetString(MyReader.GetOrdinal("fullname")),
                    cashWin = MyReader.GetInt64(MyReader.GetOrdinal("vCash"))
                };
                rs.Add(u);
            }
            MyReader.Close();
            conn.Close();
            return rs;
        }

        public List<UserInfo> GetTopExp()
        {
            var rs = new List<UserInfo>();
            var sql = "SELECT top 100 * FROM [portal].[dbo].view_user_info ORDER BY EXP DESC";
            SqlConnection conn = new SqlConnection(Constants.DBConnection);
            SqlCommand MyCommand = new SqlCommand(sql, conn);
            SqlDataReader MyReader;
            conn.Open();
            MyReader = MyCommand.ExecuteReader();
            while (MyReader.Read())
            {
                var u = new UserInfo
                {
                    uid = MyReader.GetInt64(MyReader.GetOrdinal("id")),
                    userName = MyReader.GetString(MyReader.GetOrdinal("username")),
                    fullName = MyReader.GetString(MyReader.GetOrdinal("fullname")),
                    cashWin = MyReader.GetInt64(MyReader.GetOrdinal("EXP"))
                };
                rs.Add(u);
            }
            MyReader.Close();
            conn.Close();
            return rs;
        }

        public List<UserInfo> GetTopDoiThuong()
        {
            var rs = new List<UserInfo>();
            var sql = "SELECT b.id, b.fullname, b.username, ABS(SUM(a.cash)) as cashWin FROM [portal].[dbo].game_history a INNER JOIN [portal].[dbo].view_user_info b ON a.user_id = b.id WHERE trans_type = 16 AND a.cash < 0 GROUP BY b.id, b.fullname, b.username ORDER BY ABS(SUM(a.cash)) DESC";
            SqlConnection conn = new SqlConnection(Constants.DBConnection);
            SqlCommand MyCommand = new SqlCommand(sql, conn);
            SqlDataReader MyReader;
            conn.Open();
            MyReader = MyCommand.ExecuteReader();
            while (MyReader.Read())
            {
                var u = new UserInfo
                {
                    uid = MyReader.GetInt64(MyReader.GetOrdinal("id")),
                    userName = MyReader.GetString(MyReader.GetOrdinal("username")),
                    fullName = MyReader.GetString(MyReader.GetOrdinal("fullname")),
                    cashWin = MyReader.GetInt64(MyReader.GetOrdinal("cashWin"))
                };
                rs.Add(u);
            }
            MyReader.Close();
            conn.Close();
            return rs;
        }

        public List<UserInfo> GetTopNapTien()
        {
            var rs = new List<UserInfo>();
            var sql = "SELECT b.id, b.fullname, b.username, ABS(SUM(a.cash)) AS cashWin FROM [portal].[dbo].game_history a INNER JOIN [portal].[dbo].view_user_info b ON a.user_id = b.id WHERE trans_type = 4 AND a.cash > 0 GROUP BY b.id, b.fullname, b.username ORDER BY ABS(SUM(a.cash)) DESC";
            SqlConnection conn = new SqlConnection(Constants.DBConnection);
            SqlCommand MyCommand = new SqlCommand(sql, conn);
            SqlDataReader MyReader;
            conn.Open();
            MyReader = MyCommand.ExecuteReader();
            while (MyReader.Read())
            {
                var u = new UserInfo
                {
                    uid = MyReader.GetInt64(MyReader.GetOrdinal("id")),
                    userName = MyReader.GetString(MyReader.GetOrdinal("username")),
                    fullName = MyReader.GetString(MyReader.GetOrdinal("fullname")),
                    cashWin = MyReader.GetInt64(MyReader.GetOrdinal("cashWin"))
                };
                rs.Add(u);
            }
            MyReader.Close();
            conn.Close();
            return rs;
        }

        public List<UserInfo> GetGiaoDich(out int total, int type, int skip, int take, int uid = 0)
        {
            var rs = new List<UserInfo>();
            var sql = "SELECT b.id, b.fullname, b.username, a.description, a.time, a.trans_type FROM [portal].[dbo].game_history a INNER JOIN [portal].[dbo].view_user_info b ON a.user_id = b.id WHERE a.trans_type = " + type;

            var sqlTotalQuery = " SELECT Count(a.id) as totalRecord FROM [portal].[dbo].game_history a WHERE a.trans_type = " + type;
            if (uid > 0)
            {
                sql += " AND user_id = " + uid;
                sqlTotalQuery += " AND user_id = " + uid;
            }

            sql += " ORDER BY a.id desc OFFSET "+ skip +" ROWS FETCH NEXT "+ take + " ROWS ONLY";

            var conn = new SqlConnection(Constants.DBConnection);
            var MyCommand = new SqlCommand(sql, conn);
            SqlDataReader MyReader;
            conn.Open();
            MyReader = MyCommand.ExecuteReader();
            while (MyReader.Read())
            {
                var u = new UserInfo
                {
                    uid = MyReader.GetInt64(MyReader.GetOrdinal("id")),
                    userName = MyReader.GetString(MyReader.GetOrdinal("username")),
                    fullName = MyReader.GetString(MyReader.GetOrdinal("fullname")),
                    description = MyReader.GetString(MyReader.GetOrdinal("description")),
                    dateRegister = MyReader.GetString(MyReader.GetOrdinal("time")),
                    typyPlay = MyReader.GetInt32(MyReader.GetOrdinal("trans_type"))

                };
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

            conn.Close();
            return rs;
        }

        public List<PaymentItem> GetPayment(out int total, int skip, int take, int uid = 0, string username = "", string date = "", string status = "")
        {
          
            var rs = new List<PaymentItem>();
            var sql = "SELECT us.username, us.fullname, pay.id, pay.buyer_uid, pay.buyer_fullname, pay.buyer_mobile, pay.total_amount, pay.payment_method, pay.bank_code, pay.transaction_status, pay.transaction_id, pay.time_request, pay.time_receive, pay.token FROM [portal].[dbo].a_bankcharge pay " +
                      " LEFT JOIN [portal].[dbo].user us ON  pay.buyer_uid = us.id where pay.id > 0 ";

            var sqlTotalQuery = "SELECT Count(pay.id) as totalRecord FROM [portal].[dbo].a_bankcharge pay LEFT JOIN [portal].[dbo].user us ON  pay.buyer_uid = us.id where pay.id > 0 ";
            if (uid > 0)
            {
                sql += " AND pay.buyer_uid = " + uid;
                sqlTotalQuery += " AND pay.buyer_uid = " + uid;
            }

            if (!string.IsNullOrEmpty(username))
            {
                sql += " AND (LOWER(pay.buyer_mobile) LIKE '%" + username.ToLower() + "%' or LOWER(pay.buyer_fullname) LIKE '%" + username.ToLower() + "%' or LOWER(us.username) LIKE '%" + username.ToLower() + "%' or LOWER(us.fullname) LIKE '%" + username.ToLower() + "%')";
                sqlTotalQuery += " AND (LOWER(pay.buyer_mobile) LIKE '%" + username.ToLower() + "%' or LOWER(pay.buyer_fullname) LIKE '%" + username.ToLower() + "%' or LOWER(us.username) LIKE '%" + username.ToLower() + "%' or LOWER(us.fullname) LIKE '%" + username.ToLower() + "%')";
            }

            if (!string.IsNullOrEmpty(date))
            {
                var datex = Convert.ToDateTime(date).ToString(Constants.DateFormat);
                sql += " AND time_request >= '" + datex + "'";
                sqlTotalQuery += " and time_request >= '" + datex + "'";
            }

            if (!string.IsNullOrEmpty(status))
            {
                if (status == "00")
                {
                    sql += " AND transaction_status = '00'";
                    sqlTotalQuery += " and transaction_status = '00'";   
                }
                else
                {
                    sql += " AND transaction_status <> '00'";
                    sqlTotalQuery += " and transaction_status <> '00'"; 
                }
            }

            sql += " ORDER BY pay.id desc OFFSET "+ skip +" ROWS FETCH NEXT "+take+" ROWS ONLY";

            SqlConnection conn = new SqlConnection(Constants.DBConnection);
            SqlCommand MyCommand = new SqlCommand(sql, conn);
            SqlDataReader MyReader;
            conn.Open();
            MyReader = MyCommand.ExecuteReader();
            while (MyReader.Read())
            {
                var u = new PaymentItem();
                u.id = MyReader.GetInt32(MyReader.GetOrdinal("id"));
                u.buyer_mobile = MyReader.GetString(MyReader.GetOrdinal("buyer_mobile"));
                u.total_amount = MyReader.GetInt32(MyReader.GetOrdinal("total_amount"));
                u.buyer_mobile = MyReader.GetString(MyReader.GetOrdinal("buyer_mobile"));
                u.buyer_fullname = MyReader.GetString(MyReader.GetOrdinal("buyer_fullname"));
                u.payment_method = MyReader.GetString(MyReader.GetOrdinal("payment_method"));
                u.bank_code = MyReader.GetString(MyReader.GetOrdinal("bank_code"));
                u.transaction_status = MyReader.GetString(MyReader.GetOrdinal("transaction_status"));
                u.token = MyReader.GetString(MyReader.GetOrdinal("token"));
                  // index start 0

                if (!MyReader.IsDBNull(3))
                {
                    u.buyer_uid = MyReader.GetInt32(MyReader.GetOrdinal("buyer_uid"));
                }

                if (!MyReader.IsDBNull(11))
                {
                    u.time_request = MyReader.GetDateTime(MyReader.GetOrdinal("time_request"));
                    u.time_request_string = u.time_request.ToShortDateString();
                }
                if (!MyReader.IsDBNull(12))
                {
                    u.time_receive = MyReader.GetDateTime(MyReader.GetOrdinal("time_receive"));
                    u.time_receive_string = u.time_receive.ToShortDateString();
                }
                if (!MyReader.IsDBNull(10))
                {
                    u.transaction_id = MyReader.GetString(MyReader.GetOrdinal("transaction_id"));
                }
                if (!MyReader.IsDBNull(0))
                {
                    u.username = MyReader.GetString(MyReader.GetOrdinal("username"));
                }
                if (!MyReader.IsDBNull(1))
                {
                    u.fullname = MyReader.GetString(MyReader.GetOrdinal("fullname"));
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

            conn.Close();
            return rs;
        }

        public PaymentItem PaymentDetail(int id)
        {

            var rs = new PaymentItem();
            var sql = "SELECT us.username, us.fullname, pay.id, pay.buyer_uid, pay.buyer_fullname, pay.buyer_mobile, pay.total_amount, pay.payment_method, pay.bank_code, pay.transaction_status, pay.transaction_id, pay.time_request, pay.time_receive, pay.token FROM [portal].[dbo].a_bankcharge pay " +
                      " LEFT JOIN [portal].[dbo].user us ON  pay.buyer_uid = us.id where pay.id = " + id;


            var conn = new SqlConnection(Constants.DBConnection);
            var MyCommand = new SqlCommand(sql, conn);
            SqlDataReader MyReader;
            conn.Open();
            MyReader = MyCommand.ExecuteReader();
            while (MyReader.Read())
            {
                var u = new PaymentItem
                {
                    id = MyReader.GetInt32(MyReader.GetOrdinal("id")),
                    buyer_mobile = MyReader.GetString(MyReader.GetOrdinal("buyer_mobile")),
                    total_amount = MyReader.GetInt32(MyReader.GetOrdinal("total_amount"))
                };
                u.buyer_mobile = MyReader.GetString(MyReader.GetOrdinal("buyer_mobile"));
                u.buyer_fullname = MyReader.GetString(MyReader.GetOrdinal("buyer_fullname"));
                u.payment_method = MyReader.GetString(MyReader.GetOrdinal("payment_method"));
                u.bank_code = MyReader.GetString(MyReader.GetOrdinal("bank_code"));
                u.transaction_status = MyReader.GetString(MyReader.GetOrdinal("transaction_status"));
                u.token = MyReader.GetString(MyReader.GetOrdinal("token"));
                // index start 0

                if (!MyReader.IsDBNull(3))
                {
                    u.buyer_uid = MyReader.GetInt32(MyReader.GetOrdinal("buyer_uid"));
                }

                if (!MyReader.IsDBNull(11))
                {
                    u.time_request = MyReader.GetDateTime(MyReader.GetOrdinal("time_request"));
                    u.time_request_string = u.time_request.ToShortDateString();
                }
                if (!MyReader.IsDBNull(12))
                {
                    u.time_receive = MyReader.GetDateTime(MyReader.GetOrdinal("time_receive"));
                    u.time_receive_string = u.time_receive.ToShortDateString();
                }
                if (!MyReader.IsDBNull(10))
                {
                    u.transaction_id = MyReader.GetString(MyReader.GetOrdinal("transaction_id"));
                }
                if (!MyReader.IsDBNull(0))
                {
                    u.username = MyReader.GetString(MyReader.GetOrdinal("username"));
                }
                if (!MyReader.IsDBNull(1))
                {
                    u.fullname = MyReader.GetString(MyReader.GetOrdinal("fullname"));
                }

                rs = u;
            }
            MyReader.Close();

            conn.Close();
            return rs;
        }

    }
}


