using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web365Base;
using Web365Domain;

namespace Web365Business.Front_End.IRepository
{
    public interface ICustomerRepositoryFE
    {
        bool AddCustomer(out int customerId, tblCustomer customer);
        void UpdateCustomer(CustomerItem customer);
        bool TryGetByUserNameAndPassword(out CustomerItem customer, string userName, string password);
        bool TryGetById(out CustomerItem customer, int id);
        bool ForgetPassword(out string token, string email);
        bool ResetPassword(string email, string key, string password);
    }
}
