using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Web365Base;
using Web365BaseReader;
using Web365Business.Back_End.IRepository;
using Web365Business.Game;
using Web365Domain;
using Web365Domain.Game;
using Web365Models;
using Web365Utility;

namespace Web365Business.Back_End.Repository
{
    public class GamePlayerRepository : IGamePlayerRepository
    {
        protected Entities365 web365db = new Entities365();
        protected Web365BaseReaderEntities web365DbReader = new Web365BaseReaderEntities();
        readonly UserInfoBusiness _userInfoBusiness = new UserInfoBusiness();
        readonly ChargeBusiness _chargeBusiness = new ChargeBusiness();
        readonly GameBusiness _gameBusiness = new GameBusiness();
        readonly ChangeAward _changeAward = new ChangeAward();
        readonly AnnouncementBusiness _announcementBusiness = new AnnouncementBusiness();
        readonly GiftCodeBusiness _giftCodeBusiness = new GiftCodeBusiness();
        readonly NoticeTextBusiness _noticeTextBusiness = new NoticeTextBusiness();
        readonly RoomBusiness _roomBusiness = new RoomBusiness();
        readonly CardCodeBusiness _cardCodeBusiness = new CardCodeBusiness();
        readonly PocGameBusiness _gamePocBusiness = new PocGameBusiness();

        #region quản lý người chơi

