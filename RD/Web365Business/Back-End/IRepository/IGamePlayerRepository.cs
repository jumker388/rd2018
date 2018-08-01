using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web365Base;
using Web365Domain;
using Web365Domain.Game;
using Web365Models;

namespace Web365Business.Back_End.IRepository
{
    public interface IGamePlayerRepository
    {
        RetetionItem GetRetention(string fDateReg, string tDateReg, string fDateReciveMoney, string tDateReciveMoney);
        List<GameReportItemNew> GetReportGameNew(out int total, FilterItem objFilter);
        GameSlotCheckItem GameSlotGetDetail(int gameId, int matchId);
        DaiLyShowItem BlockUserMember(string username);
        List<GiftCodeItem2> GetGiftCodeUsed(out int total, int currentRecord = 0, int numberRecord = 10);
        List<CardTelcoItem> CardChecker(out int total, string name, int uid, string mathe, string seri, int currentPage, int currentRecord, int numberRecord);
        List<GameBankItem> GetGameBank();
        List<GiftCodeItem2> CheckGiftCode(string content);
        List<UserInfo2> GetTopPlayer(out int total, string fDate = "", string tDate = "", string username = "", int currentRecord = 0, int numberRecord = 10, int lastlogin = 0, decimal moneyGiaoDich = 0);
        List<SumPocItem> GetThongKePoc2(out int total, int currentPage = 0, int numberRecord = 0, int currentRecord = 0);
        DaiLyShowItem UpdateLevelDaiLy(string username, int level, string parent);
        List<TaiXiuGameHistoryItem> GetGameHistoryTaiXiu(out int total, long id, int gameid, int mathid, int skip, int take, string fromdate = "", string todate = "");
        UserInfo InsertLogsAdmin(string content);
        List<DaiLyShowItem> GetDaiLyShow();
        DaiLyShowItem InserUpdatetDaiLyShow(DaiLyShowItem obj);
        DaiLyShowItem UpdatetDaiLyShow(DaiLyShowItem obj);
        DaiLyShowItem ActioDaiLy(int id, int status);
        DaiLyShowItem GetDaiLyShowByID(int id);
        int InsertOffMessenger(HopThuItem obj);
        int SendToMessenger(HopThuItem obj);
        UserInfo DeleteOffMessenger(int id);
        UserInfoNew RefundPoc(int id, int type);
        List<GameLogsMoneyItem> GetFilterLogsPoc(out int total, string nickSend, string nickRecive, int id, int currentPage, int pageSize);
        List<SumPocItem> GetThongKePoc(out int total, int currentPage = 0, int numberRecord = 0, int currentRecord = 0);
        UserInfo2 SelectOne(long id);
        List<HopThuItem> GetHopThu(out int total, FilterItem obj);
        List<UserInfo2> GetList(out int total, string fDate, string tDate, string username = "",
            int currentRecord = 0, int numberRecord = 10, int lastlogin = 0);
        List<GameReportItem> GetReportGameCcuOneDay(FilterItem objFilter);
        GamePheInGameModel GetPhe(string fDate, string tDate);
        List<UserInfo> RealTimeAccPlayingByGameId(out int total, int gameId, int top);
        List<UserInfo> PlayerExportExcel(DateTime? from, DateTime? to);
        UserInfo GetItemById(long id);
        UserInfo UpdatePass(UserInfo objSubmit);
        List<GiftCodeTypeItem> GetListTypeGift(FilterItem objFilter);
        bool DeleteGiftCode(int id);
        UserInfo UnBlockUser(int uid);
        UserInfo BlockUser(int uid);
        UserInfo Update(UserInfo objSubmit);
        GiftCodeTypeItem GetDetailGameGiftType(int id);
        GiftCodeTypeItem InsertGameGiftType(GiftCodeTypeItem obj);
        GiftCodeTypeItem UpdateGameGiftType(GiftCodeTypeItem obj);
        GiftCodeTypeItem DeleteGameGiftType(int id);
        List<GameHistoryItem> GetGameHistoryMoney(out int total, long id, int gameid, int skip, int take, string fromdate = "", string todate = "");
        UserInfo Add(string username, string fullname, string mobile, string email, string cmnd, int sex,
            string password);
        int UpdateTurnGift(int ID, int status);
        List<GameHistoryItem> GetGameHistory(out int total, long id, int gameid, int mathid, int skip, int take, string fromdate = "", string todate = "");
        GameHistoryItem GetDetailGameHistory(long id);

