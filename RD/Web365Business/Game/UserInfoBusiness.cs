using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web365Domain;
using Web365Domain.Game;
using Web365Models;
using Web365Utility;
using Web365BaseReader;
using Web365Base;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;

namespace Web365Business.Game
{
    public class UserInfoBusiness
    {
        public ReportOnlineItem ReportOnline()
        {
            var rs = new ReportOnlineItem
            {
                AvgAll = "0",
                AvgTraPhi = "0",
                CcuNow = "0",
                CcuMaxToday = "0",
                CcuMaxyesterday = "0",
                TcoiTon = "0",
                TcoiPhe = "0",
                TcoiChanged = "0",
                RevenueToday = "0",
                RevenueWeek = "0",
                RevenueMonth = "0",
                RevenueLastMonth = "0",
                TotalNguoiChoiTraPhi = 0,
                TotalNguoiChoi = 0,
                TotalNguoiChoiTraPhiHomNay = 0,
                TongDoanhThu = 0,
                TotalYcDoiThuongHomNay = 0
            };
            rs.GameTaxList = new List<GameTaxItem>();

            var ccuhomnay = "SELECT TOP 1 * FROM [portal].[dbo].report_CCU WHERE DATEDIFF(DateCreated,GETDATE()) = 0 ORDER BY CCU DESC";
            var ccuhomqua = "SELECT TOP 1  * FROM [portal].[dbo].report_CCU WHERE DATEDIFF(DateCreated,GETDATE()) = -1 ORDER BY CCU DESC";
            var ccuhientai = "SELECT TOP 1 * FROM [portal].[dbo].report_CCU ORDER BY ID DESC";

            var doanhsohomnay = "SELECT ((SELECT CASE WHEN SUM(cardPrice) IS NULL OR SUM(cardPrice) = '' THEN 0 ELSE SUM(cardPrice) END AS total FROM [portal].[dbo].a_paycard WHERE DATEDIFF(DateCreated,GETDATE()) = 0 AND cardPrice >0) + (SELECT CASE WHEN SUM(total_amount) IS NULL OR SUM(total_amount) = '' THEN 0 ELSE SUM(total_amount) END AS total  FROM [portal].[dbo].a_bankcharge WHERE transaction_status = '00' AND transaction_id > 0 AND DATEDIFF(time_receive,GETDATE()) = 0 )) AS homnay";
            var doanhsotuannay = "SELECT ((SELECT CASE WHEN SUM(cardPrice) IS NULL OR SUM(cardPrice) = '' THEN 0 ELSE SUM(cardPrice) END AS total FROM [portal].[dbo].a_paycard WHERE WEEK(DateCreated, 1) = WEEK(GETDATE(), 1) AND cardPrice >0) + (SELECT CASE WHEN SUM(total_amount) IS NULL OR SUM(total_amount) = '' THEN 0 ELSE SUM(total_amount) END AS total FROM [portal].[dbo].a_bankcharge	WHERE transaction_status = '00' AND transaction_id > 0 AND WEEK(time_receive, 1) = WEEK(GETDATE(), 1) )) AS tuannay";
            var doanhsothangnay = "SELECT ((SELECT CASE WHEN SUM(cardPrice) IS NULL OR SUM(cardPrice) = '' THEN 0 ELSE SUM(cardPrice) END AS total FROM [portal].[dbo].a_paycard WHERE MONTH(DateCreated)= MONTH(GETDATE()) AND cardPrice >0) + (SELECT CASE WHEN SUM(total_amount) IS NULL OR SUM(total_amount) = '' THEN 0 ELSE SUM(total_amount) END AS total FROM [portal].[dbo].a_bankcharge	WHERE transaction_status = '00' AND transaction_id > 0 AND MONTH(time_receive) = MONTH(GETDATE()) )) AS thangnay";
            var doanhsothangtruoc = "SELECT ((SELECT CASE WHEN SUM(cardPrice) IS NULL OR SUM(cardPrice) = '' THEN 0 ELSE SUM(cardPrice) END AS total FROM [portal].[dbo].a_paycard WHERE MONTH(DateCreated)= (MONTH(GETDATE()) - 1) AND cardPrice >0) + (SELECT CASE WHEN SUM(total_amount) IS NULL OR SUM(total_amount) = '' THEN 0 ELSE SUM(total_amount) END AS total FROM [portal].[dbo].a_bankcharge	WHERE transaction_status = '00' AND transaction_id > 0 AND MONTH(time_receive) = (MONTH(GETDATE()) - 1) )) AS thangtruoc";

            var tcoiton = "SELECT SUM(gameCash) AS coiton FROM [POC_User].[dbo].g_user";
            var tcoiphe = "SELECT SUM(tax) AS coiphe FROM [portal].[dbo].game_history WHERE game_id IN(1,7,5,8,4,2,12,13,3,18,19,20)";
            var tcoidoithuong = "SELECT SUM(cash) AS coidoi FROM [portal].[dbo].game_history WHERE trans_type = 16";

            var songuoichoitraphi = "SELECT COUNT(uid) AS total FROM (SELECT uid FROM (SELECT userName AS uid FROM [portal].[dbo].a_paycard a WHERE a.cardPrice > 0 GROUP BY userName UNION SELECT buyer_uid AS uid FROM [portal].[dbo].a_bankcharge a WHERE transaction_id IS NOT NULL AND transaction_status = '00' AND total_amount > 0 GROUP BY buyer_uid) t GROUP BY uid) f";
            var tongnguoichoi = "SELECT COUNT(id) AS total FROM [portal].[dbo].user";
            var songuoichoitraphihomnay = "SELECT COUNT(uid) AS total FROM (SELECT uid FROM ( SELECT userName AS uid FROM [portal].[dbo].a_paycard a  WHERE a.cardPrice > 0 AND DATE(dateCreated) = GETDATE() GROUP BY userName UNION SELECT buyer_uid AS uid FROM [portal].[dbo].a_bankcharge a  WHERE transaction_id IS NOT NULL AND transaction_status = '00' AND total_amount > 0 AND DATE(time_receive) = GETDATE() GROUP BY buyer_uid) t GROUP BY uid) f";
            var tongdoanhthu = "SELECT SUM(money) AS total FROM (SELECT SUM(cardPrice) AS money FROM [portal].[dbo].a_paycard a WHERE a.cardPrice > 0 UNION  SELECT SUM(total_amount) AS money FROM [portal].[dbo].a_bankcharge a WHERE transaction_id IS NOT NULL AND transaction_status = '00' AND total_amount > 0) mon";
            var ycdoithuonghomnay = "SELECT COUNT(user_id) AS total FROM (SELECT user_id FROM [portal].[dbo].game_history a WHERE a.trans_type = 21 AND DATE(a.time) = GETDATE() GROUP BY user_id)f";
            //var taxgame = "SELECT  game.name,his.game_id, SUM(his.tax) AS tax FROM [portal].[dbo].game_history his LEFT JOIN [portal].[dbo].game game ON game.id = his.game_id WHERE game_id IN(1,7,5,8,4,2,12,13,3,18,19,20) GROUP BY game_id";


            var conn = new SqlConnection(Constants.DBConnection);
            var myCommand = new SqlCommand(ccuhomnay, conn);
            SqlDataReader MyReader;
            conn.Open();

            //ccu hom nay
            MyReader = myCommand.ExecuteReader();
            while (MyReader.Read())
            {
                rs.CcuMaxToday = MyReader.GetString(MyReader.GetOrdinal("CCU"));
            }

            MyReader.Close();
            //ccu hom qua
            myCommand = new SqlCommand(ccuhomqua, conn);
            MyReader = myCommand.ExecuteReader();
            while (MyReader.Read())
            {
                rs.CcuMaxyesterday = MyReader.GetString(MyReader.GetOrdinal("CCU"));
            }
            MyReader.Close();

            //hien tai
            myCommand = new SqlCommand(ccuhientai, conn);
            MyReader = myCommand.ExecuteReader();
            while (MyReader.Read())
            {
                rs.CcuNow = MyReader.GetString(MyReader.GetOrdinal("CCU"));
            }
            MyReader.Close();


            //doanh so hom nay
            myCommand = new SqlCommand(doanhsohomnay, conn);
            MyReader = myCommand.ExecuteReader();
            while (MyReader.Read())
            {
                if (!MyReader.IsDBNull(0))
                {
                    rs.RevenueToday = Web365Utility.Web365Utility.FormatPrice(Convert.ToDecimal(MyReader.GetString(MyReader.GetOrdinal("homnay"))));
                }
            }
            MyReader.Close();
            //doanh so tuan nay
            myCommand = new SqlCommand(doanhsotuannay, conn);
            MyReader = myCommand.ExecuteReader();
            while (MyReader.Read())
            {
                if (!MyReader.IsDBNull(0))
                {
                    rs.RevenueWeek = Web365Utility.Web365Utility.FormatPrice(Convert.ToDecimal(MyReader.GetString(MyReader.GetOrdinal("tuannay"))));
                }
            }
            MyReader.Close();
            //doanh so thang nay
            myCommand = new SqlCommand(doanhsothangnay, conn);
            MyReader = myCommand.ExecuteReader();
            while (MyReader.Read())
            {
                if (!MyReader.IsDBNull(0))
                {
                    rs.RevenueMonth = Web365Utility.Web365Utility.FormatPrice(Convert.ToDecimal(MyReader.GetString(MyReader.GetOrdinal("thangnay"))));
                }
            }
            MyReader.Close();

            //doanh so thang truoc
            myCommand = new SqlCommand(doanhsothangtruoc, conn);
            MyReader = myCommand.ExecuteReader();
            while (MyReader.Read())
            {
                if (!MyReader.IsDBNull(0))
                {
                    rs.RevenueLastMonth = Web365Utility.Web365Utility.FormatPrice(Convert.ToDecimal(MyReader.GetString(MyReader.GetOrdinal("thangtruoc"))));
                }
            }
            MyReader.Close();

            //coi ton
            myCommand = new SqlCommand(tcoiton, conn);
            MyReader = myCommand.ExecuteReader();
            while (MyReader.Read())
            {
                if (!MyReader.IsDBNull(0))
                {
                    rs.TcoiTon = Web365Utility.Web365Utility.FormatPrice(Convert.ToDecimal(MyReader.GetString(MyReader.GetOrdinal("coiton"))));
                }
            }
            MyReader.Close();

            //coi phe
            myCommand = new SqlCommand(tcoiphe, conn);
            MyReader = myCommand.ExecuteReader();
            while (MyReader.Read())
            {
                if (!MyReader.IsDBNull(0))
                {
                    rs.TcoiPhe = Web365Utility.Web365Utility.FormatPrice(Convert.ToDecimal(MyReader.GetString(MyReader.GetOrdinal("coiphe"))));
                }
            }
            MyReader.Close();

            //coi doi thuong
            myCommand = new SqlCommand(tcoidoithuong, conn);
            MyReader = myCommand.ExecuteReader();
            while (MyReader.Read())
            {
                if (!MyReader.IsDBNull(0))
                {
                    rs.TcoiChanged = Web365Utility.Web365Utility.FormatPrice(Convert.ToDecimal(MyReader.GetString(MyReader.GetOrdinal("coidoi"))));
                }
            }
            MyReader.Close();

            //so nguoi choi tra phi
            myCommand = new SqlCommand(songuoichoitraphi, conn);
            MyReader = myCommand.ExecuteReader();
            while (MyReader.Read())
            {
                if (!MyReader.IsDBNull(0))
                {
                    rs.TotalNguoiChoiTraPhi = Convert.ToInt32(MyReader.GetString(MyReader.GetOrdinal("total")));
                }
            }
            MyReader.Close();

            //tong nguoi choi
            myCommand = new SqlCommand(tongnguoichoi, conn);
            MyReader = myCommand.ExecuteReader();
            while (MyReader.Read())
            {
                if (!MyReader.IsDBNull(0))
                {
                    rs.TotalNguoiChoi = Convert.ToInt32(MyReader.GetString(MyReader.GetOrdinal("total")));
                }
            }
            MyReader.Close();

            //tong nguoi choi
            myCommand = new SqlCommand(songuoichoitraphihomnay, conn);
            MyReader = myCommand.ExecuteReader();
            while (MyReader.Read())
            {
                if (!MyReader.IsDBNull(0))
                {
                    rs.TotalNguoiChoiTraPhiHomNay = Convert.ToInt32(MyReader.GetString(MyReader.GetOrdinal("total")));
                }
            }
            MyReader.Close();


            //tong doanh thu
            myCommand = new SqlCommand(tongdoanhthu, conn);
            MyReader = myCommand.ExecuteReader();
            while (MyReader.Read())
            {
                if (!MyReader.IsDBNull(0))
                {
                    rs.TongDoanhThu = Convert.ToInt32(MyReader.GetString(MyReader.GetOrdinal("total")));
                }
            }
            MyReader.Close();


            //tong doanh thu
            myCommand = new SqlCommand(ycdoithuonghomnay, conn);
            MyReader = myCommand.ExecuteReader();
            while (MyReader.Read())
            {
                if (!MyReader.IsDBNull(0))
                {
                    rs.TotalYcDoiThuongHomNay = Convert.ToInt32(MyReader.GetString(MyReader.GetOrdinal("total")));
                }
            }
            MyReader.Close();

            //list game tax
            //myCommand = new SqlCommand(taxgame, conn);
            //MyReader = myCommand.ExecuteReader();
            //while (MyReader.Read())
            //{
            //    var item1 = new GameTaxItem()
            //    {
            //        game_id = MyReader.GetInt32(MyReader.GetOrdinal("game_id"),
            //        name = MyReader.GetString(MyReader.GetOrdinal("name"),
            //        tax = Web365Utility.Web365Utility.FormatPrice(Convert.ToDecimal(MyReader.GetString(MyReader.GetOrdinal("tax")))
            //    };
            //    rs.GameTaxList.Add(item1);
            //}
            //MyReader.Close();

            conn.Close();
            if (rs.TotalNguoiChoi > 0 && rs.TongDoanhThu > 0)
            {
                rs.AvgAll = Web365Utility.Web365Utility.FormatPrice(Convert.ToDecimal(rs.TongDoanhThu / rs.TotalNguoiChoi));
            }
            if (rs.TotalNguoiChoiTraPhi > 0 && rs.TongDoanhThu > 0)
            {
                rs.AvgTraPhi =
                    Web365Utility.Web365Utility.FormatPrice(Convert.ToDecimal(rs.TongDoanhThu / rs.TotalNguoiChoiTraPhi));
            }

            return rs;
        }

