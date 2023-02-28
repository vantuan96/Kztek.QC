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
	public class ProductController : Controller
	{
		private IProductService _ProductService;
		private IProductCategoryService _ProductCategoryService;
		public ProductController(IProductService _ProductService, IProductCategoryService _ProductCategoryService)
		{
			this._ProductService = _ProductService;
            this._ProductCategoryService = _ProductCategoryService;
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
			return View(new Product());
		}
		public PartialViewResult Product_PagingList(ListPagingModel model)
		{
			var _total = 0;
			var list = _ProductService.GetAllPagingListTSQL(model.keyword, model.filter, model.page, model.pageSize, ref _total);
			var gridModel = PageModelCustom<Product>.GetPage(list, model.page, model.pageSize, _total);
			ViewBag.Key = model.keyword;
			return PartialView(gridModel); 
		}
		[HttpGet]
		[CheckSessionLogin]
		[CheckAuthorize]
		public ActionResult Create()
		{
			ViewBag.urlValue = (string)TempData["url"] ?? Url.Action("Index", "Product");
            ViewBag.ProductCategoryIdDDLtree = ProductCategoryIdToDDLtree();

            return View();
		}
		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Create(ProductView obj , string TypeSelect, bool SaveAndCountinue = false)
		{


			//if (string.IsNullOrWhiteSpace(obj.Name))
			//{
			//    ModelState.AddModelError("Name", "Vui lòng nhập tên");
			//    return View(obj);
			//}
			if(ModelState.IsValid)
			{
                Product objCreate = new Product()
                {
                    Id = Common.GenerateId(),
                    DateCreated = DateTime.Now,
                    Name = obj.Name,
                    Barcode = obj.Barcode,
                    ProductCategoryId = obj.ProductCategoryId,
                    Summary = obj.Summary,
                    Description = obj.Description,
                    NameUrl = obj.NameUrl,
                    MetaTitle = obj.MetaTitle,
                    MetaDesc = obj.MetaDesc,
                    MetaKeywork = obj.MetaKeywork,
                    CorverPath = obj.CorverPath,
                    Price = Convert.ToDecimal(obj.Price),
                    PricePromotion = Convert.ToDecimal(obj.PricePromotion),
                    Quantity = obj.Quantity,
                    Ordering = obj.Ordering,
                    Active = obj.Active,
                    SummaryHome = obj.SummaryHome
                };


				var report = _ProductService.Create(objCreate);
				if(report.isSuccess)
				{
					if(SaveAndCountinue)
					{
						TempData["Success"] = "Thêm mới thành công";
						return RedirectToAction("Create", "Product", new { controllername = "ProductController" });
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
			var obj = _ProductService.GetById(id);

            ProductView objUpdate = new ProductView()
            {
                Id = Common.GenerateId(),
                DateCreated = DateTime.Now,
                Name = obj.Name,
                Barcode = obj.Barcode,
                ProductCategoryId = obj.ProductCategoryId,
                Summary = obj.Summary,
                Description = obj.Description,
                NameUrl = obj.NameUrl,
                MetaTitle = obj.MetaTitle,
                MetaDesc = obj.MetaDesc,
                MetaKeywork = obj.MetaKeywork,
                CorverPath = obj.CorverPath,
                Price = obj.Price.ToString(),
                PricePromotion = obj.PricePromotion.ToString(),
                Quantity = obj.Quantity,
                Ordering = obj.Ordering,
                Active = obj.Active,
                SummaryHome = obj.SummaryHome
            };

            ViewBag.urlValue = (string)TempData["url"] ?? Url.Action("Index", "Product");
            ViewBag.ProductCategoryIdDDLtree = ProductCategoryIdToDDLtree();

            return View(objUpdate);
		}
		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Update(ProductView obj , string TypeSelect, string urlValue = "")
		{
            ViewBag.ProductCategoryIdDDLtree = ProductCategoryIdToDDLtree();

            //if (string.IsNullOrWhiteSpace(obj.Name))
            //{
            //    ModelState.AddModelError("Name", "Vui lòng nhập tên");
            //    return View(obj);
            //}
            if (ModelState.IsValid)
			{
				var oldobj = _ProductService.GetById(obj.Id);
				if (oldobj == null) return View(obj);
				oldobj.Name = obj.Name;
				oldobj.Barcode = obj.Barcode;
				oldobj.ProductCategoryId = obj.ProductCategoryId;
				oldobj.Summary = obj.Summary;
                oldobj.SummaryHome = obj.SummaryHome;
                oldobj.Description = obj.Description;
				oldobj.NameUrl = obj.NameUrl;
				oldobj.MetaTitle = obj.MetaTitle;
				oldobj.MetaDesc = obj.MetaDesc;
				oldobj.MetaKeywork = obj.MetaKeywork;
				oldobj.CorverPath = obj.CorverPath;
				oldobj.Price = Convert.ToDecimal(obj.Price);
                oldobj.PricePromotion = Convert.ToDecimal(obj.PricePromotion);
				oldobj.Quantity = obj.Quantity;
				oldobj.Ordering = obj.Ordering;
				oldobj.DateCreated = obj.DateCreated;
				oldobj.Active = obj.Active;
				var report = _ProductService.Update(oldobj);
				if (report.isSuccess)
				{
					urlValue = urlValue ?? Url.Action("Index", "Product");
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
			var report = _ProductService.DeleteById(id);
			return Json(report, JsonRequestBehavior.AllowGet);
		}
		public JsonResult MultiDelete(string lstId)
		{
			var result = _ProductService.DeleteByIds(lstId);
			return Json(result, JsonRequestBehavior.AllowGet);
		}

        public List<SelectListModelTree> ProductCategoryIdToDDLtree()  //bind NewsCategoryId to dropdownlist tree
        {
            var list = new List<SelectListModelTree>();
            var listCategory = _ProductCategoryService.GetAllActive().ToList();
            if (listCategory.Any())
            {
                foreach (var item in listCategory)
                {
                    list.Add(new SelectListModelTree { ItemValue = item.Id, ItemText = item.Name, ParentValue = item.ParentId });
                }
                //Chạy hàm đệ quy
                list = FunctionHelper.addParentItemToDDLtree(list);
            }
            list.Insert(0, new SelectListModelTree { ItemValue = "0", ItemText = "-- Lựa chọn--" });
            return list;
        }
    }
}
