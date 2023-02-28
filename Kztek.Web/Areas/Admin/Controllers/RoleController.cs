using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Helpers;
using Kztek.Web.Core.Models;
using Kztek.Web.Attributes;
using Kztek.Web.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Kztek.Web.Areas.Admin.Controllers
{
    public class RoleController : Controller
    {
        #region Khai báo services

        private IRoleService _RoleService;
        private IRoleMenuService _RoleMenuService;
        private IMenuFunctionService _MenuFunctionService;
        private IUserRoleService _UserRoleService;

        public RoleController(IRoleService _RoleService, IRoleMenuService _RoleMenuService, IMenuFunctionService _MenuFunctionService, IUserRoleService _UserRoleService)
        {
            this._RoleService = _RoleService;
            this._RoleMenuService = _RoleMenuService;
            this._MenuFunctionService = _MenuFunctionService;
            this._UserRoleService = _UserRoleService;
        }

        #endregion Khai báo services

        private static string url = "";

        #region Danh sách

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Index(string key)
        {
            var list = _RoleService.GetAllByFirst(key);

            url = Request.Url.PathAndQuery;

            return View(list);
        }

        #endregion Danh sách

        #region Thêm mới

        [HttpGet]
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Create()
        {
            ViewBag.Selected = "";
            ViewBag.urlValue = url;

            ViewBag.MenuFunctionList = _MenuFunctionService.GetAllActive().ToList();
            return View();
        }

        public ActionResult Create(Role obj, string menufunctionvalues, bool SaveAndCountinue = false)
        {
            //
            ViewBag.Selected = menufunctionvalues;
            ViewBag.urlValue = url;

            //Kiểm tra
            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            //Gán giá trị
            obj.Id = Common.GenerateId();

            //Thêm danh sách cây menu
            if (!string.IsNullOrWhiteSpace(menufunctionvalues))
            {
                var ids = menufunctionvalues.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (ids.Any())
                {
                    foreach (var id in ids)
                    {
                        RoleMenu objRoleMenu = new RoleMenu();
                        objRoleMenu.Id = Common.GenerateId();
                        objRoleMenu.RoleId = obj.Id;
                        objRoleMenu.MenuId = id;
                        _RoleMenuService.Create(objRoleMenu);
                    }
                }
            }

            //Thực hiện thêm mới
            var result = _RoleService.Create(obj);
            if (result.isSuccess)
            {
                if (SaveAndCountinue)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction("Create");
                }

                return RedirectToAction("Index");
            }
            else
            {
                return View(obj);
            }
        }

        public JsonResult CreateRole(Role obj, string lstId)
        {
            var report = new MessageReport();
            obj.Id = Common.GenerateId();

            if (ModelState.IsValid)
            {
                report = _RoleService.Create(obj);
            }

            if (report.isSuccess)
            {
                string[] ids = lstId.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (ids != null)
                {
                    foreach (var item in ids)
                    {
                        RoleMenu objRoleMenu = new RoleMenu();
                        objRoleMenu.Id = Common.GenerateId();
                        objRoleMenu.RoleId = obj.Id;
                        objRoleMenu.MenuId = item;
                        _RoleMenuService.Create(objRoleMenu);
                    }
                }
            }

            return Json(report, JsonRequestBehavior.AllowGet);
        }

        #endregion Thêm mới

        #region Cập nhật

        [HttpGet]
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Update(string id)
        {
            string str = "";
            var obj = _RoleService.GetById(id);

            var t = _RoleMenuService.GetAllByRoleId(id).ToList();
            if (t != null && t.Any())
            {
                foreach (var item in t)
                {
                    str += item.MenuId + ",";
                }
            }

            ViewBag.Selected = str;
            ViewBag.urlValue = url;

            return View(obj);
        }

        [HttpPost]
        public ActionResult Update(Role obj, string menufunctionvalues)
        {
            //
            ViewBag.Selected = menufunctionvalues;
            ViewBag.urlValue = url;

            //Kiểm tra
            var oldObj = _RoleService.GetById(obj.Id);
            if (oldObj == null)
            {
                ViewBag.Error = "Bản ghi đã tồn tại";
                return View(obj);
            }

            if (!ModelState.IsValid)
            {
                return View(oldObj);
            }

            //Gán giá trị
            oldObj.RoleName = obj.RoleName;
            oldObj.Description = obj.Description;
            oldObj.Active = obj.Active;

            //Cập nhật lại danh sách menu
            var list = _RoleMenuService.GetAllByRoleId(obj.Id).ToList();
            if (list.Any())
            {
                foreach (var item in list)
                {
                    _RoleMenuService.DeleteById(item.Id);
                }
            }

            if (!string.IsNullOrWhiteSpace(menufunctionvalues))
            {
                string[] ids = menufunctionvalues.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (ids != null)
                {
                    foreach (var item in ids)
                    {
                        RoleMenu objRoleMenu = new RoleMenu();
                        objRoleMenu.Id = Common.GenerateId();
                        objRoleMenu.RoleId = oldObj.Id;
                        objRoleMenu.MenuId = item;
                        _RoleMenuService.Create(objRoleMenu);
                    }
                }
            }

            //Thực hiện cập nhật
            var result = _RoleService.Update(oldObj);
            if (result.isSuccess)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Có lỗi xảy ra");
                return View(oldObj);
            }
        }

        //[HttpPost]
        //public ActionResult Update(Role obj)
        //{
        //    //ViewBag.MenuFunctionList = _MenuFunctionService.GetAllActive().ToList();
        //    if (ModelState.IsValid)
        //    {
        //        bool isSuccess = _RoleService.Update(obj);
        //        if (isSuccess)
        //        {
        //            MessageReport report = new MessageReport(true, "Cập nhật thành công");
        //            WriteLog.Write(report, GetCurrentUser.GetUser(), obj.Id, obj.RoleName, "Role");

        //            FunctionHelper.ClearCache(ConstField.ListRoleMenu, obj.Id);

        //            return RedirectToAction("Index");
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "Có lỗi xảy ra");
        //            return View(obj);
        //        }
        //    }
        //    return View(obj);
        //}

        public JsonResult UpdateRole(string lstId, string RoleId)
        {
            bool isSuccess = false;
            try
            {
                var list = _RoleMenuService.GetAllByRoleId(RoleId).ToList();
                if (list.Any())
                {
                    foreach (var item in list)
                    {
                        _RoleMenuService.DeleteById(item.Id);
                    }
                }

                if (!string.IsNullOrWhiteSpace(lstId))
                {
                    string[] ids = lstId.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (ids != null)
                    {
                        foreach (var item in ids)
                        {
                            RoleMenu objRoleMenu = new RoleMenu();
                            objRoleMenu.Id = Common.GenerateId();
                            objRoleMenu.RoleId = RoleId;
                            objRoleMenu.MenuId = item;
                            isSuccess = _RoleMenuService.Create(objRoleMenu);
                        }
                    }
                }
                else
                {
                    isSuccess = true;
                }

                if (isSuccess)
                {
                    FunctionHelper.ClearCache(ConstField.ListRoleMenu, RoleId);
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
                throw ex;
            }

            return Json(isSuccess, JsonRequestBehavior.AllowGet);
        }

        #endregion Cập nhật

        #region Xóa

        [CheckAuthorize]
        public JsonResult Delete(string id)
        {
            var obj = _RoleService.GetById(id);

            var report = _RoleService.DeleteById(id);
            if (report.isSuccess)
            {
                FunctionHelper.ClearCache(ConstField.ListRoleMenu, id);

                var listRoleMenu = _RoleMenuService.GetAllByRoleId(id);
                if (listRoleMenu.Any())
                {
                    foreach (var item in listRoleMenu.ToList())
                    {
                        _RoleMenuService.DeleteById(item.Id);
                    }
                }

                var listUserRole = _UserRoleService.GetAllByRoleId(id);
                if (listUserRole.Any())
                {
                    foreach (var item in listUserRole.ToList())
                    {
                        _UserRoleService.DeleteById(item.Id);
                    }
                }
            }
            return Json(report, JsonRequestBehavior.AllowGet);
        }

        #endregion Xóa

        public PartialViewResult MenuFunctionParent(string roleid)
        {
            var listRoleMenu = _RoleMenuService.GetAllByRoleId(roleid).ToList();

            ViewBag.RoleMenuList = listRoleMenu;
            ViewBag.RoleId = roleid;

            var list = _MenuFunctionService.GetAllActive().ToList();
            return PartialView(list);
        }

        public PartialViewResult MenuFunctionChild(string parentid, string roleid)
        {
            var listRoleMenu = _RoleMenuService.GetAllByRoleId(roleid).ToList();

            ViewBag.RoleMenuList = listRoleMenu;
            ViewBag.RoleId = roleid;
            ViewBag.MenuList = _MenuFunctionService.GetAllActive().ToList();

            var list = _MenuFunctionService.GetAllActiveChildByParentId(parentid).ToList();
            return PartialView(list);
        }

        #region Cây menu

        /// <summary>
        /// Cây menu hệ thống
        /// </summary>
        /// <modified>
        /// Author                  Date                Comments
        /// TrungNQ                 04/08/2017          Tạo mới
        /// </modified>
        /// <param name="str">List đã chọn</param>
        /// <returns></returns>
        public PartialViewResult MenuFunctionList(string str, string gId)
        {
            ViewBag.Selected = str;

            var list = _MenuFunctionService.GetAllParentByFirst("").ToList();
            return PartialView(list);
        }

        public PartialViewResult Child(string parentId, string selectedId)
        {
            ViewBag.Selected = selectedId;
            var list = _MenuFunctionService.GetAllChildActiveByParentId(parentId).ToList();
            return PartialView(list);
        }

        #endregion Cây menu
    }
}