        //public List<GameTaxItem> GetPhe(string fDate, string tDate)
        //{
        //    var gameTaxList = new List<GameTaxItem>();

        //    var taxgame = "SELECT  game.name,his.game_id, SUM(his.tax) AS tax, SUM(his.cash) AS cash FROM [portal].[dbo].game_history his LEFT JOIN [portal].[dbo].game game ON game.id = his.game_id WHERE game_id IN(1,7,5,8,4,2,12,13,3,18,19,20) ";


        //    if (fDate != "" && tDate != "")
        //    {
        //        var ffDate = Convert.ToDateTime(fDate);
        //        var ttDate = Convert.ToDateTime(tDate + " 23:59:59");
        //        taxgame += " and his.time >= '" + ffDate.ToString(Constants.DateFormat) + "' and his.time <= '" + ttDate.ToString(Constants.DateFormat) + "'";
        //    }

        //    taxgame += " GROUP BY game_id";
        //    var conn = new SqlConnection(Constants.DBConnection);
        //    var myCommand = new SqlCommand(taxgame, conn);
        //    SqlDataReader MyReader;
        //    conn.Open();
        //    MyReader = myCommand.ExecuteReader();
        //    while (MyReader.Read())
        //    {
        //        var user = new GameTaxItem
        //        {
        //            game_id = MyReader.GetInt32(MyReader.GetOrdinal("game_id"),
        //            name = MyReader.GetString(MyReader.GetOrdinal("name"),
        //            tax = Web365Utility.Web365Utility.FormatPrice(Convert.ToDecimal(MyReader.GetInt64(MyReader.GetOrdinal("tax"))),
        //            taxMonney = Convert.ToDecimal(MyReader.GetInt64(MyReader.GetOrdinal("tax")),
        //            cash = Web365Utility.Web365Utility.FormatPrice(Convert.ToDecimal(MyReader.GetInt64(MyReader.GetOrdinal("cash"))),
        //            cashMonney = Convert.ToDecimal(MyReader.GetInt64(MyReader.GetOrdinal("cash"))
        //        };

