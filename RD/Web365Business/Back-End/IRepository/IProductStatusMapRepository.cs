using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web365Base;
using Web365Domain;

namespace Web365Business.Back_End.IRepository
{
    public interface IProductStatusMapRepository
    {
        void ResetStatusOfProduct(int productId, int[] statusId);
        void ResetProductOfProduct(int statusId, int[] productId);
    }
}
