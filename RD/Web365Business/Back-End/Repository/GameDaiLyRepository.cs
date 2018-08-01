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
    public class GameDaiLyRepository : IDaiLyRepository
    {
        protected Entities365 web365db = new Entities365();
        protected Web365BaseReaderEntities web365DbReder = new Web365BaseReaderEntities();
        public List<DaiLyItem> GetListDaiLy(out int total, FilterItem obj)
        {
            total = 0;
            using (var db = new Entities365())
            {
                db.Database.Initialize(force: false);

                var cmd = db.Database.Connection.CreateCommand();

                cmd.CommandText = "exec [dbo].[POC_GetListDaiLy] " + obj.GameId + "," + obj.currentRecord + "," + obj.numberRecord;

                try
                {
                    db.Database.Connection.Open();
                    var reader = cmd.ExecuteReader();
                    var queryProduct = ((IObjectContextAdapter)db)
                        .ObjectContext
                        .Translate<DaiLyItem>(reader, "tblGameLogsMoney", MergeOption.AppendOnly);
                    var result = queryProduct.Select(p => new DaiLyItem
                    {
                        id = p.id,
                        username = p.username,
                        parent = p.parent,
                        ShopLevel = p.ShopLevel
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

        public List<UserInfoNew3> GetDaiLyCap1(int currentPage = 0, int pageSize = 0, string fDate = "", string tDate = "", string key = "", string propertyNameSort = "", bool descending = true)
        {

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

            var query = web365DbReder.Database.SqlQuery<UserInfoNew3>("exec [dbo].[PRC_AdminGetListDailyCap1] @fDate, @tDate, @key, @currentPage, @pageSize, @total OUTPUT",
                new SqlParameter("currentPage", currentPage),
                new SqlParameter("pageSize", pageSize),
                new SqlParameter("key", key),
                new SqlParameter("fDate", fDate),
                new SqlParameter("tDate", tDate),
                 paramTotal);

            var result = query.Select(p => new UserInfoNew3()
            {
                user_id = p.user_id,
                indexRow = p.indexRow,
                fullname = p.fullname,
                username = p.username,
                gameCashN = p.gameCashN.HasValue ? p.gameCashN.Value : 0,
                CashKet = p.CashKet.HasValue ? p.CashKet.Value : 0,
                ShopLevel = p.ShopLevel.HasValue ? p.ShopLevel.Value : 0,
                totalSendToUser = p.totalSendToUser.HasValue ? p.totalSendToUser.Value : 0,
                totalRecivefromUser = p.totalRecivefromUser.HasValue ? p.totalRecivefromUser.Value : 0,
                total = (p.totalSendToUser.HasValue ? p.totalSendToUser.Value : 0) + (p.totalRecivefromUser.HasValue ? p.totalRecivefromUser.Value : 0),
                totalTax = p.totalTax.HasValue ? p.totalTax.Value : 0,
                totalSendToUserCap1 = p.totalSendToUserCap1.HasValue ? p.totalSendToUserCap1.Value : 0,
                totalRecivefromUserCap1 = p.totalRecivefromUserCap1.HasValue ? p.totalRecivefromUserCap1.Value : 0,
                totalCap1 = (p.totalSendToUserCap1.HasValue ? p.totalSendToUserCap1.Value : 0) + (p.totalRecivefromUserCap1.HasValue ? p.totalRecivefromUserCap1.Value : 0),
                totalTaxCap1 = p.totalTaxCap1.HasValue ? p.totalTaxCap1.Value : 0,
            }).OrderByDescending(c => c.total).ToList();


            if (!string.IsNullOrEmpty(propertyNameSort))
            {
                var rsQueryable = result.AsQueryable();
                var result2 = descending ? QueryableHelper.OrderByDescending(rsQueryable, propertyNameSort) : QueryableHelper.OrderBy(rsQueryable, propertyNameSort);

                return result2.ToList();
            }
            else
            {
                return result;
            }
        }

        public List<UserInfoNew2> GetDaiLyCap2(string username, int currentPage = 0, int pageSize = 0, string fDate = "", string tDate = "", string key = "", string propertyNameSort = "", bool descending = true)
        {

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

            var query = web365DbReder.Database.SqlQuery<UserInfoNew2>("exec [dbo].[PRC_AdminGetListDailyCap2] @parentName, @fDate, @tDate, @key, @currentPage, @pageSize, @total OUTPUT",
                new SqlParameter("parentName", username),
                 new SqlParameter("currentPage", currentPage),
                new SqlParameter("pageSize", pageSize),
                new SqlParameter("key", key),
                new SqlParameter("fDate", fDate),
                new SqlParameter("tDate", tDate),
                 paramTotal);

            var result = query.Select(p => new UserInfoNew2()
            {
                user_id = p.user_id,
                fullname = p.fullname,
                indexRow = p.indexRow,
                username = p.username,
                parent = p.parent,
                Note = p.Note,
                gameCashN = p.gameCashN.HasValue ? p.gameCashN.Value : 0,
                CashKet = p.CashKet.HasValue ? p.CashKet.Value : 0,
                totalTax = p.totalTax.HasValue ? p.totalTax.Value : 0,
                ShopLevel = p.ShopLevel.HasValue ? p.ShopLevel.Value : 0,
                totalSendToUser = p.totalSendToUser.HasValue ? p.totalSendToUser.Value : 0,
                totalRecivefromUser = p.totalRecivefromUser.HasValue ? p.totalRecivefromUser.Value : 0,
            }).ToList();

            if (!string.IsNullOrEmpty(propertyNameSort))
            {
                var rsQueryable = result.AsQueryable();
                var result2 = descending ? QueryableHelper.OrderByDescending(rsQueryable, propertyNameSort) : QueryableHelper.OrderBy(rsQueryable, propertyNameSort);

                return result2.ToList();
            }
            else
            {
                return result;
            }

            return result;
        }

        public List<CardOptionItem> GetListCardOptions(out int total, int currentRecord, int numberRecord)
        {
            var paramTotal = new SqlParameter
            {
                ParameterName = "total",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };

            var query = web365db.Database.SqlQuery<CardOptionItem>("exec [dbo].[POC_GetListCardOption] @currentRecord, @pageSize, @total OUTPUT",
                new SqlParameter("currentRecord", currentRecord),
                new SqlParameter("pageSize", numberRecord),
                paramTotal);

            var result = query.Select(p => new CardOptionItem()
            {
                id = p.id,
                isDeleted = p.isDeleted.HasValue ? p.isDeleted.Value : 0,
                isShow = p.isShow.HasValue ? p.isShow.Value : 0,
                name = p.name,
                rate = p.rate.HasValue ? p.rate.Value : 0,
                value = p.value.HasValue ? p.value.Value : 0,
                telcoid = p.telcoid.HasValue ? p.telcoid.Value : 0,
                DisplayOrder = p.DisplayOrder.HasValue ? p.DisplayOrder.Value : 0
            }).ToList();

            total = Convert.ToInt32(paramTotal.Value);

            return result;
        }

        public CardOptionItem GetCardOptionDetail(int id)
        {
            var query = web365db.Database.SqlQuery<CardOptionItem>("SELECT * FROM [portal].[dbo].[exchangeOption] where id = @id",
                new SqlParameter("id", id));

            var result = query.Select(p => new CardOptionItem()
            {
                id = p.id,
                isDeleted = p.isDeleted.HasValue ? p.isDeleted.Value : 0,
                isShow = p.isShow.HasValue ? p.isShow.Value : 0,
                IsShowd = p.isShow.HasValue && p.isShow.Value != 0,
                name = p.name,
                rate = p.rate.HasValue ? p.rate.Value : 0,
                DisplayOrder = p.DisplayOrder.HasValue ? p.DisplayOrder.Value : 0,
                value = p.value.HasValue ? p.value.Value : 0,
                telcoid = p.telcoid.HasValue ? p.telcoid.Value : 0,

            }).FirstOrDefault();

            return result;
        }

        public CardOptionItem ActionCardOption(CardOptionItem obj)
        {
            CardOptionItem result;
            if (obj.id > 0)
            {
                var query = web365db.Database.SqlQuery<CardOptionItem>("update [portal].[dbo].[exchangeOption] set isShow = @isShow,name=@name,rate=@rate,value=@value,DisplayOrder=@DisplayOrder,telcoid=@telcoid where id = @id select * from [portal].[dbo].[exchangeOption] where id = @id",
                    new SqlParameter("id", obj.id),
                    new SqlParameter("isShow", obj.isShow),
                    new SqlParameter("name", obj.name),
                    new SqlParameter("rate", obj.rate),
                    new SqlParameter("value", obj.value),
                    new SqlParameter("DisplayOrder", obj.DisplayOrder),
                    new SqlParameter("telcoid", obj.telcoid)
                    );

                result = query.Select(p => new CardOptionItem()
                {
                    id = p.id,
                    isDeleted = p.isDeleted.HasValue ? p.isDeleted.Value : 0,
                    isShow = p.isShow.HasValue ? p.isShow.Value : 0,
                    IsShowd = p.isShow.HasValue && p.isShow.Value != 0,
                    name = p.name,
                    rate = p.rate.HasValue ? p.rate.Value : 0,
                    DisplayOrder = p.DisplayOrder.HasValue ? p.DisplayOrder.Value : 0,
                    value = p.value.HasValue ? p.value.Value : 0,
                    telcoid = p.telcoid.HasValue ? p.telcoid.Value : 0,

                }).FirstOrDefault();
            }
            else
            {
                var query = web365db.Database.SqlQuery<CardOptionItem>("insert into [portal].[dbo].[exchangeOption](isShow, name, rate, value, DisplayOrder, telcoid, isDeleted) values(@isShow, @name, @rate, @value, @DisplayOrder, @telcoid, 0) select top 1 * from [portal].[dbo].[exchangeOption] order by id desc",
                   new SqlParameter("isShow", obj.isShow),
                   new SqlParameter("name", obj.name),
                   new SqlParameter("rate", obj.rate),
                   new SqlParameter("value", obj.value),
                   new SqlParameter("DisplayOrder", obj.DisplayOrder),
                   new SqlParameter("telcoid", obj.telcoid)
                   );

                result = query.Select(p => new CardOptionItem()
                {
                    id = p.id,
                    isDeleted = p.isDeleted.HasValue ? p.isDeleted.Value : 0,
                    isShow = p.isShow.HasValue ? p.isShow.Value : 0,
                    IsShowd = p.isShow.HasValue && p.isShow.Value != 0,
                    name = p.name,
                    rate = p.rate.HasValue ? p.rate.Value : 0,
                    DisplayOrder = p.DisplayOrder.HasValue ? p.DisplayOrder.Value : 0,
                    value = p.value.HasValue ? p.value.Value : 0,
                    telcoid = p.telcoid.HasValue ? p.telcoid.Value : 0,

                }).FirstOrDefault();
            }

            return result;
        }

        public CardOptionItem UpdateIsShow(int id, int isshow)
        {
            var result = new CardOptionItem();
            if (id > 0)
            {
                var query = web365db.Database.SqlQuery<CardOptionItem>("update [portal].[dbo].[exchangeOption] set isShow = @isShow where id = @id select * from [portal].[dbo].[exchangeOption] where id = @id",
                    new SqlParameter("id", id),
                    new SqlParameter("isShow", isshow)
                    );

                result = query.Select(p => new CardOptionItem()
                {
                    id = p.id,
                    isDeleted = p.isDeleted.HasValue ? p.isDeleted.Value : 0,
                    isShow = p.isShow.HasValue ? p.isShow.Value : 0,
                    IsShowd = p.isShow.HasValue && p.isShow.Value != 0,
                    name = p.name,
                    rate = p.rate.HasValue ? p.rate.Value : 0,
                    DisplayOrder = p.DisplayOrder.HasValue ? p.DisplayOrder.Value : 0,
                    value = p.value.HasValue ? p.value.Value : 0,
                    telcoid = p.telcoid.HasValue ? p.telcoid.Value : 0,

                }).FirstOrDefault();
            }

            return result;
        }

        public CardOptionItem UpdateIsDel(int id, int isDel)
        {
            var result = new CardOptionItem();
            if (id > 0)
            {
                var query = web365db.Database.SqlQuery<CardOptionItem>("update [portal].[dbo].[exchangeOption] set isDeleted = @isDel where id = @id select * from [portal].[dbo].[exchangeOption] where id = @id",
                    new SqlParameter("id", id),
                    new SqlParameter("isDel", isDel)
                    );

                result = query.Select(p => new CardOptionItem()
                {
                    id = p.id,
                    isDeleted = p.isDeleted.HasValue ? p.isDeleted.Value : 0,
                    isShow = p.isShow.HasValue ? p.isShow.Value : 0,
                    IsShowd = p.isShow.HasValue && p.isShow.Value != 0,
                    name = p.name,
                    rate = p.rate.HasValue ? p.rate.Value : 0,
                    DisplayOrder = p.DisplayOrder.HasValue ? p.DisplayOrder.Value : 0,
                    value = p.value.HasValue ? p.value.Value : 0,
                    telcoid = p.telcoid.HasValue ? p.telcoid.Value : 0,

                }).FirstOrDefault();
            }

            return result;
        }

    }
}
