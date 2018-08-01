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
    public interface IDaiLyRepository
    {
        List<DaiLyItem> GetListDaiLy(out int total, FilterItem obj);
        List<UserInfoNew2> GetDaiLyCap2(string username, int currentPage = 0, int pageSize = 0, string fDate = "", string tDate = "", string key = "", string propertyNameSort = "", bool descending = true);
        List<UserInfoNew3> GetDaiLyCap1(int currentPage = 0, int pageSize = 0, string fDate = "", string tDate = "", string key = "", string propertyNameSort = "", bool descending = true);
        List<CardOptionItem> GetListCardOptions(out int total, int currentRecord, int numberRecord);
        CardOptionItem GetCardOptionDetail(int id);
        CardOptionItem  ActionCardOption(CardOptionItem obj);
        CardOptionItem UpdateIsShow(int id, int isshow);
        CardOptionItem UpdateIsDel(int id, int isDel);
    }
}
