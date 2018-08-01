using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web365Domain
{
    public class UserRoleItem : BaseModel
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Detail { get; set; }
        public int[] ListPageId { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }
        public bool? IsShow { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
