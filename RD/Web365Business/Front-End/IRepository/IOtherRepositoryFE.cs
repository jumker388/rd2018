using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web365Base;
using Web365Domain;

namespace Web365Business.Front_End.IRepository
{
    public interface IOtherRepositoryFE
    {
        bool AddContact(tblContact contact);
        bool AddEmailReciveInfo(tblReceiveInfo obj);
    }
}
