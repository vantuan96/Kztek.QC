using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Helpers;
using Kztek.Web.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Kztek.Web.Areas.Admin.Controllers
{
    public class MenuFunctionController : Controller
    {
        #region Khai báo services

        private IMenuFunctionService _MenuFunctionService;

        public MenuFunctionController(IMenuFunctionService _MenuFunctionService)
        {
            this._MenuFunctionService = _MenuFunctionService;
        }

        #endregion Khai báo services

        private static string url = "";

        #region Danh sách

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Index(string key, int? page)
        {
            var lst = _MenuFunctionService.GetAllMenu(key);

            //Viewbag
            ViewBag.DDLActive = FunctionHelper.ActiveStatus();
            ViewBag.Keyword = key;

            url = Request.Url.PathAndQuery;

            return View(lst);
        }

        #endregion Danh sách

        public PartialViewResult MenuChilden(List<MenuFunction> childList, List<MenuFunction> AllList, string group = "")
        {
            //Viewbag
            ViewBag.ListMenu = AllList;
            ViewBag.GroupID = group;

            return PartialView(childList);
        }

        #region Thêm mới

        [HttpGet]
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Create(string controllername, string parentid, string menytype, string grouplist, string ordernu = "1")
        {
            //ViewBag
            ViewBag.DDLMenu = GetMenuList();
            ViewBag.IconList = ListViewCustom.GetListIcon("~/Templates/AwesomeIcon.xml");
            ViewBag.DDLMenuType = FunctionHelper.MenuType();
            ViewBag.controller = controllername;
            ViewBag.parent = parentid;
            ViewBag.menytypeValue = menytype;
            ViewBag.grouplistValue = grouplist;
            ViewBag.ordernuValue = ordernu;

            ViewBag.urlValue = url;

            return View();
        }

        [HttpPost]
        public ActionResult Create(MenuFunction obj, bool SaveAndCountinue = false)
        {
            //ViewBag
            ViewBag.DDLMenu = GetMenuList();
            ViewBag.IconList = ListViewCustom.GetListIcon("~/Templates/AwesomeIcon.xml");
            ViewBag.DDLMenuType = FunctionHelper.MenuType();

            ViewBag.urlValue = url;

            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            obj.Id = Common.GenerateId();
            obj.ControllerName = obj.ControllerName != null ? obj.ControllerName : string.Format("controller_{0}", obj.Id);
            obj.ActionName = obj.ActionName != null ? obj.ActionName : string.Format("action_{0}", obj.Id);
            obj.Url = string.Format("/{0}/{1}", obj.ControllerName, obj.ActionName);

            var report = _MenuFunctionService.Create(obj);
            if (report.isSuccess)
            {
                //For cache
                CacheLayer.ClearAll();

                if (SaveAndCountinue)
                {
                    TempData["Success"] = report.Message;

                    return RedirectToAction("Create", "MenuFunction", new { controllername = obj.ControllerName, parentid = obj.ParentId, menytype = obj.MenuType, ordernu = obj.OrderNumber + 1 });
                }

                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Có lỗi xảy ra trong quá trình khởi tạo.");
                return View(obj);
            }

        }

        #endregion Thêm mới

        #region Cập nhật

        [HttpGet]
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Update(string id, string group = "")
        {
            ViewBag.DDLMenu = GetMenuList();
            ViewBag.IconList = ListViewCustom.GetListIcon("~/Templates/AwesomeIcon.xml");
            ViewBag.DDLMenuType = FunctionHelper.MenuType();

            ViewBag.GroupID = group;

            ViewBag.urlValue = url;

            var obj = _MenuFunctionService.getById(id);
            if (obj != null)
            {
                return View(obj);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Update(MenuFunction obj, string group = "")
        {
            //ViewBag
            ViewBag.IconList = ListViewCustom.GetListIcon("~/Templates/AwesomeIcon.xml");
            ViewBag.DDLMenuType = FunctionHelper.MenuType();
            ViewBag.DDLMenu = GetMenuList();
            ViewBag.GroupID = group;

            ViewBag.urlValue = url;

            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            var oldObj = _MenuFunctionService.getById(obj.Id);
            if (oldObj == null)
            {
                return View(obj);
            }

            oldObj.MenuName = obj.MenuName;
            oldObj.ParentId = obj.ParentId;
            oldObj.OrderNumber = obj.OrderNumber;
            oldObj.MenuType = obj.MenuType;
            oldObj.Icon = obj.Icon;
            oldObj.Active = obj.Active;
            oldObj.ControllerName = obj.ControllerName != null ? obj.ControllerName : string.Format("controller_{0}", obj.Id);
            oldObj.ActionName = obj.ActionName != null ? obj.ActionName : string.Format("action_{0}", obj.Id);
            oldObj.Url = string.Format("/{0}/{1}", obj.ControllerName, obj.ActionName);

            var report = _MenuFunctionService.Update(oldObj);

            if (report.isSuccess)
            {
                CacheLayer.ClearAll();

                return RedirectToAction("Index", new { group = group });
            }
            else
            {
                ModelState.AddModelError("", "Có lỗi xảy ra trong quá trình khởi tạo.");
                return View(oldObj);
            }
            
        }

        #endregion Cập nhật

        #region Xóa nhiều

        public JsonResult MutilDelete(string lstId)
        {
            var report = _MenuFunctionService.DeleteByIds(lstId);

            if (report.isSuccess)
            {
                CacheLayer.ClearAll();
            }

            return Json(report, JsonRequestBehavior.AllowGet);
        }

        #endregion Xóa nhiều

        #region Chuyển đổi trạng thái

        public JsonResult Active(string lstId, string nhaptrangthai)
        {
            var report = _MenuFunctionService.ActiveByIds(lstId, nhaptrangthai);

            if (report.isSuccess)
            {
                CacheLayer.ClearAll();
            }

            return Json(report, JsonRequestBehavior.AllowGet);
        }

        #endregion Chuyển đổi trạng thái

        private List<MenuFunctionSubmit> GetMenuList()
        {
            var list = new List<MenuFunctionSubmit>
            {
                new MenuFunctionSubmit {  Id = "0", MenuName = "- Chọn danh mục -" }
            };
            var MenuList = _MenuFunctionService.GetAllActive();
            var parent = MenuList.Where(c => c.ParentId == "0");
            if (parent.Any())
            {
                foreach (var item in parent.OrderBy(c => c.OrderNumber))
                {
                    //Nếu có thì duyệt tiếp để lưu vào list
                    list.Add(new MenuFunctionSubmit { Id = item.Id, MenuName = item.MenuName });
                    //Gọi action để lấy danh sách submenu theo id
                    var submenu = Children(item.Id);
                    //Kiểm tra có submenu không
                    if (submenu.Count > 0)
                    {
                        //Nếu có thì duyệt tiếp để lưu vào list
                        foreach (var item1 in submenu)
                        {
                            list.Add(new MenuFunctionSubmit { Id = item1.Id, MenuName = item.MenuName + " / " + item1.MenuName });
                        }
                        //Phân tách các danh mục
                        list.Add(new MenuFunctionSubmit { Id = "-1", MenuName = "-----" });
                    }
                    else
                    {
                        //Phân tách các danh mục
                        list.Add(new MenuFunctionSubmit { Id = "-1", MenuName = "-----" });
                    }
                }
            }
            return list;
        }

        private List<MenuFunctionSubmit> Children(string parentID)
        {
            //Khai báo danh sách
            List<MenuFunctionSubmit> lst = new List<MenuFunctionSubmit>();
            //Lấy danh sách submenu theo id truyền từ action Parent()
            var menu = _MenuFunctionService.GetAllChildByParentId(parentID).ToList();
            //Kiểm tra có dữ liệu chưa
            if (menu.Any())
            {
                foreach (var item in menu)
                {
                    //Nếu có thì duyệt tiếp để lưu vào list
                    lst.Add(new MenuFunctionSubmit { Id = item.Id, MenuName = item.MenuName });
                    //Gọi action để lấy danh sách submenu theo id
                    var submenu = Children(item.Id);
                    //Kiểm tra có submenu không
                    if (submenu.Count > 0)
                    {
                        foreach (var item1 in submenu)
                        {
                            //Nếu có thì duyệt tiếp để lưu vào list
                            lst.Add(new MenuFunctionSubmit { Id = item1.Id, MenuName = item.MenuName + " / " + item1.MenuName });
                        }
                    }
                }
            }
            return lst;
        }
    }
}