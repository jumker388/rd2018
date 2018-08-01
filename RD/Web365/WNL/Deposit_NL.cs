using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml;
using Web365.NL;

namespace Web365.WNL
{
    public class Deposit_NL
    {
        private string _merchant_site_code = "20507";
        private string _receiver = "seller@nganluong.vn";
        private string _Password = "123456";
        private string _return_url = "http://sandbox.nganluong.vn/demo/payment_success.php";
        private string _cancel_url = "http://sandbox.nganluong.vn/demo/payment_cancel.php";
        private string _language = "vn";
        private string _amount = "";
        private string _currency_code = "";
        private string _tax_amount = "";
        private string _discount_amount = "";
        private string _fee_shipping = "";
        private string _request_confirm_shipping = "";
        private string _no_shipping = "";
        private string _token = "";
        private string order_id = DateTime.Now.ToString("yyyyMMddHHmmss");

        private string _ws_url = "";

        NGANLUONG_API api;
        public Deposit_NL(string pathConfig)
        {
            XmlDocument dom = new XmlDocument();
            dom.Load(pathConfig);

            XmlNodeList root = dom.DocumentElement.ChildNodes;
            this._merchant_site_code = root[0].InnerText;
            this._receiver = root[1].InnerText;
            this._Password = root[2].InnerText;
            this._return_url = root[3].InnerText;
            this._cancel_url = root[4].InnerText;
            this._ws_url = root[5].InnerText;

            api = new NGANLUONG_API();
            api.Url = this._ws_url;
        }
        public Hashtable setExpressCheckOutDeposit()
        {
            /*
             * <receiver>diendc@peacesoft.net</receiver><order_code>1320985549</order_code><return_url>http://sandbox.nganluong.vn/demo/payment_success.php</return_url><cancel_url>http://sandbox.nganluong.vn/demo/payment_cancel.php</cancel_url><language>vn</language>
             */
            string param = "<params>";
            param += "<receiver>" + _receiver + "</receiver>";
            param += "<order_code>" + order_id + "</order_code>";
            param += "<return_url>" + _return_url + "</return_url>";
            param += "<cancel_url>" + _cancel_url + "</cancel_url>";
            param += "<language>" + _language + "</language>";
            param += "</params>";

            //$checksum = MD5($receiver+$order_code+$return_url+$cancel_url+$password); 

            //string checksum = _receiver + order_id + _amount + _currency_code + _tax_amount;
            //checksum += _discount_amount + _fee_shipping + _request_confirm_shipping + _no_shipping + _return_url;
            //checksum += _cancel_url + _language + _token + _Password;

            String StrcheckSum = _receiver + order_id + _return_url + _cancel_url + _language + _Password;


            String CheckSumCode = CreateMD5Hash(StrcheckSum);


            String rs = api.SetExpressCheckoutDeposit(_merchant_site_code, CheckSumCode, param);
            ResultSetExpressDeposit resu = XmlToObjectResult(rs);

            //Dung test 
            //resu.Link_checkout = "https://www.nganluong.vn/micro_checkout.php?token=1447-b5e142db784be7076967b2b7dd70806c";

            //Thiết lập chuỗi test nhập tiền.
            string str = "<script language=\"javascript\">";
            str += "var mc_flow = new NGANLUONG.apps.MCFlow({trigger:'btn_deposit',url:'" + resu.Link_checkout + "&payment_option=card'});";
            str += "</script>";

            Hashtable ha = new Hashtable();
            ha.Add("token", resu.Token);
            ha.Add("linkcheckout", str);
            ha.Add("status", resu.Result_code);
            ha.Add("description", resu.Result_description);
            return ha;

        }

        #region GetCheckSum
        public string CreateMD5Hash(string input)
        {
            // Use input string to calculate MD5 hash
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
                // To force the hex string to lower-case letters instead of
                // upper-case, use he following line instead:
                // sb.Append(hashBytes[i].ToString("x2")); 
            }
            return sb.ToString();
        }
        #endregion
        #region XML TO OBJ ResultSetExpressDeposit
        private ResultSetExpressDeposit XmlToObjectResult(string xmlResult)
        {
            XmlDocument dom = new XmlDocument();
            dom.LoadXml(xmlResult);
            XmlNodeList root = dom.DocumentElement.ChildNodes;
            ResultSetExpressDeposit obj = new ResultSetExpressDeposit();
            obj.Result_code = root.Item(0).InnerText;
            obj.Token = root.Item(1).InnerText;
            obj.Link_checkout = root.Item(2).InnerText;
            obj.Timelimit = root.Item(3).InnerText;
            obj.Result_description = root.Item(4).InnerText;

            return obj;
        }
        #endregion
    }
    #region Doi tuong tra về từ hàm được gọi SetExpressCheckOutDeposit
    public class ResultSetExpressDeposit
    {
        private string result_code = string.Empty;

        public string Result_code
        {
            get { return result_code; }
            set { result_code = value; }
        }
        private string token = string.Empty;

        public string Token
        {
            get { return token; }
            set { token = value; }
        }
        private string link_checkout = string.Empty;

        public string Link_checkout
        {
            get { return link_checkout; }
            set { link_checkout = value; }
        }
        private string timelimit = string.Empty;

        public string Timelimit
        {
            get { return timelimit; }
            set { timelimit = value; }
        }
        private string result_description = string.Empty;

        public string Result_description
        {
            get { return result_description; }
            set { result_description = value; }
        }

    }
    #endregion
}