        //        gameTaxList.Add(user);
        //    }
        //    //close reader cũ
        //    MyReader.Close();
        //    conn.Close();

        //    return gameTaxList;
        //}

        public List<GameCashItem> GetCash(string date)
        {
            var gameTaxList = new List<GameCashItem>();
            var conn = new SqlConnection(Constants.DBConnection);
            conn.Open();
            var listgame = new List<GameItem>
            {
                new GameItem(){zoneID = 1,name = "Ba cây"},
                new GameItem(){zoneID = 2,name = "Xì tố"},
                new GameItem(){zoneID = 3,name = "Poker"},
                new GameItem(){zoneID = 4,name = "Phỏm"},
                new GameItem(){zoneID = 5,name = "Tiến Lên Miền Nam"},
                new GameItem(){zoneID = 7,name = "Tiến Lên Đếm Lá"},
                new GameItem(){zoneID = 8,name = "Tiến Lên Miền Bắc"},
                new GameItem(){zoneID = 12,name = "Sâm"},
                new GameItem(){zoneID = 13,name = "Liêng"},
                new GameItem(){zoneID = 18,name = "Tài Xỉu"},
                new GameItem(){zoneID = 19,name = "MiniPoker"},
                new GameItem(){zoneID = 20,name = "Kim Cương"},
                new GameItem(){zoneID = 15,name = "Xèng"},

            };

            var listTimeStringDown = new List<string>(new[] { "0:00:00", "4:00:00", "8:00:00", "12:00:00", "16:00:00", "20:00:00", "23:59:59" });

            if (date != "")
            {

                foreach (var item in listgame)
                {
                    var user = new GameCashItem
                    {
                        game_id = item.zoneID,
                        name = item.name,
                        cash4h = "0",
                        cashMonney4h = 0,
                        cash8h = "0",
                        cashMonney8h = 0,
                        cash12h = "0",
                        cashMonney12h = 0,
                        cash16h = "0",
                        cashMonney16h = 0,
                        cash20h = "0",
                        cashMonney20h = 0,
                        cash24h = "0",
                        cashMonney24h = 0
                    };


                    for (var i = 0; i <= 5; i++)
                    {
                        var dateNewDown = (date + " " + listTimeStringDown[i]);
                        var dateNewTop = (date + " " + listTimeStringDown[i + 1]);
                        var cashgame = "";
                        if (item.zoneID == 18 || item.zoneID == 19 || item.zoneID == 20 || item.zoneID == 15)
                        {
                            cashgame = "SELECT  game.name,his.game_id, (SUM(his.cash) + SUM(his.tax)) AS cash FROM [portal].[dbo].game_history his LEFT JOIN [portal].[dbo].game game ON game.id = his.game_id WHERE game_id = " + item.zoneID;
                        }
                        else
                        {
                            cashgame = "SELECT  game.name,his.game_id, SUM(his.cash) AS cash FROM [portal].[dbo].game_history his LEFT JOIN [portal].[dbo].game game ON game.id = his.game_id WHERE game_id = " + item.zoneID;
                        }
                        cashgame += " AND his.time >= '" + Convert.ToDateTime(dateNewDown).ToString(Constants.DateFormat) + "'";
                        cashgame += " AND his.time < '" + Convert.ToDateTime(dateNewTop).ToString(Constants.DateFormat) + "' AND cash <> 0 GROUP BY  his.game_id";

                        var myCommand = new SqlCommand(cashgame, conn);
                        SqlDataReader MyReader;

                        MyReader = myCommand.ExecuteReader();

                        while (MyReader.Read())
                        {
                            if (i == 0)
                            {
                                user.cash4h =
                                    Web365Utility.Web365Utility.FormatPrice(Convert.ToDecimal(MyReader.GetInt64(MyReader.GetOrdinal("cash"))) * (-1));
                                user.cashMonney4h = Convert.ToDecimal(MyReader.GetInt64(MyReader.GetOrdinal("cash") * (-1)));
                            }

                            if (i == 1)
                            {
                                user.cash8h =
                                    Web365Utility.Web365Utility.FormatPrice(Convert.ToDecimal(MyReader.GetInt64(MyReader.GetOrdinal("cash")) * (-1)));
                                user.cashMonney8h = Convert.ToDecimal(MyReader.GetInt64(MyReader.GetOrdinal("cash") * (-1)));
                            }

                            if (i == 2)
                            {
                                user.cash12h =
                                    Web365Utility.Web365Utility.FormatPrice(Convert.ToDecimal(MyReader.GetInt64(MyReader.GetOrdinal("cash")) * (-1)));
                                user.cashMonney12h = Convert.ToDecimal(MyReader.GetInt64(MyReader.GetOrdinal("cash") * (-1)));
                            }

                            if (i == 3)
                            {
                                user.cash16h =
                                    Web365Utility.Web365Utility.FormatPrice(Convert.ToDecimal(MyReader.GetInt64(MyReader.GetOrdinal("cash")) * (-1)));
                                user.cashMonney16h = Convert.ToDecimal(MyReader.GetInt64(MyReader.GetOrdinal("cash") * (-1)));
                            }

                            if (i == 4)
                            {
                                user.cash20h =
                                    Web365Utility.Web365Utility.FormatPrice(Convert.ToDecimal(MyReader.GetInt64(MyReader.GetOrdinal("cash")) * (-1)));
                                user.cashMonney20h = Convert.ToDecimal(MyReader.GetInt64(MyReader.GetOrdinal("cash") * (-1)));
                            }

                            if (i == 5)
                            {
                                user.cash24h =
                                    Web365Utility.Web365Utility.FormatPrice(Convert.ToDecimal(MyReader.GetInt64(MyReader.GetOrdinal("cash")) * (-1)));
                                user.cashMonney24h = Convert.ToDecimal(MyReader.GetInt64(MyReader.GetOrdinal("cash") * (-1)));
                            }
                        }
                        //close reader
                        MyReader.Close();

                    }

                    user.cashMonneyAll = user.cashMonney4h + user.cashMonney8h + user.cashMonney12h + user.cashMonney16h +
                                         user.cashMonney20h + user.cashMonney24h;
                    user.cashAll = Web365Utility.Web365Utility.FormatPrice(Convert.ToDecimal(user.cashMonneyAll));
                    gameTaxList.Add(user);

                }
            }
            conn.Close();
            return gameTaxList;
        }

