using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web365Base;
using Web365Domain;

namespace Web365Business.Back_End.IRepository
{
    public interface IArticleGroupTypeMapRepository
    {
        void ResetGroupOfType(int typeId, int[] groupId);
        void ResetTypeOfGroup(int groupId, int[] typeId);
    }
}
