using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web365Domain
{
    public class CustomerItem : BaseModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public bool? Gender { get; set; }
        public DateTime? BirthDay { get; set; }
        public int? PictureID { get; set; }
        public string Token { get; set; }
        public DateTime? DateExpiredToken { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool IsActive { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
