using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBusiness
{
    public static class Constants
    {
        public static string DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
        public const string DateFormat = "yyyy-MM-dd HH:mm:ss";
    }
}
