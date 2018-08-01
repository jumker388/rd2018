using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace Web365Utility
{
    public static class Web365Utility
    {
        public static void Statistics()
        {
            int num;

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(HttpContext.Current.Server.MapPath("~/statistics.xml"));

                XmlNode str = xmlDocument.SelectSingleNode("/Statistics/Day");
                if (DateTime.Now.Day == Convert.ToInt32(str.ChildNodes[0].InnerText))
                {
                    num = Int32.Parse(str.ChildNodes[1].InnerText) + 1;
                    str.ChildNodes[1].InnerText = num.ToString();
                }
                else
                {
                    XmlNode yesterday = xmlDocument.SelectSingleNode("/Statistics/Yesterday");
                    yesterday.ChildNodes[0].InnerText = str.ChildNodes[1].InnerText;
                    str.ChildNodes[1].InnerText = "1";
                }
                str.ChildNodes[0].InnerText = DateTime.Now.Day.ToString();

                XmlNode week = xmlDocument.SelectSingleNode("/Statistics/Week");
                if (DateTime.Now.DayOfWeek.ToString() == "Monday" && week.ChildNodes[0].InnerText == "false")
                {

                    XmlNode preWeek = xmlDocument.SelectSingleNode("/Statistics/PreWeek");
                    preWeek.ChildNodes[0].InnerText = week.ChildNodes[1].InnerText;
                    week.ChildNodes[0].InnerText = "true";
                    week.ChildNodes[1].InnerText = "1";
                }
                else
                {
                    num = Int32.Parse(week.ChildNodes[1].InnerText) + 1;
                    week.ChildNodes[0].InnerText = "false";
                    week.ChildNodes[1].InnerText = num.ToString();
                }

                XmlNode month = xmlDocument.SelectSingleNode("/Statistics/Month");
                if (DateTime.Now.Day > 1)
                {
                    num = Int32.Parse(month.ChildNodes[0].InnerText) + 1;
                    month.ChildNodes[0].InnerText = num.ToString();
                }
                else
                {
                    month.ChildNodes[0].InnerText = "1";
                }

                XmlNode xmlNodes = xmlDocument.SelectSingleNode("/Statistics/Total");
                num = Int32.Parse(xmlNodes.ChildNodes[0].InnerText) + 1;
                xmlNodes.ChildNodes[0].InnerText = num.ToString();

                xmlDocument.Save(HttpContext.Current.Server.MapPath("~/statistics.xml"));
                xmlDocument = null;
            }
            catch (Exception ex)
            {

            }
        }
        public static string FormatPrice(decimal? price)
        {
            if (price.HasValue && price != 0)
            {
                string result = price.Value.ToString("#,#");
                if (result.Contains(","))
                    result = result.Replace(',', '.');
                return result;
            }
            return "0";
        }
        public static string FormatPrice(double? price)
        {
            if (price.HasValue && price != 0)
            {
                string result = price.Value.ToString("#,#");
                if (result.Contains(","))
                    result = result.Replace(',', '.');
                return result;
            }
            return "0";
        }

        public static string GetInfoStatistics()
        {
            try
            {
                var xDocument = XDocument.Load(HttpContext.Current.Server.MapPath("~/Statistics.xml"));

                var collection =
                    from item in xDocument.Descendants("Statistics")
                    select item.Element("Yesterday").Element("Total").Value
                        + "," + item.Element("Day").Element("Total").Value
                        + "," + item.Element("Week").Element("Total").Value
                        + "," + item.Element("PreWeek").Element("Total").Value
                        + "," + item.Element("Month").Element("Total").Value
                        + "," + item.Element("Total").Element("Number").Value;

                return collection.FirstOrDefault();
            }
            catch (Exception ex)
            {
                return string.Empty;
            }

        }

        public static string GetPathFile(string fileName)
        {
            try
            {
                return ConfigWeb.Filepath + fileName;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string GetPathThumbPicture(string fileName)
        {
            try
            {
                return ConfigWeb.ImageThumpPath + fileName;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string GetPathPicture(string fileName)
        {
            try
            {
                return ConfigWeb.ImagePath + fileName;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string GetPathPictureWithDomain(string fileName)
        {
            try
            {
                return "http://nghethuatquyenru.com" + ConfigWeb.ImagePath + fileName;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string ConvertToAscii(string str)
        {
            str = str.ToLower().Trim();
            str = Regex.Replace(str, "[á|à|ả|ã|ạ|â|ă|ấ|ầ|ẩ|ẫ|ậ|ắ|ằ|ẳ|ẵ|ặ]", "a", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "[é|è|ẻ|ẽ|ẹ|ê|ế|ề|ể|ễ|ệ]", "e", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "[ú|ù|ủ|ũ|ụ|ư|ứ|ừ|ử|ữ|ự]", "u", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "[í|ì|ỉ|ĩ|ị]", "i", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "[ó|ò|ỏ|õ|ọ|ô|ơ|ố|ồ|ổ|ỗ|ộ|ớ|ờ|ở|ỡ|ợ]", "o", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "[đ|Đ]", "d", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "[ý|ỳ|ỷ|ỹ|ỵ|Ý|Ỳ|Ỷ|Ỹ|Ỵ]", "y", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "[,|~|@|/|.|:|?|#|$|%|&|*|(|)|+|”|“|'|\"|!|`|–]", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"\s+", " ");
            str = Regex.Replace(str, "[\\s]", "-");
            str = Regex.Replace(str, @"-+", "-");

            return str;
        }

        public static int[] StringToArrayInt(string str)
        {
            try
            {
                return str.Split(',').Select(x => int.Parse(x)).ToArray();
            }
            catch (Exception)
            {
                return new int[0];
            }
        }

        public static string ConvertNumber(string number)
        {
            var result = "0";
            try
            {
                if (!string.IsNullOrEmpty(number))
                {
                    result = ConvertNumber(Int32.Parse(number));
                }
            }
            catch (Exception)
            {
            }

            return result;
        }

        public static string ConvertNumber(int? number)
        {
            var result = "0";
            try
            {
                if (number.HasValue && number != 0)
                {
                    result = number.Value.ToString("#,#");
                    if (result.Contains(","))
                        result = result.Replace(',', '.');
                }
            }
            catch (Exception)
            {
            }

            return result;
        }
        public static int ConvertToInt32(string number)
        {
            try
            {
                return Convert.ToInt32(number);
            }
            catch (Exception)
            {
                return 0;
            }

            return 0;
        }

        public static string ConvertNumber(decimal? number)
        {
            var result = "0";
            try
            {
                if (number.HasValue && number != 0)
                {
                    result = number.Value.ToString("#,#");
                    if (result.Contains(","))
                        result = result.Replace(',', '.');
                }
            }
            catch (Exception)
            {
            }

            return result;
        }

        public static string StringBase64ToString(string str)
        {
            var encodedDataAsBytes = Convert.FromBase64String(str);

            return Encoding.UTF8.GetString(encodedDataAsBytes);
        }

        public static string StringToBase64(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);

            return Convert.ToBase64String(bytes);

        }

        public static string MD5Hash(string input)
        {
            using (var md5Hash = MD5.Create())
            {
                var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                var sBuilder = new StringBuilder();

                for (var i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                return sBuilder.ToString();
            }
        }
        public static string MD5Cryptor(string input)
        {
            using (var md5Hash = MD5.Create())
            {
                var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                var sBuilder = new StringBuilder();

                for (var i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                return sBuilder.ToString();
            }
        }

        public static void SendMail(string email, string title, string content)
        {

            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com", // smtp server address here…
                Port = 25,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new System.Net.NetworkCredential("qaz09wsx@gmail.com", "0312696652"),
                Timeout = 30000,
            };

            MailMessage message = new MailMessage("qaz09wsx@gmail.com", email, title, content);

            message.IsBodyHtml = true;

            smtp.Send(message);

            //using (MailMessage mail = new MailMessage())
            //{
            //    mail.To.Add(email);
            //    mail.Subject = title;
            //    mail.Body = title;
            //    mail.IsBodyHtml = true;

            //    using (SmtpClient smtp = new SmtpClient())
            //    {
            //        smtp.Send(mail);
            //    }
            //}
        }

        public static int ToInt32(object obj)
        {
            var retVal = 0;
            try
            {
                retVal = Convert.ToInt32(obj);
            }
            catch
            {
                retVal = 0;
            }
            return retVal;
        }
        
        public static string StringToBoolString(string obj)
        {
            var retVal = "0";
            if (obj.ToLower() == "true")
                retVal = "1";
            return retVal;
        }

        public static int ToInt32(object obj, int defaultValue)
        {
            int retVal = defaultValue;

            if (obj == null || obj == DBNull.Value)
                return retVal;

            try
            {
                retVal = Convert.ToInt32(obj);
            }
            catch
            {
                retVal = defaultValue;
            }

            return retVal;
        }
        public static string RemoveHtmlTag(string source)
        {
            return string.IsNullOrEmpty(source) == false ? Regex.Replace(source, "<.*?>", "") : string.Empty;
        }

        public static double ToDouble(object obj)
        {
            double retVal = 0;

            try
            {
                retVal = Convert.ToDouble(obj);
            }
            catch
            {
                retVal = 0;
            }

            return retVal;
        }

        public static long ToLong(object obj)
        {
            long retVal = 0;

            try
            {
                retVal = Convert.ToInt64(obj);
            }
            catch
            {
                retVal = 0;
            }

            return retVal;
        }

        public static decimal ToDecimal(object obj)
        {
            decimal retVal = 0;

            try
            {
                retVal = Convert.ToDecimal(obj);
            }
            catch
            {
                retVal = 0;
            }

            return retVal;
        }

        public static double ToDouble(object obj, double defaultValue)
        {
            double retVal = 0;

            if (obj == null || obj == DBNull.Value)
                return retVal;

            try
            {
                retVal = Convert.ToDouble(obj);
            }
            catch
            {
                retVal = defaultValue;
            }

            return retVal;
        }

        public static string ToString(object obj)
        {
            string retVal = String.Empty;

            try
            {
                retVal = Convert.ToString(obj);
            }
            catch
            {
                retVal = String.Empty;
            }

            return retVal;
        }

        public static string ToString(object obj, string defaultValue)
        {
            string retVal = String.Empty;

            if (obj == null || obj == DBNull.Value)
                return retVal;

            try
            {
                retVal = Convert.ToString(obj);
            }
            catch
            {
                retVal = defaultValue;
            }

            return retVal;
        }

        public static DateTime ToDate(object obj)
        {
            DateTime retVal = DateTime.Now;
            string[] strArr = obj.ToString().Split(' ');
            int lenStrArr = strArr.Length;
            try
            {
                if (lenStrArr >= 1)
                {
                    string str = strArr[0].ToString();
                    string[] strTemp = str.ToString().Split('/');
                    if (strTemp.Length == 3)
                    {
                        string t = string.Empty;
                        if (lenStrArr == 2)
                        {
                            t = strArr[1].ToString();
                        }
                        string input = string.Format("{0}-{1}-{2} {3}", strTemp[2].ToString(), strTemp[1].ToString(), strTemp[0].ToString(), t);

                        retVal = Convert.ToDateTime(input);
                    }
                }
            }
            catch
            {
                retVal = DateTime.Now;
            }

            return retVal;
        }

        public static DateTime ToDateTime(string obj)
        {
            DateTime retVal = DateTime.Now;
            string[] strArr = obj.Split(' ');
            int lenStrArr = strArr.Length;
            try
            {
                if (lenStrArr >= 1)
                {
                    string str = strArr[0].ToString();
                    string[] strTemp = str.ToString().Split('/');
                    if (strTemp.Length == 3)
                    {
                        string t = string.Empty;
                        if (lenStrArr == 2)
                        {
                            t = strArr[1].ToString();
                        }
                        string input = string.Format("{0}-{2}-{1} {3}", strTemp[2].ToString(), strTemp[1].ToString(), strTemp[0].ToString(), t);

                        retVal = Convert.ToDateTime(input);
                    }
                }
            }
            catch
            {
                retVal = DateTime.Now;
            }

            return retVal;
        }
        public static bool IsRequestMobile(string agent)
        {
            try
            {
                var b = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od|ad)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                var v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                var isMobile = (b.IsMatch(agent) || v.IsMatch(agent.Substring(0, 4)));
                return isMobile;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static string RemoveStyleInHtmlTag(string source)
        {

            if (!string.IsNullOrEmpty(source))
            {
                return Regex.Replace(source,
                    @"style\s*\x3D\x5c?(\x27|\x22)[a-zA-Z0-9\x3A\x3B\s\x2D\x2C\x2E\x28\x29\x25\x23\x40\x21]+\x5c?(\x27|\x22)", "",
                    RegexOptions.IgnoreCase | RegexOptions.Singleline);

            }
            return string.Empty;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        /// <param name="tokens"></param>
        /// <param name="htmlEncode"></param>
        /// <returns></returns>
        public static string Replace(string template, IEnumerable<TokenEmailItem> tokens, bool htmlEncode)
        {
            if (string.IsNullOrWhiteSpace(template))
                throw new ArgumentNullException("template");

            if (tokens == null)
                throw new ArgumentNullException("tokens");

            foreach (var token in tokens)
            {
                var tokenValue = token.Value;
                //do not encode URLs
                if (htmlEncode && !token.NeverHtmlEncoded)
                    tokenValue = HttpUtility.HtmlEncode(tokenValue);
                template = Replace(template, String.Format(@"%{0}%", token.Key), tokenValue);
            }
            return template;

        }
        private static readonly StringComparison _stringComparison;
        private static string Replace(string original, string pattern, string replacement)
        {
            if (_stringComparison == StringComparison.Ordinal)
            {
                return original.Replace(pattern, replacement);
            }
            else
            {
                int count, position0, position1;
                count = position0 = position1 = 0;
                var inc = (original.Length / pattern.Length) * (replacement.Length - pattern.Length);
                var chars = new char[original.Length + Math.Max(0, inc)];
                while ((position1 = original.IndexOf(pattern, position0, _stringComparison)) != -1)
                {
                    for (int i = position0; i < position1; ++i)
                        chars[count++] = original[i];
                    for (int i = 0; i < replacement.Length; ++i)
                        chars[count++] = replacement[i];
                    position0 = position1 + pattern.Length;
                }
                if (position0 == 0) return original;
                for (var i = position0; i < original.Length; ++i)
                    chars[count++] = original[i];
                return new string(chars, 0, count);
            }
        }

        public class TokenEmailItem
        {
            private readonly string _key;
            private readonly string _value;
            private readonly bool _neverHtmlEncoded;

            public TokenEmailItem(string key, string value) :
                this(key, value, false)
            {

            }
            public TokenEmailItem(string key, string value, bool neverHtmlEncoded)
            {
                this._key = key;
                this._value = value;
                this._neverHtmlEncoded = neverHtmlEncoded;
            }

            /// <summary>
            /// Token key
            /// </summary>
            public string Key { get { return _key; } }
            /// <summary>
            /// Token value
            /// </summary>
            public string Value { get { return _value; } }
            /// <summary>
            /// Indicates whether this token should not be HTML encoded
            /// </summary>
            public bool NeverHtmlEncoded { get { return _neverHtmlEncoded; } }

            public override string ToString()
            {
                return string.Format("{0}: {1}", Key, Value);
            }
        }
    }
}
