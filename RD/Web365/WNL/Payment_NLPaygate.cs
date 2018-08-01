using System;
using System.Data;
using System.Configuration;
//using System.Linq;
using System.Web;
//using System.Xml.Linq;

using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.IO;
using System.Collections;
using Web365.NL;


namespace Web365.WNL
{
    public class Payment_NLPaygate
    {
        private NGANLUONG_API api;
        private string _merchant_site_code = "20507";
        private string _receiver = "hunghd268@gmail.com";
        private string _Password = "123";
        private string _return_url = "http://sandbox.nganluong.vn/demo/payment_success.php";
        private string _cancel_url = "http://sandbox.nganluong.vn/demo/payment_cancel.php";
        private string _language = "vn";
        private string _amount = "50000";
        private string _currency_code = "vnd";
        private string _tax_amount = "0";
        private string _discount_amount = "0";
        private string _fee_shipping = "0";
        private string _request_confirm_shipping = "0";
        private string _no_shipping = "1";
        private string _token = "";
        private string order_id = DateTime.Now.ToString("yyyyMMddHHmmss");
        private string _ws_url = "";


        public Payment_NLPaygate(string pathConfig)
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

        public Hashtable SetExpressCheckoutPayment(DataTable request, String tax, String feeship)
        {
            _amount = GetTotalAmount(request, tax, feeship);
            string checksum = _receiver + order_id + _amount + _currency_code + tax;
            checksum += _discount_amount + feeship + _request_confirm_shipping + _no_shipping + _return_url;
            checksum += _cancel_url + _language + _token + _Password;



            String param = GetParams(request, tax, feeship);

            String urlCheckout = api.SetExpressCheckoutPayment(_merchant_site_code, getCheckSum(checksum), param);

            ResultExpressSetCheckOut resu = XmlToObjectResult(urlCheckout);


            //Dung test 
            // resu.Link_checkout = "https://www.nganluong.vn/micro_checkout.php?token=1462-35b98139626089a7aa9c16c98b194b99&email=diendc@gmail.com&mobile=0904515105";

            //Thiết lập chuỗi test nhập tiền.
            string str = "<script language=\"javascript\">";
            str += "var mc_flow = new NGANLUONG.apps.MCFlow({trigger:'btn_payment',url:'" + resu.Link_checkout +
                   "&payment_option=bank'});";
            str += "</script>";

            Hashtable ha = new Hashtable();
            ha.Add("token", resu.Token);
            ha.Add("linkcheckout", str);
            ha.Add("status", resu.Result_code);
            ha.Add("description", resu.Result_description);
            return ha;
        }

        public ResultGetExpressCheckOut GetExpressCheckout(string token)
        {
            String param = "<params><token>" + token + "</token></params>";
            String Checksum = getCheckSum(token + _Password);
            String result = api.GetExpressCheckout(_merchant_site_code, Checksum, param);
            return XmlToObjResultGetExpressCheckOut(result);
        }

        #region Xml Result GetExpressCheckOut

        /*
     * <result>
   <result_code></result_code>
   <result_description></result_description>   
   <time_limit></time_limit>
   <merchant_site_code></merchant_site_code>
   <transaction_id></transaction_id>
   <amount></amount>
   <currency_code></currency_code>
   <transaction_type></transaction_type>
   <transaction_status></transaction_status>
   <payer_name></payer_name>
   <payer_email></payer_email>
   <payer_mobile></payer_mobile>
   <receiver_name></receiver_name>
   <receiver_address></receiver_address>
   <receiver_mobile></receiver_mobile>
   <method_payment_name></method_payment_name>
   <card_type></card_type>
   <card_amount></card_amount>
  </result>
     * 
     */