        public GamePheInGameModel GetPhe(string fDate = "", string tDate = "")
        {
            var result = new GamePheInGameModel();
            var listgame = new List<GameItem>
            {
                new GameItem(){zoneID = 1,name = "Ba cây"},
                new GameItem(){zoneID = 2,name = "Xì tố"},
                new GameItem(){zoneID = 3,name = "Poker"},
                new GameItem(){zoneID = 4,name = "Phỏm"},
                new GameItem(){zoneID = 5,name = "Tiến Lên Miền Nam"},
                new GameItem(){zoneID = 7,name = "Tiến Lên Đếm Lá"},
                new GameItem(){zoneID = 8,name = "Tiến Lên Miền Bắc"},
                new GameItem(){zoneID = 12,name = "Sâm"},
                new GameItem(){zoneID = 13,name = "Liêng"},
                new GameItem(){zoneID = 18,name = "Tài Xỉu"},
                new GameItem(){zoneID = 19,name = "MiniPoker"},
                new GameItem(){zoneID = 20,name = "Kim Cương"},

            };
            result.ListGame = listgame;
            var datefrom = DateTime.Now;
            var dateto = DateTime.Now;

            if (fDate == "" || tDate == "")
            {
                datefrom = datefrom.AddDays(-7);
            }
            else
            {

                datefrom = Convert.ToDateTime(fDate).AddHours(0).AddMinutes(0).AddSeconds(0);
                dateto = Convert.ToDateTime(tDate).AddHours(23).AddMinutes(59).AddSeconds(59);
            }

            var totalDays = 0;
            var gameTaxList = new List<List<GameTaxItem>>();
            var conn = new SqlConnection(Constants.DBConnection);

            totalDays = Convert.ToInt32((dateto - datefrom).TotalDays);

            for (var i = 0; i < (totalDays <= 7 ? totalDays : 7); i++)
            {
                var listex = "";
                var taxgame = "SELECT  game.name, his.game_id, SUM(his.tax) AS tax  FROM [portal].[dbo].game_history his LEFT JOIN [portal].[dbo].game game ON game.id = his.game_id WHERE game_id IN(1,7,5,8,4,2,12,13,3,18,19,20) ";
                taxgame += " AND DATEDIFF(his.time, '" + datefrom.ToString(Constants.DateFormat) + "') = " + i;
                taxgame += " GROUP BY game_id";
                var myCommand = new SqlCommand(taxgame, conn);
                SqlDataReader MyReader;
                conn.Open();
                MyReader = myCommand.ExecuteReader();
                var listin = new List<GameTaxItem>();
                var ix = 0;

                while (MyReader.Read())
                {
                    var user = new GameTaxItem
                    {
                        game_id = MyReader.GetInt32(MyReader.GetOrdinal("game_id")),
                        name = MyReader.GetString(MyReader.GetOrdinal("name")),
                        tax = Web365Utility.Web365Utility.FormatPrice(Convert.ToDecimal(MyReader.GetInt64(MyReader.GetOrdinal("tax")))),
                        taxMonney = Convert.ToDecimal(MyReader.GetInt64(MyReader.GetOrdinal("tax")))
                    };
                    listex += "," + user.game_id + ",";
                    listin.Add(user);
                    ix++;
                }

                foreach (var gameItem in listgame)
                {
                    if (!listex.Contains("," + gameItem.zoneID + ","))
                    {
                        var game = new GameTaxItem
                        {
                            game_id = gameItem.zoneID,
                            name = gameItem.name,
                            tax = "0",
                            taxMonney = 0
                        };
                        listin.Add(game);
                        ix++;
                    }
                }

                gameTaxList.Add(listin.OrderBy(c => c.game_id).ToList());
                //close reader cũ
                MyReader.Close();
                conn.Close();
            }

            result.ListTax = gameTaxList;

            return result;
        }

