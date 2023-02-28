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
	public class MainMenuController : Controller
	{
		private IMainMenuService _MainMenuService;
		public MainMenuController(IMainMenuService _MainMenuService)
		{
			this._MainMenuService = _MainMenuService;

		}
		public List<SelectListModelTree> ParentIdToDDLtree()  //bind ParentId to dropdownlist tree
		{
			var list = new List<SelectListModelTree> ();
			var listMainMenu = _MainMenuService.GetAllActive().ToList();
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
			var list = _MainMenuService.GetAll().ToList();
			TempData["url"] = Request.Url.PathAndQuery;
			//Đưa ra giao diện
			return View(list);
		}

		public PartialViewResult Child(List<MainMenu> listChild, List<MainMenu> allFunction, string group)
		{
			ViewBag.allList = allFunction;
			return PartialView(listChild);
		}
		[HttpGet]
		[CheckSessionLogin]
		[CheckAuthorize]
		public ActionResult Create()
		{
			ViewBag.urlValue = (string)TempData["url"] ?? Url.Action("Index", "MainMenu");
			ViewBag.ParentIdDDLtree = ParentIdToDDLtree();
            ViewBag.Target = FunctionHelper.TargetMenu();
            ViewBag.Positions = FunctionHelper.PositionModel();
            ViewBag.Pages = FunctionHelper.PageModel();
            return View();
		}
		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Create(MainMenu obj , string TypeSelect, bool SaveAndCountinue = false)
		{
			ViewBag.ParentIdDDLtree = ParentIdToDDLtree();
            ViewBag.Target = FunctionHelper.TargetMenu();
            ViewBag.Positions = FunctionHelper.PositionModel();
            ViewBag.Pages = FunctionHelper.PageModel();
            //if (string.IsNullOrWhiteSpace(obj.Name))
            //{
            //    ModelState.AddModelError("Name", "Vui lòng nhập tên");
            //    return View(obj);
            //}
            if (ModelState.IsValid)
			{
			obj.Id = Common.GenerateId();
			obj.DateCreated = DateTime.Now;
                if (string.IsNullOrWhiteSpace(obj.NameUrl))
                {
                    obj.NameUrl = StringUtil.RemoveSpecialCharactersVn(obj.Name.ToLower());
                }
                else
                {
                    obj.NameUrl = StringUtil.RemoveSpecialCharactersVn(obj.NameUrl.ToLower());
                }
                var report = _MainMenuService.Create(obj);
				if(report.isSuccess)
				{
					if(SaveAndCountinue)
					{
						TempData["Success"] = "Thêm mới thành công";
						return RedirectToAction("Create", "MainMenu", new { controllername = "MainMenuController" });
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
			var obj = _MainMenuService.GetById(id);
			ViewBag.urlValue = (string)TempData["url"] ?? Url.Action("Index", "MainMenu");
			ViewBag.ParentIdDDLtree = ParentIdToDDLtree();
            ViewBag.Target = FunctionHelper.TargetMenu();
            ViewBag.Positions = FunctionHelper.PositionModel();
            ViewBag.PageSelectLst = FunctionHelper.PageModel();
            return View(obj);
		}
		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Update(MainMenu obj , string TypeSelect, string urlValue = "")
		{
			ViewBag.ParentIdDDLtree = ParentIdToDDLtree();
            ViewBag.Target = FunctionHelper.TargetMenu();
            ViewBag.Positions = FunctionHelper.PositionModel();
            ViewBag.PageSelectLst = FunctionHelper.PageModel();

            //if (string.IsNullOrWhiteSpace(obj.Name))
            //{
            //    ModelState.AddModelError("Name", "Vui lòng nhập tên");
            //    return View(obj);
            //}
            if (ModelState.IsValid)
			{
				var oldobj = _MainMenuService.GetById(obj.Id);
				if (oldobj == null) return View(obj);
				oldobj.Name = obj.Name;
				oldobj.ParentId = obj.ParentId;
				oldobj.Depth = obj.Depth;
				oldobj.BreadCrumb = obj.BreadCrumb;
                if (string.IsNullOrWhiteSpace(obj.NameUrl))
                {
                    oldobj.NameUrl = StringUtil.RemoveSpecialCharactersVn(obj.Name.ToLower());
                }
                else
                {
                    oldobj.NameUrl = StringUtil.RemoveSpecialCharactersVn(obj.NameUrl.ToLower());
                }
                oldobj.Url = obj.Url;
				oldobj.IconPath = obj.IconPath;
				oldobj.CoverPath = obj.CoverPath;
				oldobj.Description = obj.Description;
				oldobj.Ordering = obj.Ordering;
				oldobj.Page = obj.Page;
				oldobj.Position = obj.Position;
				oldobj.Target = obj.Target;
				oldobj.DateCreated = obj.DateCreated;
				oldobj.Active = obj.Active;
				var report = _MainMenuService.Update(oldobj);
				if (report.isSuccess)
				{
					urlValue = urlValue ?? Url.Action("Index", "MainMenu");
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
			var report = _MainMenuService.DeleteById(id);
			return Json(report, JsonRequestBehavior.AllowGet);
		}
		public JsonResult MultiDelete(string lstId)
		{
			var result = _MainMenuService.DeleteByIds(lstId);
			return Json(result, JsonRequestBehavior.AllowGet);
		}
	}
}