        private ResultGetExpressCheckOut XmlToObjResultGetExpressCheckOut(string xmlResult)
        {
            XmlDocument dom = new XmlDocument();
            dom.LoadXml(xmlResult);
            XmlNodeList root = dom.DocumentElement.ChildNodes;

            ResultGetExpressCheckOut obj = new ResultGetExpressCheckOut();
            obj.Result_code = root.Item(0).InnerText;
            obj.Result_description = root.Item(1).InnerText;
            obj.Time_limit = root.Item(2).InnerText;
            obj.Merchant_site_code = root.Item(3).InnerText;
            obj.Transaction_id = root.Item(4).InnerText;
            obj.Amount = root.Item(5).InnerText;
            obj.Currency_code = root.Item(6).InnerText;
            obj.Transaction_type = root.Item(7).InnerText;
            obj.Transaction_status = root.Item(8).InnerText;
            obj.Payer_name = root.Item(9).InnerText;
            obj.Payer_email = root.Item(10).InnerText;
            obj.Payer_mobile = root.Item(11).InnerText;
            obj.Receiver_name = root.Item(12).InnerText;
            obj.Receiver_address = root.Item(13).InnerText;
            obj.Receiver_mobile = root.Item(14).InnerText;
            obj.Method_payment_name = root.Item(15).InnerText;
            obj.CardType = root.Item(16).InnerText;
            obj.Card_Amount = root.Item(17).InnerText;

            return obj;
        }

        #endregion

        #region XML TO OBJ SetExpressCheckOut

        private ResultExpressSetCheckOut XmlToObjectResult(string xmlResult)
        {
            XmlDocument dom = new XmlDocument();
            dom.LoadXml(xmlResult);
            XmlNodeList root = dom.DocumentElement.ChildNodes;
            ResultExpressSetCheckOut obj = new ResultExpressSetCheckOut();
            obj.Result_code = root.Item(0).InnerText;
            obj.Token = root.Item(1).InnerText;
            obj.Link_checkout = root.Item(2).InnerText;
            obj.Timelimit = root.Item(3).InnerText;
            obj.Result_description = root.Item(4).InnerText;

            return obj;
        }

        #endregion

        #region GetCheckSum

        public string getCheckSum(string input)
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

        #region Prameters

        private String GetTotalAmount(DataTable request, String tax, String feeship)
        {
            float price = 0;
            foreach (DataRow row in request.Rows)
            {
                price = price + float.Parse(row["ProPrice"].ToString());
            }
            price = price + float.Parse(tax) + float.Parse(feeship);
            return price.ToString();
        }

        private string GetParams(DataTable request, string tax, string feeship)
        {
            string param = "<params>";
            param += "<receiver>" + _receiver + "</receiver>";
            param += "<order_code>" + order_id + "</order_code>";
            param += "<amount>" + _amount + "</amount>";
            param += "<currency_code>" + _currency_code + "</currency_code>";
            param += "<tax_amount>" + tax + "</tax_amount>";
            param += "<discount_amount>" + _discount_amount + "</discount_amount>";
            param += "<fee_shipping>" + feeship + "</fee_shipping>";
            param += "<request_confirm_shipping>" + _request_confirm_shipping + "</request_confirm_shipping>";
            param += "<no_shipping>" + _no_shipping + "</no_shipping>";
            param += "<return_url>" + _return_url + "</return_url>";
            param += "<cancel_url>" + _cancel_url + "</cancel_url>";
            param += "<language>vn</language>";
            param += "<items>";
            foreach (DataRow row in request.Rows)
            {
                param += "<item>";
                param += "<item_name>" + row["ProName"].ToString() + "</item_name>";
                param += "<item_quanty>" + row["ProAmount"] + "</item_quanty>";
                param += "<item_amount>" + row["ProPrice"] + "</item_amount>";
                param += "</item>";
            }
            param += "</items>";
            param += "</params>";



            return param;
        }

        #endregion
    }

    #region Đối tượng trả về từ hàm được gọi GetEpressCheckOut

    public class ResultGetExpressCheckOut
    {
        private string result_code = string.Empty;

        public string Result_code
        {
            get { return result_code; }
            set { result_code = value; }
        }

        private string result_description = string.Empty;

        public string Result_description
        {
            get { return result_description; }
            set { result_description = value; }
        }

        private string time_limit = string.Empty;

        public string Time_limit
        {
            get { return time_limit; }
            set { time_limit = value; }
        }

        private string merchant_site_code = string.Empty;

        public string Merchant_site_code
        {
            get { return merchant_site_code; }
            set { merchant_site_code = value; }
        }

        private string transaction_id = string.Empty;

        public string Transaction_id
        {
            get { return transaction_id; }
            set { transaction_id = value; }
        }

