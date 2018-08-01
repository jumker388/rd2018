using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using HtmlAgilityPack;
using OfficeOpenXml;
using Web365Utility;
using Web365Base;
using Web365Business.Back_End.IRepository;
using Web365Domain;
using System;
using Newtonsoft.Json.Linq;
namespace Web365.Areas.Admin.Controllers
{
    public class AutoArticleController : BaseController
    {
        private readonly ITypeArticleAutoRepository typeArticleAutoRepository;
        private IAutoArticleRepository autoArticleRepository;
        private IArticleRepository articleRepository;
        private IPictureRepository pictureRepository;
        private IArticleGroupRepository articleGroupRepository;
        private IArticleTypeRepository articleTypeRepository;
        private IArticleGroupMapRepository articleGroupMapRepository;

        // GET: /Admin/ProductType/

        public AutoArticleController(ITypeArticleAutoRepository _typeArticleAutoRepository, IAutoArticleRepository autoArticleRepository, IArticleRepository articleRepository, IPictureRepository pictureRepository,
            IArticleGroupRepository _articleGroupRepository,
            IArticleTypeRepository _articleTypeRepository,
            IArticleGroupMapRepository _articleGroupMapRepository)
        {
            this.typeArticleAutoRepository = _typeArticleAutoRepository;
            this.baseRepository = autoArticleRepository;
            this.pictureRepository = pictureRepository;
            this.autoArticleRepository = autoArticleRepository;
            this.articleRepository = articleRepository;
            this.articleGroupRepository = _articleGroupRepository;
            this.articleTypeRepository = _articleTypeRepository;
            this.articleGroupMapRepository = _articleGroupMapRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetList(string name, int? typeId, int? groupId, int currentRecord, int numberRecord, string propertyNameSort, bool descending)
        {
            var total = 0;
            var list = autoArticleRepository.GetList(out total, name, typeId, groupId, currentRecord, numberRecord, propertyNameSort, descending);

            return Json(new
            {
                total = total,
                list = list
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetPropertyFilter()
        {
            var listArticleType = articleTypeRepository.GetListForTree<object>();

            var listArticleGroup = articleGroupRepository.GetListForTree<object>();

            return Json(new
            {
                listType = listArticleType,
                listGroup = listArticleGroup
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditForm(int? id)
        {
            var obj = new ArticleItem();

            var listArticleType = articleTypeRepository.GetListForTree<object>();

            var listArticleGroup = articleGroupRepository.GetListForTree<object>();

            if (id.HasValue)
                obj = autoArticleRepository.GetItemById<ArticleItem>(id.Value);

            return Json(new
            {
                data = obj,
                listType = listArticleType,
                listGroup = listArticleGroup
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ExcelForm(int? id)
        {
            var obj = new ArticleItem();

            var listArticleType = articleTypeRepository.GetListForTree<object>();

            var listArticleGroup = articleGroupRepository.GetListForTree<object>();

            if (id.HasValue)
                obj = autoArticleRepository.GetItemById<ArticleItem>(id.Value);

            return Json(new
            {
                data = obj,
                listType = listArticleType,
                listGroup = listArticleGroup
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddAutoNewsForm(int? id)
        {
            var obj = new ArticleItem();

            var listArticleType = articleTypeRepository.GetListForTree<object>();

            var listArticleGroup = articleGroupRepository.GetListForTree<object>();

            var listAutoArticleType = autoArticleRepository.GetListForTree<object>();

            return Json(new
            {
                data = obj,
                listType = listArticleType,
                listGroup = listArticleGroup,
                listAutoType = listAutoArticleType
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Action(tblArticle objSubmit)
        {

            if (objSubmit.ID == 0)
            {
                objSubmit.DateCreated = DateTime.Now;
                objSubmit.DateUpdated = DateTime.Now;
                objSubmit.IsDeleted = false;
                objSubmit.IsShow = true;
                autoArticleRepository.Add(objSubmit);

            }
            else
            {
                var obj = autoArticleRepository.GetById<tblArticle>(objSubmit.ID);

                UpdateModel(obj);

                objSubmit.DateUpdated = DateTime.Now;

                autoArticleRepository.Update(obj);
            }

            articleGroupMapRepository.ResetGroupOfNews(objSubmit.ID, Web365Utility.Web365Utility.StringToArrayInt(Request["groupID"]));

            return Json(new
            {
                Error = false

            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddAutoNews(int TypeAuto, int TypeID, string groupID)
        {
            RunCrawler(TypeAuto, TypeID, groupID);
            return Json(new
            {
                Error = false

            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddAutoAll()
        {
            var lstTypeAuto = autoArticleRepository.GetAllTypeAutoArticle();
            foreach (var articleAutoItem in lstTypeAuto)
            {
                // 139 nhom tin tu dong
                RunCrawler(articleAutoItem.ID, articleAutoItem.TypeID.HasValue ? articleAutoItem.TypeID.Value : 3329, articleAutoItem.ListGroupArticle);
            }
            return Json(new
            {
                Error = false

            }, JsonRequestBehavior.AllowGet);
        }

        public void RunCrawler(int TypeAuto, int TypeID, string groupID)
        {
            var objTypeAuto = autoArticleRepository.GetTypeAutoArticle(TypeAuto);

            if (objTypeAuto != null && !string.IsNullOrEmpty(objTypeAuto.Link) && !string.IsNullOrEmpty(objTypeAuto.Xpath) && !string.IsNullOrEmpty(objTypeAuto.XpathTitle))
            {
                if (!string.IsNullOrEmpty(objTypeAuto.Xpage) && objTypeAuto.Link.Contains("(so-trang)"))
                {
                    var lstPage = SplitPage(objTypeAuto.Xpage);
                    if (lstPage == null)
                    {
                        ReadLink(objTypeAuto, objTypeAuto.Link, TypeID, groupID);
                    }
                    else
                    {
                        for (var i = lstPage.FirstOrDefault(); i <= lstPage.LastOrDefault(); i++)
                        {
                            var checkRead = ReadLink(objTypeAuto, objTypeAuto.Link.Replace("(so-trang)", i.ToString()), TypeID, groupID, i);
                            if (checkRead == false)
                            {
                                break;
                            }
                        }
                    }

                }
                else
                {

                    ReadLink(objTypeAuto, objTypeAuto.Link, TypeID, groupID);
                }
            }
        }

        public int SaveImage(string link)
        {
            // file name ex: image1.jpg
            var fileName = Path.GetFileName(link);
            // file extend
            var exts = Path.GetExtension(link);
            using (var client = new WebClient())
            {
                var fileNameDownload = DateTime.Now.Ticks.ToString() + "_" + fileName;
                client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                client.DownloadFile(link, HttpContext.Server.MapPath(ConfigWeb.ImageThumpPath + fileNameDownload));
                System.IO.File.Copy(HttpContext.Server.MapPath(ConfigWeb.ImageThumpPath + fileNameDownload), HttpContext.Server.MapPath(ConfigWeb.ImagePath + fileNameDownload));

                var picture = new tblPicture
                {
                    FileName = fileNameDownload,
                    CreatedBy = string.Empty,
                    UpdatedBy = string.Empty,
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    IsShow = true,
                    IsDeleted = false
                };

                pictureRepository.Add(picture);

                return picture.ID;
            }
        }
        [HttpGet]
        public ActionResult GetKeyAuto(bool? isCrawler)
        {
            var webConfigApp = WebConfigurationManager.OpenWebConfiguration("~");
            var obj = new ArticleItem { IsShow = Convert.ToBoolean(webConfigApp.AppSettings.Settings["IsCrawler"].Value) };
            return Json(new
            {
                data = obj,
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateKeyAuto(tblArticle objSubmit)
        {
            //if (objSubmit.IsShow.HasValue)
            //{
            //    var webConfigApp = WebConfigurationManager.OpenWebConfiguration("~");
            //    //Modifying the AppKey from AppValue to IsCrawler
            //    webConfigApp.AppSettings.Settings["IsCrawler"].Value = objSubmit.IsShow.Value.ToString();
            //    //Save the Modified settings of AppSettings.
            //    webConfigApp.Save();
            //}
            return Json(new
            {
                Error = false

            }, JsonRequestBehavior.AllowGet);
        }

        public void WriteLogs(string log)
        {
            // Write the string to a file.
            var file = new StreamWriter(ConfigWeb.FolderLogsTxt + "Logs.txt", true);
            file.WriteLine(log + " - DataCreate : " + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString());
            file.Close();
        }
        public void JobGetNews()
        {
            AddAutoAll();
            //if (ConfigWeb.IsCrawler)
            //{
            //    Task.Factory.StartNew(Crawler);
            //    AddAutoAll();
            //}
        }

        public void Crawler()
        {
            //Thread.Sleep(60000000);
            AddAutoAll();
            //Crawler();
        }

        public bool Compute(List<ArticleItem> list, string s1A)
        {
            var listA = s1A.Split('-').GroupBy(word => word).OrderByDescending(group => group.Count()).Select(group => group.Key).ToList();
            foreach (var item in list)
            {
                var listB = item.TitleAscii.Split('-').GroupBy(word => word).OrderByDescending(group => group.Count()).Select(group => group.Key).ToList();

                var lenghtMax = s1A.Length > item.TitleAscii.Length ? s1A.Length : item.TitleAscii.Length;
                var listMax = s1A.Length == lenghtMax ? listA : listB;
                var listMin = s1A.Length == lenghtMax ? listB : listA;


                var check = (listMax).Count(listMin.Contains);

                if (check * 100 / listMax.Count() >= 80)
                {
                    return false;
                }
            }
            return true;
        }

        public bool ReadLink(ArticleAutoItem objTypeAuto, string linkRead, int TypeID, string groupID, int page = 1)
        {
            try
            {
                var linkImage = string.Empty;
                var url = new Uri(objTypeAuto.Link);
                var web = new HtmlWeb();
                // load link
                var doc = web.Load(linkRead);

                var aTags = doc.DocumentNode.SelectSingleNode(page > 1 ? objTypeAuto.XpathPaging : objTypeAuto.Xpath);

                if (aTags != null)
                {
                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(aTags.InnerHtml);
                    // get tags a of xpath
                    var listA = htmlDoc.DocumentNode.SelectNodes(".//a");

                    if (listA != null)
                    {
                        // check link content
                        foreach (var item in listA.Where(t => t.Attributes["href"] != null))
                        {
                            try
                            {
                                var link = item.Attributes["href"].Value;
                                if (!item.Attributes["href"].Value.Contains(url.Host))
                                {
                                    link = url.Scheme + "://" + url.Host + link;
                                }
                                var docDetail = web.Load(link);
                                var htmlDocDetail = new HtmlDocument();
                                htmlDocDetail.LoadHtml(item.InnerHtml);

                                // get image default news if image exits
                                var img = htmlDocDetail.DocumentNode.SelectNodes(".//img");
                                if (img != null)
                                {
                                    // image src
                                    var imgDetail = img.FirstOrDefault().Attributes["src"].Value;
                                    if (!imgDetail.Contains(url.Scheme + "//") && !imgDetail.Contains("https://") && !imgDetail.Contains("http://"))
                                    {
                                        imgDetail = url.Scheme + "://" + url.Host + imgDetail;
                                    }
                                    linkImage = imgDetail;
                                }
                                var linkRefer = link;
                                if (docDetail.DocumentNode.SelectNodes(objTypeAuto.XpathTitle) == null) continue;

                                var title = docDetail.DocumentNode.SelectNodes(objTypeAuto.XpathTitle)[0].InnerHtml.Trim();
                                var titleAscii = Web365Utility.Web365Utility.ConvertToAscii(title);
                                var objExits = articleRepository.GetItemByTitleAscii(titleAscii);
                                if (objExits != null)
                                {
                                    if (string.IsNullOrEmpty(objExits.LinkReferenPicture) && !string.IsNullOrEmpty(linkImage))
                                    {
                                        objExits.LinkReferenPicture = linkImage;
                                        articleRepository.Update(objExits);
                                    }
                                    continue;
                                }
                                string summary;
                                try
                                {
                                    if (string.IsNullOrEmpty(objTypeAuto.XpathSummary))
                                    {
                                        summary = string.Empty;
                                    }
                                    else
                                    {
                                        summary = Web365Utility.Web365Utility.RemoveStyleInHtmlTag(docDetail.DocumentNode.SelectNodes(objTypeAuto.XpathSummary)[0].InnerHtml);
                                    }
                                }
                                catch (Exception e)
                                {
                                    summary = string.Empty;
                                    WriteLogs("Error read Sumary " + item.Attributes["href"].Value + "AutoID : " + objTypeAuto.ID);
                                }
                                string detail;
                                try
                                {
                                    if (string.IsNullOrEmpty(objTypeAuto.XpathDetail))
                                    {
                                        detail = string.Empty;
                                    }
                                    else
                                    {
                                        detail = Web365Utility.Web365Utility.RemoveStyleInHtmlTag(docDetail.DocumentNode.SelectNodes(objTypeAuto.XpathDetail)[0].InnerHtml);
                                        if (string.IsNullOrEmpty(linkImage))
                                        {
                                            try
                                            {
                                                htmlDocDetail.LoadHtml(detail);
                                            }
                                            catch (Exception ex)
                                            {
                                                WriteLogs("Error read Detail" + item.Attributes["href"].Value + "AutoID : " + objTypeAuto.ID);
                                            }

                                            // get image default news if image exits
                                            var imgDetailContent = htmlDocDetail.DocumentNode.SelectNodes(".//img");
                                            if (imgDetailContent != null)
                                            {
                                                try
                                                {
                                                    // image src details
                                                    var imgDetailSrc = imgDetailContent.FirstOrDefault().Attributes["src"].Value;
                                                    if (!imgDetailSrc.Contains(url.Scheme + "//") && !imgDetailSrc.Contains("https://") && !imgDetailSrc.Contains("http://"))
                                                    {
                                                        imgDetailSrc = url.Scheme + "://" + url.Host + imgDetailSrc;
                                                    }
                                                    linkImage = imgDetailSrc;
                                                }
                                                catch (Exception ex)
                                                {
                                                    WriteLogs("Error read images Detail" + item.Attributes["href"].Value + "AutoID : " + objTypeAuto.ID);
                                                }

                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    WriteLogs("Error " + item.Attributes["href"].Value + "AutoID : " + objTypeAuto.ID);
                                    detail = string.Empty;
                                }
                                var objNews = new tblArticle
                                                  {
                                                      DateCreated = DateTime.Now,
                                                      DateUpdated = DateTime.Now,
                                                      SEOTitle = title,
                                                      SEOKeyword = title,
                                                      SEODescription = title,
                                                      Number = 0,
                                                      TypeID = TypeID,
                                                      IsDeleted = false,
                                                      IsShow = true,
                                                      Title = title,
                                                      TitleAscii = titleAscii,
                                                      Summary = summary,
                                                      Detail = detail,
                                                      LinkReferenPicture = linkImage,
                                                      LinkReference = linkRefer
                                                  };

                                autoArticleRepository.Add(objNews);
                                articleGroupMapRepository.ResetGroupOfNews(objNews.ID, Web365Utility.Web365Utility.StringToArrayInt(groupID));
                            }
                            catch (Exception ex)
                            {
                                WriteLogs("Error " + item.Attributes["href"].Value + "AutoID : " + objTypeAuto.ID);
                                Elmah.ErrorLog.GetDefault(System.Web.HttpContext.Current).Log(new Elmah.Error(ex));
                            }
                        }
                    }
                }
                else
                {
                    WriteLogs("Can't read tags <a> with link " + linkRead + "AutoID : " + objTypeAuto.ID);
                    return false;
                }
            }
            catch (Exception)
            {
                WriteLogs("Error " + linkRead + "AutoID : " + objTypeAuto.ID);
                return false;
            }

            return true;
        }

        private List<int> SplitPage(string page)
        {
            // page ex : 1-200
            try
            {
                return page.Split('-').Select(int.Parse).ToList();
            }
            catch (Exception)
            {
                //var primes = new List<int>(new[] { 1, 1 });
                return null;
            }

        }

        // Insert ProductCampaign Form Excel
        [HttpPost]
        public ActionResult InsertByExcel(object data)
        {
            var file = Request["file"];
            if (System.IO.File.Exists(Server.MapPath("~/UploadFile/Files/temLink.xlsx")))
            {
                System.IO.File.Delete(Server.MapPath("~/UploadFile/Files/temLink.xlsx"));
            }
            System.IO.File.Move(Server.MapPath(ConfigWeb.TempPath + file), Server.MapPath("~/UploadFile/Files/temLink.xlsx"));

            autoArticleRepository.HideAll();


            var fileName = Server.MapPath("~/UploadFile/Files/temLink.xlsx");
            const int startRow = 1;
            var existingFile = new FileInfo(fileName);
            // Open and read the XlSX file.
            using (var package = new ExcelPackage(existingFile))
            {
                // Get the work book in the file
                ExcelWorkbook workBook = package.Workbook;
                if (workBook != null)
                {
                    if (workBook.Worksheets.Count > 0)
                    {
                        ExcelWorksheet currentWorksheet = workBook.Worksheets.First();
                        for (int i = startRow + 1; i <= currentWorksheet.Dimension.End.Row; i++)
                        {
                            if (currentWorksheet.Cells[i, 2].Value != null)
                            {
                                var typeAuto = new tblArticleAuto
                                                   {
                                                       Name = currentWorksheet.Cells[i, 1].Value.ToString(),
                                                       Link = currentWorksheet.Cells[i, 2].Value.ToString(),
                                                       Xpath = currentWorksheet.Cells[i, 3].Value.ToString(),
                                                       XpathTitle = currentWorksheet.Cells[i, 4].Value.ToString(),
                                                       XpathSummary = currentWorksheet.Cells[i, 5].Value != null ? currentWorksheet.Cells[i, 5].Value.ToString() : string.Empty,
                                                       XpathDetail = currentWorksheet.Cells[i, 6].Value != null ? currentWorksheet.Cells[i, 6].Value.ToString() : string.Empty,
                                                       XpathImage = currentWorksheet.Cells[i, 7].Value != null ? currentWorksheet.Cells[i, 7].Value.ToString() : string.Empty,
                                                       XpathPaging = currentWorksheet.Cells[i, 8].Value != null ? currentWorksheet.Cells[i, 8].Value.ToString() : string.Empty,
                                                       TypeID =
                                                           Convert.ToInt32(currentWorksheet.Cells[i, 9].Value != null ? currentWorksheet.Cells[i, 9].Value.ToString() : "3329"),
                                                       Xpage = currentWorksheet.Cells[i, 10].Value != null ? currentWorksheet.Cells[i, 10].Value.ToString() : string.Empty,
                                                       ListGroupArticle = currentWorksheet.Cells[i, 11].Value != null ? currentWorksheet.Cells[i, 11].Value.ToString() : string.Empty,
                                                       IsDeleted = false,
                                                       IsShow = true
                                                   };

                                typeArticleAutoRepository.Add(typeAuto);

                            }

                        }
                    }
                }
            }
            return Content(string.Empty);

        }

    }
}