        public List<UserInfo2> GetList(out int total, string fDate = "", string tDate = "", string username = "", int currentRecord = 0, int numberRecord = 10, int lastlogin = 0)
        {
            long uid = 0;
            if (Web365Utility.Web365Utility.ToLong(username) > 0 && !username.StartsWith("0"))
            {
                uid = Web365Utility.Web365Utility.ToLong(username);
            }
            var rs = new UserInfoSearchResult();
            var lst = new List<UserInfo2>();
            int totalRecord = 0;
            using (var db = new Web365BaseReaderEntities())
            {
                db.Database.Initialize(force: false);
                var queryComand = "";


                if (uid > 0)
                {
                    queryComand = "EXEC [dbo].[POC_ListPlayer] '" + username + "'," + uid + "," + currentRecord + "," + numberRecord;
                }
                else
                {
                    queryComand = "EXEC [dbo].[POC_ListPlayer] '" + username + "'," + 0 + "," + currentRecord + "," + numberRecord;
                }

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
                        fullname = p.fullname,
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
            total = totalRecord;

            return rs.data2;


            //var query = _userInfoBusiness.GetAll(fDate, tDate, username, currentRecord, numberRecord, lastlogin);

            //total = query.totalRecord;

            //return query.data2;
        }

        public List<GiftCodeItem2> CheckGiftCode(string content)
        {

            long uid = 0;
            if (Web365Utility.Web365Utility.ToLong(content) > 0)
            {
                uid = Web365Utility.Web365Utility.ToLong(content);
            }
            var queryComand = "";
            if (uid > 0)
            {
                queryComand = "EXEC [dbo].[POC_CheckGiftCode] '', " + uid;
            }
            else
            {
                queryComand = "EXEC [dbo].[POC_CheckGiftCode] '" + content + "', " + 0;

            }

            var query1 = web365DbReader.Database.SqlQuery<GiftCodeItem2>(queryComand);

            var result1 = query1.Select(p => new GiftCodeItem2()
            {
                ID = p.ID,
                Code = p.Code,
                DateCreated = p.DateCreated.HasValue ? p.DateCreated.Value : DateTime.Now,
                DateCreatedString = p.DateCreated.HasValue ? p.DateCreated.Value.ToString("dd-MM-yyyy HH:mm:ss") : DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"),
                DateUsed = p.DateUsed.HasValue ? p.DateUsed.Value : DateTime.Now,
                DateUsedString = p.DateUsed.HasValue ? p.DateUsed.Value.ToString("dd-MM-yyyy HH:mm:ss") : "Chưa sử dụng",
                user_id = p.user_id.HasValue ? p.user_id.Value : 0,
                DateExpired = p.DateExpired.HasValue ? p.DateExpired.Value : DateTime.Now,
                DateExpiredString = p.DateExpired.HasValue ? p.DateExpired.Value.ToString("dd-MM-yyyy HH:mm:ss") : DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"),
                TurnID = p.TurnID.HasValue ? p.TurnID.Value : 0,
                Money = p.Money.HasValue ? p.Money.Value : 0,
                fullname = p.user_id.HasValue ? p.fullname : "Chưa sử dụng",
                IsShowTurn = p.IsShowTurn.HasValue ? p.IsShowTurn.Value : true,
                username = p.username,
                TypeName = p.TypeName
            }).ToList();

            return result1;
        }

        public List<GiftCodeItem2> GetGiftCodeUsed(out int total, int currentRecord = 0, int numberRecord = 10)
        {
            var result = new List<GiftCodeItem2>();
            var paramTotal = new SqlParameter
            {
                ParameterName = "total",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };

            var query = web365db.Database.SqlQuery<GiftCodeItem2>("exec [dbo].[POC_GiftCodeUsed] @currentRecord, @numberRecord, @total OUTPUT",
                new SqlParameter("currentRecord", currentRecord),
                new SqlParameter("numberRecord", numberRecord),
                paramTotal);

            result = query.Select(p => new GiftCodeItem2()
            {
                ID = p.ID,
                Code = p.Code,
                DateCreated = p.DateCreated.HasValue ? p.DateCreated.Value : DateTime.Now,
                DateCreatedString = p.DateCreated.HasValue ? p.DateCreated.Value.ToString("dd-MM-yyyy HH:mm:ss") : DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"),
                DateUsed = p.DateUsed.HasValue ? p.DateUsed.Value : DateTime.Now,
                DateUsedString = p.DateUsed.HasValue ? p.DateUsed.Value.ToString("dd-MM-yyyy HH:mm:ss") : "Chưa sử dụng",
                user_id = p.user_id.HasValue ? p.user_id.Value : 0,
                DateExpired = p.DateExpired.HasValue ? p.DateExpired.Value : DateTime.Now,
                DateExpiredString = p.DateExpired.HasValue ? p.DateExpired.Value.ToString("dd-MM-yyyy HH:mm:ss") : DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"),
                TurnID = p.TurnID.HasValue ? p.TurnID.Value : 0,
                Money = p.Money.HasValue ? p.Money.Value : 0,
                fullname = p.user_id.HasValue ? p.fullname : "Chưa sử dụng",
                IsShowTurn = p.IsShowTurn.HasValue ? p.IsShowTurn.Value : true,
                username = p.username,
                TypeName = p.TypeName
            }).ToList();

            total = Convert.ToInt32(paramTotal.Value);

            return result;
        }

        public List<CardTelcoItem> CardChecker(out int total, string name, int uid, string mathe, string seri, int currentPage,
            int currentRecord, int numberRecord)
        {
            var paramTotal = new SqlParameter
            {
                ParameterName = "total",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };

            var query = web365db.Database.SqlQuery<CardTelcoItem>("exec [dbo].[POC_GetListCard] @name, @uid, @mathe, @seri, @currentPage, @pageSize, @total OUTPUT",
                new SqlParameter("name", name),
                new SqlParameter("uid", uid),
                new SqlParameter("mathe", mathe),
                new SqlParameter("seri", seri),
                new SqlParameter("currentPage", currentPage),
                new SqlParameter("pageSize", numberRecord),
                paramTotal);

            var result = query.Select(p => new CardTelcoItem()
            {
                ID = p.ID,
                UserID = p.UserID.HasValue ? p.UserID.Value : 0,
                MenhGia = p.MenhGia.HasValue ? p.MenhGia.Value : 0,
                Code = p.Code,
                Seria = p.Seria,
                IsUsed = p.IsUsed.HasValue && p.IsUsed.Value,
                DateCreated = p.DateCreated.HasValue ? p.DateCreated.Value : DateTime.Now,
                DateCreatedString = p.DateCreated.HasValue ? p.DateCreated.Value.ToString("dd-MM-yyyy HH:mm:ss") : DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"),
                DateUsed = p.DateUsed.HasValue ? p.DateUsed.Value : DateTime.Now,
                DateUsedString = p.DateUsed.HasValue ? p.DateUsed.Value.ToString("dd-MM-yyyy HH:mm:ss") : "Chưa xác nhận",
                MenhGiaResponse = p.MenhGiaResponse.HasValue ? p.MenhGiaResponse.Value : 0,
                StatusID = p.StatusID.HasValue ? p.StatusID.Value : 0,
                TelcoID = p.TelcoID.HasValue ? p.TelcoID.Value : 0,
                fullname = p.fullname
            }).ToList();

            total = Convert.ToInt32(paramTotal.Value);

            return result;
        }

        public List<UserInfo2> GetTopPlayer(out int total, string fDate = "", string tDate = "", string username = "", int currentRecord = 0, int numberRecord = 10, int lastlogin = 0, decimal moneyGiaoDich = 0)
        {
            long uid = 0;
            if (Web365Utility.Web365Utility.ToLong(username) > 0 && !username.StartsWith("0"))
            {
                uid = Web365Utility.Web365Utility.ToLong(username);
            }
            var rs = new UserInfoSearchResult();
            var lst = new List<UserInfo2>();
            int totalRecord = 0;
            using (var db = new Web365BaseReaderEntities())
            {
                db.Database.Initialize(force: false);
                var queryComand = "";


                if (uid > 0)
                {
                    queryComand = "EXEC [dbo].[POC_ListTopPlayer] ''," + 0 + "," + currentRecord + "," + numberRecord + "," + moneyGiaoDich;
                }
                else
                {
                    queryComand = "EXEC [dbo].[POC_ListTopPlayer] '" + "" + "'," + 0 + "," + currentRecord + "," + numberRecord + "," + moneyGiaoDich;
                }

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
                        fullname = p.fullname,
                        Ket = p.Ket.HasValue ? p.Ket.Value : decimal.Zero,
                        totalPlay = p.totalPlay.HasValue ? p.totalPlay.Value : 0,
                        cash = p.cash.HasValue ? p.cash.Value : decimal.Zero,
                        gameCash = p.gameCash.HasValue ? p.gameCash.Value : decimal.Zero,
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
            total = totalRecord;

            return rs.data2;


            //var query = _userInfoBusiness.GetAll(fDate, tDate, username, currentRecord, numberRecord, lastlogin);

            //total = query.totalRecord;

            //return query.data2;
        }
        public List<GameHistoryItem> GetGameHistoryMoney(out int total, long id, int gameid, int skip, int take, string fromdate = "", string todate = "")
        {
            var query = _userInfoBusiness.GetGameHistoryMoney(id, gameid, skip, take, fromdate, todate);

            total = query.totalRecord;

            return query.data;
        }

        public List<SumPocItem> GetThongKePoc(out int total, int currentPage = 0, int numberRecord = 0, int currentRecord = 0)
        {
            var result = new List<SumPocItem>();
            var nowItem = new SumPocItem();

            using (var db = new Entities365())
            {
                db.Database.Initialize(force: false);
                var queryComand = "EXEC [dbo].[POC_ThongKePoc] " + currentPage + "," + numberRecord + "," + currentRecord + "";
                var cmd = db.Database.Connection.CreateCommand();
                cmd.CommandText = queryComand;

                try
                {

                    db.Database.Connection.Open();
                    var reader = cmd.ExecuteReader();

                    var product = ((IObjectContextAdapter)db)
                        .ObjectContext
                        .Translate<SumPocItem>(reader, "tblGameLogsMoney", MergeOption.AppendOnly);

                    result = product.Select(p => new SumPocItem()
                    {
                        ID = p.ID
                          ,
                        TongTienUser = p.TongTienUser.HasValue ? p.TongTienUser.Value : 0
                          ,
                        TongTienKetSat = p.TongTienKetSat.HasValue ? p.TongTienKetSat.Value : 0
                          ,
                        TongTienNickTongPoc = p.TongTienNickTongPoc.HasValue ? p.TongTienNickTongPoc.Value : 0
                          ,
                        TongHu = p.TongHu.HasValue ? p.TongHu.Value : 0
                          ,
                        TongGameBank = p.TongGameBank.HasValue ? p.TongGameBank.Value : 0
                          ,
                        TongTienDaiLyCap1 = p.TongTienDaiLyCap1.HasValue ? p.TongTienDaiLyCap1.Value : 0
                          ,
                        TongTienDaiLyCap2 = p.TongTienDaiLyCap2.HasValue ? p.TongTienDaiLyCap2.Value : 0
                          ,
                        TongTienGiftCodeChuaNap = p.TongTienGiftCodeChuaNap.HasValue ? p.TongTienGiftCodeChuaNap.Value : 0
                          ,
                        DateCreated = p.DateCreated.HasValue ? p.DateCreated.Value : DateTime.Now
                          ,
                        DateCreatedString = p.DateCreated.HasValue ? p.DateCreated.Value.ToString("HH:mm:ss dd-MM-yyyy") : DateTime.Now.ToString("HH:mm:ss dd-MM-yyyy"),
                    }).ToList();

                    reader.NextResult();

                    var tt = ((IObjectContextAdapter)db)
                        .ObjectContext
                        .Translate<int>(reader);

                    total = tt.FirstOrDefault();

                    reader.NextResult();

                    var nru = ((IObjectContextAdapter)db)
                       .ObjectContext
                       .Translate<SumPocItem>(reader, "tblGameLogsMoney", MergeOption.AppendOnly);

                    nowItem = nru.Select(p => new SumPocItem()
                    {
                        ID = p.ID
                          ,
                        TongTienUser = p.TongTienUser.HasValue ? p.TongTienUser.Value : 0
                          ,
                        TongTienKetSat = p.TongTienKetSat.HasValue ? p.TongTienKetSat.Value : 0
                          ,
                        TongTienNickTongPoc = p.TongTienNickTongPoc.HasValue ? p.TongTienNickTongPoc.Value : 0
                          ,
                        TongHu = p.TongHu.HasValue ? p.TongHu.Value : 0
                          ,
                        TongGameBank = p.TongGameBank.HasValue ? p.TongGameBank.Value : 0
                          ,
                        TongTienDaiLyCap1 = p.TongTienDaiLyCap1.HasValue ? p.TongTienDaiLyCap1.Value : 0
                          ,
                        TongTienDaiLyCap2 = p.TongTienDaiLyCap2.HasValue ? p.TongTienDaiLyCap2.Value : 0
                          ,
                        TongTienGiftCodeChuaNap = p.TongTienGiftCodeChuaNap.HasValue ? p.TongTienGiftCodeChuaNap.Value : 0
                          ,
                        DateCreated = p.DateCreated.HasValue ? p.DateCreated.Value : DateTime.Now
                          ,
                        DateCreatedString = p.DateCreated.HasValue ? p.DateCreated.Value.ToString("HH:mm:ss dd-MM-yyyy") : DateTime.Now.ToString("HH:mm:ss dd-MM-yyyy"),
                    }).FirstOrDefault();

                }
                finally
                {
                    db.Database.Connection.Dispose();
                    db.Database.Connection.Close();
                }
            }

            result.Insert(0, nowItem);

            return result;
        }

        public List<SumPocItem> GetThongKePoc2(out int total, int currentPage = 0, int numberRecord = 0, int currentRecord = 0)
        {
            var result = new List<SumPocItem>();
            var nowItem = new SumPocItem();

            using (var db = new Entities365())
            {
                db.Database.Initialize(force: false);
                var queryComand = "EXEC [dbo].[POC_ThongKePocInHour] " + currentPage + "," + numberRecord + "," + currentRecord + "";
                var cmd = db.Database.Connection.CreateCommand();
                cmd.CommandText = queryComand;

                try
                {

                    db.Database.Connection.Open();
                    var reader = cmd.ExecuteReader();

                    var firstItem = ((IObjectContextAdapter)db).ObjectContext.Translate<SumPocItem>(reader, "tblGameLogsMoney", MergeOption.AppendOnly);

                    nowItem = firstItem.Select(p => new SumPocItem()
                    {
                        ID = p.ID,
                        TongTienUser = p.TongTienUser.HasValue ? p.TongTienUser.Value : 0,
                        TongTienKetSatUser = p.TongTienKetSatUser.HasValue ? p.TongTienKetSatUser.Value : 0,
                        TongTienKetSat = p.TongTienKetSat.HasValue ? p.TongTienKetSat.Value : 0,
                        TongTienNickTongPoc = p.TongTienNickTongPoc.HasValue ? p.TongTienNickTongPoc.Value : 0,
                        TongTienKetSatNickTongPoc = p.TongTienKetSatNickTongPoc.HasValue ? p.TongTienKetSatNickTongPoc.Value : 0,
                        TongHu = p.TongHu.HasValue ? p.TongHu.Value : 0,
                        TongGameBank = p.TongGameBank.HasValue ? p.TongGameBank.Value : 0,
                        TongTienDaiLyCap1 = p.TongTienDaiLyCap1.HasValue ? p.TongTienDaiLyCap1.Value : 0,
                        TongTienDaiLyCap2 = p.TongTienDaiLyCap2.HasValue ? p.TongTienDaiLyCap2.Value : 0,
                        TongTienKetSatDaiLyCap1 = p.TongTienKetSatDaiLyCap1.HasValue ? p.TongTienKetSatDaiLyCap1.Value : 0,
                        TongTienKetSatDaiLyCap2 = p.TongTienKetSatDaiLyCap2.HasValue ? p.TongTienKetSatDaiLyCap2.Value : 0,
                        TongTienGiftCodeChuaNap = p.TongTienGiftCodeChuaNap.HasValue ? p.TongTienGiftCodeChuaNap.Value : 0,
                        DateCreated = p.DateCreated.HasValue ? p.DateCreated.Value : DateTime.Now,
                        DateCreatedString = p.DateCreated.HasValue ? p.DateCreated.Value.ToString("HH:mm:ss dd-MM-yyyy") : DateTime.Now.ToString("HH:mm:ss dd-MM-yyyy"),
                    }).FirstOrDefault();

                    reader.NextResult();

                    var product = ((IObjectContextAdapter)db)
                        .ObjectContext
                        .Translate<SumPocItem>(reader, "tblGameLogsMoney", MergeOption.AppendOnly);

                    result = product.Select(p => new SumPocItem()
                    {
                        ID = p.ID,
                        TongTienUser = p.TongTienUser.HasValue ? p.TongTienUser.Value : 0,
                        TongTienKetSatUser = p.TongTienKetSatUser.HasValue ? p.TongTienKetSatUser.Value : 0,
                        TongTienKetSat = p.TongTienKetSat.HasValue ? p.TongTienKetSat.Value : 0,
                        TongTienNickTongPoc = p.TongTienNickTongPoc.HasValue ? p.TongTienNickTongPoc.Value : 0,
                        TongTienKetSatNickTongPoc = p.TongTienKetSatNickTongPoc.HasValue ? p.TongTienKetSatNickTongPoc.Value : 0,
                        TongHu = p.TongHu.HasValue ? p.TongHu.Value : 0,
                        TongGameBank = p.TongGameBank.HasValue ? p.TongGameBank.Value : 0,
                        TongTienDaiLyCap1 = p.TongTienDaiLyCap1.HasValue ? p.TongTienDaiLyCap1.Value : 0,
                        TongTienDaiLyCap2 = p.TongTienDaiLyCap2.HasValue ? p.TongTienDaiLyCap2.Value : 0,
                        TongTienKetSatDaiLyCap1 = p.TongTienKetSatDaiLyCap1.HasValue ? p.TongTienKetSatDaiLyCap1.Value : 0,
                        TongTienKetSatDaiLyCap2 = p.TongTienKetSatDaiLyCap2.HasValue ? p.TongTienKetSatDaiLyCap2.Value : 0,
                        TongTienGiftCodeChuaNap = p.TongTienGiftCodeChuaNap.HasValue ? p.TongTienGiftCodeChuaNap.Value : 0,
                        DateCreated = p.DateCreated.HasValue ? p.DateCreated.Value : DateTime.Now,
                        DateCreatedString = p.DateCreated.HasValue ? p.DateCreated.Value.ToString("HH:mm:ss dd-MM-yyyy") : DateTime.Now.ToString("HH:mm:ss dd-MM-yyyy"),
                    }).ToList();

                    reader.NextResult();

                    var tt = ((IObjectContextAdapter)db)
                        .ObjectContext
                        .Translate<int>(reader);

                    total = tt.FirstOrDefault();
                }
                finally
                {
                    db.Database.Connection.Dispose();
                    db.Database.Connection.Close();
                }
            }

            result.Insert(0, nowItem);

            return result;
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

                    cmd.CommandText = "exec [dbo].[POC_GetHopThu] " + obj.currentRecord + ", " + obj.numberRecord + ",'" + obj.Key + "','" + obj.FromDate + "','" + obj.ToDate + "'";

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
                            datetimeSendString = a.datetimeSend.ToString("yyyy-MM-dd HH:mm:ss"),
                            SendName = a.SendName,
                            ReceiveName = a.ReceiveName
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

        public GamePheInGameModel GetPhe(string fDate, string tDate)
        {
            return _userInfoBusiness.GetPhe(fDate, tDate);
        }

        public List<UserInfo> PlayerExportExcel(DateTime? from, DateTime? to)
        {
            var query = _userInfoBusiness.ExportExcel(from, to);

            return query.data.Select(p => new UserInfo()
            {
                uid = p.uid,
                userName = p.userName,
                fullName = p.fullName,
                dateRegister = p.dateRegister,
                isMale = p.isMale,
                level = p.level,
                cash = p.cash,
                vcash = p.vcash,
                playsNumber = p.playsNumber,
                playsWin = p.playsWin,
                email = p.email,
                phone = p.phone,
                cmnd = p.cmnd,
                ipAddress = p.ipAddress,
                isMobile = p.isMobile,
                lastLogin = p.lastLogin,
                is_active = p.is_active
            }).ToList();
        }

        public UserInfo2 SelectOne(long id)
        {
            var result = new UserInfo2();
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
                        .Translate<UserInfo2>(reader, "tblGameLogsMoney", MergeOption.AppendOnly);

                    result = product.Select(p => new UserInfo2()
                    {
                        id = p.id,
                        username = p.username,
                        sex = p.sex.HasValue ? p.sex.Value : 0,
                        ip = p.ip,
                        fullname = p.fullname,
                        cmnd = p.cmnd,
                        email = p.email,
                        mobile = p.mobile,
                        device = p.device,
                        cash = p.cash.HasValue ? p.cash.Value : 0,
                        register_date = p.register_date.HasValue ? p.register_date.Value : DateTime.Now,
                        register_date_string = p.register_date.HasValue ? p.register_date.Value.ToString("HH:mm:ss dd-MM-yyyy") : DateTime.Now.ToString("HH:mm:ss dd-MM-yyyy"),
                        last_login_string = p.last_login.HasValue ? p.last_login.Value.ToString("HH:mm:ss dd-MM-yyyy") : DateTime.Now.ToString("HH:mm:ss dd-MM-yyyy"),
                        is_active = p.is_active.HasValue ? p.is_active.Value : 0,
                        gameCash = p.gameCash.HasValue ? p.gameCash.Value : 0,
                        last_login = p.last_login.HasValue ? p.last_login.Value : DateTime.Now,
                        is_block = p.is_block.HasValue ? p.is_block.Value : 0,
                        IsChat = p.IsChat.HasValue ? p.IsChat.Value : false
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

        public UserInfo GetItemById(long id)
        {
            var p = _userInfoBusiness.SelectOne(id);

            return new UserInfo()
            {
                uid = p.uid,
                userName = p.userName,
                fullName = p.fullName,
                dateRegister = p.dateRegister,
                isMale = p.isMale,
                level = p.level,
                cash = p.cash,
                vcash = p.vcash,
                playsNumber = p.playsNumber,
                playsWin = p.playsWin,
                email = p.email,
                phone = p.phone,
                cmnd = p.cmnd,
                ipAddress = p.ipAddress,
                isMobile = p.isMobile,
                lastLogin = p.lastLogin,
                is_active = p.is_active
            };
        }

        public static string Md5Hash(string input)
        {
            var hash = new StringBuilder();
            var md5Provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5Provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            foreach (var t in bytes)
            {
                hash.Append(t.ToString("x2"));
            }

            return hash.ToString();
        }

        public DaiLyShowItem BlockUserMember(string username)
        {
            var newName = "";
            if (username.Contains("_isLock"))
            {
                newName = username.Replace("_isLock", "");
            }
            else
            {
                newName = username + "_isLock";
            }
            var query1 = web365db.Database.SqlQuery<DaiLyShowItem>("[dbo].[POC_BlockUserMember] @currentName, @newName"
                 , new SqlParameter("currentName", username)
                 , new SqlParameter("newName", newName)
                 );

            var result1 = query1.Select(p => new DaiLyShowItem()
            {
                NickName = newName
            }).FirstOrDefault();

            return result1;
        }

        public UserInfo Update(UserInfo objSubmit)
        {
            if (!string.IsNullOrEmpty(objSubmit.passWord) && objSubmit.sex == true)
            {
                var passMd5 = Md5Hash(objSubmit.passWord);

                var query1 = web365db.Database.SqlQuery<UserInfo>("update AUTHEN.[POC_User].[dbo].[user] set [password] = @pass where [id] = @userId and [username] != 'tongpoc'",
                   new SqlParameter("userId", objSubmit.id),
                   new SqlParameter("pass", passMd5)
                   );

                var result1 = query1.Select(p => new UserInfo()
                {
                    userName = "OK"
                }).FirstOrDefault();
            }

            var queryChat = web365db.Database.SqlQuery<UserInfo>("update AUTHEN.[POC_User].[dbo].[user] set [IsChat] = @ischat where [id] = @userId and [username] != 'tongpoc'",
                new SqlParameter("ischat", objSubmit.IsChat.HasValue && objSubmit.IsChat.Value),
                new SqlParameter("userId", objSubmit.id)
               );

            var chat = queryChat.Select(p => new UserInfo()
            {
                userName = "OK"
            }).FirstOrDefault();


            var mb = "";

            if (!string.IsNullOrEmpty(objSubmit.mobile))
            {
                mb = objSubmit.mobile;
            }

            if (objSubmit.sex == true)
            {
                var query = web365db.Database.SqlQuery<UserInfo>("exec [dbo].[POC_UpdatePlayerInfo] @userId, 0, 0, @mobile",
                new SqlParameter("userId", objSubmit.id),
                new SqlParameter("mobile", mb)
                );
                var result = query.Select(p => new UserInfo()
                {
                    userName = "OK"
                }).FirstOrDefault();
            }


            return new UserInfo();
        }

        public DaiLyShowItem UpdateLevelDaiLy(string username, int level, string parent)
        {

            var query1 = web365db.Database.SqlQuery<DaiLyShowItem>("[dbo].[PRC_UpdateLevelDaiLy] @username, @level, @parent"
                 , new SqlParameter("username", username)
                 , new SqlParameter("level", level)
                 , new SqlParameter("parent", parent)
                 );

            var result1 = query1.Select(p => new DaiLyShowItem()
            {
                NickName = p.NickName
            }).FirstOrDefault();

            return result1;
        }

        public List<GameBankItem> GetGameBank()
        {

            var query1 = web365db.Database.SqlQuery<GameBankItem>("SELECT hu.*,game.name as Game  FROM [newDB].[dbo].[HuReadTime] hu left join [newDB].[dbo].[game] on game.id = hu.GameID");

            var result1 = query1.Select(p => new GameBankItem()
            {
                Hu1 = p.Hu1,
                Hu2 = p.Hu2,
                Hu3 = p.Hu3,
                Hu4 = p.Hu4,
                Hu5 = p.Hu5,
                Cash_Public1 = p.Cash_Public1,
                Cash_Public2 = p.Cash_Public2,
                Cash_Public3 = p.Cash_Public3,
                Cash_Public4 = p.Cash_Public4,
                Cash_Public5 = p.Cash_Public5,
                Game = p.Game
            }).ToList();

            return result1;
        }

        public List<DaiLyShowItem> GetDaiLyShow()
        {

            var query1 = web365db.Database.SqlQuery<DaiLyShowItem>("SELECT [ID],[DiaChi] ,[FB],[NickName],[TenDaiLy],[Phone],[IsDelete],[IsShow] FROM [newDB].[dbo].[DaiLy] where IsDelete  <> 1");

            var result1 = query1.Select(p => new DaiLyShowItem()
            {
                ID = p.ID,
                DiaChi = p.DiaChi,
                FB = p.FB,
                NickName = p.NickName,
                TenDaiLy = p.TenDaiLy,
                Phone = p.Phone,
                IsDelete = p.IsDelete.HasValue ? p.IsDelete.Value : false,
                IsShow = p.IsShow.HasValue ? p.IsShow.Value : false,
            }).ToList();

            return result1;
        }

        public DaiLyShowItem GetDaiLyShowByID(int id)
        {
            var query1 = web365db.Database.SqlQuery<DaiLyShowItem>("SELECT TOP 1 [ID],[DiaChi] ,[FB],[NickName],[TenDaiLy],[Phone],[IsDelete],[IsShow] FROM [newDB].[dbo].[DaiLy] where ID = " + id);

            var result1 = query1.Select(p => new DaiLyShowItem()
            {
                ID = p.ID,
                DiaChi = p.DiaChi,
                FB = p.FB,
                NickName = p.NickName,
                TenDaiLy = p.TenDaiLy,
                Phone = p.Phone,
                IsDelete = p.IsDelete.HasValue ? p.IsDelete.Value : false,
                IsShow = p.IsShow.HasValue ? p.IsShow.Value : false,
            }).FirstOrDefault();

            return result1;
        }

        public DaiLyShowItem InserUpdatetDaiLyShow(DaiLyShowItem obj)
        {

            var query1 = web365db.Database.SqlQuery<DaiLyShowItem>("Insert into [newDB].[dbo].[DaiLy]([DiaChi],[FB],[NickName],[TenDaiLy],[Phone],[IsDelete],[IsShow])values(@DiaChi,@FB,@NickName,@TenDaiLy,@Phone,0,@IsShow) select 'ok' as 'NickName'"
                , new SqlParameter("DiaChi", obj.DiaChi)
                , new SqlParameter("FB", obj.FB)
                , new SqlParameter("NickName", obj.NickName)
                , new SqlParameter("TenDaiLy", obj.TenDaiLy)
                , new SqlParameter("Phone", obj.Phone)
                , new SqlParameter("IsShow", obj.IsShow.Value)
                );

            var result1 = query1.Select(p => new DaiLyShowItem()
            {
                NickName = p.NickName
            }).FirstOrDefault();

            return result1;
        }

        public DaiLyShowItem UpdatetDaiLyShow(DaiLyShowItem obj)
        {

            var query1 = web365db.Database.SqlQuery<DaiLyShowItem>("update [newDB].[dbo].[DaiLy] set [DiaChi] = @DiaChi ,[FB] = @FB ,[NickName] = @NickName,[TenDaiLy] = @TenDaiLy,[Phone] = @Phone ,[IsShow] = @IsShow where ID = @ID select 'ok' as 'NickName'"
                , new SqlParameter("DiaChi", obj.DiaChi)
                , new SqlParameter("FB", obj.FB)
                , new SqlParameter("NickName", obj.NickName)
                , new SqlParameter("TenDaiLy", obj.TenDaiLy)
                , new SqlParameter("Phone", obj.Phone)
                , new SqlParameter("IsShow", obj.IsShow.Value)
                , new SqlParameter("ID", obj.ID)
                );

            var result1 = query1.Select(p => new DaiLyShowItem()
            {
                NickName = p.NickName
            }).FirstOrDefault();

            return result1;
        }

        public DaiLyShowItem ActioDaiLy(int id, int status)
        {

            if (status == 1)
            {
                // ẩn
                var query1 = web365db.Database.SqlQuery<DaiLyShowItem>("select 'ok' as 'NickName' Update [newDB].[dbo].[DaiLy] set IsShow = 0 where ID=" + id);

                var result1 = query1.Select(p => new DaiLyShowItem()
                {
                    NickName = p.NickName
                }).FirstOrDefault();

                return result1;

            }

            if (status == 2)
            {
                //hiển thị
                var query1 = web365db.Database.SqlQuery<DaiLyShowItem>("select 'ok' as 'NickName' Update [newDB].[dbo].[DaiLy] set IsShow = 1 where ID=" + id);

                var result1 = query1.Select(p => new DaiLyShowItem()
                {
                    NickName = p.NickName
                }).FirstOrDefault();

                return result1;
            }

            if (status == 3)
            {
                // xóa
                var query1 = web365db.Database.SqlQuery<DaiLyShowItem>("select 'ok' as 'NickName' Update [newDB].[dbo].[DaiLy] set IsDelete = 1 where ID=" + id);

                var result1 = query1.Select(p => new DaiLyShowItem()
                {
                    NickName = p.NickName
                }).FirstOrDefault();

                return result1;
            }

            var result2 = new DaiLyShowItem();

            return result2;
        }


        public UserInfo InsertLogsAdmin(string content)
        {

            var query1 = web365db.Database.SqlQuery<UserInfo>("EXEC [dbo].[POC_InserLogsAdmin] N'" + content + "'");

            var result1 = query1.Select(p => new UserInfo()
            {
                userName = "OK"
            }).FirstOrDefault();

            return result1;
        }

        public UserInfo UpdatePass(UserInfo objSubmit)
        {
            var passMD5 = Md5Hash(objSubmit.passWord);

            var query1 = web365db.Database.SqlQuery<UserInfo>("update [portal].[dbo].[user] set [password] = @pass where [id] = @userId or username = '" + objSubmit.userName + "'",
               new SqlParameter("userId", objSubmit.id),
               new SqlParameter("pass", passMD5)
               );

            var result1 = query1.Select(p => new UserInfo()
            {
                uid = p.uid
            }).FirstOrDefault();

            return result1;
        }

        public UserInfo DeleteOffMessenger(int id)
        {

            var query1 = web365db.Database.SqlQuery<UserInfo>("update [newDB].[dbo].[offlinemessage] set [IsDeleted] = 1 where [id] = @id select 'ok' as 'userName'",
               new SqlParameter("id", id)
               );

            var result1 = query1.Select(p => new UserInfo()
            {
                userName = p.userName
            }).FirstOrDefault();

            return result1;
        }

        public UserInfo UnBlockUser(int uid)
        {
            var query = web365db.Database.SqlQuery<UserInfo>("exec [dbo].[PRC_UnBlockUser] @userId",
                new SqlParameter("userId", uid));

            var result = query.Select(p => new UserInfo()
            {
                userName = p.userName
            }).FirstOrDefault();

            return result;
        }

        public UserInfo BlockUser(int uid)
        {
            var query = web365db.Database.SqlQuery<UserInfo>("exec [dbo].[PRC_BlockUser] @userId",
                new SqlParameter("userId", uid));

            var result = query.Select(p => new UserInfo()
            {
                userName = p.userName
            }).FirstOrDefault();

            return result;
        }

        public UserInfo Add(string username, string fullname, string mobile, string email, string cmnd, int sex, string password)
        {
            return _userInfoBusiness.Add(username, fullname, mobile, email, cmnd, sex, password);
        }

        public int InsertOffMessenger(HopThuItem obj)
        {
            var rs = 0;
            string sql = "EXEC [dbo].[PRC_InsertOFFMessenger] @nicknameSend, @nicknameRecive, @content";
            SqlConnection conn = new SqlConnection(Constants.DBConnection);
            SqlCommand MyCommand = new SqlCommand(sql, conn);
            MyCommand.Parameters.AddWithValue("nicknameSend", obj.SendName);
            MyCommand.Parameters.AddWithValue("nicknameRecive", obj.ReceiveName);
            MyCommand.Parameters.AddWithValue("content", obj.mes);
            conn.Open();
            rs = MyCommand.ExecuteNonQuery();
            conn.Close();
            return rs;
        }

        public int SendToMessenger(HopThuItem obj)
        {
            var rs = 0;
            string sql = "EXEC [dbo].[PRC_SendToMessenger] @nicknameSend, @content";
            SqlConnection conn = new SqlConnection(Constants.DBConnection);
            SqlCommand MyCommand = new SqlCommand(sql, conn);
            MyCommand.Parameters.AddWithValue("nicknameSend", obj.SendName);
            MyCommand.Parameters.AddWithValue("content", obj.mes);
            conn.Open();
            rs = MyCommand.ExecuteNonQuery();
            conn.Close();
            return rs;
        }


        public List<GameHistoryItem> GetGameHistory(out int total, long id, int gameid, int mathid, int skip, int take, string fromdate = "", string todate = "")
        {
            var query = _userInfoBusiness.GetGameHistory(id, gameid, mathid, skip, take, fromdate, todate);

            total = query.totalRecord;

            return query.data;
        }

        public List<TaiXiuGameHistoryItem> GetGameHistoryTaiXiu(out int total, long id, int gameid, int mathid, int skip, int take, string fromdate = "", string todate = "")
        {
            var result = new List<TaiXiuGameHistoryItem>();
            total = 0;
            try
            {
                using (var db = new Web365BaseReaderEntities())
                {
                    db.Database.Initialize(force: false);

                    var cmd = db.Database.Connection.CreateCommand();

                    cmd.CommandText = "exec [dbo].[POC_GetGameHistoryTaiXiu] " + id + "," + gameid + ", " + mathid + ", " + skip + ", " + take + ",'" + fromdate + "','" + todate + "'";

                    try
                    {
                        db.Database.Connection.Open();
                        var reader = cmd.ExecuteReader();
                        var queryProduct = ((IObjectContextAdapter)db)
                            .ObjectContext
                            .Translate<TaiXiuGameHistoryItem>(reader, "tblGameLogsMoney", MergeOption.AppendOnly);
                        result = queryProduct.Select(a => new TaiXiuGameHistoryItem
                        {
                            id = a.id.HasValue ? a.id.Value : 0,
                            matchId = a.matchId.HasValue ? a.matchId.Value : 0,
                            user_id = a.user_id.HasValue ? a.user_id.Value : 0,
                            game_id = a.game_id.HasValue ? a.game_id.Value : 0,
                            username = a.username,
                            fullname = a.fullname,
                            gamename = a.gamename,
                            CuaDat = a.CuaDat.HasValue ? a.CuaDat.Value : 0,
                            betTaiXiu = a.betTaiXiu.HasValue ? a.betTaiXiu.Value : 0,
                            TienHoan = a.TienHoan.HasValue ? a.TienHoan.Value : 0,
                            TienThang = a.TienThang.HasValue ? a.TienThang.Value : 0,
                            time = a.time.HasValue ? a.time.Value : DateTime.Now,
                            timestring = a.time.HasValue ? a.time.Value.ToString("HH:mm:ss dd-MM-yyyy") : DateTime.Now.ToString("HH:mm:ss dd-MM-yyyy"),
                        }).ToList();

                        reader.NextResult();

                        var tt = ((IObjectContextAdapter)db)
                            .ObjectContext
                            .Translate<int>(reader);

                        total = tt.FirstOrDefault();

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
                return new List<TaiXiuGameHistoryItem>();
            }
        }

        public GameHistoryItem GetDetailGameHistory(long id)
        {
            try
            {
                using (var db = new Web365BaseReaderEntities())
                {
                    db.Database.Initialize(force: false);

                    var cmd = db.Database.Connection.CreateCommand();

                    cmd.CommandText = "exec [dbo].[POC_GetDetailGameHistory] " + id;

                    try
                    {
                        db.Database.Connection.Open();
                        var reader = cmd.ExecuteReader();
                        var queryProduct = ((IObjectContextAdapter)db)
                            .ObjectContext
                            .Translate<GameHistoryItem>(reader, "tblGameLogsMoney", MergeOption.AppendOnly);
                        var result = queryProduct.Select(a => new GameHistoryItem
                         {
                             id = a.id.HasValue ? a.id.Value : 0,
                             user_id = a.user_id.HasValue ? a.user_id.Value : 0,
                             cash = a.cash.HasValue ? a.cash.Value : 0,
                             current_cash = a.current_cash.HasValue ? a.current_cash.Value : 0,
                             description = a.description,
                             game_id = a.game_id.HasValue ? a.game_id.Value : 0,
                             trans_type = a.trans_type.HasValue ? a.trans_type.Value : 0,
                             time = a.time.HasValue ? a.time.Value : DateTime.Now,
                             taxPercent = a.taxPercent.HasValue ? a.taxPercent.Value : 0,
                             tax = a.tax.HasValue ? a.tax.Value : 0,
                             before_cash = a.before_cash.HasValue ? a.before_cash.Value : 0,
                             matchId = a.matchId.HasValue ? a.matchId.Value : 0,
                             gamename = a.gamename,
                             matchNum = a.matchNum.HasValue ? a.matchNum.Value : 0,
                             betTaiXiu = a.betTaiXiu.HasValue ? a.betTaiXiu.Value : 0,
                             cashPlay = a.cashPlay.HasValue ? a.cashPlay.Value : 0,
                             sourceDevice = a.sourceDevice,
                             CashFinal = a.CashFinal.HasValue ? a.CashFinal.Value : 0,
                             timestring = a.time.HasValue ? a.time.Value.ToString("yyyy-MM-dd HH:mm:ss") : DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),

                             username = a.username,
                         }).FirstOrDefault();

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


        #endregion

        #region quản lý nạp
        public List<ChargeItem> GetListCharge(out int total, string type, string name, int currentRecord, int numberRecord)
        {
            var query = _chargeBusiness.GetAll(type, name, currentRecord, numberRecord);

            total = query.totalRecord;

            return query.data.Select(p => new ChargeItem()
            {
                Stt = p.Stt,
                uid = p.uid,
                username = p.username,
                fullname = p.fullname,
                type = p.type,
                cardNumber = p.cardNumber,
                cardSerial = p.cardSerial,
                telco = p.telco,
                Price = p.Price,
                refNo = p.refNo,
                tranNo = p.tranNo,
                source = p.source,
                timeString = p.timeString
            }).ToList();
        }

        public List<ChargeItem> ChargeExportExcel(DateTime? from, DateTime? to, string type = "")
        {
            var query = _chargeBusiness.ExportExcel(from, to, type);

            return query.data.Select(p => new ChargeItem()
            {
                Stt = p.Stt,
                uid = p.uid,
                username = p.username,
                fullname = p.fullname,
                type = p.type,
                cardNumber = p.cardNumber,
                cardSerial = p.cardSerial,
                telco = p.telco,
                Price = p.Price,
                refNo = p.refNo,
                tranNo = p.tranNo,
                source = p.source,
                timeString = p.timeString
            }).ToList();
        }

        #endregion

        #region quản lý game

        public List<GameItem> GetAllGame()
        {
            var query = _gameBusiness.GetAll();

            return query.Select(p => new GameItem()
            {
                zoneID = p.zoneID,
                name = p.name,
                displayStatus = p.displayStatus,
                gameOrder = p.gameOrder
            }).ToList();
        }

        public GameItem GetGameItemById(int id)
        {
            var p = _gameBusiness.SelectOne(id);

            return new GameItem()
            {
                zoneID = p.zoneID,
                name = p.name,
                displayStatus = p.displayStatus,
                gameOrder = p.gameOrder,
                IsShow = p.displayStatus == 2,
            };
        }

        public bool UpdateGame(int id, string name, int status, int gameOrder)
        {
            try
            {
                var p = _gameBusiness.Update(id, name, status, gameOrder);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<GameHistoryItem> GetGameHistoryByZoneId(out int total, int zoneId, int skip, int take)
        {
            var query = _gameBusiness.GetGameHistoryByZoneID(zoneId, skip, take);

            total = query.totalRecord;

            return query.data.Select(p => new GameHistoryItem()
            {
                id = p.id,
                user_id = p.user_id,
                cash = p.cash,
                current_cash = p.current_cash,
                description = p.description,
                game_id = p.game_id,
                trans_type = p.trans_type,
                time = p.time,
                before_cash = p.before_cash,
                username = p.username,
                fullname = p.fullname
            }).ToList();
        }

        public List<UserInfo> GetTopUserByZoneId(int zoneId, int top)
        {
            var query = _gameBusiness.GetTopUserByZoneID(zoneId, top);

            return query.Select(p => new UserInfo()
            {
                uid = p.uid,
                userName = p.userName,
                fullName = p.fullName,
                dateRegister = p.dateRegister,
                isMale = p.isMale,
                level = p.level,
                cash = p.cash,
                vcash = p.vcash,
                playsNumber = p.playsNumber,
                playsWin = p.playsWin,
                email = p.email,
                phone = p.phone,
                cmnd = p.cmnd,
                ipAddress = p.ipAddress,
                isMobile = p.isMobile,
                lastLogin = p.lastLogin,
            }).ToList();
        }

        public List<UserInfo> RealTimeAccPlayingByGameId(out int total, int gameId, int top)
        {
            var query = _gameBusiness.RealTimeAccPlayingByGameId(out total, gameId, top);

            return query.Select(p => new UserInfo()
            {
                userName = p.userName,
                fullName = p.fullName
            }).ToList();
        }

        #endregion

        #region quản lý đổi thưởng

        public List<GameHistoryItem> GetListChangeAward(out int total, long uid, int type, int skip, int take)
        {
            var query = _changeAward.ShowAll(uid, type, skip, take);
            total = query.totalRecord;

            return query.data.Select(p => new GameHistoryItem()
            {
                id = p.id,
                user_id = p.user_id,
                cash = p.cash,
                current_cash = p.current_cash,
                description = p.description,
                game_id = p.game_id,
                trans_type = p.trans_type,
                time = p.time,
                before_cash = p.before_cash,
                username = p.username,
                fullname = p.fullname,
                timestring = p.timestring
            }).ToList();

        }

        public bool Approval(long id, bool approval)
        {
            try
            {
                var xx = _changeAward.Approval(id, approval);
                return xx;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<GameHistoryItem> ChangeAwardExportExcel(DateTime? from, DateTime? to)
        {
            var query = _changeAward.ExportExcel(from, to);

            return query.data.Select(p => new GameHistoryItem()
            {
                id = p.id,
                user_id = p.user_id,
                cash = p.cash,
                current_cash = p.current_cash,
                description = p.description,
                game_id = p.game_id,
                trans_type = p.trans_type,
                time = p.time,
                before_cash = p.before_cash,
                username = p.username,
                fullname = p.fullname
            }).ToList();

        }
        #endregion

        #region quản lý sự kiện
        public List<AnnouncementItem> GetAllEvent()
        {
            var query = _announcementBusiness.GetAll();
            return query.Select(p => new AnnouncementItem()
            {
                ID = p.ID,
                Subject = p.Subject,
                DisplayOrder = p.DisplayOrder,
                Content = p.Content,
                begin_time = p.begin_time,
                end_time = p.end_time,
                begin_timestring = p.begin_time.ToShortDateString(),
                end_timestring = p.end_time.ToShortDateString()
            }).ToList();
        }

        public List<RuleEventItem> GetGameRuleEvent(int currentRecord = 0, int numberRecord = 10)
        {
            string sql = "SELECT ev.*, game.name as gamename FROM [newDB].[dbo].[Rule_Event] ev left join [newDB].[dbo].[game] game on ev.GameID = game.id where [IsDelete] = 0 order by ID desc OFFSET " + currentRecord + " ROWS FETCH NEXT " + numberRecord + "ROWS ONLY ";


            var query = web365db.Database.SqlQuery<RuleEventItem>(sql);

            var result = query.Select(p => new RuleEventItem()
            {
                ID = p.ID,
                GameID = p.GameID.HasValue ? p.GameID.Value : 0,
                DateStart = p.DateStart.HasValue ? p.DateStart.Value : DateTime.Now,
                DateEnd = p.DateEnd.HasValue ? p.DateEnd.Value : DateTime.Now,
                IsShow = p.IsShow.HasValue && p.IsShow.Value,
                DateCreated = p.DateCreated.HasValue ? p.DateCreated.Value : DateTime.Now,
                DateStartString = p.DateStart.HasValue ? p.DateStart.Value.ToString("dd-MM-yyyy HH:mm") : DateTime.Now.ToString("dd-MM-yyyy HH:mm"),
                DateEndString = p.DateEnd.HasValue ? p.DateEnd.Value.ToString("dd-MM-yyyy HH:mm") : DateTime.Now.ToString("dd-MM-yyyy HH:mm"),
                CountHu = p.CountHu.HasValue ? p.CountHu.Value : 0,
                LevelRoom = p.LevelRoom.HasValue ? p.LevelRoom.Value : 0,
                Nhan = p.Nhan.HasValue ? p.Nhan.Value : 0,
                MaxHu = p.MaxHu.HasValue ? p.MaxHu.Value : 0,
                MaxCashHu = p.MaxCashHu.HasValue ? p.MaxCashHu.Value : 0,
                gamename = p.gamename
            }).ToList();

            return result;


        }

        public int InsertRuleEvent(RuleEventItem obj)
        {
            var rs = 0;
            string sql = "INSERT INTO [newDB].[dbo].[Rule_Event]([GameID],[DateStart],[DateEnd],[IsShow],[IsDelete],[DateCreated],[CountHu],[Nhan],[LevelRoom],[MaxHu],[MaxCashHu]) VALUES(@GameID,@DateStart,@DateEnd,1,0,GETDATE(),@CountHu,@Nhan,@LevelRoom,@MaxHu,@MaxCashHu);";
            SqlConnection conn = new SqlConnection(Constants.DBConnection);
            SqlCommand MyCommand = new SqlCommand(sql, conn);
            MyCommand.Parameters.AddWithValue("GameID", obj.GameID);
            MyCommand.Parameters.AddWithValue("DateStart", obj.DateStart);
            MyCommand.Parameters.AddWithValue("DateEnd", obj.DateEnd);
            MyCommand.Parameters.AddWithValue("CountHu", obj.CountHu);
            MyCommand.Parameters.AddWithValue("Nhan", obj.Nhan);
            MyCommand.Parameters.AddWithValue("LevelRoom", obj.LevelRoom);
            MyCommand.Parameters.AddWithValue("MaxHu", obj.MaxHu);
            MyCommand.Parameters.AddWithValue("MaxCashHu", obj.MaxCashHu);
            conn.Open();
            rs = MyCommand.ExecuteNonQuery();
            conn.Close();
            return rs;
        }

        public RuleEventItem GetDetailRuleEvent(int ID)
        {
            string sql = "SELECT * FROM [newDB].[dbo].[Rule_Event] where ID = " + ID;


            var query = web365db.Database.SqlQuery<RuleEventItem>(sql);

            var result = query.Select(p => new RuleEventItem()
            {
                ID = p.ID,
                GameID = p.GameID.HasValue ? p.GameID.Value : 0,
                DateStart = p.DateStart.HasValue ? p.DateStart.Value : DateTime.Now,
                DateEnd = p.DateEnd.HasValue ? p.DateEnd.Value : DateTime.Now,
                IsShow = p.IsShow.HasValue && p.IsShow.Value,
                DateStartString = p.DateStart.HasValue ? p.DateStart.Value.ToString("dd-MM-yyyy HH:mm") : DateTime.Now.ToString("dd-MM-yyyy HH:mm"),
                DateEndString = p.DateEnd.HasValue ? p.DateEnd.Value.ToString("dd-MM-yyyy HH:mm") : DateTime.Now.ToString("dd-MM-yyyy HH:mm"),
                DateCreated = p.DateCreated.HasValue ? p.DateCreated.Value : DateTime.Now,
                CountHu = p.CountHu.HasValue ? p.CountHu.Value : 0,
                LevelRoom = p.LevelRoom.HasValue ? p.LevelRoom.Value : 0,
                Nhan = p.Nhan.HasValue ? p.Nhan.Value : 0,
                MaxHu = p.MaxHu.HasValue ? p.MaxHu.Value : 0,
                MaxCashHu = p.MaxCashHu.HasValue ? p.MaxCashHu.Value : 0,
            }).FirstOrDefault();

            return result;
        }

        public int UpdateRuleEvent(RuleEventItem obj)
        {
            var rs = 0;
            string sql = "UPDATE [newDB].[dbo].[Rule_Event] set GameID = @GameID,DateStart=@DateStart,DateEnd=@DateEnd,CountHu=@CountHu,Nhan=@Nhan,LevelRoom=@LevelRoom,MaxHu=@MaxHu,MaxCashHu=@MaxCashHu where ID = @ID";
            SqlConnection conn = new SqlConnection(Constants.DBConnection);
            SqlCommand MyCommand = new SqlCommand(sql, conn);
            MyCommand.Parameters.AddWithValue("GameID", obj.GameID);
            MyCommand.Parameters.AddWithValue("DateStart", obj.DateStart);
            MyCommand.Parameters.AddWithValue("DateEnd", obj.DateEnd);
            MyCommand.Parameters.AddWithValue("CountHu", obj.CountHu);
            MyCommand.Parameters.AddWithValue("Nhan", obj.Nhan);
            MyCommand.Parameters.AddWithValue("LevelRoom", obj.LevelRoom);
            MyCommand.Parameters.AddWithValue("MaxHu", obj.MaxHu);
            MyCommand.Parameters.AddWithValue("MaxCashHu", obj.MaxCashHu);
            MyCommand.Parameters.AddWithValue("ID", obj.ID);
            conn.Open();
            rs = MyCommand.ExecuteNonQuery();
            conn.Close();
            return rs;
        }

        public int DeleteRuleEvent(int ID)
        {
            var rs = 0;
            string sql = "update  [newDB].[dbo].[Rule_Event]  set IsDelete = 1 where ID = @id";
            SqlConnection conn = new SqlConnection(Constants.DBConnection);
            SqlCommand MyCommand = new SqlCommand(sql, conn);
            MyCommand.Parameters.AddWithValue("id", ID);
            conn.Open();
            rs = MyCommand.ExecuteNonQuery();
            conn.Close();
            return rs;
        }


        public AnnouncementItem GetOneEventItem(int ID)
        {
            return _announcementBusiness.GetOne(ID);
        }

        public int InsertEvent(string subject, string content, DateTime begin_time, DateTime and_time, string UrlImage, int gameid, string doituong, string thoigian, int DisplayOrder)
        {
            var obj = _announcementBusiness.Insert(subject, content, begin_time, and_time, UrlImage, gameid, doituong, thoigian, DisplayOrder);
            return obj;
        }

        public int UpdateEvent(int ID, string subject, string content, DateTime begin_time, DateTime and_time, string UrlImage, int gameid, string doituong, string thoigian, int DisplayOrder)
        {
            var obj = _announcementBusiness.Update(ID, subject, content, begin_time, and_time, UrlImage, gameid, doituong, thoigian, DisplayOrder);
            return obj;
        }

        public int DeleteEvent(int ID)
        {
            var obj = _announcementBusiness.Delete(ID);
            return obj;
        }
        #endregion

        #region quản lý gifcode

        public List<GiftCodeItem2> GetAllGiftCodeByTurnID(int turnId)
        {
            using (var db = new Entities365())
            {
                db.Database.Initialize(force: false);

                var cmd = db.Database.Connection.CreateCommand();

                cmd.CommandText = "exec [dbo].[POC_GetListGiftCodeByTurnID] " + turnId;

                try
                {
                    db.Database.Connection.Open();
                    var reader = cmd.ExecuteReader();
                    var queryProduct = ((IObjectContextAdapter)db)
                        .ObjectContext
                        .Translate<GiftCodeItem2>(reader, "tblGameLogsMoney", MergeOption.AppendOnly);
                    var result = queryProduct.Select(p => new GiftCodeItem2
                    {
                        Code = p.Code,
                        DateCreated = p.DateCreated.HasValue ? p.DateCreated.Value : DateTime.Now,
                        DateCreatedString = p.DateCreated.HasValue ? p.DateCreated.Value.ToString("dd-MM-yyyy HH:mm") : DateTime.Now.ToString("dd-MM-yyyy HH:mm"),
                        Money = p.Money.HasValue ? p.Money.Value : 0,
                        DateExpired = p.DateExpired.HasValue ? p.DateExpired.Value : DateTime.Now,
                        DateExpiredString = p.DateExpired.HasValue ? p.DateExpired.Value.ToString("dd-MM-yyyy HH:mm") : DateTime.Now.ToString("dd-MM-yyyy HH:mm"),
                        username = !string.IsNullOrEmpty(p.username) ? p.username : "0",
                        user_id = p.user_id.HasValue ? p.user_id.Value : 0,
                        DateUsed = p.DateUsed.HasValue ? p.DateUsed.Value : DateTime.Now,
                        DateUsedString = p.DateUsed.HasValue ? p.DateUsed.Value.ToString("dd-MM-yyyy HH:mm") : "",
                        TypeName = p.TypeName,
                    }).ToList();

                    return result;
                }
                finally
                {
                    db.Database.Connection.Close();
                }
            }
        }

        public int UpdateTurnGift(int ID, int status)
        {
            var rs = 0;
            string sql = "";
            if (status == 0)
            {
                sql = "update [newDB].[dbo].[gift_turn] set IsShow = 0 where ID = @id";
            }
            else if (status == 1)
            {
                sql = "update [newDB].[dbo].[gift_turn] set IsShow = 1 where ID = @id";
            }

            SqlConnection conn = new SqlConnection(Constants.DBConnection);
            SqlCommand MyCommand = new SqlCommand(sql, conn);
            MyCommand.Parameters.AddWithValue("id", ID);
            conn.Open();
            rs = MyCommand.ExecuteNonQuery();
            conn.Close();
            return rs;
        }

        public List<GiftTurnItem> GetAllGiftCode(out int total, bool used, int skip, int take)
        {
            total = 0;
            using (var db = new Entities365())
            {
                db.Database.Initialize(force: false);

                var cmd = db.Database.Connection.CreateCommand();

                cmd.CommandText = "exec [dbo].[POC_GetListGiftCodeTurn] " + Convert.ToInt32(used) + "," + skip + "," + take;

                try
                {
                    db.Database.Connection.Open();
                    var reader = cmd.ExecuteReader();
                    var queryProduct = ((IObjectContextAdapter)db)
                        .ObjectContext
                        .Translate<GiftTurnItem>(reader, "tblGameLogsMoney", MergeOption.AppendOnly);
                    var result = queryProduct.Select(p => new GiftTurnItem
                    {
                        ID = p.ID,
                        Quantity = p.Quantity,
                        Name = p.Name,
                        NoteFix = p.NoteFix,
                        Prefix = p.Prefix,
                        DateExpired = p.DateExpired.HasValue ? p.DateExpired.Value : DateTime.Now,
                        DateExpiredString = p.DateExpired.HasValue ? p.DateExpired.Value.ToString("dd-MM-yyyy HH:mm") : DateTime.Now.ToString("dd-MM-yyyy HH:mm"),
                        TypeID = p.TypeID,
                        Note = p.Note,
                        IsShow = p.IsShow.HasValue ? p.IsShow.Value : false,
                        IsDelete = p.IsDelete.HasValue ? p.IsDelete.Value : false,
                        DateCreated = p.DateCreated.HasValue ? p.DateCreated.Value : DateTime.Now,
                        DateCreatedString = p.DateCreated.HasValue ? p.DateCreated.Value.ToString("dd-MM-yyyy HH:mm") : DateTime.Now.ToString("dd-MM-yyyy HH:mm"),
                        TypeName = p.TypeName,
                        //Money = p.Money.HasValue ? p.Money.Value : 0
                    }).ToList();

                    reader.NextResult();

                    var tt = ((IObjectContextAdapter)db)
                        .ObjectContext
                        .Translate<int>(reader);

                    total = tt.FirstOrDefault();

                    return result;
                }
                finally
                {
                    db.Database.Connection.Close();
                }
            }
        }

        public bool DeleteGiftCode(int id)
        {
            try
            {
                var query = web365db.Database.SqlQuery<UserInfoNew>("Update [newDB].[dbo].giftcode set IsDelete = 1 where ID = @id select 'xxx' as username",
                new SqlParameter("id", id)
                );

                var result = query.Select(p => new UserInfoNew()
                {
                    username = p.username
                }).FirstOrDefault();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool GeneratorGiftCode(string name, DateTime dateExpired, int Gift_ID, string prefix, string note, string giatri, string soluong)
        {
            try
            {
                _giftCodeBusiness.Generator(name, dateExpired, Gift_ID, prefix, note, giatri, soluong);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool InsertOneItemGameGift(string code, int value, string name, DateTime dateExpired, int isVCash)
        {
            try
            {
                _giftCodeBusiness.InsertOneItem(code, value, name, dateExpired, isVCash);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region quản lý chữ chạy

        public string GetText()
        {
            return _noticeTextBusiness.GetText();
        }

        public int UpdateText(string noticeText)
        {
            return _noticeTextBusiness.Update(noticeText);
        }

        public List<GameTextItem> GetAllTextRun()
        {
            var query = _noticeTextBusiness.GetAll();
            return query.Select(p => new GameTextItem()
            {
                ID = p.ID,
                Title = p.Title,
                Link = p.Link,
                DataStart = p.DataStart,
                DateEnd = p.DateEnd,
                IsDelete = p.IsDelete,
                Order = p.Order,
                DataStartstring = p.DataStartstring,
                DateEndstring = p.DateEndstring
            }).ToList();
        }

        public GameTextItem GetOneTextRunItem(int ID)
        {
            return _noticeTextBusiness.GetOne(ID);
        }

        public int InsertTextRun(string Title, string Link, DateTime DataStart, DateTime DateEnd, bool IsDelete, int Order)
        {
            var obj = _noticeTextBusiness.Insert(Title, Link, DataStart, DateEnd, IsDelete, Order);
            return obj;
        }

        public int UpdateTextRun(int ID, string Title, string Link, DateTime DataStart, DateTime DateEnd, bool IsDelete, int Order)
        {
            var obj = _noticeTextBusiness.Update(ID, Title, Link, DataStart, DateEnd, IsDelete, Order);
            return obj;
        }

        public int DeleteTextRun(int ID)
        {
            var obj = _noticeTextBusiness.Delete(ID);
            return obj;
        }

        #endregion

        public List<RoomItem> RoomGetAll(int gameId)
        {
            return _roomBusiness.GetAll(gameId);
        }

        public int RoomUpdate(RoomItem room)
        {
            return _roomBusiness.Update(room);
        }

        public int RoomInsert(RoomItem room)
        {
            return _roomBusiness.Insert(room);
        }

        #region 9999

        public List<UserInfo> GetTopGem()
        {
            return _gameBusiness.GetTopGem();
        }

        public List<UserInfo> GetTopXu()
        {
            return _gameBusiness.GetTopXu();
        }

        public List<UserInfo> GetTopExp()
        {
            return _gameBusiness.GetTopExp();
        }

        public List<UserInfo> GetTopDoiThuong()
        {
            return _gameBusiness.GetTopDoiThuong();
        }

        public List<UserInfo> GetTopNapTien()
        {
            return _gameBusiness.GetTopNapTien();
        }

        public List<UserInfo> GetGiaoDich(out int total, int type, int skip, int take, int uid = 0)
        {
            return _gameBusiness.GetGiaoDich(out total, type, skip, take, uid);
        }

        public PaymentItem InsertPayment(PaymentItem pay)
        {
            return _userInfoBusiness.InsertPayment(pay);
        }

        public PaymentItem GetDetailPaymentById(long id)
        {
            return _userInfoBusiness.GetDetailPaymentById(id);
        }

        public PaymentItem GetDetailPaymentByToken(string token)
        {
            return _userInfoBusiness.GetDetailPaymentByToken(token);
        }

        public PaymentItem UpdatePayment(PaymentItem pay)
        {
            return _userInfoBusiness.UpdatePayment(pay);
        }

        public PaymentItem InsertMoney(PaymentItem pay)
        {
            return _userInfoBusiness.InsertMoney(pay);
        }

        public RoomItem GetDetailRoom(int roomId)
        {
            return _roomBusiness.GetDetailRoom(roomId);
        }

        #endregion

        public ReportOnlineItem ReportOnline()
        {
            return _userInfoBusiness.ReportOnline();
        }
        public List<GameCashItem> GetCash(string date)
        {
            return _userInfoBusiness.GetCash(date);
        }

        #region quản lý thẻ nạp
        public int CardInsertItem(CardItem obj)
        {
            return _cardCodeBusiness.InsertItem(obj);
        }

        public List<CardItem> GetAllCard(out int total, int skip, int take, int used = 0, int telcoId = 0, int value = 0, string seri = "")
        {
            return _cardCodeBusiness.GetAllCard(out total, skip, take, used, telcoId, value, seri);
        }
        #endregion


        #region config Poker
        public List<ConfigMiniPokerItem> GetConfigMiniPoker()
        {
            return _cardCodeBusiness.GetConfigMiniPoker();
        }

        public ConfigMiniPokerItem GetConfigMiniPokerDetail(int id)
        {
            return _cardCodeBusiness.GetConfigMiniPokerDetail(id);
        }

        public int UpdateConfigMiniPoker(ConfigMiniPokerItem obj)
        {
            return _cardCodeBusiness.UpdateConfigMiniPoker(obj);
        }
        #endregion


        #region config Xeng
        public List<ConfigMiniPokerItem> GetConfigXeng()
        {
            return _cardCodeBusiness.GetConfigXeng();
        }

        public ConfigMiniPokerItem GetConfigXengDetail(int id)
        {
            return _cardCodeBusiness.GetConfigXengDetail(id);
        }

        public int UpdateConfigXeng(ConfigMiniPokerItem obj)
        {
            return _cardCodeBusiness.UpdateConfigXeng(obj);
        }
        #endregion

        #region config TaiXiu
        public List<ConfigTaiXiuItem> GetConfigTaiXiu()
        {
            var query = web365db.Database.SqlQuery<ConfigTaiXiuItem>("SELECT * FROM [newDB].[dbo].configBotTaixiu");

            var result = query.Select(p => new ConfigTaiXiuItem()
            {
                ID = p.ID,
                HourStart = p.HourStart,
                HourEnd = p.HourEnd,
                MaxCashBot = p.MaxCashBot.HasValue ? p.MaxCashBot.Value : 0,
                MaxBetBot = p.MaxBetBot.HasValue ? p.MaxBetBot.Value : 0,
                MinBetBot = p.MinBetBot.HasValue ? p.MinBetBot.Value : 0,
                HanMucAm = p.MinBetBot.HasValue ? p.HanMucAm.Value : 0,
                IsShow = p.IsShow.Value,
                IsDelete = p.IsDelete.Value
            }).ToList();

            return result;
        }

        public ConfigTaiXiuItem GetConfigTaiXiuDetail(int id)
        {
            var query = web365db.Database.SqlQuery<ConfigTaiXiuItem>("SELECT * FROM [newDB].[dbo].configBotTaixiu where ID = @id", new SqlParameter("id", id));

            var result = query.Select(p => new ConfigTaiXiuItem()
            {
                ID = p.ID,
                HourStart = p.HourStart,
                HourEnd = p.HourEnd,
                MaxCashBot = p.MaxCashBot.HasValue ? p.MaxCashBot.Value : 0,
                MaxBetBot = p.MaxBetBot.HasValue ? p.MaxBetBot.Value : 0,
                MinBetBot = p.MinBetBot.HasValue ? p.MinBetBot.Value : 0,
                HanMucAm = p.HanMucAm.HasValue ? p.HanMucAm.Value : 0,
                IsShow = p.IsShow.Value,
                IsDelete = p.IsDelete.Value
            }).FirstOrDefault();

            return result;
        }

        public ConfigTaiXiuItem UpdateConfigTaiXiu(ConfigTaiXiuItem obj)
        {
            if (obj.ID > 0)
            {
                var query = web365db.Database.SqlQuery<ConfigTaiXiuItem>("Update [newDB].[dbo].configBotTaixiu set HanMucAm = @HanMucAm, IsShow = @IsShow, [HourStart] = @HourStart,[HourEnd] = @HourEnd, [MaxCashBot] = @MaxCashBot, [MaxBetBot] = @MaxBetBot, [MinBetBot] = @MinBetBot  where ID = @id select 1 as ID"
                    , new SqlParameter("id", obj.ID)
                    , new SqlParameter("HourStart", obj.HourStart)
                    , new SqlParameter("HourEnd", obj.HourEnd)
                    , new SqlParameter("MaxCashBot", obj.MaxCashBot)
                    , new SqlParameter("MaxBetBot", obj.MaxBetBot)
                    , new SqlParameter("MinBetBot", obj.MinBetBot)
                    , new SqlParameter("HanMucAm", obj.HanMucAm)
                    , new SqlParameter("IsShow", obj.IsShow)
                    );

                var result = query.Select(p => new ConfigTaiXiuItem()
                {
                    ID = p.ID
                }).FirstOrDefault();
                return result;
            }
            else
            {
                var query = web365db.Database.SqlQuery<ConfigTaiXiuItem>("INSERT INTO [newDB].[dbo].[configBotTaixiu]([HourStart],[HourEnd],[MaxCashBot],[MaxBetBot],[MinBetBot],[IsShow],[IsDelete],[HanMucAm])VALUES(@HourStart, @HourEnd,  @MaxCashBot, @MaxBetBot, @MinBetBot, 1, 0, @HanMucAm)"
                   , new SqlParameter("HourStart", obj.HourStart)
                   , new SqlParameter("HourEnd", obj.HourEnd)
                   , new SqlParameter("MaxCashBot", obj.MaxCashBot)
                   , new SqlParameter("MaxBetBot", obj.MaxBetBot)
                   , new SqlParameter("MinBetBot", obj.MinBetBot)
                   , new SqlParameter("HanMucAm", obj.HanMucAm)
                   , new SqlParameter("IsShow", obj.IsShow)
                   );

                var result = query.Select(p => new ConfigTaiXiuItem()
                {
                    ID = p.ID
                }).FirstOrDefault();
                return result;
            }

        }
        #endregion

        #region game logs
        //public bool AddGameLogs(tblGameConfigLogs obj)
        //{
        //    try
        //    {
        //        web365db.tblGameConfigLogs.Add(obj);
        //        web365db.SaveChanges();
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }

        //}

        public List<LogGameItem> GetListGameLogs(out int total, int gameId, int currentRecord, int numberRecord)
        {
            var query = from p in web365db.tblGameConfigLogs
                        where p.gameId == gameId
                        select p;


            total = query.Count();
            query = query.OrderByDescending("ID");
            return query.Select(p => new LogGameItem()
            {
                ID = p.ID,
                CreateBy = p.CreateBy,
                DateCreared = p.DateCreared.HasValue ? p.DateCreared.Value : DateTime.Now,
                a = p.a.HasValue ? p.a.Value : 0,
                a1 = p.a1.HasValue ? p.a1.Value : 0,
                a2 = p.a2.HasValue ? p.a2.Value : 0,
                a3 = p.a3.HasValue ? p.a3.Value : 0,
                a4 = p.a4.HasValue ? p.a4.Value : 0,
                a5 = p.a5.HasValue ? p.a5.Value : 0,
                a6 = p.a6.HasValue ? p.a6.Value : 0,
                a7 = p.a7.HasValue ? p.a7.Value : 0,
                a8 = p.a8.HasValue ? p.a8.Value : 0,
                a9 = p.a9.HasValue ? p.a9.Value : 0,
                a10 = p.a10.HasValue ? p.a10.Value : 0,
                gameId = p.gameId.HasValue ? p.gameId.Value : 0
            }).Skip(currentRecord).Take(numberRecord).ToList();
        }

        public LogGameItem GetLogDetail(int id)
        {
            var query = web365db.tblGameConfigLogs.Where(c => c.ID == id).ToList();

            return query.Select(p => new LogGameItem()
            {
                ID = p.ID,
                CreateBy = p.CreateBy,
                DateCreared = p.DateCreared.HasValue ? p.DateCreared.Value : DateTime.Now,
                a = p.a.HasValue ? p.a.Value : 0,
                a1 = p.a1.HasValue ? p.a1.Value : 0,
                a2 = p.a2.HasValue ? p.a2.Value : 0,
                a3 = p.a3.HasValue ? p.a3.Value : 0,
                a4 = p.a4.HasValue ? p.a4.Value : 0,
                a5 = p.a5.HasValue ? p.a5.Value : 0,
                a6 = p.a6.HasValue ? p.a6.Value : 0,
                a7 = p.a7.HasValue ? p.a7.Value : 0,
                a8 = p.a8.HasValue ? p.a8.Value : 0,
                a9 = p.a9.HasValue ? p.a9.Value : 0,
                a10 = p.a10.HasValue ? p.a10.Value : 0,
                //a11 = p.a11.HasValue ? p.a11.Value : 0,
                //a12 = p.a12.HasValue ? p.a12.Value : 0,
                //a13 = p.a13.HasValue ? p.a13.Value : 0,
                //a14 = p.a14.HasValue ? p.a14.Value : 0,
                //a15 = p.a15.HasValue ? p.a15.Value : 0,
                //a16 = p.a16.HasValue ? p.a16.Value : 0,
                //a17 = p.a17.HasValue ? p.a17.Value : 0,
                //a18 = p.a18.HasValue ? p.a18.Value : 0,
                //a19 = p.a19.HasValue ? p.a19.Value : 0,
                gameId = p.gameId.HasValue ? p.gameId.Value : 0
            }).FirstOrDefault();
        }

        public List<PaymentItem> GetPayment(out int total, int skip, int take, int uid = 0, string username = "",
            string date = "", string status = "")
        {
            return _gameBusiness.GetPayment(out total, skip, take, uid, username, date, status);
        }

        public PaymentItem PaymentDetail(int id)
        {
            return _gameBusiness.PaymentDetail(id);
        }

        #endregion

        #region game guide

        public int GameGuideUpdate(GameGuideItem obj)
        {
            return _noticeTextBusiness.GameGuideUpdate(obj);
        }
        public int GameGuideInsert(GameGuideItem obj)
        {
            return _noticeTextBusiness.GameGuideInsert(obj);
        }
        public int GameGuideDelete(int id)
        {
            return _noticeTextBusiness.GameGuideDelete(id);
        }
        public List<GameGuideItem> GameGuideGetAll()
        {
            return _noticeTextBusiness.GameGuideGetAll();
        }
        public GameGuideItem GameGuideGetOne(int id)
        {
            return _noticeTextBusiness.GameGuideGetOne(id);
        }
        #endregion


        #region tool dai ly
        public List<GameLogsMoneyItem> DlGetListGameLogs(out int total, string userName, int type, int currentPage, int pageSize, string key, int statusId, string fDate, string tDate)
        {

            var result = new List<GameLogsMoneyItem>();
            var paramTotal = new SqlParameter
            {
                ParameterName = "total",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };

            if (!string.IsNullOrEmpty(fDate))
            {
                fDate = Convert.ToDateTime(fDate).ToString("yyyy-MM-dd 00:00:00.000");
            }
            if (!string.IsNullOrEmpty(tDate))
            {
                tDate = Convert.ToDateTime(tDate).ToString("yyyy-MM-dd 23:59:59.999");
            }

            var query = web365db.Database.SqlQuery<GameLogsMoneyItem>("exec [dbo].[PRC_GetLogsMoneyByUserName] @userName, @type, @currentPage, @pageSize, @key, @statusId, @fDate, @tDate, @total OUTPUT",
                new SqlParameter("userName", userName),
                new SqlParameter("type", type),
                new SqlParameter("currentPage", currentPage),
                new SqlParameter("pageSize", pageSize),
                new SqlParameter("key", key),
                new SqlParameter("statusId", statusId),
                new SqlParameter("fDate", fDate),
                new SqlParameter("tDate", tDate),
                paramTotal);

            result = query.Select(p => new GameLogsMoneyItem()
            {
                ID = p.ID,
                LogID = p.LogID,
                UserSend = p.UserSend,
                StatusID = p.StatusID.HasValue ? p.StatusID.Value : 0,
                UserRecive = p.UserRecive,
                MoneySend = p.MoneySend.HasValue ? p.MoneySend.Value : 0,
                MoneyRecive = p.MoneyRecive.HasValue ? p.MoneyRecive.Value : 0,
                TaxMoney = p.TaxMoney.HasValue ? p.TaxMoney.Value : 0,
                TaxMoneyParent = p.TaxMoneyParent.HasValue ? p.TaxMoneyParent.Value : 0,
                Note = p.Note,
                DateCreated = p.DateCreated,
                DateCreatedString = p.DateCreated.ToString()
            }).ToList();

            total = Convert.ToInt32(paramTotal.Value);

            return result;
        }

        public List<GameLogsMoneyItem> GetFilterLogsPoc(out int total, string nickSend, string nickRecive, int id, int currentPage, int pageSize)
        {

            var result = new List<GameLogsMoneyItem>();
            var paramTotal = new SqlParameter
            {
                ParameterName = "total",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };

            var query = web365db.Database.SqlQuery<GameLogsMoneyItem>("exec [dbo].[PRC_GetLogsPoc] @nickSend,@nickRecive, @id ,@currentPage, @pageSize, @total OUTPUT",
                new SqlParameter("nickSend", nickSend),
                new SqlParameter("nickRecive", nickRecive),
                new SqlParameter("id", id),
                new SqlParameter("currentPage", currentPage),
                new SqlParameter("pageSize", pageSize),
                paramTotal);

            result = query.Select(p => new GameLogsMoneyItem()
            {
                ID = p.ID,
                UserSend = p.UserSend,
                UserRecive = p.UserRecive,
                MoneySend = p.MoneySend.HasValue ? (float)Math.Round(p.MoneySend.Value) : 0,
                MoneyRecive = p.MoneyRecive.HasValue ? (float)Math.Round(p.MoneyRecive.Value) : 0,
                TaxMoney = p.TaxMoney.HasValue ? (float)Math.Round(p.TaxMoney.Value) : 0,
                TaxMoneyParent = p.TaxMoneyParent.HasValue ? (float)Math.Round(p.TaxMoneyParent.Value) : 0,
                ContentTransaction = p.ContentTransaction,
                DateCreated = p.DateCreated,
                DateCreatedString = p.DateCreated.ToString(),
                SoDuNguoiNhan = p.SoDuNguoiNhan.HasValue ? p.SoDuNguoiNhan.Value : 0,
                KetNguoiNhan = p.KetNguoiNhan.HasValue ? p.KetNguoiNhan.Value : 0,
                ShopLevelSend = p.ShopLevelSend.HasValue ? p.ShopLevelSend.Value : 0,
                ShopLevelRecive = p.ShopLevelRecive.HasValue ? p.ShopLevelRecive.Value : 0,
            }).ToList();

            total = Convert.ToInt32(paramTotal.Value);

            return result;
        }


        public RetetionItem GetRetention(string fDateReg, string tDateReg, string fDateReciveMoney, string tDateReciveMoney)
        {

            if (!string.IsNullOrEmpty(fDateReg))
            {
                fDateReg = Convert.ToDateTime(fDateReg).ToString("yyyy-MM-dd 00:00:00.000");
            }
            if (!string.IsNullOrEmpty(tDateReg))
            {
                tDateReg = Convert.ToDateTime(tDateReg).ToString("yyyy-MM-dd 23:59:59.999");
            }

            if (!string.IsNullOrEmpty(fDateReciveMoney))
            {
                fDateReciveMoney = Convert.ToDateTime(fDateReciveMoney).ToString("yyyy-MM-dd 00:00:00.000");
            }
            if (!string.IsNullOrEmpty(tDateReciveMoney))
            {
                tDateReciveMoney = Convert.ToDateTime(tDateReciveMoney).ToString("yyyy-MM-dd 23:59:59.999");
            }

            var query = web365db.Database.SqlQuery<RetetionItem>("exec [dbo].[POC_RETENTIONRATE] @fDateReg, @tDateReg, @fDateReciveMoney, @tDateReciveMoney",
                new SqlParameter("fDateReg", fDateReg),
                new SqlParameter("tDateReg", tDateReg),
                new SqlParameter("fDateReciveMoney", fDateReciveMoney),
                new SqlParameter("tDateReciveMoney", tDateReciveMoney)
                );

            var result = query.Select(p => new RetetionItem()
            {
                Nru = p.Nru.HasValue ? p.Nru.Value : 0,
                Pu = p.Pu.HasValue ? p.Pu.Value : 0

            }).FirstOrDefault();

            return result;
        }

        public UserInfoNew RefundPoc(int id, int type)
        {
            var query = web365db.Database.SqlQuery<UserInfoNew>("exec [dbo].[PRC_RefundPoc] @id, @type",
                new SqlParameter("id", id),
                new SqlParameter("type", type)
                );

            var result = query.Select(p => new UserInfoNew()
            {
                Cash = p.Cash.HasValue ? p.Cash.Value : 0

            }).FirstOrDefault();

            return result;
        }

        public UserInfoNew UpdateLogsGameDetail(int logDetailId, int statusId, string note)
        {
            var query = web365db.Database.SqlQuery<UserInfoNew>("exec [dbo].[PRC_UpdateLogsGameDetail] @logDetailId, @statusId, @note",
                new SqlParameter("logDetailId", logDetailId),
                new SqlParameter("statusId", statusId),
                new SqlParameter("note", note)
                );

            var result = query.Select(p => new UserInfoNew()
            {
                username = p.username
            }).FirstOrDefault();

            return result;
        }

        public UserInfoNew GetDetailPlayer(string username)
        {
            var query = web365db.Database.SqlQuery<UserInfoNew>("exec [dbo].[PRC_GetDetailPlayer] @userName",
                new SqlParameter("userName", username));

            var result = query.Select(p => new UserInfoNew()
            {
                user_id = p.user_id,
                username = p.username,
                gameCash = p.gameCash,
                parent = p.parent,
                ShopLevel = p.ShopLevel.HasValue ? p.ShopLevel.Value : 0
            }).FirstOrDefault();

            return result;
        }

        public UserInfoNew CheckPlayerName(string username)
        {
            var query = web365db.Database.SqlQuery<UserInfoNew>("exec [dbo].[PRC_GetDetailPlayer] @userName",
                new SqlParameter("userName", username));

            var result = query.Select(p => new UserInfoNew()
            {
                username = p.username,
                ShopLevel = p.ShopLevel.HasValue ? p.ShopLevel.Value : 0
            }).FirstOrDefault();

            return result;
        }

        public UserInfoNew CheckDaiLy(string username)
        {
            var query = web365db.Database.SqlQuery<UserInfoNew>("exec [dbo].[PRC_PortalUser] @userName",
                new SqlParameter("userName", username));

            var result = query.Select(p => new UserInfoNew()
            {
                username = p.username,
                ShopLevel = p.ShopLevel.HasValue ? p.ShopLevel.Value : 0
            }).FirstOrDefault();

            return result;
        }

        public UserInfoNew GetPortalUser(string username)
        {
            var query = web365db.Database.SqlQuery<UserInfoNew>("exec [dbo].[PRC_PortalUser] @userName",
                new SqlParameter("userName", username));

            var result = query.Select(p => new UserInfoNew()
            {
                username = p.username,
                password = p.password,
                ShopLevel = p.ShopLevel.HasValue ? p.ShopLevel.Value : 0
            }).FirstOrDefault();

            return result;
        }

        public UserInfoNew CheckUserInMember(string username)
        {
            var query = web365db.Database.SqlQuery<UserInfoNew>("exec [dbo].[PRC_CheckUserInMember] @userName",
                new SqlParameter("userName", username));

            var result = query.Select(p => new UserInfoNew()
            {
                username = p.username,
                UserId = p.UserId.HasValue ? p.UserId.Value : 0,
                ShopLevel = p.ShopLevel.HasValue ? p.ShopLevel.Value : 0
            }).FirstOrDefault();

            return result;
        }

        public UserInfoNew SetDaiLy(string username, int level, string parent = "")
        {
            var query = web365db.Database.SqlQuery<UserInfoNew>("exec [dbo].[PRC_SetDaiLy] @userName, @level, @parent",
                new SqlParameter("userName", username),
                new SqlParameter("level", level),
                new SqlParameter("parent", parent)
                );

            var result = query.Select(p => new UserInfoNew()
            {
                username = p.username,
                ShopLevel = p.ShopLevel.HasValue ? p.ShopLevel.Value : 0
            }).FirstOrDefault();

            return result;
        }

        public GameSlotCheckItem GameSlotGetDetail(int gameId, int matchId)
        {
            var queryStr = "";
            // ngu hanh
            if (gameId == 37)
            {
                queryStr = "SELECT [id],[game_id],[host_id],[cash],[time],[room_name],[slotMachineTotalLine],[slotMachineWinLine],[slotMachineWinCash],[slotMachineNoHu],[slotMachineNumber] FROM [newDB].[dbo].[game_log_bomtan_read] where [game_id] = " + gameId + " and [id] =" + matchId;
            }

            //galaxy
            if (gameId == 36)
            {
                queryStr = "SELECT [id],[game_id],[host_id],[cash],[time],[room_name],[slotMachineTotalLine],[slotMachineWinLine],[slotMachineWinCash],[slotMachineNoHu],[slotMachineNumber] FROM [newDB].[dbo].[game_log_galaxy_read] where [game_id] = " + gameId + " and [id] =" + matchId;
            }

            //ai cap
            if (gameId == 35)
            {
                queryStr = "SELECT [id],[game_id],[host_id],[cash],[time],[room_name],[slotMachineTotalLine],[slotMachineWinLine],[slotMachineWinCash],[slotMachineNoHu],[slotMachineNumber] FROM [newDB].[dbo].[game_log_aicap_read] where [game_id] = " + gameId + " and [id] =" + matchId;
            }

            //Thuy cung
            if (gameId == 34)
            {
                queryStr = "SELECT [id],[game_id],[host_id],[cash],[time],[room_name],[slotMachineTotalLine],[slotMachineWinLine],[slotMachineWinCash],[slotMachineNoHu],[slotMachineNumber] FROM [newDB].[dbo].[game_log_daoca_read] where [game_id] = " + gameId + " and [id] =" + matchId;
            }


            //mini poker
            if (gameId == 19)
            {
                queryStr = "SELECT [id],[game_id],[host_id],[cash],[time],[room_name],[miniPokerWinCash],[miniPokerCards],[miniPokerResult],[miniPokerString] FROM [newDB].[dbo].[game_log_minipoker_read] where [game_id] = " + gameId + " and [id] =" + matchId;
            }

            //kim cuong
            if (gameId == 20)
            {
                queryStr = "SELECT [id],[game_id],[host_id],[cash],[time],[room_name],[slotMachineTotalLine],[slotMachineWinLine],[slotMachineWinCash],[slotMachineNoHu],[slotMachineNumber] FROM [newDB].[dbo].[game_log_slotmachine_read] where [game_id] = " + gameId + " and [id] =" + matchId;
            }

            var query = web365db.Database.SqlQuery<GameSlotCheckItem>(queryStr);
            var result = new GameSlotCheckItem();
            if (gameId == 37 || gameId == 36 || gameId == 35 || gameId == 20 || gameId == 34)
            {
                result = query.Select(p => new GameSlotCheckItem()
                {
                    id = p.id.HasValue ? p.id.Value : 0,
                    cash = p.cash.HasValue ? p.cash.Value : 0,
                    game_id = p.game_id.HasValue ? p.game_id.Value : 0,
                    host_id = p.host_id.HasValue ? p.host_id.Value : 0,
                    room_name = p.room_name,
                    slotMachineNumber = p.slotMachineNumber,
                    slotMachineTotalLine = p.slotMachineTotalLine.HasValue ? p.slotMachineTotalLine.Value : 0,
                    slotMachineWinCash = p.slotMachineWinCash.HasValue ? p.slotMachineWinCash.Value : 0,
                    slotMachineWinLine = p.slotMachineWinLine,
                    miniPokerCards = p.miniPokerCards,
                    miniPokerResult = p.miniPokerResult,
                    miniPokerString = p.miniPokerString,
                    miniPokerWinCash = p.miniPokerWinCash.HasValue ? p.miniPokerWinCash.Value : 0,
                    time = p.time
                }).FirstOrDefault();
            }

            if (gameId == 19)
            {
                result = query.Select(p => new GameSlotCheckItem()
                {
                    id = p.id.HasValue ? p.id.Value : 0,
                    cash = p.cash.HasValue ? p.cash.Value : 0,
                    game_id = p.game_id.HasValue ? p.game_id.Value : 0,
                    host_id = p.host_id.HasValue ? p.host_id.Value : 0,
                    room_name = p.room_name,
                    miniPokerCards = p.miniPokerCards,
                    miniPokerResult = p.miniPokerResult,
                    miniPokerString = p.miniPokerString,
                    miniPokerWinCash = p.miniPokerWinCash.HasValue ? p.miniPokerWinCash.Value : 0,
                    time = p.time
                }).FirstOrDefault();
            }
            return result;
        }

        public ChuyenKhoanItem ChuyenKhoan(ChuyenKhoanItem itemCk)
        {
            try
            {
                using (var db = new Entities365())
                {
                    db.Database.Initialize(force: false);

                    var cmd = db.Database.Connection.CreateCommand();

                    cmd.CommandText = "exec [dbo].[PRC_InsertLogsGame] '" + itemCk.UserSend + "', '" + itemCk.UserRecive + "', " + itemCk.MoneySend.ToString().Replace(",", ".") + ", " + itemCk.MoneyRecive.ToString().Replace(",", ".") + ", " + itemCk.Tax.ToString().Replace(",", ".") + ", " + itemCk.TaxParent.ToString().Replace(",", ".") + ", " + itemCk.StatusID + ", '" + itemCk.Note + "', " + itemCk.TaxSystem.ToString().Replace(",", ".") + ", " + itemCk.TaxMoneyParent.ToString().Replace(",", ".") + ", " + itemCk.TaxSystem.ToString().Replace(",", ".") + ", " + itemCk.TaxMoneySystem.ToString().Replace(",", ".");

                    try
                    {
                        db.Database.Connection.Open();
                        var reader = cmd.ExecuteReader();
                        var queryProduct = ((IObjectContextAdapter)db)
                            .ObjectContext
                            .Translate<ChuyenKhoanItem>(reader, "tblGameLogsMoney", MergeOption.AppendOnly);
                        var result = queryProduct.Select(a => new ChuyenKhoanItem
                        {
                            UserSend = a.UserSend
                        }).FirstOrDefault();
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


        #endregion

        #region POC GAME

        public GiftCodeTypeItem UpdateGameGiftType(GiftCodeTypeItem obj)
        {
            var query = web365db.Database.SqlQuery<GiftCodeTypeItem>("exec [dbo].[PRC_UpdateDetailGameGiftType] @id,@name, @IsShow, @Money",
                new SqlParameter("id", obj.ID),
                new SqlParameter("name", obj.name),
                new SqlParameter("IsShow", obj.IsShow),
                new SqlParameter("Money", obj.Money)
                );

            var result = query.Select(p => new GiftCodeTypeItem()
            {
                ID = p.ID,
            }).FirstOrDefault();

            return result;
        }

        public GiftCodeTypeItem DeleteGameGiftType(int id)
        {
            var query = web365db.Database.SqlQuery<GiftCodeTypeItem>("exec [dbo].[PRC_DeleteDetailGameGiftType] @id",
                new SqlParameter("id", id)
                );

            var result = query.Select(p => new GiftCodeTypeItem()
            {
                ID = p.ID,
            }).FirstOrDefault();

            return result;
        }

        public GiftCodeTypeItem InsertGameGiftType(GiftCodeTypeItem obj)
        {
            var query = web365db.Database.SqlQuery<GiftCodeTypeItem>("exec [dbo].[PRC_InsertDetailGameGiftType] @name, @IsShow, @Money",
                new SqlParameter("name", obj.name),
                new SqlParameter("IsShow", obj.IsShow),
                new SqlParameter("@Money", obj.Money)
                );

            var result = query.Select(p => new GiftCodeTypeItem()
            {
                ID = p.ID,
            }).FirstOrDefault();

            return result;
        }

        public GiftCodeTypeItem GetDetailGameGiftType(int id)
        {
            var query = web365db.Database.SqlQuery<GiftCodeTypeItem>("exec [dbo].[PRC_GetDetailGameGiftType] @id",
                new SqlParameter("id", id));

            var result = query.Select(p => new GiftCodeTypeItem()
            {
                ID = p.ID,
                name = p.name,
                DateCreated = p.DateCreated.HasValue ? p.DateCreated.Value : DateTime.Now,
                IsShow = p.IsShow.HasValue ? p.IsShow.Value : false,
                IsDelete = p.IsDelete.HasValue ? p.IsDelete.Value : false,
                Money = p.Money.HasValue ? p.Money.Value : 0,
                DateCreatedString = p.DateCreated.HasValue ? p.DateCreated.Value.ToString("dd-MM-yyyy HH:mm") : DateTime.Now.ToString("dd-MM-yyyy HH:mm")
            }).FirstOrDefault();

            return result;
        }

        public List<GiftCodeTypeItem> GetListTypeGift(FilterItem objFilter)
        {
            var result = new List<GiftCodeTypeItem>();

            using (var db = new Entities365())
            {
                db.Database.Initialize(force: false);
                var queryComand = "EXEC [dbo].[POC_GetListGiftType] " + objFilter.currentRecord + "," + objFilter.numberRecord;
                var cmd = db.Database.Connection.CreateCommand();
                cmd.CommandText = queryComand;

                try
                {

                    db.Database.Connection.Open();
                    var reader = cmd.ExecuteReader();

                    var product = ((IObjectContextAdapter)db)
                        .ObjectContext
                        .Translate<GiftCodeTypeItem>(reader, "tblGameLogsMoney", MergeOption.AppendOnly);

                    result = product.Select(p => new GiftCodeTypeItem()
                    {
                        ID = p.ID,
                        name = p.name,
                        DateCreated = p.DateCreated.HasValue ? p.DateCreated.Value : DateTime.Now,
                        IsShow = p.IsShow.HasValue ? p.IsShow.Value : false,
                        IsDelete = p.IsDelete.HasValue ? p.IsDelete.Value : false,
                        Money = p.Money.HasValue ? p.Money.Value : 0,
                        DateCreatedString = p.DateCreated.HasValue ? p.DateCreated.Value.ToString("dd-MM-yyyy HH:mm") : DateTime.Now.ToString("dd-MM-yyyy HH:mm")
                    }).ToList();
                }
                finally
                {
                    db.Database.Connection.Dispose();
                    db.Database.Connection.Close();
                }
            }


            return result;
        }

        public List<GameReportItem> GetReportGameCcu(FilterItem objFilter)
        {
            var result = new List<GameReportItem>();
            var result2 = new List<GameReportItem>();

            if (!string.IsNullOrEmpty(objFilter.FromDate) && !string.IsNullOrEmpty(objFilter.ToDate))
            {
                var fdt = Convert.ToDateTime(objFilter.FromDate).ToString("yyyy-MM-dd HH:mm:ss");
                var tdt = Convert.ToDateTime(objFilter.ToDate).ToString("yyyy-MM-dd 23:59:59");
                objFilter.FromDate = fdt;
                objFilter.ToDate = tdt;
            }


            using (var db = new Entities365())
            {
                db.Database.Initialize(force: false);
                var queryComand = "EXEC [dbo].[POC_GetReportCCU] '" + objFilter.FromDate + "','" + objFilter.ToDate + "'";
                var cmd = db.Database.Connection.CreateCommand();
                cmd.CommandText = queryComand;

                try
                {

                    db.Database.Connection.Open();
                    var reader = cmd.ExecuteReader();

                    var product = ((IObjectContextAdapter)db)
                        .ObjectContext
                        .Translate<GameReportItem>(reader, "tblGameLogsMoney", MergeOption.AppendOnly);

                    result = product.Select(p => new GameReportItem()
                    {
                        DateCreate = p.DateCreate,
                        DateCreateString = p.DateCreate.ToString("yyyy-MM-dd"),
                        maxccu = p.maxccu.HasValue ? p.maxccu.Value : 0,
                        minccu = p.minccu.HasValue ? p.minccu.Value : 0,
                        avgccu = p.avgccu.HasValue ? p.avgccu.Value : 0,
                        totalAccountRegiter = p.totalAccountRegiter.HasValue ? p.totalAccountRegiter.Value : 0,
                        totalTrans = p.totalTrans.HasValue ? p.totalTrans.Value : 0,
                        totalCashPlay = p.totalCashPlay.HasValue ? p.totalCashPlay.Value : 0,
                    }).ToList();

                }
                finally
                {
                    db.Database.Connection.Dispose();
                    db.Database.Connection.Close();
                }
            }


            return result;
        }

        public List<GameReportItem> GetReportGameCcuOneDay(FilterItem objFilter)
        {
            var result = new List<GameReportItem>();

            if (!string.IsNullOrEmpty(objFilter.FromDate))
            {
                var fdt = Convert.ToDateTime(objFilter.FromDate).ToString("yyyy-MM-dd HH:mm:ss");
                objFilter.FromDate = fdt;
            }
            else
            {
                return result;
            }


            using (var db = new Entities365())
            {
                db.Database.Initialize(force: false);
                var queryComand = "EXEC [dbo].[POC_GetReportCCUOneDay] '" + objFilter.FromDate + "'";
                var cmd = db.Database.Connection.CreateCommand();
                cmd.CommandText = queryComand;

                try
                {

                    db.Database.Connection.Open();
                    var reader = cmd.ExecuteReader();

                    var product = ((IObjectContextAdapter)db)
                        .ObjectContext
                        .Translate<GameReportItem>(reader, "tblGameLogsMoney", MergeOption.AppendOnly);

                    result = product.Select(p => new GameReportItem()
                    {
                        DateCreate = p.DateCreate,
                        avgccu = p.avgccu.HasValue ? p.avgccu.Value : 0
                    }).ToList();

                }
                finally
                {
                    db.Database.Connection.Dispose();
                    db.Database.Connection.Close();
                }
            }

            var fdt2 = Convert.ToDateTime(objFilter.FromDate);
            var result2 = new List<GameReportItem>();
            var currentHour = 0;
            var currentMinute = 0;
            for (int i = 0; i < 24; i++)
            {
                for (int j = 4; j <= 59; j = j + 5)
                {
                    if (j == 4)
                    {
                        currentMinute = 0;
                        currentHour = i;
                    }
                    var objTime = new DateTime(fdt2.Year, fdt2.Month, fdt2.Day, i, j, 59, 999);
                    var objTimeDown = new DateTime(fdt2.Year, fdt2.Month, fdt2.Day, currentHour, currentMinute, 59, 999);

                    var lstF = result.Where(c => c.DateCreate <= objTime && c.DateCreate >= objTimeDown);

                    var obj = new GameReportItem();
                    obj.DateCreate = objTime;
                    obj.DateCreateString = objTime.ToString("dd-MM-yyyy HH:mm:ss");
                    obj.avgccu = lstF.Sum(o => o.avgccu);
                    result2.Add(obj);

                    currentMinute = j;
                }
                currentHour = i;
            }


            return result2;
        }

        public List<GameReportItemNew> GetReportGameNew(out int total, FilterItem objFilter)
        {
            var result = new List<GameReportItemNew>();
            using (var db = new Entities365())
            {
                db.Database.Initialize(force: false);
                var queryComand = "SELECT * FROM [dbo].[tblPocReport] order by [ID] desc OFFSET " + objFilter.currentRecord + " ROWS FETCH NEXT " + objFilter.numberRecord + " ROWS ONLY  select count(ID) from [dbo].[tblPocReport] ";
                var cmd = db.Database.Connection.CreateCommand();
                cmd.CommandText = queryComand;

                try
                {

                    db.Database.Connection.Open();
                    var reader = cmd.ExecuteReader();

                    var product = ((IObjectContextAdapter)db)
                        .ObjectContext
                        .Translate<GameReportItemNew>(reader, "tblGameLogsMoney", MergeOption.AppendOnly);

                    result = product.Select(p => new GameReportItemNew()
                    {
                        ID = p.ID,
                        CashFlow = p.CashFlow.HasValue ? p.CashFlow.Value : 0,
                        DateCreated = p.DateCreated,
                        ExchangeNumber = p.ExchangeNumber.HasValue ? p.ExchangeNumber.Value : 0,
                        NPUShop = p.NPUShop.HasValue ? p.NPUShop.Value : 0,
                        NPUUser = p.NPUUser.HasValue ? p.NPUUser.Value : 0,
                        NRU = p.NRU.HasValue ? p.NRU.Value : 0,
                        Profit = p.Profit.HasValue ? p.Profit.Value : 0,
                        PU = p.PU.HasValue ? p.PU.Value : 0,
                        TotalUser = p.TotalUser.HasValue ? p.TotalUser.Value : 0,
                        UserAction = p.UserAction.HasValue ? p.UserAction.Value : 0
                    }).ToList();

                    reader.NextResult();
                    var tt = ((IObjectContextAdapter)db)
                       .ObjectContext
                       .Translate<int>(reader);

                    total = tt.FirstOrDefault();

                }
                finally
                {
                    db.Database.Connection.Dispose();
                    db.Database.Connection.Close();
                }
            }


            return result;
        }

        public List<GameReportItem> GetReportGame(FilterItem objFilter)
        {
            var result = new List<GameReportItem>();

            if (!string.IsNullOrEmpty(objFilter.FromDate) && !string.IsNullOrEmpty(objFilter.ToDate))
            {
                var fdt = Convert.ToDateTime(objFilter.FromDate).ToString("yyyy-MM-dd HH:mm:ss");
                var tdt = Convert.ToDateTime(objFilter.ToDate).ToString("yyyy-MM-dd 23:59:59");
                objFilter.FromDate = fdt;
                objFilter.ToDate = tdt;
            }


            using (var db = new Web365BaseReaderEntities())
            {
                db.Database.Initialize(force: false);
                var queryComand = "EXEC [dbo].[POC_GetReportGame] " + objFilter.GameId + ",'" + objFilter.Source + "','" + objFilter.FromDate + "','" + objFilter.ToDate + "'";
                var cmd = db.Database.Connection.CreateCommand();
                cmd.CommandText = queryComand;

                try
                {

                    db.Database.Connection.Open();
                    var reader = cmd.ExecuteReader();

                    var product = ((IObjectContextAdapter)db)
                        .ObjectContext
                        .Translate<GameReportItem>(reader, "tblGameLogsMoney", MergeOption.AppendOnly);

                    result = product.Select(p => new GameReportItem()
                    {
                        DateCreate = p.DateCreate,
                        DateCreateString = p.DateCreate.ToString("yyyy-MM-dd"),
                        game = p.game,
                        totalCash = p.totalCash.HasValue ? p.totalCash.Value : 0,
                        totalTax = p.totalTax.HasValue ? p.totalTax.Value : decimal.Zero,
                        totalTransaction = p.totalTransaction.HasValue ? p.totalTransaction.Value : 0,
                        totalUser = p.totalUser.HasValue ? p.totalUser.Value : 0,
                        totalAccountRegiter = p.totalAccountRegiter.HasValue ? p.totalAccountRegiter.Value : 0
                    }).ToList();
                }
                finally
                {
                    db.Database.Connection.Dispose();
                    db.Database.Connection.Close();
                }
            }


            return result;
        }

        public ModelCcu GetReportGameOneDay(FilterItem objFilter)
        {
            var ccu0 = new List<GameReportItem>();
            var ccu1 = new List<GameReportItem>();
            var ccu7 = new List<GameReportItem>();

            if (!string.IsNullOrEmpty(objFilter.FromDate))
            {
                var fdt = Convert.ToDateTime(objFilter.FromDate).ToString("yyyy-MM-dd HH:mm:ss");
                objFilter.FromDate = fdt;
            }


            using (var db = new Web365BaseReaderEntities())
            {
                db.Database.Initialize(force: false);
                var queryComand = "EXEC [dbo].[Billing_CcuMinute] " + objFilter.GameId + ",'" + objFilter.Source + "','" + objFilter.FromDate + "'";
                var cmd = db.Database.Connection.CreateCommand();
                cmd.CommandText = queryComand;

                try
                {

                    db.Database.Connection.Open();
                    var reader = cmd.ExecuteReader();

                    var product = ((IObjectContextAdapter)db)
                        .ObjectContext
                        .Translate<GameReportItem>(reader, "tblGameLogsMoney", MergeOption.AppendOnly);

                    ccu0 = product.Select(p => new GameReportItem()
                    {
                        time = p.time.HasValue ? p.time.Value : DateTime.Now,
                        user_id = p.user_id.HasValue ? p.user_id.Value : 0
                    }).ToList();

                    reader.NextResult();

                    var product1 = ((IObjectContextAdapter)db).ObjectContext.Translate<GameReportItem>(reader, "tblGameLogsMoney", MergeOption.AppendOnly);

                    ccu1 = product1.Select(p => new GameReportItem()
                    {
                        time = p.time.HasValue ? p.time.Value : DateTime.Now,
                        user_id = p.user_id.HasValue ? p.user_id.Value : 0
                    }).ToList();

                    reader.NextResult();

                    var product7 = ((IObjectContextAdapter)db).ObjectContext.Translate<GameReportItem>(reader, "tblGameLogsMoney", MergeOption.AppendOnly);

                    ccu7 = product7.Select(p => new GameReportItem()
                    {
                        time = p.time.HasValue ? p.time.Value : DateTime.Now,
                        user_id = p.user_id.HasValue ? p.user_id.Value : 0
                    }).ToList();

                }
                finally
                {
                    db.Database.Connection.Dispose();
                    db.Database.Connection.Close();
                }
            }

            var fdt0 = Convert.ToDateTime(objFilter.FromDate);
            var fdt1 = Convert.ToDateTime(objFilter.FromDate).AddDays(-1);
            var fdt7 = Convert.ToDateTime(objFilter.FromDate).AddDays(-7);

            var result0 = new List<TimeCcuItem>();
            var result1 = new List<TimeCcuItem>();
            var result7 = new List<TimeCcuItem>();

            var currentHour = 0;
            var currentMinute = 0;
            for (int i = 0; i < 24; i++)
            {
                for (int j = 4; j <= 59; j = j + 5)
                {
                    if (j == 4)
                    {
                        currentMinute = 0;
                        currentHour = i;
                    }

                    var objTime0 = new DateTime(fdt0.Year, fdt0.Month, fdt0.Day, i, j, 59, 999);
                    var objTimeDown0 = new DateTime(fdt0.Year, fdt0.Month, fdt0.Day, currentHour, currentMinute, 59, 999);

                    var objTime1 = new DateTime(fdt1.Year, fdt1.Month, fdt1.Day, i, j, 59, 999);
                    var objTimeDown1 = new DateTime(fdt1.Year, fdt1.Month, fdt1.Day, currentHour, currentMinute, 59, 999);

                    var objTime7 = new DateTime(fdt7.Year, fdt7.Month, fdt7.Day, i, j, 59, 999);
                    var objTimeDown7 = new DateTime(fdt7.Year, fdt7.Month, fdt7.Day, currentHour, currentMinute, 59, 999);

                    var lst0 = ccu0.Where(c => c.time <= objTime0 && c.time >= objTimeDown0).GroupBy(c => c.user_id).Count();
                    var lst1 = ccu1.Where(c => c.time <= objTime1 && c.time >= objTimeDown1).GroupBy(c => c.user_id).Count();
                    var lst7 = ccu7.Where(c => c.time <= objTime7 && c.time >= objTimeDown7).GroupBy(c => c.user_id).Count();


                    var obj = new TimeCcuItem
                    {
                        id = (currentHour*60 * 6000) + ((currentMinute + 1) * 60),
                        Time = (currentHour < 10 ? "0" : "") + currentHour + ":" + (currentMinute < 10 ? "0" : "") + currentMinute,
                        Ccu = lst0
                    };

                    result0.Add(obj);

                    var obj1 = new TimeCcuItem
                    {
                        id = (currentHour * 60 * 6000) + ((currentMinute + 1) * 60),
                        Time = (currentHour < 10 ? "0" : "") + currentHour + ":" + (currentMinute < 10 ? "0" : "") + currentMinute,
                        Ccu = lst1
                    };

                    result1.Add(obj1);

                    var obj7 = new TimeCcuItem
                    {
                        id = (currentHour * 60 * 6000) + ((currentMinute + 1) * 60),
                        Time = (currentHour < 10 ? "0" : "") + currentHour + ":" + (currentMinute < 10 ? "0" : "") + currentMinute,
                        Ccu = lst7
                    };

                    result7.Add(obj7);

                    currentMinute = j;
                }
                currentHour = i;
            }
            var xx = new List<TimeCcuItem2>();
            var xx1 = new List<TimeCcuItem2>();
            var xx7 = new List<TimeCcuItem2>();
            xx = result0.OrderByDescending(c => c.id).Select(c => new TimeCcuItem2 {Ccu = c.Ccu, Time = c.Time}).ToList();
            xx1 = result1.OrderByDescending(c => c.id).Select(c => new TimeCcuItem2 { Ccu = c.Ccu, Time = c.Time }).ToList();
            xx7 = result7.OrderByDescending(c => c.id).Select(c => new TimeCcuItem2 { Ccu = c.Ccu, Time = c.Time }).ToList();
            var model = new ModelCcu { Day0 = xx, Day1 = xx1, Day7 = xx7 };
            return model;
        }

        public ModelCcu GetReportCcuDay(FilterItem objFilter)
        {
            var ccuday = new List<GameReportItem>();
            if (!string.IsNullOrEmpty(objFilter.FromDate))
            {
                var fdt = Convert.ToDateTime(objFilter.FromDate).ToString("yyyy-MM-dd HH:mm:ss");
                objFilter.FromDate = fdt;
            }
            if (!string.IsNullOrEmpty(objFilter.ToDate))
            {
                var fdt = Convert.ToDateTime(objFilter.ToDate).ToString("yyyy-MM-dd HH:mm:ss");
                objFilter.ToDate = fdt;
            }


            using (var db = new Web365BaseReaderEntities())
            {
                db.Database.Initialize(force: false);
                var queryComand = "EXEC [dbo].[Billing_CcuDay] " + objFilter.GameId + ",'" + objFilter.Source + "','" + objFilter.FromDate + "'" + "','" + objFilter.ToDate + "'"; ;
                var cmd = db.Database.Connection.CreateCommand();
                cmd.CommandText = queryComand;

                try
                {

                    db.Database.Connection.Open();
                    var reader = cmd.ExecuteReader();

                    var product = ((IObjectContextAdapter)db)
                        .ObjectContext
                        .Translate<GameReportItem>(reader, "tblGameLogsMoney", MergeOption.AppendOnly);

                    ccuday = product.Select(p => new GameReportItem()
                    {
                        time = p.time.HasValue ? p.time.Value : DateTime.Now,
                        user_id = p.user_id.HasValue ? p.user_id.Value : 0
                    }).ToList();
                }
                finally
                {
                    db.Database.Connection.Dispose();
                    db.Database.Connection.Close();
                }
            }

            var fdt0 = Convert.ToDateTime(objFilter.FromDate);

            var result0 = new List<TimeCcuItem>();

            var currentHour = 0;
            var currentMinute = 0;
            for (int i = 0; i < 24; i++)
            {
                for (int j = 4; j <= 59; j = j + 5)
                {
                    if (j == 4)
                    {
                        currentMinute = 0;
                        currentHour = i;
                    }

                    var objTime0 = new DateTime(fdt0.Year, fdt0.Month, fdt0.Day, i, j, 59, 999);
                    var objTimeDown0 = new DateTime(fdt0.Year, fdt0.Month, fdt0.Day, currentHour, currentMinute, 59, 999);

                    var lst0 = ccu0.Where(c => c.time <= objTime0 && c.time >= objTimeDown0).GroupBy(c => c.user_id).Count();

                    var obj = new TimeCcuItem
                    {
                        id = (currentHour * 60 * 6000) + ((currentMinute + 1) * 60),
                        Time = (currentHour < 10 ? "0" : "") + currentHour + ":" + (currentMinute < 10 ? "0" : "") + currentMinute,
                        Ccu = lst0
                    };

                    result0.Add(obj);

                    currentMinute = j;
                }
                currentHour = i;
            }
            var xx = new List<TimeCcuItem2>();
            xx = result0.OrderByDescending(c => c.id).Select(c => new TimeCcuItem2 { Ccu = c.Ccu, Time = c.Time }).ToList();
            var model = new ModelCcu { Day0 = xx};
            return model;
        }
        #endregion
    }
}
