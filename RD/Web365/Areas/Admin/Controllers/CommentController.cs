using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web365Utility;
using Web365Base;
using Web365Business.Back_End.IRepository;
using Web365Domain;
using System;
namespace Web365.Areas.Admin.Controllers
{
    public class CommentController : BaseController
    {

        private ICommentRepository _commentRepository;

        // GET: /Admin/ProductType/

        public CommentController(ICommentRepository commentRepository)
        {
            this.baseRepository = commentRepository;
            this._commentRepository = commentRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetList(string name, int currentRecord, int numberRecord, string propertyNameSort, bool descending)
        {
            var total = 0;
            var list = _commentRepository.GetList(out total, name, currentRecord, numberRecord, propertyNameSort, descending);

            return Json(new
            {
                total = total,
                list = list
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetListOfGroup(int groupId)
        {
            var list = "";

            return Json(new
            {
                list = list
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditForm(int? id)
        {
            var obj = new CommentItem();

            var listType = _commentRepository.GetListForTree<object>();

            if (id.HasValue)
                obj = _commentRepository.GetItemById<CommentItem>(id.Value);

            return Json(new
            {
                data = obj,
                listType = listType
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Action(tblComment objSubmit)
        {

            if (objSubmit.ID == 0)
            {
                objSubmit.DateCreated = DateTime.Now;
                objSubmit.IsShow = true;
                _commentRepository.Add(objSubmit);
            }
            else
            {
                var obj = _commentRepository.GetById<tblComment>(objSubmit.ID);

                UpdateModel(obj);

                _commentRepository.Update(obj);
            }

            return Json(new
            {
                Error = false
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
