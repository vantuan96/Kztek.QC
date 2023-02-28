/* **************************************
* HỆ THỐNG GENCODE TỰ ĐỘNG
* CREATE: 04/06/2019 3:12:27 PM
* AUTHOR: HNG-0988388000
*/
using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Web.Attributes;
using Kztek.Web.Core.Functions;
using System.Collections.Generic;
using System.Web;
using System;
using System.Web.Mvc;
using Kztek.Web.Core.Helpers;
using System.Linq;
using Kztek.Model.CustomModel;
using System.Configuration;
namespace Kztek.Web.Areas.Admin.Controllers
{
	public class MediaController : Controller
	{
		private IMediaService _MediaService;
		public MediaController(IMediaService _MediaService)
		{
			this._MediaService = _MediaService;

		}
		[CheckSessionLogin]
		[CheckAuthorize]
		public ActionResult Index(string key,string filter, int page = 1,string mediaPage = "", string position ="", int pageSize = 20)
		{

			var model = new ListPagingModel
			{
				keyword = key,
				page = page,
				pageSize = pageSize,
				filter = filter
			};
            ViewBag.PositionSelectLst = FunctionHelper.PositionModel();
            ViewBag.PageSelectLst = FunctionHelper.PageModel();
            ViewBag.ParamHeader = model;
			ViewBag.mediaPage = mediaPage;
			ViewBag.position = position;

			TempData["url"] = Request.Url.PathAndQuery;
			return View(new Media());
		}
		public PartialViewResult Media_PagingList(ListPagingModel model,string mediaPage, string position)
		{
			var _total = 0;
			var list = _MediaService.GetAllPagingListTSQL(model.keyword, mediaPage, position, model.filter, model.page, model.pageSize, ref _total);
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (!String.IsNullOrEmpty(item.Path))
                        item.Path = "<img src = '" + item.Path + "' style = 'max-width:110px;max-height:110px;' />";
                    else
                        item.Path = "<img src = '/Content/Image/default.jpg' style = 'max-width:110px;max-height:110px;' />";
                }
            }
            
			var gridModel = PageModelCustom<Media>.GetPage(list, model.page, model.pageSize, _total);
			ViewBag.Key = model.keyword;
            ViewBag.mediaPage = mediaPage;
            ViewBag.position = position;
            return PartialView(gridModel); 
		}
		[HttpGet]
		[CheckSessionLogin]
		[CheckAuthorize]
		public ActionResult Create()
		{
			ViewBag.urlValue = (string)TempData["url"] ?? Url.Action("Index", "Media");
            ViewBag.Positions = FunctionHelper.PositionModel();
            ViewBag.Page = FunctionHelper.PageModel();
            ViewBag.Type = FunctionHelper.MediaType();
            return View();
		}
		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Create(Media obj , HttpPostedFileBase PathFileUpload, string TypeSelect, bool SaveAndCountinue = false)
		{
            ViewBag.Positions = FunctionHelper.PositionModel();
            ViewBag.Page = FunctionHelper.PageModel();
            ViewBag.Type = FunctionHelper.MediaType();

            //if (string.IsNullOrWhiteSpace(obj.Name))
            //{
            //    ModelState.AddModelError("Name", "Vui lòng nhập tên");
            //    return View(obj);
            //}
            if (ModelState.IsValid)
			{
			obj.Id = Common.GenerateId();
			obj.DateCreated = DateTime.Now;
                obj.TotalView = 0;
			string error = "";
			string fullfolder = ConfigurationManager.AppSettings["IconFileUpload"];
			if (PathFileUpload != null)
			{
				var name = Common.UploadFile(out error, Server.MapPath(fullfolder), PathFileUpload);
				if (string.IsNullOrWhiteSpace(error))
				{
					obj.Path = string.Format("{0}{1}", fullfolder, name);
				}
			}
				var report = _MediaService.Create(obj);
				if(report.isSuccess)
				{
					if(SaveAndCountinue)
					{
						TempData["Success"] = "Thêm mới thành công";
						return RedirectToAction("Create", "Media", new { controllername = "MediaController" });
					}
					else
					{
						return RedirectToAction("Index");
					}
				}
				else
				{
					ModelState.AddModelError("", "Tạo dữ liệu không thành công. Hãy kiểm tra lại dữ liệu nhập vào");
					return View();
				}
				}
			else
			{
			ModelState.AddModelError("", "Tạo dữ liệu không thành công. Hãy kiểm tra lại dữ liệu nhập vào");
			return View();
			}
		}
		[HttpGet]
		[CheckSessionLogin]
		[CheckAuthorize]
		public ActionResult Update(string id)
		{
			var obj = _MediaService.GetById(id);
			ViewBag.urlValue = (string)TempData["url"] ?? Url.Action("Index", "Media");
            ViewBag.Positions = FunctionHelper.PositionModel();
            ViewBag.Page = FunctionHelper.PageModel();
            ViewBag.Type = FunctionHelper.MediaType();

            return View(obj);
		}
		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Update(Media obj , HttpPostedFileBase PathFileUpload, string TypeSelect, string urlValue = "")
		{

            ViewBag.Positions = FunctionHelper.PositionModel();
            ViewBag.Page = FunctionHelper.PageModel();
            ViewBag.Type = FunctionHelper.MediaType();
            //if (string.IsNullOrWhiteSpace(obj.Name))
            //{
            //    ModelState.AddModelError("Name", "Vui lòng nhập tên");
            //    return View(obj);
            //}
            if (ModelState.IsValid)
			{
				var oldobj = _MediaService.GetById(obj.Id);
				if (oldobj == null) return View(obj);
				oldobj.Name = obj.Name;
				oldobj.Description = obj.Description;
				oldobj.Alt = obj.Alt;
				oldobj.Url = obj.Url;
				oldobj.Path = obj.Path;
				oldobj.Page = obj.Page;
				oldobj.Position = obj.Position;
				oldobj.MediaType = obj.MediaType;
				oldobj.Ordering = obj.Ordering;
				oldobj.DateCreated = obj.DateCreated;
				oldobj.Active = obj.Active;
                
			string error = "";
			string fullfolder = ConfigurationManager.AppSettings["IconFileUpload"];
			if (PathFileUpload != null)
			{
				var name = Common.UploadFile(out error, Server.MapPath(fullfolder), PathFileUpload);
				if (string.IsNullOrWhiteSpace(error))
				{
					oldobj.Path = string.Format("{0}{1}", fullfolder, name);
				}
			}
				var report = _MediaService.Update(oldobj);
				if (report.isSuccess)
				{
					urlValue = urlValue ?? Url.Action("Index", "Media");
					return Redirect(urlValue);
				}
				else
				{
					ModelState.AddModelError("", "Tạo dữ liệu không thành công. Hãy kiểm tra lại dữ liệu nhập vào");
					return View(obj);
				}
			}
			return View(obj);
		}
		[CheckAuthorize]
		public JsonResult Delete(string id)
		{
			var report = _MediaService.DeleteById(id);
			return Json(report, JsonRequestBehavior.AllowGet);
		}
		public JsonResult MultiDelete(string lstId)
		{
			var result = _MediaService.DeleteByIds(lstId);
			return Json(result, JsonRequestBehavior.AllowGet);
		}
	}
}
