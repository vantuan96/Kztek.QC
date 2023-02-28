using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Helpers;
using Kztek.Web.Core.Models;
using Kztek.Web.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Kztek.Web.Models;
using KMF.Web.Core.Models;

namespace Kztek.Web.Controllers
{
    public class HomeController : Controller
    {
        private IWebInfoService _WebInfoService;
        private IMainMenuService _MainMenuService;
        private IMediaService _MediaService;
        private IProductService _ProductService;
        private INewsService _NewsService;
        private INewsCategoryService _NewsCategoryService;
        private ICustomerService _CustomerService;
        private ICustomerCommentService _CustomerCommentService;
        private IContactService _ContactService;
        public HomeController( IWebInfoService _WebInfoService, IMainMenuService _MainMenuService, IMediaService _MediaService, IProductService _ProductService, INewsService _NewsService, ICustomerService _CustomerService, ICustomerCommentService _CustomerCommentService, IContactService _ContactService, INewsCategoryService _NewsCategoryService)
        {
            this._MainMenuService = _MainMenuService;
            this._WebInfoService = _WebInfoService;
            this._MediaService = _MediaService;
            this._ProductService = _ProductService;
            this._NewsService = _NewsService;
            this._CustomerService = _CustomerService;
            this._CustomerCommentService = _CustomerCommentService;
            this._ContactService = _ContactService;
            this._NewsCategoryService = _NewsCategoryService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult Header()
        {
            ViewBag.Webconfig = _WebInfoService.GetDefault();
            var listmenu = _MainMenuService.GetAllActive().ToList();
            return PartialView(listmenu);
        }
        public PartialViewResult Banner()
        {
            var list = _MediaService.GetBanner(PageConfig.Home,PositionConfig.Top,"0").ToList();
            return PartialView(list);
        }
        public PartialViewResult Product()
        {
            var list = _ProductService.GetAllActive().ToList();
            return PartialView(list);
        }
        public PartialViewResult Service()
        {
            var list = _NewsService.GetNewsServiceHome();
            return PartialView(list);
        }
        public PartialViewResult Choose()
        {
            return PartialView();
        }
        public PartialViewResult Work()
        {
            var list = _MediaService.GetImageHome();        
            return PartialView(list);
        }
        public PartialViewResult Expert()
        {
            ViewBag.Comment = _CustomerCommentService.GetCommentHome();
            var listCustomer = _CustomerService.GetHome().ToList();
            return PartialView(listCustomer);
        }
        public PartialViewResult Blog()
        {
            var list = _NewsService.GetLatestNews();
            ViewBag.ListCate = _NewsCategoryService.GetAllActive().ToList();
            return PartialView(list);
        }
        public PartialViewResult Contact()
        {
            return PartialView();
        }
        public PartialViewResult ModalContact()
        {
            return PartialView();
        }
        public PartialViewResult Footer()
        {
            var webconfig = _WebInfoService.GetDefault();
            return PartialView(webconfig);
        }

        public PartialViewResult Child(List<MainMenu> listChild, List<MainMenu> allFunction, string group)
        {
            ViewBag.allList = allFunction;
            return PartialView(listChild);
        }

        public JsonResult Send(ContactCustom obj)
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
                var newmodel = new Contact
                {
                    Id = Common.GenerateId(),
                    DateCreated = DateTime.Now,
                    Description = obj.Description,
                    CustomerId = customer.Id  ,
                    IPCustomer = customer.Id
                };
              
                result = _ContactService.Create(newmodel);
            }
          
            if (result.isSuccess)
            {
                result = new MessageReport(true, "Gửi thành công!");          
            }           
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ActiveMenu(string url)
        {
            var data = "";
            var _url = System.Web.HttpContext.Current.Request.UrlReferrer.Segments;

            if (_url.Length > 2)
            {
                data = _url[2];
            }else if(_url.Length == 2)
            {
                data = _url[1];
            }

            return Json(data.Split('/'), JsonRequestBehavior.AllowGet);
        }
    }
}