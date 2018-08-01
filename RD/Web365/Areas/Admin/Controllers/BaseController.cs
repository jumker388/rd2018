using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using Web365.Filters;
using Web365Business.Back_End.IRepository;

namespace Web365.Areas.Admin.Controllers
{
    [AuthorizeFilter]
    [InitializeSimpleMembership]
    public class BaseController : Controller
    {
        //
        // GET: /Admin/Base/
        public IBaseRepository baseRepository;

        [HttpGet]
        public ActionResult GetListForTree()
        {
            var list = baseRepository.GetListForTree<object>();

            return Json(new
            {
                list = list
            },
            JsonRequestBehavior.AllowGet);
        }
        public string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        [HttpGet]
        public ActionResult Detail(int id)
        {
            var obj = baseRepository.GetItemById<object>(id);

            return Json(new
            {
                data = obj
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ShowAll(int[] listId)
        {
            foreach (var item in listId)
            {
                baseRepository.Show(item);
            }

            return Json(new
            {
                Error = false
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult HideAll(int[] listId)
        {
            foreach (var item in listId)
            {
                baseRepository.Hide(item);
            }

            return Json(new
            {
                Error = false
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int[] listId)
        {
            foreach (var item in listId)
            {
                baseRepository.Delete(item);
            }

            return Json(new
            {
                Error = false
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NameExist(int id, string name)
        {
            var result = baseRepository.NameExist(id, name);

            return Json(new
            {
                exist = !result
            }, JsonRequestBehavior.AllowGet);
        }

        public virtual void ExportToExcel<T>(List<T> list, string filePath = "")
        {
            var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var properties = Array.ConvertAll(fields, field => field.Name.Replace(">k__BackingField", "").Replace("<", ""));

            var newFile = new FileInfo(filePath);
            // ok, we can run the real code of the sample now
            using (var xlPackage = new ExcelPackage(newFile))
            {
                // uncomment this line if you want the XML written out to the outputDir
                //xlPackage.DebugMode = true; 

                // get handle to the existing worksheet
                var worksheet = xlPackage.Workbook.Worksheets.Add("Record");
                xlPackage.Workbook.CalcMode = ExcelCalcMode.Manual;
                //Create Headers and format them

                for (var i = 0; i < properties.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = properties[i];
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                }

                var row = 2;
                foreach (var item in list)
                {
                    int col = 1;
                    foreach (var property in properties)
                    {
                        worksheet.Cells[row, col].Value = HttpUtility.HtmlDecode(GetPropValue(item, property).ToString());
                        col++;
                    }


                    row++;
                }


                var nameexcel = "Danh sách" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
                xlPackage.Workbook.Properties.Title = string.Format("{0} SUBET", nameexcel);
                xlPackage.Workbook.Properties.Author = "Admin-IT";
                xlPackage.Workbook.Properties.Subject = string.Format("{0} SUBET", "");
                //xlPackage.Workbook.Properties.Keywords = string.Format("{0} orders", _storeInformationSettings.StoreName);
                xlPackage.Workbook.Properties.Category = "TT99";
                //xlPackage.Workbook.Properties.Comments = string.Format("{0} orders", _storeInformationSettings.StoreName);

                // set some extended property values
                xlPackage.Workbook.Properties.Company = "SUBET";
                //xlPackage.Workbook.Properties.HyperlinkBase = new Uri(_storeInformationSettings.StoreUrl);
                // save the new spreadsheet
                xlPackage.Save();
            }
        }
        public static string GetPropValue(object src, string propName)
        {
            if (src.GetType().GetProperty(propName).GetValue(src, null) != null)
            {
                return src.GetType().GetProperty(propName).GetValue(src, null).ToString();
            }
            else
            {
                return "";
            }
            
        }


    }
}
