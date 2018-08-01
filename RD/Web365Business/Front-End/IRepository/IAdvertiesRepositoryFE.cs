using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web365Base;
using Web365Domain;

namespace Web365Business.Front_End.IRepository
{
    public interface IAdvertiesRepositoryFE
    {
        AdvertiesItem GetItemById(int id);
    }
}
