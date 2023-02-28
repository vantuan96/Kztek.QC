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
	public class NewsController : Controller
	{
		private INewsService _NewsService;
		private INewsCategoryService _NewsCategoryService;
		public NewsController(INewsService _NewsService, INewsCategoryService _NewsCategoryService)
		{
			this._NewsService = _NewsService;

			this._NewsCategoryService = _NewsCategoryService;
		}
		public List<SelectListModelTree> NewsCategoryIdToDDLtree()  //bind NewsCategoryId to dropdownlist tree
		{
			var list = new List<SelectListModelTree> ();
			var listNewsCategory = _NewsCategoryService.GetAllActive().ToList();
			if (listNewsCategory.Any())
			{
				foreach (var item in listNewsCategory)
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
			return View(new News());
		}
		public PartialViewResult News_PagingList(ListPagingModel model)
		{
			var _total = 0;
			var list = _NewsService.GetAllPagingListTSQL(model.keyword, model.filter, model.page, model.pageSize, ref _total);
			var gridModel = PageModelCustom<News>.GetPage(list, model.page, model.pageSize, _total);
			ViewBag.Key = model.keyword;
			return PartialView(gridModel); 
		}
		[HttpGet]
		[CheckSessionLogin]
		[CheckAuthorize]
		public ActionResult Create()
		{
			ViewBag.urlValue = (string)TempData["url"] ?? Url.Action("Index", "News");
			ViewBag.NewsCategoryIdDDLtree = NewsCategoryIdToDDLtree();

			return View();
		}
		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Create(News obj , string TypeSelect, bool SaveAndCountinue = false)
		{
			ViewBag.NewsCategoryIdDDLtree = NewsCategoryIdToDDLtree();

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
                if (string.IsNullOrWhiteSpace(obj.NameUrl))
                {
                    obj.NameUrl = StringUtil.RemoveSpecialCharactersVn(obj.Name.ToLower());
                }
                else
                {
                    obj.NameUrl = StringUtil.RemoveSpecialCharactersVn(obj.NameUrl.ToLower());
                }
                var report = _NewsService.Create(obj);
				if(report.isSuccess)
				{
					if(SaveAndCountinue)
					{
						TempData["Success"] = "Thêm mới thành công";
						return RedirectToAction("Create", "News", new { controllername = "NewsController" });
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
                .ToArray();
                ModelState.AddModelError("", "Tạo dữ liệu không thành công. Hãy kiểm tra lại dữ liệu nhập vào");
			return View();
			}
		}
		[HttpGet]
		[CheckSessionLogin]
		[CheckAuthorize]
		public ActionResult Update(string id)
		{
			var obj = _NewsService.GetById(id);
			ViewBag.urlValue = (string)TempData["url"] ?? Url.Action("Index", "News");
			ViewBag.NewsCategoryIdDDLtree = NewsCategoryIdToDDLtree();

			return View(obj);
		}
		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Update(News obj , string TypeSelect, string urlValue = "")
		{
			ViewBag.NewsCategoryIdDDLtree = NewsCategoryIdToDDLtree();

			//if (string.IsNullOrWhiteSpace(obj.Name))
			//{
			//    ModelState.AddModelError("Name", "Vui lòng nhập tên");
			//    return View(obj);
			//}
			if (ModelState.IsValid)
			{
				var oldobj = _NewsService.GetById(obj.Id);
				if (oldobj == null) return View(obj);
				oldobj.NewsCategoryId = obj.NewsCategoryId;
				oldobj.Name = obj.Name;
                if (string.IsNullOrWhiteSpace(obj.NameUrl))
                {
                    oldobj.NameUrl = StringUtil.RemoveSpecialCharactersVn(obj.Name.ToLower());
                }
                else
                {
                    oldobj.NameUrl = StringUtil.RemoveSpecialCharactersVn(obj.NameUrl.ToLower());
                }
                oldobj.Summary = obj.Summary;
				oldobj.Description = obj.Description;
				oldobj.MetaTitle = obj.MetaTitle;
				oldobj.MetaDesc = obj.MetaDesc;
				oldobj.MetaKeywork = obj.MetaKeywork;
				oldobj.CoverPath = obj.CoverPath;
                oldobj.DetailPath = obj.DetailPath;
				oldobj.DateCreated = obj.DateCreated;
				oldobj.Active = obj.Active;
				var report = _NewsService.Update(oldobj);
				if (report.isSuccess)
				{
					urlValue = urlValue ?? Url.Action("Index", "News");
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
			var report = _NewsService.DeleteById(id);
			return Json(report, JsonRequestBehavior.AllowGet);
		}
		public JsonResult MultiDelete(string lstId)
		{
			var result = _NewsService.DeleteByIds(lstId);
			return Json(result, JsonRequestBehavior.AllowGet);
		}
	}
}
