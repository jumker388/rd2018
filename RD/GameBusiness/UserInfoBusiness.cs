using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web365Domain;
using Web365Utility;

namespace GameBusiness
{
    public class UserInfoBusiness
    {
        /// <summary>
        /// Danh sách user, sắp xếp theo đăng ký mới nhất
        /// </summary>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <param name="fullname"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <param name="cmnd"></param>
        /// <param name="regFrom"></param>
        /// <param name="regTo"></param>
        /// <param name="mobile"></param>
        /// <param name="active"></param>
        /// <param name="limit"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public UserInfoSearchResult Search(long id, string username, string fullname, string phone, string email, string cmnd, DateTime regFrom, DateTime regTo, int mobile, bool? active, int limit, int take)
        {
            UserInfoSearchResult rs = new UserInfoSearchResult();
            var lst = new List<UserInfo>();
            int totalRecord = 0;
            

            string sql = "select * from newDB.view_user_info where register_date >= '" + regFrom.ToString(Constants.DateFormat) + "' and register_date <= '" + regTo.ToString(Constants.DateFormat) + "'";
            string sqlTotalQuery = "select count(*) as totalRecord from newDB.view_user_info where register_date >= '" + regFrom.ToString(Constants.DateFormat) + "' and register_date <= '" + regTo.ToString(Constants.DateFormat) + "'";
            if(id > 0)
            {
                sql += " and id = " + id.ToString();
                sqlTotalQuery += " and id = " + id.ToString();
            }
            if(!String.IsNullOrEmpty(username))
            {
                sql += " and username like '%" + username + "%'";
                sqlTotalQuery += " and username like '%" + username + "%'";
            }
            if(!String.IsNullOrEmpty(fullname))
            {
                sql += " and fullname like '%" + fullname + "%'";
                sqlTotalQuery += " and fullname like '%" + fullname + "%'";
            }
            if(!String.IsNullOrEmpty(phone))
            {
                sql += " and mobile like '%" + phone + "%'";
                sqlTotalQuery += " and mobile like '%" + phone + "%'";
            }
            if (!String.IsNullOrEmpty(email))
            {
                sql += " and email like '%" + email + "%'";
                sqlTotalQuery += " and email like '%" + email + "%'";
            }
            if (!String.IsNullOrEmpty(cmnd))
            {
                sql += " and cmnd like '%" + cmnd + "%'";
                sqlTotalQuery += " and cmnd like '%" + cmnd + "%'";
            }
            if(mobile > 0)
            {
                sql += " and isMobile = " + mobile.ToString();
                sqlTotalQuery += " and isMobile = " + mobile.ToString();
            }
            if(active != null)
            {
                if(Convert.ToBoolean(active))
                {
                    sql += " and is_active = 1";
                    sqlTotalQuery += " and is_active = 1";
                }
                else
                {
                    sql += " and is_active = 2";
                    sqlTotalQuery += " and is_active = 2";
                }
                
            }
            sql += " order by id desc";
            sql += " limit " + limit + ", " + take;
            MySqlConnection conn = new MySqlConnection(Constants.DBConnection);
            MySqlCommand MyCommand = new MySqlCommand(sql, conn);
            MySqlDataReader MyReader;
            conn.Open();
            MyReader = MyCommand.ExecuteReader();
            while (MyReader.Read())
            {
                UserInfo user = new UserInfo();
                user.uid = MyReader.GetInt64("id");
                user.userName = MyReader.GetString("username");
                user.fullName = MyReader.GetString("fullname");
                user.dateRegister = MyReader.GetDateTime("register_date");
                user.isMale = MyReader.GetInt32("sex") == 1;
                user.level = MyReader.GetInt32("level_id");
                user.cash = MyReader.GetInt64("gameCash");
                user.vcash = MyReader.GetInt64("vCash");
                user.playsNumber = MyReader.GetInt32("totalGame");
                user.playsWin = MyReader.GetInt32("totalWin");
                if (!MyReader.IsDBNull(8))
                {
                    user.email = MyReader.GetString("email");
                }
                if (!MyReader.IsDBNull(7))
                {
                    user.phone = MyReader.GetString("mobile");
                }
                if (!MyReader.IsDBNull(21))
                {
                    user.cmnd = MyReader.GetString("cmnd");
                }
                user.ipAddress = MyReader.GetString("ip");
                user.isMobile = MyReader.GetInt32("isMobile");
                user.lastLogin = MyReader.GetDateTime("last_login");
                lst.Add(user);
            }
            //close reader cũ
            MyReader.Close();
            MyCommand = new MySqlCommand(sqlTotalQuery, conn);
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
        /// Cập nhật thông tin cho user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fullname"></param>
        /// <param name="mobile"></param>
        /// <param name="email"></param>
        /// <param name="cmnd"></param>
        /// <param name="sex"></param>
        /// <param name="newPassword"></param>
        /// <param name="locked"></param>
        /// <returns></returns>
        public UserInfo Update(long id, string fullname, string mobile, string email, string cmnd, int sex, string newPassword, bool locked)
        {
            var user = new UserInfo();
            user = SelectOne(id);
            //sql update
            if (user != null)
            {
                string sql = "UPDATE portal.user SET is_active = " + Convert.ToInt32(locked);
                if (!String.IsNullOrEmpty(newPassword))
                {
                    sql += ", password='" + Web365Utility.Web365Utility.MD5Cryptor(newPassword) + "'";
                }
                if (!String.IsNullOrEmpty(fullname))
                {
                    sql += ", fullname='" + fullname + "'";
                }
                if (!String.IsNullOrEmpty(mobile))
                {
                    sql += ", mobile='" + mobile + "'";
                }
                if (!String.IsNullOrEmpty(email))
                {
                    sql += ", email='" + email + "'";
                }
                if (!String.IsNullOrEmpty(cmnd))
                {
                    sql += ", cmnd='" + cmnd + "'";
                }
                sql += ", sex='" + sex + "'";
                sql += " WHERE id= " + id + ";";
                MySqlConnection conn = new MySqlConnection(Constants.DBConnection);
                MySqlCommand MyCommand = new MySqlCommand(sql, conn);
                
                conn.Open();
                MyCommand.ExecuteNonQuery();                
                
                conn.Close();
                user = SelectOne(id);
            }
            return user;
        }
        
        /// <summary>
        /// Select một user theo user_id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserInfo SelectOne(long id)
        {
            var user = new UserInfo();
            string sql = "select * from newDB.view_user_info where id = " + id;
            MySqlConnection conn = new MySqlConnection(Constants.DBConnection);
            MySqlCommand MyCommand = new MySqlCommand(sql, conn);
            MySqlDataReader MyReader;
            conn.Open();
            MyReader = MyCommand.ExecuteReader();
            while (MyReader.Read())
            {                
                user.uid = MyReader.GetInt64("id");
                user.userName = MyReader.GetString("username");
                user.fullName = MyReader.GetString("fullname");
                user.dateRegister = MyReader.GetDateTime("register_date");
                user.isMale = MyReader.GetInt32("sex") == 1;
                user.level = MyReader.GetInt32("level_id");
                user.cash = MyReader.GetInt64("gameCash");
                user.vcash = MyReader.GetInt64("vCash");
                user.playsNumber = MyReader.GetInt32("totalGame");
                user.playsWin = MyReader.GetInt32("totalWin");
                if (!MyReader.IsDBNull(8))
                {
                    user.email = MyReader.GetString("email");
                }
                if (!MyReader.IsDBNull(7))
                {
                    user.phone = MyReader.GetString("mobile");
                }
                if (!MyReader.IsDBNull(21))
                {
                    user.cmnd = MyReader.GetString("cmnd");
                }
                user.ipAddress = MyReader.GetString("ip");
                user.isMobile = MyReader.GetInt32("isMobile");
                user.lastLogin = MyReader.GetDateTime("last_login");                
            }
            //close reader cũ
            MyReader.Close();            
            conn.Close();
            
            return user;
        }
        
        /// <summary>
        /// Thêm mới một tài khoản user chơi game
        /// </summary>
        /// <param name="username"></param>
        /// <param name="fullname"></param>
        /// <param name="mobile"></param>
        /// <param name="email"></param>
        /// <param name="cmnd"></param>
        /// <param name="sex"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public UserInfo Add(string username, string fullname, string mobile, string email, string cmnd, int sex, string password)
        {
            var user = new UserInfo();
            if (checkUserExist(username))
            {
                user.uid = -1;
            }else
            {
                string passwordMd5 = Web365Utility.Web365Utility.MD5Cryptor(password);
                String sql = "insert into portal.user (username, fullname, password,sex,email,mobile,cmnd,is_active) values (@username,@fullname,@password,@sex,@email,@mobile,@cmnd,1) ";
                MySqlConnection conn = new MySqlConnection(Constants.DBConnection);
                MySqlCommand MyCommand = new MySqlCommand(sql, conn);
                MyCommand.Parameters.AddWithValue("username", username);
                MyCommand.Parameters.AddWithValue("fullname", fullname);
                MyCommand.Parameters.AddWithValue("mobile", mobile);
                MyCommand.Parameters.AddWithValue("email", email);
                MyCommand.Parameters.AddWithValue("cmnd", cmnd);
                MyCommand.Parameters.AddWithValue("sex", sex);                
                MyCommand.Parameters.AddWithValue("password", passwordMd5);
                conn.Open();
                MyCommand.ExecuteNonQuery();
                long lastId = MyCommand.LastInsertedId;
                if(lastId > 0)
                {
                    //create user game
                    sql = "insert into newDB.g_user(user_id,username,cp,isMobile) values (@user_id,@username,@cp,@isMobile)";
                    MyCommand = new MySqlCommand(sql, conn);
                    MyCommand.Parameters.AddWithValue("user_id", lastId);
                    MyCommand.Parameters.AddWithValue("username", username);
                    MyCommand.Parameters.AddWithValue("cp", 0);
                    MyCommand.Parameters.AddWithValue("isMobile", 1);
                    MyCommand.ExecuteNonQuery();
                }
                conn.Close();
                user = SelectOne(lastId);
            }

            return user;
        }
        
        /// <summary>
        /// Kiểm tra username đã tồn tại chưa
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool checkUserExist(string username)
        {
            string sql = "select id from portal.user where username = '" + username.Trim().ToLower() + "';";
            bool exist = false;
            MySqlConnection conn = new MySqlConnection(Constants.DBConnection);
            MySqlCommand MyCommand = new MySqlCommand(sql, conn);
            MySqlDataReader MyReader;
            conn.Open();
            MyReader = MyCommand.ExecuteReader();
            while (MyReader.Read())
            {
                exist = true;
            }
            //close reader cũ
            MyReader.Close();
            conn.Close();
            return exist;
        }
        
        /// <summary>
        /// Lấy ra lịch sử của một người chơi
        /// </summary>
        /// <param name="id"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public UserInfoHistoryResult GetGameHistory(long id, int skip, int take)
        {
            var rs = new UserInfoHistoryResult();
            var lst = new List<GameHistoryItem>();
            int totalRecord = 0;
            string sql = "SELECT * FROM newDB.game_history WHERE user_id = @id ORDER BY id DESC LIMIT @skip, @take";
            string sqlTotalQuery = "SELECT count(*) as totalRecord FROM newDB.game_history WHERE user_id = @id";
            MySqlConnection conn = new MySqlConnection(Constants.DBConnection);
            MySqlCommand MyCommand = new MySqlCommand(sql, conn);
            MyCommand.Parameters.AddWithValue("id", id);
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
                lst.Add(item1);          
            }
            //close reader cũ
            MyReader.Close();
            MyCommand = new MySqlCommand(sqlTotalQuery, conn);
            MyCommand.Parameters.AddWithValue("id", id);
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
        /// Báo cáo Danh sách top người chơi nhiều nhất (tính theo số trận đã chơi)
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="top"></param>
        /// <param name="vCash"></param>
        /// <returns></returns>
        public List<ReportUserPlayGameItem> ReportUserPlayMost(DateTime from, DateTime to, int top, bool vCash)
        {
            var rs = new List<ReportUserPlayGameItem>();
            string sql = String.Format("select user_id, username, fullname, ip, gameCash, vCash, count(0) as 'totalGame' from newDB.view_user_history_total where is_active = 1 and time > '{0}' and time < '{1}' group by user_id, username, fullname, ip order by COUNT(0) desc limit 0, {2}", from.ToString("yyyy-MM-dd HH:mm:ss"), to.ToString("yyyy-MM-dd HH:mm:ss"), top);
            if(vCash)
            {
                sql = String.Format("select user_id, username, fullname, ip, gameCash, vCash, count(0) as 'totalGame' from newDB.view_user_history_total_vCash where is_active = 1 and time > '{0}' and time < '{1}' group by user_id, username, fullname, ip order by COUNT(0) desc limit 0, {2}", from.ToString("yyyy-MM-dd HH:mm:ss"), to.ToString("yyyy-MM-dd HH:mm:ss"), top);
            }
            MySqlConnection conn = new MySqlConnection(Constants.DBConnection);
            MySqlCommand MyCommand = new MySqlCommand(sql, conn);
            MySqlDataReader MyReader;
            conn.Open();
            MyReader = MyCommand.ExecuteReader();
            while (MyReader.Read())
            {
                ReportUserPlayGameItem item1 = new ReportUserPlayGameItem();
                item1.user_id = MyReader.GetInt32("user_id");
                item1.username = MyReader.GetString("username");
                item1.fullname = MyReader.GetString("fullname");
                item1.ip = MyReader.GetString("ip");
                item1.gameCash = MyReader.GetInt64("gameCash");
                item1.vCash = MyReader.GetInt64("vCash");
                if (String.IsNullOrEmpty(item1.fullname))
                {
                    item1.fullname = item1.username;
                }
                item1.totalGame = MyReader.GetInt32("totalGame");
                rs.Add(item1);
            }
            //close reader cũ
            MyReader.Close();
            conn.Close();
            return rs;
        }
        /// <summary>
        /// Báo cáo Danh sách top người chơi thắng nhiều nhất (tính theo số trận thắng)
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="top"></param>
        /// <param name="vCash"></param>
        /// <returns></returns>
        public List<ReportUserPlayGameItem> ReportUserPlayWin(DateTime from, DateTime to, int top, bool vCash)
        {
            var rs = new List<ReportUserPlayGameItem>();
            string sql = String.Format("select user_id, username, fullname, ip, gameCash, vCash, count(0) as 'totalGame' from newDB.view_user_history_total where is_active = 1 and time > '{0}' and time < '{1}' and cash > 0 group by user_id, username, fullname, ip order by COUNT(0) desc limit 0, {2}", from.ToString("yyyy-MM-dd HH:mm:ss"), to.ToString("yyyy-MM-dd HH:mm:ss"), top);
            if (vCash)
            {
                sql = String.Format("select user_id, username, fullname, ip, gameCash, vCash, count(0) as 'totalGame' from newDB.view_user_history_total_vCash where is_active = 1 and time > '{0}' and time < '{1}' and cash > 0 group by user_id, username, fullname, ip order by COUNT(0) desc limit 0, {2}", from.ToString("yyyy-MM-dd HH:mm:ss"), to.ToString("yyyy-MM-dd HH:mm:ss"), top);
            }
            MySqlConnection conn = new MySqlConnection(Constants.DBConnection);
            MySqlCommand MyCommand = new MySqlCommand(sql, conn);
            MySqlDataReader MyReader;
            conn.Open();
            MyReader = MyCommand.ExecuteReader();
            while (MyReader.Read())
            {
                ReportUserPlayGameItem item1 = new ReportUserPlayGameItem();
                item1.user_id = MyReader.GetInt32("user_id");
                item1.username = MyReader.GetString("username");
                item1.fullname = MyReader.GetString("fullname");
                item1.ip = MyReader.GetString("ip");
                item1.gameCash = MyReader.GetInt64("gameCash");
                item1.vCash = MyReader.GetInt64("vCash");
                if (String.IsNullOrEmpty(item1.fullname))
                {
                    item1.fullname = item1.username;
                }
                item1.totalGame = MyReader.GetInt32("totalGame");
                rs.Add(item1);
            }
            //close reader cũ
            MyReader.Close();
            conn.Close();
            return rs;
        }

        /// <summary>
        /// Báo cáo Danh sách user nạp nhiều tiền nhất (theo tổng số tiền Su đã nạp)
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public List<ReportUserChargeMoneyItem> ReportUserChargedMost(DateTime from, DateTime to, int top)
        {
            var rs = new List<ReportUserChargeMoneyItem>();
            string sql = String.Format("SELECT h.user_id, u.username, u.fullname, u.ip, u.gameCash, u.vCash, SUM(h.cash) AS 'SuNap' FROM newDB.game_history h INNER JOIN newDB.view_user_info u ON h.user_id = u.id WHERE h.trans_type = 4 and h.time > '{0}' and h.time < '{1}' group by h.user_id, u.username, u.fullname order by SUM(h.cash) desc limit 0, {2}", from.ToString("yyyy-MM-dd HH:mm:ss"), to.ToString("yyyy-MM-dd HH:mm:ss"), top);
            MySqlConnection conn = new MySqlConnection(Constants.DBConnection);
            MySqlCommand MyCommand = new MySqlCommand(sql, conn);
            MySqlDataReader MyReader;
            conn.Open();
            MyReader = MyCommand.ExecuteReader();
            while (MyReader.Read())
            {
                ReportUserChargeMoneyItem item1 = new ReportUserChargeMoneyItem();
                item1.user_id = MyReader.GetInt32("user_id");
                item1.username = MyReader.GetString("username");
                item1.fullname = MyReader.GetString("fullname");
                item1.ip = MyReader.GetString("ip");
                item1.gameCash = MyReader.GetInt64("gameCash");
                item1.vCash = MyReader.GetInt64("vCash");
                if (String.IsNullOrEmpty(item1.fullname))
                {
                    item1.fullname = item1.username;
                }
                item1.moenyCharged = MyReader.GetInt64("SuNap");
                rs.Add(item1);
            }
            MyReader.Close();
            conn.Close();
            return rs;
        }
    }
}
