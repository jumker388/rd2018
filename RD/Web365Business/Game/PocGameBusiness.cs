using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web365Domain;
using Web365Domain.Game;
using Web365Base;
using Web365Business.Back_End;
using Web365Domain;
using Web365Utility;

namespace Web365Business.Game
{
    public class PocGameBusiness : BaseBE<tblGroup_TypeArticle_Map>
    {
        public List<GameReportItem> GetReportGame(FilterItem objFilter)
        {

            var result = new List<GameReportItem>();

            var query = web365db.Database.SqlQuery<GameReportItem>("exec [dbo].[POC_GetReportGame] @gameId, @source, @fdate, @tdate",
                    new SqlParameter("gameId", objFilter.GameId),
                    new SqlParameter("source", objFilter.Source),
                    new SqlParameter("fdate", objFilter.FromDate),
                    new SqlParameter("tdate", objFilter.ToDate));

            result = query.Select(p => new GameReportItem()
                {
                    DateCreate = p.DateCreate,
                    game = p.game,
                    totalCash = p.totalCash.HasValue ? p.totalCash.Value : 0,
                    totalTax = p.totalTax.HasValue ? p.totalTax.Value : 0,
                    totalTransaction = p.totalTransaction.HasValue ? p.totalTransaction.Value : 0,
                    totalUser = p.totalUser.HasValue ? p.totalUser.Value : 0
                }).ToList();


            return result;
        }

    }
}


