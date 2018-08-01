using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Web365.App_Code
{
    public class HTMLCompress : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var response = filterContext.HttpContext.Response;
            //response.Filter = new MinifiedStream(response.Filter);
        }
    }

    //public class MinifiedStream : MemoryStream
    //{
    //    private readonly Stream _output;
    //    public MinifiedStream(Stream stream)
    //    {
    //        _output = stream;
    //    }
    //    private static readonly Regex Whitespace = new Regex(@"(?<=[^])\t{2,}|(?<=[>])\s{2,}(?=[<])|(?<=[>])\s{2,11}(?=[<])|(?=[\n])\s{2,}", RegexOptions.Compiled);
    //    public override void Write(byte[] buffer, int offset, int count)
    //    {
    //        var html = Encoding.UTF8.GetString(buffer);
    //        html = Whitespace.Replace(html, string.Empty);
    //        html = html.Trim();
    //        _output.Write(Encoding.UTF8.GetBytes(html), offset, Encoding.UTF8.GetByteCount(html));
    //    }
    //}
}