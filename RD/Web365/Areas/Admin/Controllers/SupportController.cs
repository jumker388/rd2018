using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Web365Utility;
using Web365Base;
using Web365Business.Back_End.IRepository;
using Web365Domain;
using System;
namespace Web365.Areas.Admin.Controllers
{
    public class SupportController : BaseController
    {

        private ISupportRepository supportRepository;
        private ISupportTypeRepository supportTypeRepository;
        private IGamePlayerRepository gamePlayerRepository;

        // GET: /Admin/ProductType/

        public SupportController(ISupportRepository _supportRepository,
            ISupportTypeRepository _supportTypeRepository, IGamePlayerRepository _gamePlayerRepository)
        {
            this.baseRepository = _supportRepository;
            this.supportRepository = _supportRepository;
            this.supportTypeRepository = _supportTypeRepository;
            this.gamePlayerRepository = _gamePlayerRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetList(string name, int currentRecord, int numberRecord, string propertyNameSort, bool descending)
        {
            var total = 0;
            var list = supportRepository.GetList(out total, name, currentRecord, numberRecord, propertyNameSort, descending);

            return Json(new
            {
                total = total,
                list = list
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetListMessGoogle(string name, int currentRecord, int numberRecord)
        {
            var total = 0;
            var list = supportRepository.GetListMessGoogle(out total, name, currentRecord, numberRecord);

            return Json(new
            {
                total = total,
                list = list
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditFormMessGoogle(int? id)
        {
            var obj = new GoogleMessageItem();
            obj.Title = "Tứ quý 9 - Game bài Online";
            return Json(new
            {
                data = obj
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ActionMessGoogle(tblGoogleMessage objSubmit)
        {
            objSubmit.DateCreated = DateTime.Now;
            var rnd = new Random();
            var tick = rnd.Next(1, int.MaxValue); 
            const string uri = "https://fcm.googleapis.com/fcm/send";
            var myParameters = "{\"to\": \"/topics/info\",\"data\": {\"id\" : " + tick + ",\"title\": \"" + objSubmit.Title + "\",\"content-text\": \"" + objSubmit.ContentText + "\"}}";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpWebRequest.ContentType = "application/json; charset=UTF-8";
            httpWebRequest.Headers[HttpRequestHeader.Authorization] = "key=AIzaSyDlUtC0lD4ndwv4f8T62N52yvgUURzYtE8";
            httpWebRequest.Method = "POST";
            httpWebRequest.Accept = "application/json";
            httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.87 Safari/537.3";
            byte[] postBytes = Encoding.UTF8.GetBytes(myParameters);

            var requestStream = httpWebRequest.GetRequestStream();

            // now send it
            requestStream.Write(postBytes, 0, postBytes.Length);
            requestStream.Close();

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var htmlResult = streamReader.ReadToEnd();
                var jObject = JObject.Parse(htmlResult);

                var messageId = (string)jObject.SelectToken("message_id");
                objSubmit.GoogleId = messageId;
            }
            try
            {
                supportRepository.AddGoogleMess(objSubmit);
            }
            catch (Exception)
            {
                return Json(new
                {
                    Error = true
                }, JsonRequestBehavior.AllowGet);
            }
            
            return Json(new
            {
                Error = false
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult EditForm(int? id)
        {
            var obj = new SupportItem();

            var listType = supportTypeRepository.GetListForTree<object>();

            if (id.HasValue)
                obj = supportRepository.GetItemById<SupportItem>(id.Value);

            return Json(new
            {
                data = obj,
                listType = listType
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Action(tblSupport objSubmit)
        {

            if (objSubmit.ID == 0)
            {
                objSubmit.DateCreated = DateTime.Now;
                objSubmit.DateUpdated = DateTime.Now;
                objSubmit.IsDeleted = false;
                objSubmit.IsShow = true;
                supportRepository.Add(objSubmit);
            }
            else
            {
                var obj = supportRepository.GetById<tblSupport>(objSubmit.ID);

                UpdateModel(obj);

                objSubmit.DateUpdated = DateTime.Now;

                supportRepository.Update(obj);
            }

            return Json(new
            {
                Error = false
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
