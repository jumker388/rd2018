using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Web365Base;
using Web365Business.Front_End.IRepository;
using Web365Domain;
using Web365Utility;

namespace Web365Business.Front_End.Repository
{
    public class CustomerRepositoryFE : BaseFE, ICustomerRepositoryFE
    {
        public bool AddCustomer(out int customerId, tblCustomer customer)
        {
            customerId = 0;

            var isCheck = web365db.tblCustomer.Count(c => c.Email == customer.Email && c.IsActive == true) > 0;

            if (isCheck)
            {
                return isCheck;
            }

            customer.Password = Web365Utility.Web365Utility.MD5Hash(customer.Password);
            customer.DateCreated = DateTime.Now;
            customer.DateUpdated = DateTime.Now;
            customer.IsActive = true;
            customer.IsDeleted = false;

            web365db.tblCustomer.Add(customer);

            var result = web365db.SaveChanges();

            customerId = customer.ID;

            return result > 0;

        }

        public void UpdateCustomer(CustomerItem customer)
        {
            var cus = web365db.tblCustomer.FirstOrDefault(c => c.ID == customer.ID);



            cus.Password = string.IsNullOrEmpty(customer.Password) ? cus.Password : Web365Utility.Web365Utility.MD5Hash(customer.Password);

            cus.LastName = customer.LastName;

            cus.Email = customer.Email;

            cus.Phone = customer.Phone;

            cus.Address = customer.Address;

            web365db.Entry(cus).State = EntityState.Modified;

            web365db.SaveChanges();
        }

        public bool TryGetByUserNameAndPassword(out CustomerItem customer, string userName, string password)
        {
            password = Web365Utility.Web365Utility.MD5Hash(password);

            customer = web365db.tblCustomer.Where(c => c.UserName == userName && c.Password == password).Select(c => new CustomerItem()
            {
                ID = c.ID,
                UserName = c.UserName,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                Phone = c.Phone,
                Address = c.Address,
                IsActive = c.IsActive.HasValue && c.IsActive.Value
            }).FirstOrDefault();

            return customer != null;
        }

        public bool TryGetById(out CustomerItem customer, int id)
        {
            customer = web365db.tblCustomer.Where(c => c.ID == id).Select(c => new CustomerItem()
            {
                ID = c.ID,
                UserName = c.UserName,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                Phone = c.Phone,
                Address = c.Address
            }).FirstOrDefault();

            return customer != null;
        }

        public bool ForgetPassword(out string token, string email)
        {
            token = string.Empty;

            var customer = web365db.tblCustomer.FirstOrDefault(c => c.Email == email);

            if (customer != null && customer.IsActive.HasValue && !customer.IsActive.Value)
            {
                token = "LOCK";
            }

            if (customer != null && customer.IsActive.HasValue && customer.IsActive.Value)
            {
                token = Web365Utility.Web365Utility.MD5Hash(new Random().Next(1000, 10000).ToString());

                customer.Token = token;

                web365db.Entry(customer).State = EntityState.Modified;

                web365db.SaveChanges();
            }

            return customer != null;
        }

        public bool ResetPassword(string email, string key, string password)
        {
            var customer = web365db.tblCustomer.FirstOrDefault(c => c.Email == email && c.Token == key);            

            if (customer != null)
            {
                customer.Password = Web365Utility.Web365Utility.MD5Hash(password);

                customer.Token = string.Empty;

                web365db.Entry(customer).State = EntityState.Modified;

                web365db.SaveChanges();
            }

            return customer != null;
        }
    }
}
