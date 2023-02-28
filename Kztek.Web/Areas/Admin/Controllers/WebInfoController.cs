using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Web.Attributes;
using Kztek.Web.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kztek.Web.Areas.Admin.Controllers
{
    public class WebInfoController : Controller
    {
        private IWebInfoService _WebInfoService;

        public WebInfoController(IWebInfoService _WebInfoService)
        {
            this._WebInfoService = _WebInfoService;
        }

        [HttpGet]
        [CheckSessionLogin]
        public ActionResult Index()
        {
            var obj = _WebInfoService.GetDefault();

            return View(obj);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(WebInfo obj)
        {
            //Gọi lại model đó
            var oldObj = _WebInfoService.GetDefault();
            if (oldObj != null)
            {
                oldObj.AnalyticsCode = obj.AnalyticsCode;
                oldObj.ChatCode = obj.ChatCode;
                oldObj.CompanyInfo = obj.CompanyInfo;
                oldObj.EmailBcc = obj.EmailBcc;
                oldObj.EmailCC = obj.EmailCC;
                oldObj.EmailPassSystem = obj.EmailPassSystem;
                oldObj.EmailSystem = obj.EmailSystem;
                oldObj.FaceBookCode = obj.FaceBookCode;
                oldObj.FanpageUrl = obj.FanpageUrl;
                oldObj.LinkGoogleMap = obj.LinkGoogleMap;
                oldObj.LogoPath = obj.LogoPath;
                oldObj.LogoUrl = obj.LogoUrl;
                oldObj.MasterToolCode = obj.MasterToolCode;
                oldObj.MetaDesc = obj.MetaDesc;
                oldObj.MetaKeywork = obj.MetaKeywork;
                oldObj.MetaTitle = obj.MetaTitle;
                oldObj.PagingBackEnd = obj.PagingBackEnd;
                oldObj.PagingFontEnd = obj.PagingFontEnd;
                oldObj.WebsiteName = obj.WebsiteName;
                oldObj.Phone = obj.Phone;
                oldObj.Footer = obj.Footer;

                var report = _WebInfoService.Update(oldObj);
                if (report.isSuccess)
                {
                    TempData["Success"] = report.Message;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = report.Message;
                    return View(oldObj);
                }
            }
            else
            {
                ViewBag.Message = "Bản ghi không tồn tại";
                return View(obj);
            }
        }
    }
}