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
	public class CustomerController : Controller
	{
		private ICustomerService _CustomerService;
		public CustomerController(ICustomerService _CustomerService)
		{
			this._CustomerService = _CustomerService;

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
			return View(new Customer());
		}
		public PartialViewResult Customer_PagingList(ListPagingModel model)
		{
			var _total = 0;
			var list = _CustomerService.GetAllPagingListTSQL(model.keyword, model.filter, model.page, model.pageSize, ref _total);
			var gridModel = PageModelCustom<Customer>.GetPage(list, model.page, model.pageSize, _total);
			ViewBag.Key = model.keyword;
			return PartialView(gridModel); 
		}
		[HttpGet]
		[CheckSessionLogin]
		[CheckAuthorize]
		public ActionResult Create()
		{
			ViewBag.urlValue = (string)TempData["url"] ?? Url.Action("Index", "Customer");


			return View();
		}
		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Create(Customer obj , string TypeSelect, bool SaveAndCountinue = false)
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
				var report = _CustomerService.Create(obj);
				if(report.isSuccess)
				{
					if(SaveAndCountinue)
					{
						TempData["Success"] = "Thêm mới thành công";
						return RedirectToAction("Create", "Customer", new { controllername = "CustomerController" });
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
                var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new { x.Key, x.Value.Errors })
                .ToList();
                ModelState.AddModelError("", "Tạo dữ liệu không thành công. Hãy kiểm tra lại dữ liệu nhập vào");
			return View();
			}
		}
		[HttpGet]
		[CheckSessionLogin]
		[CheckAuthorize]
		public ActionResult Update(string id)
		{
			var obj = _CustomerService.GetById(id);
			ViewBag.urlValue = (string)TempData["url"] ?? Url.Action("Index", "Customer");


			return View(obj);
		}
		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Update(Customer obj , string TypeSelect, string urlValue = "")
		{


			//if (string.IsNullOrWhiteSpace(obj.Name))
			//{
			//    ModelState.AddModelError("Name", "Vui lòng nhập tên");
			//    return View(obj);
			//}
			if (ModelState.IsValid)
			{
				var oldobj = _CustomerService.GetById(obj.Id);
				if (oldobj == null) return View(obj);
				oldobj.FullName = obj.FullName;
				oldobj.Email = obj.Email;
				oldobj.Mobile = obj.Mobile;
				oldobj.Phone = obj.Phone;
				oldobj.Address = obj.Address;
				oldobj.Gender = obj.Gender;
				oldobj.Website = obj.Website;
				oldobj.Avartar = obj.Avartar;
				oldobj.Country = obj.Country;
				oldobj.Description = obj.Description;
				oldobj.DateCreated = obj.DateCreated;
				oldobj.Active = obj.Active;
				var report = _CustomerService.Update(oldobj);
				if (report.isSuccess)
				{
					urlValue = urlValue ?? Url.Action("Index", "Customer");
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
			var report = _CustomerService.DeleteById(id);
			return Json(report, JsonRequestBehavior.AllowGet);
		}
		public JsonResult MultiDelete(string lstId)
		{
			var result = _CustomerService.DeleteByIds(lstId);
			return Json(result, JsonRequestBehavior.AllowGet);
		}
	}
}
