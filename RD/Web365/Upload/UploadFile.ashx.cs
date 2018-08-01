using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Configuration;

namespace Web365.Upload
{
    /// <summary>
    /// Summary description for UploadFile
    /// </summary>
    public class UploadFile : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = -1;
            try
            {
                for (int i = 0; i < context.Request.Files.Count; i++)
                {
                    HttpPostedFile postedFile = context.Request.Files[i];

                    var tempPath = context.Server.MapPath(ConfigurationManager.AppSettings["TempUpload"]);

                    var filename = DateTime.Now.Ticks.ToString() + "_" + postedFile.FileName;
                    if (!Directory.Exists(tempPath))
                        Directory.CreateDirectory(tempPath);

                    postedFile.SaveAs(tempPath + filename);
                    context.Response.Write(filename);
                    context.Response.StatusCode = 200;
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("Error: " + ex.Message + "-" + ex.StackTrace);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}