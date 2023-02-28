using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Helpers;
using Kztek.Web.Core.Models;
using Kztek.Web.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Kztek.Web.Models;

namespace Kztek.Web.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private IMenuFunctionService _MenuFunctionService;

        public HomeController(IMenuFunctionService _MenuFunctionService)
        {
            this._MenuFunctionService = _MenuFunctionService;
        }

        [CheckSessionLogin]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Header(string controllerName = "", string actionName = "")
        {
            var list = new List<MenuFunction>();

            if (CacheLayer.Exists(ConstField.AllListMenuFunctionCache))
            {
                list = CacheLayer.Get<List<MenuFunction>>(ConstField.AllListMenuFunctionCache);
            }
            else
            {
                list = _MenuFunctionService.GetAllActive().ToList();
                CacheLayer.Add(ConstField.AllListMenuFunctionCache, list, ConstField.TimeCache);
            }

            var obj = _MenuFunctionService.GetByControllerAction(controllerName, actionName);

            return PartialView(obj);
        }

        /// <summary>
        /// Thanh điều hướng
        /// </summary>
        /// <param name="controller">Tên controller</param>
        /// <param name="action">Tên action</param>
        /// <returns></returns>
        public PartialViewResult Breadcrumb(string controller = "", string action = "")
        {
            //Danh sách menu
            var list = new List<MenuFunction>();
            if (CacheLayer.Exists(ConstField.AllListMenuFunctionCache))
            {
                list = CacheLayer.Get<List<MenuFunction>>(ConstField.AllListMenuFunctionCache);
            }
            else
            {
                list = _MenuFunctionService.GetAllActive().ToList();
                CacheLayer.Add(ConstField.AllListMenuFunctionCache, list, ConstField.TimeCache);
            }

            //Khai báo biến
            List<string> MenuName = new List<string>();

            var obj = list.FirstOrDefault(n => n.ControllerName.Equals(controller) && n.ActionName.Equals(action));
            if (obj != null)
            {
                if (!string.IsNullOrEmpty(obj.Breadcrumb))
                {
                    string[] id = obj.Breadcrumb.Split(new string[] { '/' + obj.Id }, StringSplitOptions.RemoveEmptyEntries);
                    if (id != null)
                    {
                        string[] ids = id[0].Split(new Char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                        if (ids != null)
                        {
                            foreach (var item in ids)
                            {
                                var Name = _MenuFunctionService.getById(item);
                                if (Name != null)
                                {
                                    MenuName.Add(Name.MenuName + "," + controller.ToString());
                                }
                            }
                        }
                    }
                }
            }

            ViewBag.ObjName = (obj != null && !string.IsNullOrWhiteSpace(obj.MenuName)) ? obj.MenuName : "";
            ViewBag.listMenu = MenuName;

            return PartialView();
        }

        /// <summary>
        /// Danh sách sidebar
        /// </summary>
        /// <param name="actionName">Tên action</param>
        /// <param name="controllerName">Tên controller</param>
        /// <param name="openTab">Mở tab lớn khi có chọn con</param>
        /// <returns></returns>
        public PartialViewResult Sidebar(string actionName = "Index", string controllerName = "Home", string openTab = "")
        {
            //All menuFunction
            //var list = CacheLayer.Get<List<MenuFunction>>(ConstField.AllListMenuFunctionCache);
            //if(list==null)
            //{
            //    list = _MenuFunctionService.GetAllActive().ToList();
            //    CacheLayer.Add(ConstField.AllListMenuFunctionCache, list, ConstField.TimeCache);
            //}
            // Current User
            var user = GetCurrentUser.GetUser();

            // get all Role menu buy User
            var model = new List<MenuFunction>();
            if (user != null)
            {
                model = _MenuFunctionService.GetAllMenuByPermisstion(user.Id, user.Admin).Distinct().ToList();
            }

            var pageModel = new PageNameModel
            {
                ControllerName = controllerName,
                ActionName = actionName,
                OpenMenu = openTab
            };

            ViewBag.PageModel = pageModel;
            return PartialView(model);
        }

        public PartialViewResult Child(MenuFunction itemMenu, List<MenuFunction> listMenu, List<MenuFunction> AllMenu)
        {

            ViewBag.ItemMenu = itemMenu;
            ViewBag.AllMenuPermisstion = AllMenu;

            return PartialView(listMenu);
        }

        /// <summary>
        /// Chứa danh sách các actions có trong hệ thống + phân quyền
        /// </summary>
        /// <param name="ControllerName">Tên controller</param>
        /// <param name="ActionName">Tên action</param>
        /// <param name="id">Id đối tượng thao tác</param>
        /// <param name="pageNumber">Trang hiện tại</param>
        /// <returns></returns>
        public PartialViewResult Actions(string ControllerName, string ActionName, string id = "", int pageNumber = 1)
        {
            ViewBag.ControllerName = ControllerName;
            ViewBag.ActionName = ActionName;
            ViewBag.Id = id;
            ViewBag.PN = pageNumber;

            return PartialView();
        }

        /// <summary>
        /// Giao diện footer
        /// </summary>
        /// <returns></returns>
        public PartialViewResult Footer()
        {
            return PartialView();
        }

        public PartialViewResult Buttons(string controller, string action, string url, int pageNumber = 1)
        {
            ViewBag.Controller = controller;
            ViewBag.Action = action;
            ViewBag.url = url;
            ViewBag.PN = pageNumber;
            return PartialView();
        }


        public JsonResult DeleteCached()
        {
            bool isSucccess = true;
            try
            {
                CacheLayer.ClearAll();
            }
            catch (Exception)
            {
                isSucccess = false;
            }
            return Json(isSucccess, JsonRequestBehavior.AllowGet);
        }
    }
}