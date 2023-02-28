using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kztek.Web.Controllers
{
    public class NewsController : Controller
    {
       
        private INewsService _NewsService;
        private INewsCategoryService _NewsCategoryService;
        private ICustomerService _CustomerService;
        private ICustomerCommentService _CustomerCommentService;
        public NewsController(INewsService _NewsService, INewsCategoryService _NewsCategoryService, ICustomerCommentService _CustomerCommentService, ICustomerService _CustomerService)
        {
           
            this._NewsService = _NewsService;
            this._NewsCategoryService = _NewsCategoryService;
            this._CustomerService = _CustomerService;
            this._CustomerCommentService = _CustomerCommentService;
        }
        // GET: News
        public ActionResult Index(string newcategory = "", int? page = 1)
        {
            //Khai báo
            int pageNumber = (page ?? 1);
            int pageSize = 9;
            int totalitem = 0;

            var cateid = "";
            var caten = "";
            var catenurl = "";

            var metatitle = "";
            var metadesc = "";
            var metakeyword = "";

            var objCate = _NewsCategoryService.GetByName(newcategory);
            if (objCate != null)
            {
                cateid = objCate.Id;
                caten = objCate.Name;
                catenurl = objCate.NameUrl;

                metatitle = objCate.MetaTitle;
                metadesc = objCate.MetaDesc;
                metakeyword = objCate.MetaKeywork;
            }

         
            var list = _NewsService.GetPagingList(cateid,"3", pageNumber, pageSize,ref totalitem);

            var gridModel = PageModelCustom<News>.GetPage(list, pageNumber, pageSize, totalitem);

            var obj = new NewsView()
            {
                gridModel = gridModel,
                NewsCategoryName = caten,
                NewsCategoryNameUrl = catenurl,
                //ListCategory = _NewsCategoryService.GetAll().ToList(),

                MetaTitle = metatitle,
                MetaDesc = metadesc,
                MetaKeyword = metakeyword,
            };

            if (list.Count() == 1)
            {
                var fr = list.First();
                var ti = fr != null ? fr.NameUrl : "";
                return RedirectToAction("Detail", new { title = ti, newcategory = catenurl });
            }
            else
            {
                return View(obj);
            }
        }

        public ActionResult Detail(string title, string newcategory)
        {
            var newstitle = "";
            var newsdescn = "";
            var newsimage = "";

            var newscate = newcategory;
            var strCateIds = "";

            var metatitle = "";
            var metadesc = "";
            var metakeyword = "";

            var objNews = _NewsService.GetByTitleUrl(title);

            if (objNews != null)
            {
                newstitle = objNews.Name;
                newsdescn = objNews.Description;
                newsimage = objNews.CoverPath;

                metatitle = objNews.MetaTitle;
                metadesc = objNews.MetaDesc;
                metakeyword = objNews.MetaKeywork;

                if (objNews.TotalView == 0)
                {
                    objNews.TotalView = 1;
                }
                else
                {
                    objNews.TotalView++;
                }

                _NewsService.Update(objNews);
            }

            var newscategory = _NewsCategoryService.GetByName(newcategory);

            var listhot = _NewsService.GetLatestNewsByCategory(newscategory != null ? newscategory.Id : "", objNews != null ? objNews.Id : "","3");
          
            var obj = new ContentNews()
            {
                objNews = objNews,
                ListHot = listhot,
                ServiceTitle = newstitle,
                ServiceCategoryTitle = newscate,
                ServiceCategoryName = newscategory != null ? newscategory.Name : "",
                //ListCategory = _NewsCategoryService.GetAll().ToList(),

                MetaTitle = metatitle,
                MetaDesc = metadesc,
                MetaKeyword = metakeyword,

                OGUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri,
                OGType = "article",
                OGTitle = newstitle,
                OGDescription = newsdescn,
                OGImage = newsimage,               
        };


            return View(obj);
        }

        public PartialViewResult Partial_Comment(string id)
        {
            ViewBag.ID = id;
            return PartialView();
        }

        public JsonResult SendComment(ContactCustom obj)
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");
            var customer = new Customer
            {
                Id = Common.GenerateId(),
                DateCreated = DateTime.Now,
                Active = true,
                FullName = obj.FullName,
                Phone = obj.Phone,
                Email = obj.Email,
                Mobile = obj.Phone
            };

            result = _CustomerService.Create(customer);

            if (result.isSuccess)
            {
                var newmodel = new CustomerComment
                {
                    Id = Common.GenerateId(),
                    DateCreated = DateTime.Now,
                    Description = obj.Description,
                    Summary = obj.Id,
                    FullName = customer.FullName,
                    Active = true,
                    Type = "1",
                };

                result = _CustomerCommentService.Create(newmodel);
            }

            if (result.isSuccess)
            {
                result = new MessageReport(true, "Gửi thành công!");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}