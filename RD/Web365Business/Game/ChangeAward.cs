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
    public class ChangeAward
    {
        /// <summary>
        /// Danh sách đổi thưởng theo từng user, được duyệt hay chưa duyệt
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="approval"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public GameHistoryResult ShowAll(long uid, int transtype, int skip, int take)
        {
            var rs = new GameHistoryResult();
            var lst = new List<GameHistoryItem>();
            int totalRecord = 0;
            var sql = "SELECT a.*, b.username, b.fullname FROM [newDB].[dbo].game_history a inner join [portal].[dbo].view_user_info b on a.user_id = b.id WHERE a.trans_type = @trans_type ORDER BY id DESC OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY";
            var sqlTotalQuery = "SELECT count(*) as totalRecord FROM [newDB].[dbo].game_history WHERE trans_type = @trans_type";
            if (uid > 0)
            {
                sql = "SELECT a.*, b.username, b.fullname FROM [newDB].[dbo].game_history a inner join [portal].[dbo].view_user_info b on a.user_id = b.id WHERE a.trans_type = @trans_type and a.user_id = @user_id ORDER BY id DESC OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY";
                sqlTotalQuery = "SELECT count(*) as totalRecord FROM [newDB].[dbo].game_history a WHERE trans_type = @trans_type and a.user_id = @user_id";
                var conn = new SqlConnection(Constants.DBConnection);
                var MyCommand = new SqlCommand(sql, conn);
                MyCommand.Parameters.AddWithValue("trans_type", transtype);
                MyCommand.Parameters.AddWithValue("user_id", uid);
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
                    item1.timestring = item1.time.Value.ToShortDateString();
                    lst.Add(item1);
                }
                //close reader cũ
                MyReader.Close();
                MyCommand = new SqlCommand(sqlTotalQuery, conn);
                MyCommand.Parameters.AddWithValue("trans_type", transtype);
                MyCommand.Parameters.AddWithValue("user_id", uid);
                MyReader = MyCommand.ExecuteReader();
                while (MyReader.Read())
                {
                    totalRecord = MyReader.GetInt32(MyReader.GetOrdinal("totalRecord"));
                }
                conn.Close();
            }
            else
            {
                var conn = new SqlConnection(Constants.DBConnection);
                var MyCommand = new SqlCommand(sql, conn);
                MyCommand.Parameters.AddWithValue("trans_type", transtype);
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
                    item1.timestring = item1.time.Value.ToShortDateString();
                    lst.Add(item1);
                }
                //close reader cũ
                MyReader.Close();
                MyCommand = new SqlCommand(sqlTotalQuery, conn);
                MyCommand.Parameters.AddWithValue("trans_type", transtype);
                MyReader = MyCommand.ExecuteReader();
                while (MyReader.Read())
                {
                    totalRecord = MyReader.GetInt32(MyReader.GetOrdinal("totalRecord"));
                }
                conn.Close();
            }
            rs.data = lst;
            rs.totalRecord = totalRecord;
            return rs;
        }

        /// <summary>
        /// Duyệt một yêu cầu đổi thưởng nào đó
        /// </summary>
        /// <param name="id"></param>
        /// <param name="approval"></param>
        /// <returns></returns>
        public bool Approval(long id, bool approval)
        {
            var rs = false;
            //
            if (approval)
            {
                //lấy mã thẻ, cập nhật history, chèn sms offline
                var valueCard = 0;
                var telcoId = 0;
                var user_id = 0;
                var sql = "SELECT user_id, valueCard, telcoId FROM [newDB].[dbo].game_history WHERE id = @id";
                var conn = new SqlConnection(Constants.DBConnection);
                var MyCommand = new SqlCommand(sql, conn);
                MyCommand.Parameters.AddWithValue("id", id);
                SqlDataReader MyReader;
                conn.Open();
                MyReader = MyCommand.ExecuteReader();
                while (MyReader.Read())
                {
                    valueCard = MyReader.GetInt32(MyReader.GetOrdinal("valueCard"));
                    telcoId = MyReader.GetInt32(MyReader.GetOrdinal("telcoId"));
                    user_id = MyReader.GetInt32(MyReader.GetOrdinal("user_id"));
                }
                MyReader.Close();
                //lấy mã thẻ từ hệ thống
                var sqlCardInfo = "SELECT top 1 * FROM [portal].[dbo].exchangeCardInfo WHERE value = '" + valueCard + "' AND telcoId = '" + telcoId + "' AND used = 0";
                MyCommand = new SqlCommand(sqlCardInfo, conn);
                MyReader = MyCommand.ExecuteReader();
                var cardId = 0;
                var cardSerial = "";
                var cardNo = "";
                var hetThe = true;
                while (MyReader.Read())
                {
                    cardId = MyReader.GetInt32(MyReader.GetOrdinal("id"));
                    cardSerial = MyReader.GetString(MyReader.GetOrdinal("serial"));
                    cardNo = MyReader.GetString(MyReader.GetOrdinal("cardNo"));
                    hetThe = false;
                }
                MyReader.Close();
                if (hetThe)
                {
                    rs = false;
                }
                else
                {
                    //cập nhật mã thẻ đã sử dụng
                    string sqlUpdateCard = "UPDATE [portal].[dbo].exchangeCardInfo set used = 1, dateUse='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' WHERE id='" + cardId + "'";
                    MyCommand = new SqlCommand(sqlUpdateCard, conn);
                    int rowUpdate = MyCommand.ExecuteNonQuery();

                    if (rowUpdate > 0)
                    {
                        //update thẻ này được sử dụng cho user nào
                        var sqlUpdateLog = "INSERT INTO [portal].[dbo].exchangeHistory(cardId, userId) VALUES ('" + cardId + "','" + user_id + "')";
                        MyCommand = new SqlCommand(sqlUpdateLog, conn);
                        rowUpdate = MyCommand.ExecuteNonQuery();
                        //cập nhật lại bản ghi là đã duyệt
                        var sqlUpdateHistory = "UPDATE [newDB].[dbo].game_history set description = @description, trans_type = @trans_type WHERE id = @id";
                        MyCommand = new SqlCommand(sqlUpdateHistory, conn);
                        MyCommand.Parameters.AddWithValue("description", "Duyệt đổi thẻ cào " + cardSerial);
                        MyCommand.Parameters.AddWithValue("trans_type", 16);
                        MyCommand.Parameters.AddWithValue("id", id);
                        rowUpdate = MyCommand.ExecuteNonQuery();
                        //cập nhật vào mess offline
                        var telcoName = "";
                        sql = "select * from [portal].[dbo].exchangeTelco WHERE id = '" + telcoId + "'";
                        MyCommand = new SqlCommand(sql, conn);
                        MyReader = MyCommand.ExecuteReader();
                        while (MyReader.Read())
                        {
                            telcoName = MyReader.GetString(MyReader.GetOrdinal("name"));
                        }
                        MyReader.Close();
                        sql = "insert into [newDB].[dbo].offlinemessage (userIDSend,userIDReceive,mes,datetimeSend) values (@userIDSend,@userIDReceive,@mes,now());";
                        MyCommand = new SqlCommand(sql, conn);
                        MyCommand.Parameters.AddWithValue("userIDSend", 8934);
                        MyCommand.Parameters.AddWithValue("userIDReceive", user_id);
                        MyCommand.Parameters.AddWithValue("mes", "Duyệt đổi thẻ, Nhà mạng " + telcoName + ", Mệnh giá " + valueCard + " VND, Mã thẻ: " + cardNo + ", Serial: " + cardSerial + " lúc " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                        rowUpdate = MyCommand.ExecuteNonQuery();
                        if (rowUpdate > 0)
                        {
                            rs = true;
                        }
                    }
                }
                conn.Close();
            }
            else
            {
                //hoàn trả tiền
                long cashReturn = 0;
                var user_id = 0;
                var sql = "SELECT * FROM [newDB].[dbo].game_history WHERE id = @id";
                var conn = new SqlConnection(Constants.DBConnection);
                var MyCommand = new SqlCommand(sql, conn);
                MyCommand.Parameters.AddWithValue("id", id);
                SqlDataReader MyReader;
                conn.Open();
                MyReader = MyCommand.ExecuteReader();
                while (MyReader.Read())
                {
                    cashReturn = MyReader.GetInt32(MyReader.GetOrdinal("cash"));
                    user_id = MyReader.GetInt32(MyReader.GetOrdinal("user_id"));
                }
                MyReader.Close();
                if (cashReturn < 0)
                {
                    cashReturn = cashReturn * (-1);
                }
                //lấy ra số tiền hiện tại của user
                long currentCash = 0;
                sql = "SELECT gameCash FROM [newDB].[dbo].g_user WHERE user_id = @user_id";
                MyCommand = new SqlCommand(sql, conn);
                MyCommand.Parameters.AddWithValue("user_id", user_id);
                MyReader = MyCommand.ExecuteReader();
                while (MyReader.Read())
                {
                    currentCash = MyReader.GetInt64(MyReader.GetOrdinal("gameCash"));
                }
                MyReader.Close();
                //Cập nhật số tiền
                var sqlUpdateCash = "UPDATE [newDB].[dbo].g_user SET  gameCash = gameCash + " + cashReturn + " WHERE user_id='" + user_id + "' ";
                MyCommand = new SqlCommand(sqlUpdateCash, conn);
                var rowUpdate = MyCommand.ExecuteNonQuery();
                if (rowUpdate > 0)
                {
                    var beforCash = currentCash;
                    var afterCash = beforCash + cashReturn;
                    sql = "INSERT into [newDB].[dbo].game_history (user_id,cash,current_cash,description,game_id,trans_type,tax,taxPercent,before_cash) values (@user_id,@cash,@current_cash,@description,@game_id, @trans_type,@tax,@taxPercent,@before_cash);";
                    MyCommand = new SqlCommand(sql, conn);
                    MyCommand.Parameters.AddWithValue("user_id", user_id);
                    MyCommand.Parameters.AddWithValue("cash", cashReturn);
                    MyCommand.Parameters.AddWithValue("current_cash", afterCash);
                    MyCommand.Parameters.AddWithValue("description", "Hoàn lại tiền vì không duyệt đổi thưởng");
                    MyCommand.Parameters.AddWithValue("game_id", 0);
                    MyCommand.Parameters.AddWithValue("trans_type", 16);
                    MyCommand.Parameters.AddWithValue("tax", 0);
                    MyCommand.Parameters.AddWithValue("taxPercent", 0);
                    MyCommand.Parameters.AddWithValue("before_cash", beforCash);
                    rowUpdate = MyCommand.ExecuteNonQuery();
                    if (rowUpdate > 0)
                    {
                        //xóa record yêu cầu đổi thưởng cũ đi
                        sql = "DELETE from [newDB].[dbo].game_history where id = @id";
                        MyCommand = new SqlCommand(sql, conn);
                        MyCommand.Parameters.AddWithValue("id", id);
                        rowUpdate = MyCommand.ExecuteNonQuery();
                        if (rowUpdate > 0)
                        {
                            rs = true;
                        }
                    }
                }

                conn.Close();
            }
            return rs;
        }

        public GameHistoryResult ExportExcel(DateTime? from, DateTime? to)
        {
            var rs = new GameHistoryResult();
            var lst = new List<GameHistoryItem>();
            int totalRecord = 0;
            var sql = "SELECT a.*, b.username, b.fullname FROM [newDB].[dbo].game_history a inner join [newDB].[dbo].view_user_info b on a.user_id = b.id WHERE a.trans_type = 16";
            if (from != null && to != null)
            {
                sql += " and time >= '" + from.Value.ToString("yyyy-MM-dd HH:mm:ss") + "' and time <= '" + to.Value.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }
            sql += " order by id desc";
            var conn = new SqlConnection(Constants.DBConnection);
            var MyCommand = new SqlCommand(sql, conn);
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
                item1.timestring = item1.time.Value.ToShortDateString();
                lst.Add(item1);
            }
            //close reader cũ
            MyReader.Close();

            rs.data = lst;
            return rs;
        }
    }
}
