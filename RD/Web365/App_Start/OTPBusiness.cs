using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Web365Business.Back_End.IRepository;
using Web365Domain;

namespace Web365.App_Start
{

    public class OTPBusiness
    {
        //
        // GET: /Admin/Login/

        public string GenerateOTP()
        {
            string temp = "";
            var ran1 = new Random();
            int index1 = ran1.Next(999999);
            if (index1 < 10)
            {
                temp = "00000" + index1;
            }
            else if (index1 < 100)
            {
                temp = "0000" + index1;
            }
            else if (index1 < 1000)
            {
                temp = "000" + index1;
            }
            else if (index1 < 10000)
            {
                temp = "00" + index1;
            }
            else if (index1 < 100000)
            {
                temp = "0" + index1;
            }
            else
            {
                temp = index1.ToString();
            }
            return temp;

        }

        public OtpItem SendOPT(string Mobile, string username, int type)
        {
            string temp = GenerateOTP();
            string smsTemp = "Ma+OTP+cua+ban+la: ";
            switch (type)
            {
                case 7:// login dai ly
                    smsTemp = "Ma+OTP+dang+nhap+cua+ban+la:";
                    break;
                case 8:// ruts ket dai ly
                    smsTemp = "Ma+OTP+gui+ket+cua+ban+la:";
                    break;
                case 9://chuyen tien dai ly
                    smsTemp = "Ma+OTP+chuyen+tien++cua+ban+la:";
                    break;
                case 10:// ruts ket dai ly
                    smsTemp = "Ma+OTP+rut+ket+cua+ban+la:";
                    break;
                case 11:// ruts ket dai ly
                    smsTemp = "Ma+OTP+doi+mat+khau+cua+ban+la:";
                    break;
                default:
                    break;
            }
            var objotp = new OtpItem();

            objotp.Type = type;
            objotp.username = username;
            objotp.Otp_Plus = temp;
            objotp.smsTemp = smsTemp;
            return objotp;
        }
           
        public string SendOPTSMS(string Mobile, string username, int type, string smsTemp, string temp)
        {
            using (WebClient client = new WebClient())
            {
                client.Headers["User-Agent"] =
                    "Mozilla/4.0 (Compatible; Windows NT 5.1; MSIE 6.0) " +
                    "(compatible; MSIE 6.0; Windows NT 5.1; " +
                    ".NET CLR 1.1.4322; .NET CLR 2.0.50727)";

                // Download data.
                var arr = client.DownloadData("http://207.148.79.18/send_otp.php?phone=" + Mobile + "&content=" + temp);

            }

            return Mobile;
        }


    }
}