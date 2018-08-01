using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using Elmah.ContentSyndication;
using Web365Business.Front_End.IRepository;
using Web365Business.Front_End.Repository;
using Web365Domain;

namespace Web365.Controllers
{
    public class RssFeedController : ApiController
    {
       
        // GET api/<controller>
        public HttpResponseMessage Get()
        {
            var bob = new StringBuilder();
            // We will use stringWriter to push the formated xml into our StringBuilder bob.
            using (var stringWriter = new StringWriter(bob))
            {
                using (var xml = new XmlTextWriter(stringWriter))
                {
                    xml.Formatting = Formatting.Indented;

                    xml.WriteStartDocument();
                    // Write the root element.
                    xml.WriteStartElement("rss");
                    xml.WriteAttributeString("version", null, "2.0");
                    xml.WriteAttributeString("xmlns:content", null, "http://purl.org/rss/1.0/modules/content/");
                    xml.WriteStartElement("channel");


                    xml.WriteElementString("title", "sadasd News Publisher");
                    xml.WriteElementString("link", "http://asds.com");
                    xml.WriteElementString("description", "Read our awesome news, every day.");
                    xml.WriteElementString("language", "en-us");
                    xml.WriteElementString("lastBuildDate", DateTime.Now.ToShortDateString());

                    var articleRepositoryFe = new ArticleRepositoryFE();
                    var lst = articleRepositoryFe.GetListByType(3329, "phong-ngu", 0, 100, true, false);

                    foreach (var item in lst.List)
                    {
                        xml.WriteStartElement("item");
                        xml.WriteElementString("title", item.Title);
                        xml.WriteElementString("link", "http://ssss.com/" + item.ArticleType.NameAscii + "/" + item.TitleAscii + "-" + item.ID);
                        xml.WriteElementString("guid", "2fd4e1c67a2d28fced849ee1bb76e7391b93eb" + item.ID.ToString());
                        xml.WriteElementString("pubDate", item.DateCreated.HasValue ? item.DateCreated.ToString() : DateTime.Now.ToShortDateString());
                        xml.WriteElementString("author", "test");
                        xml.WriteElementString("description", item.Title);
                        xml.WriteElementString("content:encoded", item.Title + item.Title + item.Title + item.Title + item.Title + item.Title + item.Title + item.Title + item.Title + item.Title + item.Title + item.Title);
                        xml.WriteEndElement();
                    }
                    xml.WriteEndElement();
                    xml.WriteEndElement();
                    xml.WriteEndDocument();

                    xml.Flush();

                    xml.Close();
                }
            }
            var xm = new XmlDocument();
            xm.LoadXml(bob.ToString());

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(bob.ToString(), Encoding.UTF8, "text/xml"),
            };
            return response;

            

        }
        

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

        public string GetCData(ArticleItem item)
        {

            var str = "";
            str += "![CDATA[<!doctype html><html lang=\"en\" prefix=\"op: http://sdfsdfsd.com\"><head><meta charset=\"utf-8\"><link rel=\"canonical\" href=\"" + "http://babauonline.com/" + item.ArticleType.NameAscii + "/" + item.TitleAscii + "-" + item.ID + "\"><meta property=\"op:markup_version\" content=\"v1.0\"></head><body><article><header>" + item.Title + "</header>" + item.Detail + "<footer></footer></article></body></html>]]>";
            return str;
        }
    }

    public class RssItem
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public string Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string Link { get; set; }
    }

}