using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Web365Base;
using Web365Business.Back_End.IRepository;
using Web365Business.Game;
using Web365Domain;
using Web365Domain.Game;
using Web365Models;
using Web365Utility;

namespace Web365Business.Back_End.Repository
{
    public class PocGamePlayerRepository : IGamePlayerRepository
    {
        protected Entities365 web365db = new Entities365();

        readonly PocGameBusiness _gameBusiness = new PocGameBusiness();

        #region quản lý game

        public List<GameReportItem> GetReportGame(FilterItem objFilter)
        {
            return _gameBusiness.GetReportGame(objFilter);
        }

      
    }
}
