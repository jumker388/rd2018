using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web365Business.Game
{
    public static class Constants
    {
        //public static string DBConnection = "datasource=119.81.53.85;port=3306;username=devtuquy;password=Xungh1eu@9999sql;CharSet=utf8;";
        public static string DBConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public const string DateFormat = "yyyy-MM-dd HH:mm:ss";
    }
}
