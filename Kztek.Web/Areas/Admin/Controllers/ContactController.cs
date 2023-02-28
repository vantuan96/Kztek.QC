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
	public class ContactController : Controller
	{
		private IContactService _ContactService;
		public ContactController(IContactService _ContactService)
		{
			this._ContactService = _ContactService;

		}
		[CheckSessionLogin]
		[CheckAuthorize]
		public ActionResult Index(string key,string filter, int page = 1, int pageSize = 20)
		{

			var model = new ListPagingModel
			{
				keyword = key,
				page = page,
				pageSize = pageSize,
				filter = filter
			};
			ViewBag.ParamHeader = model;
			TempData["url"] = Request.Url.PathAndQuery;
			return View(new ContactView());
		}
		public PartialViewResult Contact_PagingList(ListPagingModel model)
		{
			var _total = 0;
			var list = _ContactService.GetAllPagingListTSQL(model.keyword, model.filter, model.page, model.pageSize, ref _total);
			var gridModel = PageModelCustom<ContactView>.GetPage(list, model.page, model.pageSize, _total);
			ViewBag.Key = model.keyword;
			return PartialView(gridModel); 
		}
		[HttpGet]
		[CheckSessionLogin]
		[CheckAuthorize]
		public ActionResult Create()
		{
			ViewBag.urlValue = (string)TempData["url"] ?? Url.Action("Index", "Contact");


			return View();
		}
		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Create(Contact obj , string TypeSelect, bool SaveAndCountinue = false)
		{


			//if (string.IsNullOrWhiteSpace(obj.Name))
			//{
			//    ModelState.AddModelError("Name", "Vui lòng nhập tên");
			//    return View(obj);
			//}
			if(ModelState.IsValid)
			{
			obj.Id = Common.GenerateId();
			obj.DateCreated = DateTime.Now;
				var report = _ContactService.Create(obj);
				if(report.isSuccess)
				{
					if(SaveAndCountinue)
					{
						TempData["Success"] = "Thêm mới thành công";
						return RedirectToAction("Create", "Contact", new { controllername = "ContactController" });
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
			var obj = _ContactService.GetById(id);
			ViewBag.urlValue = (string)TempData["url"] ?? Url.Action("Index", "Contact");


			return View(obj);
		}
		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Update(Contact obj , string TypeSelect, string urlValue = "")
		{


			//if (string.IsNullOrWhiteSpace(obj.Name))
			//{
			//    ModelState.AddModelError("Name", "Vui lòng nhập tên");
			//    return View(obj);
			//}
			if (ModelState.IsValid)
			{
				var oldobj = _ContactService.GetById(obj.Id);
				if (oldobj == null) return View(obj);
				oldobj.CustomerId = obj.CustomerId;
				oldobj.Description = obj.Description;
				oldobj.IPCustomer = obj.IPCustomer;
				oldobj.DateCreated = obj.DateCreated;
				var report = _ContactService.Update(oldobj);
				if (report.isSuccess)
				{
					urlValue = urlValue ?? Url.Action("Index", "Contact");
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
			var report = _ContactService.DeleteById(id);
			return Json(report, JsonRequestBehavior.AllowGet);
		}
		public JsonResult MultiDelete(string lstId)
		{
			var result = _ContactService.DeleteByIds(lstId);
			return Json(result, JsonRequestBehavior.AllowGet);
		}
	}
}