        public UserInfoSearchResult GetAll(string fDate = "", string tDate = "", string username = "", int currentRecord = 0, int numberRecord = 10, int lastlogin = 0)
        {
            long uid = 0;
            if (Web365Utility.Web365Utility.ToLong(username) > 0 && !username.StartsWith("0"))
            {
                uid = Web365Utility.Web365Utility.ToLong(username);
            }
            var rs = new UserInfoSearchResult();
            var lst = new List<UserInfo2>();
            int totalRecord = 0;
            using (var db = new Entities365())
            {
                db.Database.Initialize(force: false);
                var queryComand = "EXEC [dbo].[POC_ListPlayer] '" + username + "'," + uid + "," + currentRecord + "," + numberRecord;
                var cmd = db.Database.Connection.CreateCommand();
                cmd.CommandText = queryComand;

                try
                {

                    db.Database.Connection.Open();
                    var reader = cmd.ExecuteReader();

                    var product = ((IObjectContextAdapter)db)
                        .ObjectContext
                        .Translate<UserInfo2>(reader, "tblGameLogsMoney", MergeOption.AppendOnly);

                    lst = product.Select(p => new UserInfo2()
                    {
                        username = p.username,
                        mobile = p.mobile,
                        id = p.id,
                        Ket = p.Ket.HasValue ? p.Ket.Value : 0,
                        totalPlay = p.totalPlay.HasValue ? p.totalPlay.Value : 0,
                        cash = p.cash.HasValue ? p.cash.Value : 0,
                        gameCash = p.gameCash.HasValue ? p.gameCash.Value : 0,
                        last_login = p.last_login.HasValue ? p.last_login.Value : DateTime.Now,
                        last_login_string = p.last_login.HasValue ? p.last_login.Value.ToString("dd-MM-yyyy HH:mm:ss") : DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"),
                        is_block = p.is_block.HasValue ? p.is_block.Value : 0
                    }).ToList();

                    reader.NextResult();

                    var tt = ((IObjectContextAdapter)db)
                        .ObjectContext
                        .Translate<int>(reader);

                    totalRecord = tt.FirstOrDefault();

                }
                finally
                {
                    db.Database.Connection.Dispose();
                    db.Database.Connection.Close();
                }
            }

            rs.data2 = lst;
            rs.totalRecord = totalRecord;

            return rs;
        }

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
        /// <param name="take"></param>
        /// <returns></returns>
        public UserInfoSearchResult Search(long id, string username, string fullname, string phone, string email, string cmnd, DateTime regFrom, DateTime regTo, int mobile, bool? active, int limit, int take)
        {
            var rs = new UserInfoSearchResult();
            var lst = new List<UserInfo>();
            var totalRecord = 0;


            string sql = "select * from [portal].[dbo].view_user_info where register_date >= '" + regFrom.ToString(Constants.DateFormat) + "' and register_date <= '" + regTo.ToString(Constants.DateFormat) + "'";
            string sqlTotalQuery = "select count(*) as totalRecord from [portal].[dbo].view_user_info where register_date >= '" + regFrom.ToString(Constants.DateFormat) + "' and register_date <= '" + regTo.ToString(Constants.DateFormat) + "'";
            if (id > 0)
            {
                sql += " and id = " + id.ToString();
                sqlTotalQuery += " and id = " + id.ToString();
            }
            if (!String.IsNullOrEmpty(username))
            {
                sql += " and username like '%" + username + "%'";
                sqlTotalQuery += " and username like '%" + username + "%'";
            }
            if (!String.IsNullOrEmpty(fullname))
            {
                sql += " and fullname like '%" + fullname + "%'";
                sqlTotalQuery += " and fullname like '%" + fullname + "%'";
            }
            if (!String.IsNullOrEmpty(phone))
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
            if (mobile > 0)
            {
                sql += " and isMobile = " + mobile.ToString();
                sqlTotalQuery += " and isMobile = " + mobile.ToString();
            }
            if (active != null)
            {
                if (Convert.ToBoolean(active))
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

            sql += " OFFSET " + limit + " ROWS FETCH NEXT" + take + " ROWS ONLY ";
            SqlConnection conn = new SqlConnection(Constants.DBConnection);
            SqlCommand MyCommand = new SqlCommand(sql, conn);
            SqlDataReader MyReader;
            conn.Open();
            MyReader = MyCommand.ExecuteReader();
            while (MyReader.Read())
            {
                UserInfo user = new UserInfo();
                user.uid = MyReader.GetInt64(MyReader.GetOrdinal("id"));
                user.userName = MyReader.GetString(MyReader.GetOrdinal("username"));
                user.fullName = MyReader.GetString(MyReader.GetOrdinal("fullname"));
                user.dateRegister = MyReader.GetDateTime(MyReader.GetOrdinal("register_date")).ToShortDateString();
                user.isMale = MyReader.GetInt32(MyReader.GetOrdinal("sex")) == 1;
                user.level = MyReader.GetInt32(MyReader.GetOrdinal("level_id"));
                user.cash = MyReader.GetInt64(MyReader.GetOrdinal("gameCash"));
                user.vcash = MyReader.GetInt64(MyReader.GetOrdinal("vCash"));
                user.playsNumber = MyReader.GetInt32(MyReader.GetOrdinal("totalGame"));
                user.playsWin = MyReader.GetInt32(MyReader.GetOrdinal("totalWin"));
                user.is_active = MyReader.GetInt32(MyReader.GetOrdinal("is_active"));
                if (!MyReader.IsDBNull(8))
                {
                    user.email = MyReader.GetString(MyReader.GetOrdinal("email"));
                }
                if (!MyReader.IsDBNull(7))
                {
                    user.phone = MyReader.GetString(MyReader.GetOrdinal("mobile"));
                }
                if (!MyReader.IsDBNull(21))
                {
                    user.cmnd = MyReader.GetString(MyReader.GetOrdinal("cmnd"));
                }
                user.ipAddress = MyReader.GetString(MyReader.GetOrdinal("ip"));
                user.isMobile = MyReader.GetInt32(MyReader.GetOrdinal("isMobile"));
                user.lastLogin = MyReader.GetDateTime(MyReader.GetOrdinal("last_login")).ToShortDateString();
                lst.Add(user);
            }
            //close reader cũ
            MyReader.Close();
            MyCommand = new SqlCommand(sqlTotalQuery, conn);
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
        public UserInfo Update(long id, string fullname, string mobile, string email, string cmnd, int sex, string newPassword, int locked)
        {
            var user = new UserInfo();
            user = SelectOne(id);
            //sql update
            if (user != null)
            {
                string sql = "UPDATE [portal].[dbo].user SET is_active = " + Convert.ToInt32(locked);
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
                SqlConnection conn = new SqlConnection(Constants.DBConnection);
                SqlCommand MyCommand = new SqlCommand(sql, conn);

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
            var result = new UserInfo();
            using (var db = new Entities365())
            {
                db.Database.Initialize(force: false);
                var queryComand = "EXEC [dbo].[PRC_GetInfoPlayerByUserId] " + id;
                var cmd = db.Database.Connection.CreateCommand();
                cmd.CommandText = queryComand;

                try
                {

                    db.Database.Connection.Open();
                    var reader = cmd.ExecuteReader();

                    var product = ((IObjectContextAdapter)db)
                        .ObjectContext
                        .Translate<UserInfo>(reader, "tblGameLogsMoney", MergeOption.AppendOnly);

                    result = product.Select(p => new UserInfo()
                    {
                        id = p.id,
                        userName = p.userName,
                        sex = p.sex.HasValue ? p.sex.Value : false,
                        ip = p.ip,
                        fullName = p.fullName,
                        cmnd = p.cmnd,
                        email = p.email,
                        mobile = p.mobile,
                        device = p.device,
                        register_date = p.register_date.HasValue ? p.register_date.Value : DateTime.Now,
                        is_active = p.is_active,
                        cash = p.cash.HasValue ? p.cash.Value : 0,
                        gameCash = p.gameCash.HasValue ? p.gameCash.Value : 0,
                        cashPlay = p.cashPlay.HasValue ? p.cashPlay.Value : 0,
                        last_login = p.last_login.HasValue ? p.last_login.Value : DateTime.Now,
                        is_block = p.is_block.HasValue ? p.is_block.Value : false,

                    }).FirstOrDefault();
                }
                finally
                {
                    db.Database.Connection.Dispose();
                    db.Database.Connection.Close();
                }
            }

            return result;
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
                return null;
            }
            else
            {
                var passwordMd5 = Web365Utility.Web365Utility.MD5Cryptor(password);
                var sql = "insert into [portal].[dbo].user (username, fullname, password,sex,email,mobile,cmnd,is_active) OUTPUT INSERTED.id values (@username,@fullname,@password,@sex,@email,@mobile,@cmnd,1) ";
                var conn = new SqlConnection(Constants.DBConnection);
                var MyCommand = new SqlCommand(sql, conn);
                MyCommand.Parameters.AddWithValue("username", username);
                MyCommand.Parameters.AddWithValue("fullname", fullname);
                MyCommand.Parameters.AddWithValue("mobile", mobile);
                MyCommand.Parameters.AddWithValue("email", email);
                MyCommand.Parameters.AddWithValue("cmnd", cmnd);
                MyCommand.Parameters.AddWithValue("sex", sex);
                MyCommand.Parameters.AddWithValue("password", passwordMd5);
                conn.Open();

                var lastId = (long)MyCommand.ExecuteScalar();
                if (lastId > 0)
                {
                    //create user game
                    sql = "insert into [POC_User].[dbo].g_user(user_id,username,cp,isMobile) values (@user_id,@username,@cp,@isMobile)";
                    MyCommand = new SqlCommand(sql, conn);
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
            string sql = "select id from [portal].[dbo].user where username = '" + username.Trim().ToLower() + "';";
            bool exist = false;
            SqlConnection conn = new SqlConnection(Constants.DBConnection);
            SqlCommand MyCommand = new SqlCommand(sql, conn);
            SqlDataReader MyReader;
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
        public UserInfoHistoryResult GetGameHistory(long id,int gameid,int mathid, int skip, int take, string fromdate = "", string todate = "")
        {
            var rs = new UserInfoHistoryResult();
            var result = new List<GameHistoryItem>();
            var totalRecord = 0;

            try
            {
                using (var db = new Web365BaseReaderEntities())
                {
                    db.Database.Initialize(force: false);

                    var cmd = db.Database.Connection.CreateCommand();

                    cmd.CommandText = "exec [dbo].[POC_GetGameHistory] " + id + "," + gameid + ", " + mathid + ", " + skip + ", " + take + ",'" + fromdate + "','" + todate + "'";

                    try
                    {
                        db.Database.Connection.Open();
                        var reader = cmd.ExecuteReader();
                        var queryProduct = ((IObjectContextAdapter)db)
                            .ObjectContext
                            .Translate<GameHistoryItem>(reader, "tblGameLogsMoney", MergeOption.AppendOnly);
                        result = queryProduct.Select(a => new GameHistoryItem
                        {
                            timestring = a.time.HasValue ? a.time.Value.ToString("yyyy-MM-dd HH:mm:ss") : DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            fullname = a.fullname,
                            username = a.username,
                            gamename = a.gamename,
                            matchId = a.matchId.HasValue ? a.matchId.Value : 0,
                            cashPlay = a.cashPlay.HasValue ? a.cashPlay.Value : 0,
                            CashFinal = a.CashFinal.HasValue ? a.CashFinal.Value : 0,
                            id = a.id.HasValue ? a.id.Value : 0,
                            //user_id = a.user_id.HasValue ? a.user_id.Value : 0,
                            cash = a.cash.HasValue ? a.cash.Value : 0,
                            current_cash = a.current_cash.HasValue ? a.current_cash.Value : 0,
                            description = a.description,
                            game_id = a.game_id.HasValue ? a.game_id.Value : 0,
                            //trans_type = a.trans_type.HasValue ? a.trans_type.Value : 0,
                            time = a.time.HasValue ? a.time.Value : DateTime.Now,
                            //taxPercent = a.taxPercent.HasValue ? a.taxPercent.Value : 0,
                            //tax = a.tax.HasValue ? a.tax.Value : 0,
                            before_cash = a.before_cash.HasValue ? a.before_cash.Value : 0,
                            //matchNum = a.matchNum.HasValue ? a.matchNum.Value : 0,
                            //betTaiXiu = a.betTaiXiu.HasValue ? a.betTaiXiu.Value : 0,
                            
                            //sourceDevice = a.sourceDevice,
                            
                        }).ToList();

                        reader.NextResult();

                        var tt = ((IObjectContextAdapter)db)
                            .ObjectContext
                            .Translate<int>(reader);

                        totalRecord = tt.FirstOrDefault();

                        rs.data = result;
                        rs.totalRecord = totalRecord;
                        return rs;
                    }
                    finally
                    {
                        db.Database.Connection.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public UserInfoHistoryResult GetGameHistoryMoney(long id, int gameid, int skip, int take, string fromdate = "", string todate = "")
        {
            var rs = new UserInfoHistoryResult();
            var result = new List<GameHistoryItem>();
            var totalRecord = 0;

            try
            {
                using (var db = new Web365BaseReaderEntities())
                {
                    db.Database.Initialize(force: false);

                    var cmd = db.Database.Connection.CreateCommand();

                    cmd.CommandText = "exec [dbo].[POC_GetGameHistoryMoney] " + id + "," + gameid + ", " + skip + ", " + take + ",'" + fromdate + "','" + todate + "'";

                    try
                    {
                        db.Database.Connection.Open();
                        var reader = cmd.ExecuteReader();
                        var queryProduct = ((IObjectContextAdapter)db)
                            .ObjectContext
                            .Translate<GameHistoryItem>(reader, "tblGameLogsMoney", MergeOption.AppendOnly);
                        result = queryProduct.Select(a => new GameHistoryItem
                        {
                            id = a.id.HasValue ? a.id.Value : 0,
                            user_id = a.user_id.HasValue ? a.user_id.Value : 0,
                            cash = a.cash.HasValue ? a.cash.Value : 0,
                            current_cash = a.current_cash.HasValue ? a.current_cash.Value : 0,
                            description = a.description,
                            game_id = a.game_id.HasValue ? a.game_id.Value : 0,
                            trans_type = a.trans_type.HasValue ? a.trans_type.Value : 0,
                            time = a.time.HasValue ? a.time.Value : DateTime.Now,
                            before_cash = a.before_cash.HasValue ? a.before_cash.Value : 0,
                            timestring = a.time.HasValue ? a.time.Value.ToString("yyyy-MM-dd HH:mm:ss") : DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            ShopLevel = a.ShopLevel.HasValue ? a.ShopLevel.Value : 0,
                            username = a.username,
                        }).ToList();

                        reader.NextResult();

                        var tt = ((IObjectContextAdapter)db)
                            .ObjectContext
                            .Translate<int>(reader);

                        totalRecord = tt.FirstOrDefault();

                        rs.data = result;
                        rs.totalRecord = totalRecord;
                        return rs;
                    }
                    finally
                    {
                        db.Database.Connection.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<HopThuItem> GetHopThu(out int total, FilterItem obj)
        {
            var result = new List<HopThuItem>();
            var totalRecord = 0;
            total = totalRecord;
            try
            {
                using (var db = new Entities365())
                {
                    db.Database.Initialize(force: false);

                    var cmd = db.Database.Connection.CreateCommand();

                    cmd.CommandText = "exec [dbo].[POC_GetHopThu] " + obj.Id + ", " + obj.currentRecord + ", " + obj.numberRecord + ",'" + obj.Key + "','" + obj.FromDate + "','" + obj.ToDate + "'";

                    try
                    {
                        db.Database.Connection.Open();
                        var reader = cmd.ExecuteReader();
                        var queryProduct = ((IObjectContextAdapter)db)
                            .ObjectContext
                            .Translate<HopThuItem>(reader, "tblGameLogsMoney", MergeOption.AppendOnly);
                        result = queryProduct.Select(a => new HopThuItem
                        {
                            id = a.id,
                            userIDSend = a.userIDSend,
                            userIDReceive = a.userIDReceive,
                            mes = a.mes,
                            datetimeSend = a.datetimeSend,
                            datetimeSendString = a.datetimeSend.ToString("yyyy-MM-dd HH:mm:ss")
                        }).ToList();

                        reader.NextResult();

                        var tt = ((IObjectContextAdapter)db)
                            .ObjectContext
                            .Translate<int>(reader);

                        totalRecord = tt.FirstOrDefault();

                        total = totalRecord;
                        return result;
                    }
                    finally
                    {
                        db.Database.Connection.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                return null;
            }
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
            string sql = String.Format("select user_id, username, fullname, ip, gameCash, vCash, count(0) as 'totalGame' from [portal].[dbo].view_user_history_total where is_active = 1 and time > '{0}' and time < '{1}' group by user_id, username, fullname, ip order by COUNT(0) desc OFFSET 0 ROWS FETCH NEXT {2} ROWS ONLY", from.ToString("yyyy-MM-dd HH:mm:ss"), to.ToString("yyyy-MM-dd HH:mm:ss"), top);
            if (vCash)
            {
                sql = String.Format("select user_id, username, fullname, ip, gameCash, vCash, count(0) as 'totalGame' from [portal].[dbo].view_user_history_total_vCash where is_active = 1 and time > '{0}' and time < '{1}' group by user_id, username, fullname, ip order by COUNT(0) desc OFFSET 0 ROWS FETCH NEXT {2} ROWS ONLY", from.ToString("yyyy-MM-dd HH:mm:ss"), to.ToString("yyyy-MM-dd HH:mm:ss"), top);
            }
            SqlConnection conn = new SqlConnection(Constants.DBConnection);
            SqlCommand MyCommand = new SqlCommand(sql, conn);
            SqlDataReader MyReader;
            conn.Open();
            MyReader = MyCommand.ExecuteReader();
            while (MyReader.Read())
            {
                ReportUserPlayGameItem item1 = new ReportUserPlayGameItem();
                item1.user_id = MyReader.GetInt32(MyReader.GetOrdinal("user_id"));
                item1.username = MyReader.GetString(MyReader.GetOrdinal("username"));
                item1.fullname = MyReader.GetString(MyReader.GetOrdinal("fullname"));
                item1.ip = MyReader.GetString(MyReader.GetOrdinal("ip"));
                item1.gameCash = MyReader.GetInt64(MyReader.GetOrdinal("gameCash"));
                item1.vCash = MyReader.GetInt64(MyReader.GetOrdinal("vCash"));
                if (String.IsNullOrEmpty(item1.fullname))
                {
                    item1.fullname = item1.username;
                }
                item1.totalGame = MyReader.GetInt32(MyReader.GetOrdinal("totalGame"));
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
            string sql = String.Format("select user_id, username, fullname, ip, gameCash, vCash, count(0) as 'totalGame' from [portal].[dbo].view_user_history_total where is_active = 1 and time > '{0}' and time < '{1}' and cash > 0 group by user_id, username, fullname, ip order by COUNT(0) DESC OFFSET 0 ROWS FETCH NEXT {2} ROWS ONLY", from.ToString("yyyy-MM-dd HH:mm:ss"), to.ToString("yyyy-MM-dd HH:mm:ss"), top);
            if (vCash)
            {
                sql = String.Format("select user_id, username, fullname, ip, gameCash, vCash, count(0) as 'totalGame' from [portal].[dbo].view_user_history_total_vCash where is_active = 1 and time > '{0}' and time < '{1}' and cash > 0 group by user_id, username, fullname, ip order by COUNT(0) DESC OFFSET 0 ROWS FETCH NEXT {2} ROWS ONLY", from.ToString("yyyy-MM-dd HH:mm:ss"), to.ToString("yyyy-MM-dd HH:mm:ss"), top);
            }
            var conn = new SqlConnection(Constants.DBConnection);
            var MyCommand = new SqlCommand(sql, conn);
            SqlDataReader MyReader;
            conn.Open();
            MyReader = MyCommand.ExecuteReader();
            while (MyReader.Read())
            {
                var item1 = new ReportUserPlayGameItem();
                item1.user_id = MyReader.GetInt32(MyReader.GetOrdinal("user_id"));
                item1.username = MyReader.GetString(MyReader.GetOrdinal("username"));
                item1.fullname = MyReader.GetString(MyReader.GetOrdinal("fullname"));
                item1.ip = MyReader.GetString(MyReader.GetOrdinal("ip"));
                item1.gameCash = MyReader.GetInt64(MyReader.GetOrdinal("gameCash"));
                item1.vCash = MyReader.GetInt64(MyReader.GetOrdinal("vCash"));
                if (String.IsNullOrEmpty(item1.fullname))
                {
                    item1.fullname = item1.username;
                }
                item1.totalGame = MyReader.GetInt32(MyReader.GetOrdinal("totalGame"));
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
            var sql = String.Format("SELECT h.user_id, u.username, u.fullname, u.ip, u.gameCash, u.vCash, SUM(h.cash) AS 'SuNap' FROM [portal].[dbo].game_history h INNER JOIN [portal].[dbo].view_user_info u ON h.user_id = u.id WHERE h.trans_type = 4 and h.time > '{0}' and h.time < '{1}' group by h.user_id, u.username, u.fullname order by SUM(h.cash) desc OFFSET 0 ROWS FETCH NEXT {2} ROWS ONLY", from.ToString("yyyy-MM-dd HH:mm:ss"), to.ToString("yyyy-MM-dd HH:mm:ss"), top);
            var conn = new SqlConnection(Constants.DBConnection);
            var MyCommand = new SqlCommand(sql, conn);
            SqlDataReader MyReader;
            conn.Open();
            MyReader = MyCommand.ExecuteReader();
            while (MyReader.Read())
            {
                var item1 = new ReportUserChargeMoneyItem();
                item1.user_id = MyReader.GetInt32(MyReader.GetOrdinal("user_id"));
                item1.username = MyReader.GetString(MyReader.GetOrdinal("username"));
                item1.fullname = MyReader.GetString(MyReader.GetOrdinal("fullname"));
                item1.ip = MyReader.GetString(MyReader.GetOrdinal("ip"));
                item1.gameCash = MyReader.GetInt64(MyReader.GetOrdinal("gameCash"));
                item1.vCash = MyReader.GetInt64(MyReader.GetOrdinal("vCash"));
                if (String.IsNullOrEmpty(item1.fullname))
                {
                    item1.fullname = item1.username;
                }
                item1.moenyCharged = MyReader.GetInt64(MyReader.GetOrdinal("SuNap"));
                rs.Add(item1);
            }
            MyReader.Close();
            conn.Close();
            return rs;
        }

        public UserInfoSearchResult ExportExcel(DateTime? from, DateTime? to)
        {
            var rs = new UserInfoSearchResult();
            var lst = new List<UserInfo>();
            int totalRecord = 0;
            var sql = "SELECT id, fullname,username, mobile, email,birth, last_login, cmnd, register_date, ip FROM [portal].[dbo].view_user_info";
            if (from != null && to != null)
            {
                sql += " where register_date >= '" + from.Value.ToString("yyyy-MM-dd HH:mm:ss") + "' and register_date <= '" + to.Value.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }
            sql += " order by id desc";
            var conn = new SqlConnection(Constants.DBConnection);
            var myCommand = new SqlCommand(sql, conn);
            SqlDataReader MyReader;
            conn.Open();
            MyReader = myCommand.ExecuteReader();
            while (MyReader.Read())
            {
                var user = new UserInfo
                {
                    uid = MyReader.GetInt64(MyReader.GetOrdinal("id")),
                    userName = MyReader.GetString(MyReader.GetOrdinal("username")),
                    fullName = MyReader.GetString(MyReader.GetOrdinal("fullname")),
                    dateRegister = MyReader.GetDateTime(MyReader.GetOrdinal("register_date")).ToShortDateString(),
                    ipAddress = MyReader.GetString(MyReader.GetOrdinal("ip")),
                    lastLogin = MyReader.GetDateTime(MyReader.GetOrdinal("last_login")).ToShortDateString(),
                };
                if (!MyReader.IsDBNull(4))
                {
                    user.email = MyReader.GetString(MyReader.GetOrdinal("email"));
                }
                if (!MyReader.IsDBNull(3))
                {
                    user.phone = MyReader.GetString(MyReader.GetOrdinal("mobile"));
                }
                if (!MyReader.IsDBNull(7))
                {
                    user.cmnd = MyReader.GetString(MyReader.GetOrdinal("cmnd"));
                }
                lst.Add(user);
            }
            //close reader cũ
            MyReader.Close();
            conn.Close();
            rs.data = lst;
            return rs;
        }

        #region payment

        public PaymentItem InsertPayment(PaymentItem pay)
        {
            var sql = "insert into [portal].[dbo].a_bankcharge (buyer_uid, buyer_fullname, buyer_mobile,total_amount,order_code,payment_method,bank_code,payment_type, transaction_status, token, time_request, time_receive) " +
                                                        "values (@buyer_uid,@buyer_fullname,@buyer_mobile,@total_amount,@order_code,@payment_method,@bank_code,@payment_type, @transaction_status, @token, @time_request, @time_receive) ";
            var conn = new SqlConnection(Constants.DBConnection);
            var myCommand = new SqlCommand(sql, conn);
            myCommand.Parameters.AddWithValue("buyer_uid", pay.buyer_uid);
            myCommand.Parameters.AddWithValue("buyer_fullname", pay.buyer_fullname);
            myCommand.Parameters.AddWithValue("buyer_mobile", pay.buyer_mobile);
            myCommand.Parameters.AddWithValue("total_amount", pay.total_amount);
            myCommand.Parameters.AddWithValue("order_code", pay.order_code);
            myCommand.Parameters.AddWithValue("payment_method", pay.payment_method);
            myCommand.Parameters.AddWithValue("bank_code", pay.bank_code);
            myCommand.Parameters.AddWithValue("payment_type", pay.payment_type);
            myCommand.Parameters.AddWithValue("transaction_status", pay.transaction_status);
            myCommand.Parameters.AddWithValue("token", pay.token);
            myCommand.Parameters.AddWithValue("time_request", pay.time_request.ToString(Constants.DateFormat));
            myCommand.Parameters.AddWithValue("time_receive", pay.time_receive.ToString(Constants.DateFormat));
            conn.Open();

            var lastId = (int)myCommand.ExecuteNonQuery(); ;
            if (lastId > 0)
            {
                return GetDetailPaymentById(lastId);
            }
            conn.Close();

            return null;
        }

        public PaymentItem GetDetailPaymentById(long id)
        {
            var user = new PaymentItem();
            var sql = "select * from [portal].[dbo].a_bankcharge where id = " + id;
            var conn = new SqlConnection(Constants.DBConnection);
            var myCommand = new SqlCommand(sql, conn);
            SqlDataReader MyReader;
            conn.Open();
            MyReader = myCommand.ExecuteReader();
            while (MyReader.Read())
            {
                user.buyer_uid = MyReader.GetInt32(MyReader.GetOrdinal("buyer_uid"));
                user.buyer_fullname = MyReader.GetString(MyReader.GetOrdinal("buyer_fullname"));
                user.buyer_mobile = MyReader.GetString(MyReader.GetOrdinal("buyer_mobile"));
                user.total_amount = MyReader.GetInt32(MyReader.GetOrdinal("total_amount"));
                user.order_code = MyReader.GetString(MyReader.GetOrdinal("order_code"));
                user.payment_method = MyReader.GetString(MyReader.GetOrdinal("payment_method"));
                user.bank_code = MyReader.GetString(MyReader.GetOrdinal("bank_code"));
                user.payment_type = MyReader.GetString(MyReader.GetOrdinal("payment_type"));
                user.transaction_status = MyReader.GetString(MyReader.GetOrdinal("transaction_status"));
                //user.transaction_id = MyReader.GetString(MyReader.GetOrdinal("transaction_id") ?? "";
            }

            //close reader cũ
            MyReader.Close();
            conn.Close();

            return user;
        }

        public PaymentItem GetDetailPaymentByToken(string token)
        {
            try
            {
                var user = new PaymentItem();
                var sql = "select * from [portal].[dbo].a_bankcharge where token = '" + token + "'";
                var conn = new SqlConnection(Constants.DBConnection);
                var myCommand = new SqlCommand(sql, conn);
                SqlDataReader MyReader;
                conn.Open();
                MyReader = myCommand.ExecuteReader();
                while (MyReader.Read())
                {
                    user.buyer_uid = MyReader.GetInt32(MyReader.GetOrdinal("buyer_uid"));
                    user.buyer_fullname = MyReader.GetString(MyReader.GetOrdinal("buyer_fullname"));
                    user.buyer_mobile = MyReader.GetString(MyReader.GetOrdinal("buyer_mobile"));
                    user.total_amount = MyReader.GetInt32(MyReader.GetOrdinal("total_amount"));
                    user.order_code = MyReader.GetString(MyReader.GetOrdinal("order_code"));
                    user.payment_method = MyReader.GetString(MyReader.GetOrdinal("payment_method"));
                    user.bank_code = MyReader.GetString(MyReader.GetOrdinal("bank_code"));
                    user.payment_type = MyReader.GetString(MyReader.GetOrdinal("payment_type"));
                    user.transaction_status = MyReader.GetString(MyReader.GetOrdinal("transaction_status"));
                    user.token = MyReader.GetString(MyReader.GetOrdinal("token"));
                }

                //close reader cũ
                MyReader.Close();
                conn.Close();

                return user;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public PaymentItem UpdatePayment(PaymentItem pay)
        {
            try
            {
                var user = GetDetailPaymentByToken(pay.token);
                //sql update
                if (user != null)
                {
                    string sql = "UPDATE [portal].[dbo].a_bankcharge SET transaction_status = '" + pay.transaction_status + "'";

                    sql += ", total_amount= " + pay.total_amount + "";
                    sql += ", transaction_id= '" + pay.transaction_id + "'";
                    sql += ", time_receive= '" + pay.time_receive.ToString(Constants.DateFormat) + "'";

                    sql += " WHERE token= '" + pay.token + "'";
                    var conn = new SqlConnection(Constants.DBConnection);
                    var MyCommand = new SqlCommand(sql, conn);

                    conn.Open();
                    MyCommand.ExecuteNonQuery();

                    conn.Close();
                    user = GetDetailPaymentByToken(pay.token);
                }
                return user;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public PaymentItem InsertMoney(PaymentItem pay)
        {
            var currentMoney = 0;
            var qurycurrentMoney = "SELECT gameCash FROM [POC_User].[dbo].g_user WHERE user_id=" + pay.buyer_uid;

            var conn = new SqlConnection(Constants.DBConnection);
            var myCommand = new SqlCommand(qurycurrentMoney, conn);
            conn.Open();
            SqlDataReader MyReader = myCommand.ExecuteReader();
            while (MyReader.Read())
            {
                currentMoney = MyReader.GetInt32(MyReader.GetOrdinal("gameCash"));
            }
            //close reader cũ
            MyReader.Close();
            conn.Close();

            var queryUpdateMoney = "UPDATE [POC_User].[dbo].g_user SET gameCash= " + (currentMoney + pay.total_amount) + " WHERE user_id= " + pay.buyer_uid;

            var myCommand2 = new SqlCommand(queryUpdateMoney, conn);

            conn.Open();
            myCommand2.ExecuteNonQuery();
            conn.Close();

            var desNap = "Nạp tiền qua BANK_NGANLUONG";
            if (ConfigWeb.PercentSale > 0)
            {
                desNap = "- KM " + ConfigWeb.PercentSale + "%";
            }

            var queryLog =
                "INSERT into [portal].[dbo].game_history (user_id,cash,current_cash,description,game_id,trans_type,tax,taxPercent,before_cash) OUTPUT INSERTED.id" +
                "values (" + pay.buyer_uid + ", " + pay.total_amount + " , " + (currentMoney + pay.total_amount) + ", '" + desNap + "', 0, 22, 0, 0, " + currentMoney + ");";

            var myCommand3 = new SqlCommand(queryLog, conn);
            conn.Open();
            var lastId = (int)myCommand3.ExecuteScalar();
            if (lastId > 0)
            {
                return GetDetailPaymentById(lastId);
            }
            conn.Close();

            return null;
        }

        #endregion
    }
}
