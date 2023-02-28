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
	public class ProductCategoryController : Controller
	{
		private IProductCategoryService _ProductCategoryService;
		private IMainMenuService _MainMenuService;
		public ProductCategoryController(IProductCategoryService _ProductCategoryService, IMainMenuService _MainMenuService)
		{
			this._ProductCategoryService = _ProductCategoryService;

			this._MainMenuService = _MainMenuService;
		}
		public List<SelectListModelTree> ParentIdToDDLtree()  //bind ParentId to dropdownlist tree
		{
			var list = new List<SelectListModelTree> ();
			var listMainMenu = _ProductCategoryService.GetAllActive().ToList();
			if (listMainMenu.Any())
			{
				foreach (var item in listMainMenu)
				{
					list.Add(new SelectListModelTree { ItemValue = item.Id, ItemText = item.Name, ParentValue = item.ParentId });
				}
				//Chạy hàm đệ quy
				list = FunctionHelper.addParentItemToDDLtree(list);
			}
		list.Insert(0, new SelectListModelTree { ItemValue = "0", ItemText = "-- Lựa chọn--" });
			return list;
		}
		[CheckSessionLogin]
		[CheckAuthorize]
		public ActionResult Index(string key)
		{
			//Lấy danh sách
			var list = _ProductCategoryService.GetAll().ToList();
			TempData["url"] = Request.Url.PathAndQuery;
			//Đưa ra giao diện
			return View(list);
		}

		public PartialViewResult Child(List<ProductCategory> listChild, List<ProductCategory> allFunction, string group)
		{
			ViewBag.allList = allFunction;
			return PartialView(listChild);
		}
		[HttpGet]
		[CheckSessionLogin]
		[CheckAuthorize]
		public ActionResult Create()
		{
			ViewBag.urlValue = (string)TempData["url"] ?? Url.Action("Index", "ProductCategory");
			ViewBag.ParentIdDDLtree = ParentIdToDDLtree();
           

            return View();
		}
		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Create(ProductCategory obj , string TypeSelect, bool SaveAndCountinue = false)
		{
			ViewBag.ParentIdDDLtree = ParentIdToDDLtree();
            
            //if (string.IsNullOrWhiteSpace(obj.Name))
            //{
            //    ModelState.AddModelError("Name", "Vui lòng nhập tên");
            //    return View(obj);
            //}
            if (ModelState.IsValid)
			{
			obj.Id = Common.GenerateId();
			obj.DateCreated = DateTime.Now;
				var report = _ProductCategoryService.Create(obj);
				if(report.isSuccess)
				{
					if(SaveAndCountinue)
					{
						TempData["Success"] = "Thêm mới thành công";
						return RedirectToAction("Create", "ProductCategory", new { controllername = "ProductCategoryController" });
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
			var obj = _ProductCategoryService.GetById(id);
			ViewBag.urlValue = (string)TempData["url"] ?? Url.Action("Index", "ProductCategory");
			ViewBag.ParentIdDDLtree = ParentIdToDDLtree();
           
            return View(obj);
		}
		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Update(ProductCategory obj , string TypeSelect, string urlValue = "")
		{
			ViewBag.ParentIdDDLtree = ParentIdToDDLtree();
          
            //if (string.IsNullOrWhiteSpace(obj.Name))
            //{
            //    ModelState.AddModelError("Name", "Vui lòng nhập tên");
            //    return View(obj);
            //}
            if (ModelState.IsValid)
			{
				var oldobj = _ProductCategoryService.GetById(obj.Id);
				if (oldobj == null) return View(obj);
				oldobj.Name = obj.Name;
				oldobj.NameUrl = obj.NameUrl;
				oldobj.ParentId = obj.ParentId;
				oldobj.Depth = obj.Depth;
				oldobj.BreadCrumb = obj.BreadCrumb;
				oldobj.MetaTitle = obj.MetaTitle;
				oldobj.MetaDesc = obj.MetaDesc;
				oldobj.MetaKeywork = obj.MetaKeywork;
				oldobj.Description = obj.Description;
				oldobj.IconPath = obj.IconPath;
				oldobj.CorverPath = obj.CorverPath;
				oldobj.Ordering = obj.Ordering;
				oldobj.DateCreated = obj.DateCreated;
				oldobj.Active = obj.Active;
               
				var report = _ProductCategoryService.Update(oldobj);
				if (report.isSuccess)
				{
					urlValue = urlValue ?? Url.Action("Index", "ProductCategory");
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
			var report = _ProductCategoryService.DeleteById(id);
			return Json(report, JsonRequestBehavior.AllowGet);
		}
		public JsonResult MultiDelete(string lstId)
		{
			var result = _ProductCategoryService.DeleteByIds(lstId);
			return Json(result, JsonRequestBehavior.AllowGet);
		}
	}
}
