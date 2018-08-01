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
    public static class FileUtility
    {
        public static void MoveFile(StaticEnum.FileType typeFile, string name)
        {
            var path = string.Empty;

            var sourceFile = HttpContext.Current.Server.MapPath(ConfigWeb.TempPath + name);
            var thumbImage = HttpContext.Current.Server.MapPath(ConfigWeb.ImageThumpPath + name);

            switch (typeFile)
            {
                case StaticEnum.FileType.Image:
                    path = ConfigurationManager.AppSettings["UploadImage"];                    
                    break;
                case StaticEnum.FileType.Document:
                    path = ConfigurationManager.AppSettings["UploadDocument"];
                    break;
                case StaticEnum.FileType.Multimedia:
                    path = ConfigurationManager.AppSettings["UploadMultimedia"];
                    break;
                case StaticEnum.FileType.File:
                    path = ConfigurationManager.AppSettings["UploadFile"];
                    break;
            }

            var fileSave = HttpContext.Current.Server.MapPath(path + name);

            if (File.Exists(sourceFile))
                File.Move(sourceFile, fileSave);

            if (typeFile == StaticEnum.FileType.Image)
            {
                ResizeImage(Image.FromFile(fileSave), ConfigWeb.WidthThumb, ConfigWeb.HeightThumb).Save(thumbImage);
            }
        }

        public static string GetFile(StaticEnum.FileType typeFile, string name)
        {
            var path = string.Empty;

            switch (typeFile)
            {
                case StaticEnum.FileType.Image:
                    path = ConfigurationManager.AppSettings["UploadImage"];                    
                    break;
                case StaticEnum.FileType.ImageThumb:
                    path = ConfigurationManager.AppSettings["UploadImageThumb"];
                    break;
                case StaticEnum.FileType.Document:
                    path = ConfigurationManager.AppSettings["UploadDocument"];
                    break;
                case StaticEnum.FileType.Multimedia:
                    path = ConfigurationManager.AppSettings["UploadMultimedia"];
                    break;
                case StaticEnum.FileType.File:
                    path = ConfigurationManager.AppSettings["UploadFile"];
                    break;
            }

            return path + name;
        }

        /// <summary>
        /// resize image
        /// </summary>
        /// <param name="sourceImage">file</param>
        /// <param name="width">width image</param>
        /// <param name="height">height image</param>
        /// <returns>image</returns>
        public static Image ResizeImage(Image sourceImage, int width, int height)
        {
            Image oThumbNail = new Bitmap(sourceImage, width, height);
            using (Graphics oGraphic = Graphics.FromImage(oThumbNail))
            {
                oGraphic.CompositingQuality = CompositingQuality.HighQuality;
                oGraphic.SmoothingMode = SmoothingMode.HighQuality;
                oGraphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                Rectangle oRectangle = new Rectangle(0, 0, width, height);
                oGraphic.DrawImage(sourceImage, oRectangle);
            }
            return oThumbNail;
        }
    }
}