        private string amount = string.Empty;

        public string Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        private string currency_code = string.Empty;

        public string Currency_code
        {
            get { return currency_code; }
            set { currency_code = value; }
        }

        private string transaction_type = string.Empty;

        public string Transaction_type
        {
            get { return transaction_type; }
            set { transaction_type = value; }
        }

        private string transaction_status = string.Empty;

        public string Transaction_status
        {
            get { return transaction_status; }
            set { transaction_status = value; }
        }

        private string payer_name = string.Empty;

        public string Payer_name
        {
            get { return payer_name; }
            set { payer_name = value; }
        }

        private string payer_email = string.Empty;

        public string Payer_email
        {
            get { return payer_email; }
            set { payer_email = value; }
        }

        private string payer_mobile = string.Empty;

        public string Payer_mobile
        {
            get { return payer_mobile; }
            set { payer_mobile = value; }
        }

        private string receiver_name = string.Empty;

        public string Receiver_name
        {
            get { return receiver_name; }
            set { receiver_name = value; }
        }

        private string receiver_address = string.Empty;

        public string Receiver_address
        {
            get { return receiver_address; }
            set { receiver_address = value; }
        }

        private string receiver_mobile = string.Empty;

        public string Receiver_mobile
        {
            get { return receiver_mobile; }
            set { receiver_mobile = value; }
        }

        private string method_payment_name = string.Empty;

        public string Method_payment_name
        {
            get { return method_payment_name; }
            set { method_payment_name = value; }
        }

        private string _CardType = string.Empty;

        public string CardType
        {
            get { return _CardType; }
            set { _CardType = value; }
        }

        private string _Card_Amount = "0";

        public string Card_Amount
        {
            get { return _Card_Amount; }
            set { _Card_Amount = value; }
        }


    }

    #endregion

    #region Doi tuong tra về từ hàm được gọi SetExpressCheckOut

    public class ResultExpressSetCheckOut
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

    #region Đối tượng InfoRequest lưu giữ các thông tin sẽ được truyền vào trong lời gọi hàm setExpressCheckOut

    public class InfoRequest
    {

        private string password = string.Empty;



        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        private DataTable products = new DataTable();

        public DataTable Products
        {
            get { return products; }
            set { products = value; }
        }

        private string merchant_site_code = string.Empty;



        public string Merchant_site_code
        {
            get { return merchant_site_code; }
            set { merchant_site_code = value; }
        }

        private string reciver = "hunghd268@gmail.com";

        public string Reciver
        {
            get { return reciver; }
            set { reciver = value; }
        }

        private string order_code = "OD1242";

        public string Order_code
        {
            get { return order_code; }
            set { order_code = value; }
        }

        private string amount = "10000";

        public string Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        private string currency_code = "vnd";

        public string Currency_code
        {
            get { return currency_code; }
            set { currency_code = value; }
        }

        private string tax_amount = "0";

        public string Tax_amount
        {
            get { return tax_amount; }
            set { tax_amount = value; }
        }


        private string discount_amount = "0";

        public string Discount_amount
        {
            get { return discount_amount; }
            set { discount_amount = value; }
        }

        private string no_shipping = "0";

        public string No_shipping
        {
            get { return no_shipping; }
            set { no_shipping = value; }
        }


        private string free_shipping = "0";

        public string Free_shipping
        {
            get { return free_shipping; }
            set { free_shipping = value; }
        }

        private string request_conform_shipping = "0";

        public string Request_conform_shipping
        {
            get { return request_conform_shipping; }
            set { request_conform_shipping = value; }
        }

        private string return_url = string.Empty;

        public string Return_url
        {
            get { return return_url; }
            set { return_url = value; }
        }

        private string cancel_url = string.Empty;

        public string Cancel_url
        {
            get { return cancel_url; }
            set { cancel_url = value; }
        }

        private string language = "vn";

        public string Language
        {
            get { return language; }
            set { language = value; }
        }

        private string items = string.Empty;

        public string Items
        {
            get { return items; }
            set { items = value; }
        }

        private string checksum = string.Empty;

        public string Checksum
        {
            get { return checksum; }
            set { checksum = value; }
        }
    }

    #endregion

}