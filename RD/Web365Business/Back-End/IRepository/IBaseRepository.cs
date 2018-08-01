using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web365Base;
using Web365Domain;

namespace Web365Business.Back_End.IRepository
{
    public interface IBaseRepository
    {
        T GetListForTree<T>(bool isShow = true, bool isDelete = false);
        T GetById<T>(int id);
        T GetItemById<T>(int id);
        void Add(object obj);
        void Update(object obj);          
        void Show(int id);
        void Hide(int id);
        void Delete(int id);
        bool NameExist(int id, string name);
    }
}
