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
	public class CustomerCommentController : Controller
	{
		private ICustomerCommentService _CustomerCommentService;
		public CustomerCommentController(ICustomerCommentService _CustomerCommentService)
		{
			this._CustomerCommentService = _CustomerCommentService;

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
			return View(new CustomerComment());
		}
		public PartialViewResult CustomerComment_PagingList(ListPagingModel model)
		{
			var _total = 0;
			var list = _CustomerCommentService.GetAllPagingListTSQL(model.keyword, model.filter, model.page, model.pageSize, ref _total);
			var gridModel = PageModelCustom<CustomerComment>.GetPage(list, model.page, model.pageSize, _total);
			ViewBag.Key = model.keyword;
			return PartialView(gridModel); 
		}
		[HttpGet]
		[CheckSessionLogin]
		[CheckAuthorize]
		public ActionResult Create()
		{
			ViewBag.urlValue = (string)TempData["url"] ?? Url.Action("Index", "CustomerComment");
            ViewBag.Type = FunctionHelper.TypeComment();


            return View();
		}
		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Create(CustomerComment obj , string TypeSelect, bool SaveAndCountinue = false)
		{
            ViewBag.Type = FunctionHelper.TypeComment();

            //if (string.IsNullOrWhiteSpace(obj.Name))
            //{
            //    ModelState.AddModelError("Name", "Vui lòng nhập tên");
            //    return View(obj);
            //}
            if (ModelState.IsValid)
			{
			obj.Id = Common.GenerateId();
			obj.DateCreated = DateTime.Now;
				var report = _CustomerCommentService.Create(obj);
				if(report.isSuccess)
				{
					if(SaveAndCountinue)
					{
						TempData["Success"] = "Thêm mới thành công";
						return RedirectToAction("Create", "CustomerComment", new { controllername = "CustomerCommentController" });
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
			var obj = _CustomerCommentService.GetById(id);
			ViewBag.urlValue = (string)TempData["url"] ?? Url.Action("Index", "CustomerComment");
            ViewBag.Type = FunctionHelper.TypeComment();

            return View(obj);
		}
		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Update(CustomerComment obj , string TypeSelect, string urlValue = "")
		{
            ViewBag.Type = FunctionHelper.TypeComment();

            //if (string.IsNullOrWhiteSpace(obj.Name))
            //{
            //    ModelState.AddModelError("Name", "Vui lòng nhập tên");
            //    return View(obj);
            //}
            if (ModelState.IsValid)
			{
				var oldobj = _CustomerCommentService.GetById(obj.Id);
				if (oldobj == null) return View(obj);
				oldobj.FullName = obj.FullName;
				oldobj.Avartar = obj.Avartar;
				oldobj.Summary = obj.Summary;
				oldobj.Description = obj.Description;
				oldobj.Ordering = obj.Ordering;
				oldobj.DateCreated = obj.DateCreated;
				oldobj.Active = obj.Active;
                oldobj.Type = obj.Type;
				var report = _CustomerCommentService.Update(oldobj);
				if (report.isSuccess)
				{
					urlValue = urlValue ?? Url.Action("Index", "CustomerComment");
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
			var report = _CustomerCommentService.DeleteById(id);
			return Json(report, JsonRequestBehavior.AllowGet);
		}
		public JsonResult MultiDelete(string lstId)
		{
			var result = _CustomerCommentService.DeleteByIds(lstId);
			return Json(result, JsonRequestBehavior.AllowGet);
		}
	}
}
