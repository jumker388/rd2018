using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using System.Web;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Web365Utility
{
    public static class ConfigWeb
    {
        //payment
        public static readonly string LinkDB02 = ConfigurationManager.AppSettings["LinkDB02"];
        public static readonly string fbAdmin = ConfigurationManager.AppSettings["fbAdmin"];
        public static readonly string SkipOTPLogin = ConfigurationManager.AppSettings["SkipOTPLogin"];
        public static readonly string GiftPass = ConfigurationManager.AppSettings["GiftPass"];

        public static readonly string UserSetDaiLy = ConfigurationManager.AppSettings["UserSetDaiLy"];
        public static readonly string LinkPayment = ConfigurationManager.AppSettings["LinkPayment"];
        public static readonly string MerchantId = ConfigurationManager.AppSettings["MerchantId"];
        public static readonly string MerchantPassword = ConfigurationManager.AppSettings["MerchantPassword"];
        public static readonly string ReceiverEmail = ConfigurationManager.AppSettings["ReceiverEmail"];
        public static readonly string ReturnUrl = ConfigurationManager.AppSettings["ReturnUrl"];
        public static readonly string CancelUrl = ConfigurationManager.AppSettings["CancelUrl"];
        public static readonly int PercentSale = Convert.ToInt32(ConfigurationManager.AppSettings["PercentSale"]);

        //end

        public static readonly bool IsCrawler = Convert.ToBoolean(ConfigurationManager.AppSettings["IsCrawler"]);
        public static readonly bool IsBundle = Convert.ToBoolean(ConfigurationManager.AppSettings["IsBundle"]);

        public static readonly string TempPath = ConfigurationManager.AppSettings["TempUpload"];

        public static readonly string Filepath = ConfigurationManager.AppSettings["UploadFile"];

        public static readonly string ImageThumpPath = ConfigurationManager.AppSettings["UploadImageThumb"];

        public static readonly string ImagePath = ConfigurationManager.AppSettings["UploadImage"];

        public static readonly bool UseCache = Convert.ToBoolean(ConfigurationManager.AppSettings["UseCache"]);

        public static readonly bool UseOutputCache = Convert.ToBoolean(ConfigurationManager.AppSettings["UseOutputCache"]);

        public static readonly int MinOnline = Convert.ToInt32(ConfigurationManager.AppSettings["MinOnline"]);
        public static readonly int TimeGetArticle = Convert.ToInt32(ConfigurationManager.AppSettings["TimeGetArticle"]);
        

        public static readonly int NumberRecord = Convert.ToInt32(ConfigurationManager.AppSettings["NumberRecord"]);

        public static readonly int PageSizeNews = Convert.ToInt32(ConfigurationManager.AppSettings["PageSizeNews"]);
        public static readonly int PageSizeNewsHome = Convert.ToInt32(ConfigurationManager.AppSettings["PageSizeNewsHome"]);

        public static readonly string SpecialArticle = ConfigurationManager.AppSettings["SpecialArticle"];

        public static readonly string OtherArticle = ConfigurationManager.AppSettings["OtherArticle"];
        public static readonly string Domain = ConfigurationManager.AppSettings["domain"];

        public static readonly string FolderLogsTxt = ConfigurationManager.AppSettings["FolderLogsTxt"];

        public static readonly int WidthThumb = Convert.ToInt32(ConfigurationManager.AppSettings["WidthThumb"]);
        public static readonly int HeightThumb = Convert.ToInt32(ConfigurationManager.AppSettings["HeightThumb"]);
    }
}
