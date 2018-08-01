using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web365Models
{
    public class CustomerLoggedModel
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Avartar { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}