        List<ChargeItem> GetListCharge(out int total, string type, string name, int currentRecord, int numberRecord);
        List<ChargeItem> ChargeExportExcel(DateTime? from, DateTime? to, string type = "");
        List<GameItem> GetAllGame();
        GameItem GetGameItemById(int id);
        bool UpdateGame(int id, string name, int status, int gameOrder);
        List<UserInfo> GetTopUserByZoneId(int zoneId, int top);
        List<GameHistoryItem> GetGameHistoryByZoneId(out int total, int zoneId, int skip, int take);
        bool Approval(long id, bool approval);
        List<GameHistoryItem> GetListChangeAward(out int total, long uid, int type, int skip, int take);
        List<GameHistoryItem> ChangeAwardExportExcel(DateTime? from, DateTime? to);
        List<GiftCodeItem2> GetAllGiftCodeByTurnID(int turnId);
        List<AnnouncementItem> GetAllEvent();
        List<RuleEventItem> GetGameRuleEvent(int currentRecord = 0, int numberRecord = 10);
        int InsertRuleEvent(RuleEventItem obj);
        RuleEventItem GetDetailRuleEvent(int ID);
        int InsertEvent(string subject, string content, DateTime begin_time, DateTime and_time, string UrlImage, int gameid, string doituong, string thoigian, int DisplayOrder);
        int UpdateEvent(int ID, string subject, string content, DateTime begin_time, DateTime and_time, string UrlImage, int gameid, string doituong, string thoigian, int DisplayOrder);
        int DeleteEvent(int ID);
        AnnouncementItem GetOneEventItem(int ID);
        bool GeneratorGiftCode(string name, DateTime dateExpired, int Gift_ID, string prefix, string note, string giatri, string soluong);
        bool InsertOneItemGameGift(string code, int value, string name, DateTime dateExpired, int isVCash);

        List<GiftTurnItem> GetAllGiftCode(out int total, bool used, int skip, int take);
        string GetText();
        int UpdateText(string noticeText);

        List<GameTextItem> GetAllTextRun();
        GameTextItem GetOneTextRunItem(int ID);
        int InsertTextRun(string Title, string Link, DateTime DataStart, DateTime DateEnd, bool IsDelete, int Order);

        int UpdateTextRun(int ID, string Title, string Link, DateTime DataStart, DateTime DateEnd, bool IsDelete,
            int Order);
        int DeleteTextRun(int ID);
        int UpdateRuleEvent(RuleEventItem obj);
        int DeleteRuleEvent(int ID);

        List<UserInfo> GetTopGem();
        List<UserInfo> GetTopXu();
        List<UserInfo> GetTopExp();

        List<UserInfo> GetTopDoiThuong();

        List<UserInfo> GetTopNapTien();

        List<UserInfo> GetGiaoDich(out int total, int type, int skip, int take, int uid = 0);

        PaymentItem InsertPayment(PaymentItem pay);

        PaymentItem GetDetailPaymentById(long id);

        PaymentItem GetDetailPaymentByToken(string token);

        PaymentItem UpdatePayment(PaymentItem pay);

        PaymentItem InsertMoney(PaymentItem pay);

        List<RoomItem> RoomGetAll(int gameId);


        int RoomUpdate(RoomItem room);

        int RoomInsert(RoomItem room);

        RoomItem GetDetailRoom(int roomId);

        ReportOnlineItem ReportOnline();

        int CardInsertItem(CardItem obj);

        List<CardItem> GetAllCard(out int total, int skip, int take, int used = 0, int telcoId = 0, int value = 0,
            string seri = "");
        List<ConfigMiniPokerItem> GetConfigMiniPoker();
        ConfigMiniPokerItem GetConfigMiniPokerDetail(int id);
        int UpdateConfigMiniPoker(ConfigMiniPokerItem obj);

        List<ConfigMiniPokerItem> GetConfigXeng();
        ConfigMiniPokerItem GetConfigXengDetail(int id);
        int UpdateConfigXeng(ConfigMiniPokerItem obj);

        List<GameCashItem> GetCash(string date);
        //bool AddGameLogs(tblGameConfigLogs obj);
        List<LogGameItem> GetListGameLogs(out int total, int gameId, int currentRecord, int numberRecord);
        LogGameItem GetLogDetail(int id);

        List<ConfigTaiXiuItem> GetConfigTaiXiu();

        ConfigTaiXiuItem GetConfigTaiXiuDetail(int id);

        ConfigTaiXiuItem UpdateConfigTaiXiu(ConfigTaiXiuItem obj);

        List<PaymentItem> GetPayment(out int total, int skip, int take, int uid = 0, string username = "",
            string date = "", string status = "");

        PaymentItem PaymentDetail(int id);
        int GameGuideUpdate(GameGuideItem obj);
        int GameGuideInsert(GameGuideItem obj);
        int GameGuideDelete(int id);
        List<GameGuideItem> GameGuideGetAll();
        GameGuideItem GameGuideGetOne(int id);
        List<GameLogsMoneyItem> DlGetListGameLogs(out int total, string userName, int type, int currentPage, int pageSize, string key, int statusId, string fDate, string tDate);
        UserInfoNew GetDetailPlayer(string username);
        UserInfoNew CheckPlayerName(string username);

        UserInfoNew GetPortalUser(string username);
        UserInfoNew CheckUserInMember(string username);
        UserInfoNew SetDaiLy(string username, int level, string parent = "");
        UserInfoNew CheckDaiLy(string username);

        ChuyenKhoanItem ChuyenKhoan(ChuyenKhoanItem itemCk);

        UserInfoNew UpdateLogsGameDetail(int logDetailId, int statusId, string note);

        List<GameReportItem> GetReportGame(FilterItem objFilter);
        ModelCcu GetReportGameOneDay(FilterItem objFilter);
        List<GameReportItem> GetReportGameCcu(FilterItem objFilter);
    }
}
