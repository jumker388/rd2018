using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Web365Utility
{
    public static class Export
    {
        public static string ExportToExcel(out string file, string fileName, string[] properties, string[] fieldJson, JArray data)
        {
            fileName = string.Format("{0}_{1}.xlsx", fileName, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));

            file = fileName;

            var filePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "File\\Export", fileName);

            var newFile = new FileInfo(filePath);

            using (var xlPackage = new ExcelPackage(newFile))
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add("Worksheets");
                xlPackage.Workbook.CalcMode = ExcelCalcMode.Manual;

                for (var i = 0; i < properties.Length ; i++)
                {
                    worksheet.Cells[1, i + 1].Value = properties[i];

                    worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                }

                for (var i = 0; i < data.Count; i++)
                {
                    for (var j = 0; j < properties.Length ; j++)
                    {
                        worksheet.Cells[i + 2, j + 1].Value = data[i].SelectToken(fieldJson[j]).ToString();
                    }
                }

                // we had better add some document properties to the spreadsheet 
                // set some core property values
                var nameexcel = "Danh sách kết quả test " + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
                xlPackage.Workbook.Properties.Title = string.Format("{0}", nameexcel);
                xlPackage.Workbook.Properties.Author = "Admin-IT";
                xlPackage.Workbook.Properties.Subject = string.Format("{0}", "");
                //xlPackage.Workbook.Properties.Keywords = string.Format("{0} orders", _storeInformationSettings.StoreName);
                xlPackage.Workbook.Properties.Category = "Test result";
                //xlPackage.Workbook.Properties.Comments = string.Format("{0} orders", _storeInformationSettings.StoreName);

                // set some extended property values
                xlPackage.Workbook.Properties.Company = "Oxford - oxford.edu.vn";
                //xlPackage.Workbook.Properties.HyperlinkBase = new Uri(_storeInformationSettings.StoreUrl);
                // save the new spreadsheet
                xlPackage.Save();

                return filePath;
            }
        }
    }
}
