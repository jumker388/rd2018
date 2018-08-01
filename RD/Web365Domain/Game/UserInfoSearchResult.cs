using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web365Domain
{
    public class UserInfoSearchResult
    {
        public int totalRecord { get; set; }
        public List<UserInfo> data { get; set; }

        public List<UserInfo2> data2{ get; set; }
    }
}
