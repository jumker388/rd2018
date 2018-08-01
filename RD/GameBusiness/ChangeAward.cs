using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web365Domain;

namespace GameBusiness
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
        public GameHistoryResult ShowAll(long uid, bool approval, int skip, int take)
        {
            var rs = new GameHistoryResult();
            var lst = new List<GameHistoryItem>();
            int totalRecord = 0;
            int trans_type = 16;//21
            if(approval)
            {
                trans_type = 16;
            }else
            {
                trans_type = 21;
            }            
            string sql = "SELECT a.*, b.username, b.fullname FROM newDB.game_history a inner join newDB.view_user_info b on a.user_id = b.id WHERE a.trans_type = @trans_type ORDER BY id DESC LIMIT @skip, @take";
            string sqlTotalQuery = "SELECT count(*) as totalRecord FROM newDB.game_history WHERE trans_type = @trans_type";
            if(uid > 0)
            {
                sql = "SELECT a.*, b.username, b.fullname FROM newDB.game_history a inner join newDB.view_user_info b on a.user_id = b.id WHERE a.trans_type = @trans_type and a.user_id = @user_id ORDER BY id DESC LIMIT @skip, @take";
                sqlTotalQuery = "SELECT count(*) as totalRecord FROM newDB.game_history WHERE trans_type = @trans_type and a.user_id = @user_id";
                MySqlConnection conn = new MySqlConnection(Constants.DBConnection);
                MySqlCommand MyCommand = new MySqlCommand(sql, conn);
                MyCommand.Parameters.AddWithValue("trans_type", trans_type);
                MyCommand.Parameters.AddWithValue("user_id", uid);
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
                MyCommand.Parameters.AddWithValue("trans_type", trans_type);
                MyCommand.Parameters.AddWithValue("user_id", uid);
                MyReader = MyCommand.ExecuteReader();
                while (MyReader.Read())
                {
                    totalRecord = MyReader.GetInt32("totalRecord");
                }
                conn.Close();
            }else
            {
                MySqlConnection conn = new MySqlConnection(Constants.DBConnection);
                MySqlCommand MyCommand = new MySqlCommand(sql, conn);
                MyCommand.Parameters.AddWithValue("trans_type", trans_type);                
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
                MyCommand.Parameters.AddWithValue("trans_type", trans_type);                
                MyReader = MyCommand.ExecuteReader();
                while (MyReader.Read())
                {
                    totalRecord = MyReader.GetInt32("totalRecord");
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
            if(approval)
            {
                //lấy mã thẻ, cập nhật history, chèn sms offline
                int valueCard = 0;
                int telcoId = 0;
                int user_id = 0;
                string sql = "SELECT user_id, valueCard, telcoId FROM newDB.game_history WHERE id = @id";
                MySqlConnection conn = new MySqlConnection(Constants.DBConnection);
                MySqlCommand MyCommand = new MySqlCommand(sql, conn);
                MyCommand.Parameters.AddWithValue("id", id);
                MySqlDataReader MyReader;
                conn.Open();
                MyReader = MyCommand.ExecuteReader();
                while (MyReader.Read())
                {
                    valueCard = MyReader.GetInt32("valueCard");
                    telcoId = MyReader.GetInt32("telcoId");
                    user_id = MyReader.GetInt32("user_id");
                }
                MyReader.Close();
                //lấy mã thẻ từ hệ thống
                string sqlCardInfo = "SELECT * FROM portal.exchangeCardInfo WHERE value = '" + valueCard + "' AND telcoId = '" + telcoId + "' AND used = 0 LIMIT 0,1";
                MyCommand = new MySqlCommand(sqlCardInfo, conn);
                MyReader = MyCommand.ExecuteReader();
                int cardId = 0;
                string cardSerial = "";
                string cardNo = "";
                bool hetThe = true;
                while (MyReader.Read())
                {
                    cardId = MyReader.GetInt32("id");
                    cardSerial = MyReader.GetString("serial");
                    cardNo = MyReader.GetString("cardNo");
                    hetThe = false;
                }
                MyReader.Close();
                if(hetThe)
                {
                    rs = false;
                }
                else
                {
                    //cập nhật mã thẻ đã sử dụng
                    string sqlUpdateCard = "UPDATE portal.exchangeCardInfo set used = 1, dateUse='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' WHERE id='" + cardId + "'";
                    MyCommand = new MySqlCommand(sqlUpdateCard, conn);
                    int rowUpdate = MyCommand.ExecuteNonQuery();
                    
                    if(rowUpdate > 0)
                    {
                        //update thẻ này được sử dụng cho user nào
                        string sqlUpdateLog = "INSERT INTO portal.exchangeHistory(cardId, userId) VALUES ('" + cardId + "','" + user_id + "')";
                        MyCommand = new MySqlCommand(sqlUpdateLog, conn);
                        rowUpdate = MyCommand.ExecuteNonQuery();
                        //cập nhật lại bản ghi là đã duyệt
                        string sqlUpdateHistory = "UPDATE newDB.game_history set description = @description, trans_type = @trans_type WHERE id = @id";
                        MyCommand = new MySqlCommand(sqlUpdateHistory, conn);
                        MyCommand.Parameters.AddWithValue("description", "Duyệt đổi thẻ cào " + cardSerial);
                        MyCommand.Parameters.AddWithValue("trans_type", 16);                        
                        MyCommand.Parameters.AddWithValue("id", id);
                        rowUpdate = MyCommand.ExecuteNonQuery();
                        //cập nhật vào mess offline
                        string telcoName = "";
                        sql = "select * from portal.exchangeTelco WHERE id = '" + telcoId + "'";
                        MyCommand = new MySqlCommand(sql, conn);
                        MyReader = MyCommand.ExecuteReader();
                        while (MyReader.Read())
                        {
                            telcoName = MyReader.GetString("name");
                        }
                        MyReader.Close();
                        sql = "insert into newDB.offlinemessage (userIDSend,userIDReceive,mes,datetimeSend) values (@userIDSend,@userIDReceive,@mes,now());";
                        MyCommand = new MySqlCommand(sql, conn);
                        MyCommand.Parameters.AddWithValue("userIDSend", 8934);
                        MyCommand.Parameters.AddWithValue("userIDReceive", user_id);
                        MyCommand.Parameters.AddWithValue("mes", "Duyệt đổi thẻ, Nhà mạng " + telcoName + ", Mệnh giá " + valueCard + " VND, Mã thẻ: " + cardNo + ", Serial: " + cardSerial + " lúc " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                        rowUpdate = MyCommand.ExecuteNonQuery();
                        if(rowUpdate > 0)
                        {
                            rs = true;
                        }
                    }
                }
                conn.Close();
            }else
            {
                //hoàn trả tiền
                long cashReturn = 0;
                int user_id = 0;
                string sql = "SELECT * FROM newDB.game_history WHERE id = @id";
                MySqlConnection conn = new MySqlConnection(Constants.DBConnection);
                MySqlCommand MyCommand = new MySqlCommand(sql, conn);
                MyCommand.Parameters.AddWithValue("id", id);
                MySqlDataReader MyReader;
                conn.Open();
                MyReader = MyCommand.ExecuteReader();
                while (MyReader.Read())
                {
                    cashReturn = MyReader.GetInt32("cash");
                    user_id = MyReader.GetInt32("user_id");            
                }
                MyReader.Close();
                if(cashReturn < 0)
                {
                    cashReturn = cashReturn * (-1);
                }
                //lấy ra số tiền hiện tại của user
                long currentCash = 0;
                sql = "SELECT gameCash FROM newDB.g_user WHERE user_id = @user_id";
                MyCommand = new MySqlCommand(sql, conn);
                MyCommand.Parameters.AddWithValue("user_id", user_id);
                MyReader = MyCommand.ExecuteReader();
                while (MyReader.Read())
                {
                    currentCash = MyReader.GetInt64("gameCash");       
                }
                MyReader.Close();
                //Cập nhật số tiền
                string sqlUpdateCash = "UPDATE newDB.g_user SET  gameCash = gameCash + " + cashReturn + " WHERE user_id='" + user_id + "' ";
                MyCommand = new MySqlCommand(sqlUpdateCash, conn);
                int rowUpdate = MyCommand.ExecuteNonQuery();
                if(rowUpdate > 0)
                {
                    long beforCash = currentCash;
                    long afterCash = beforCash + cashReturn;
                    sql = "INSERT into newDB.game_history (user_id,cash,current_cash,description,game_id,trans_type,tax,taxPercent,before_cash) values (@user_id,@cash,@current_cash,@description,@game_id, @trans_type,@tax,@taxPercent,@before_cash);";
                    MyCommand = new MySqlCommand(sql, conn);
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
                        sql = "DELETE from newDB.game_history where id = @id";
                        MyCommand = new MySqlCommand(sql, conn);
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
    }
}
