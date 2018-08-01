using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web365Base;
using Web365Domain;
using Web365Models;

namespace Web365Business.Front_End.IRepository
{
    public interface IMenuRepositoryFE
    {
        List<MenuItem> GetListByParent(string parentId, bool isShow = true, bool isDeleted = false);
    }
}
