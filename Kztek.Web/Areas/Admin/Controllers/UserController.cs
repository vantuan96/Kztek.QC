using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Web.Core.Extensions;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Helpers;
using Kztek.Web.Core.Models;
using Kztek.Web.Attributes;
using Kztek.Web.Models;
using System;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace Kztek.Web.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        #region Khai báo services

        /// <summary>
        /// Khai báo services
        /// </summary>
        private IUserService _UserService;

        private IMenuFunctionService _MenuFunctionService;
        private IUserRoleService _UserRoleService;
        private IRoleService _RoleService;
        private IUserConfigService _UserConfigService;

        public UserController(IUserService _UserService, IMenuFunctionService _MenuFunctionService, IUserRoleService _UserRoleService, IRoleService _RoleService, IUserConfigService _UserConfigService)
        {
            this._UserService = _UserService;
            this._MenuFunctionService = _MenuFunctionService;
            this._UserRoleService = _UserRoleService;
            this._RoleService = _RoleService;
            this._UserConfigService = _UserConfigService;
        }

        #endregion Khai báo services

        private static string url = "";

        #region Danh sách

        /// <summary>
        /// Danh sách
        /// </summary>
        /// <modified>
        /// Author              Date            Comments
        /// TrungNQ             10/12/2016      Tạo mới
        /// </modified>
        /// <param name="key">Từ khóa</param>
        /// <param name="active">Trạng thái</param>
        /// <param name="page">Trang hiện tại</param>
        /// <returns></returns>
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Index(string key, int page = 1)
        {
            //Khai báo
            int pageSize = 20;

            //Lấy danh sách phân trang
            var list = _UserService.GetAllPagingByFirst(key, page, pageSize);

            //Đổ lên grid
            var gridModel = PageModelCustom<User>.GetPage(list, page, pageSize);

            //ViewBag
            ViewBag.keyValue = key;

            url = Request.Url.PathAndQuery;

            //Đưa ra giao diện
            return View(gridModel);
        }
        public PartialViewResult RoleList(string userId)
        {
            var roles = _RoleService.GetAllByUserId(userId);
            return PartialView(roles);
        }
        #endregion Danh sách

        #region Thêm mới

        /// <summary>
        /// Giao diện thêm mới
        /// </summary>
        /// <modified>
        /// Author          Date            Comments
        /// TrungNQ         04/08/2017      Tạo mới
        /// </modified>
        /// <returns></returns>
        [CheckSessionLogin]
        [CheckAuthorize]
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Selected = "";

            ViewBag.urlValue = url;

            return View();
        }

        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <modified>
        /// Author          Date            Comments
        /// TrungNQ         04/08/2017      Tạo mới
        /// </modified>
        /// <param name="obj">Đối tượng</param>
        /// <param name="rolevalues">Giá trị quyền</param>
        /// <param name="FileUpload">File Upload</param>
        /// <param name="SaveAndCountinue">Tiếp tục thêm</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(User obj, string repass, string rolevalues, HttpPostedFileBase FileUpload, bool SaveAndCountinue = false)
        {
            //
            ViewBag.Selected = rolevalues;
            ViewBag.urlValue = url;

            //Kiểm tra
            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            var isExisted = _UserService.GetByUserName(obj.Username);
            if (isExisted != null)
            {
                ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại");
                return View(obj);
            }

            //Gán giá trị
            obj.Id = Common.GenerateId();
            obj.DateCreated = DateTime.Now;
            obj.PasswordSalat = Guid.NewGuid().ToString();

            if (!string.IsNullOrWhiteSpace(obj.Password))
            {
                if (!string.IsNullOrWhiteSpace(repass))
                {
                    if (obj.Password.Equals(repass))
                    {
                        obj.Password = obj.Password.PasswordHashed(obj.PasswordSalat);
                    }
                    else
                    {
                        ModelState.AddModelError("repass", "Vui lòng nhập lại đúng mật khẩu.");
                        return View(obj);
                    }
                }
                else
                {
                    ModelState.AddModelError("repass", "Vui lòng nhập lại mật khẩu.");
                    return View(obj);
                }
            }
            else
            {
                obj.Password = "";
                obj.Password = obj.Password.PasswordHashed(obj.PasswordSalat);
            }

            //Thêm mới danh sách quyền
            if (!string.IsNullOrWhiteSpace(rolevalues))
            {
                var ids = rolevalues.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (ids.Any())
                {
                    foreach (var id in ids)
                    {
                        var UserRole = new UserRole();
                        UserRole.Id = Common.GenerateId();
                        UserRole.RoleId = id;
                        UserRole.UserId = obj.Id;

                        _UserRoleService.Create(UserRole);
                    }
                }
            }


            //File upload
            if (FileUpload != null)
            {
                var extension = Path.GetExtension(FileUpload.FileName) ?? "";
                var fileName = Path.GetFileName(string.Format("{0}{1}", StringUtil.RemoveSpecialCharactersVn(FileUpload.FileName.Replace(extension, "")).GetNormalizeString(), extension));
                obj.UserAvatar = string.Format("{0}{1}", ConfigurationManager.AppSettings["uploadfolder"], fileName);
            }

            //Thực hiện thêm mới
            var result = _UserService.Create(obj);
            if (result.isSuccess)
            {
                //FunctionHelper.UploadImage(FileUpload, Server.MapPath(ConfigurationManager.AppSettings["uploadfolder"]));

                if (SaveAndCountinue)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction("Create");
                }

                return Redirect(url);
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(obj);
            }
        }

        #endregion Thêm mới

        #region Cập nhật

        /// <summary>
        /// Giao diện cập nhật
        /// </summary>
        /// <modified>
        /// Author          Date            Comments
        /// TrungNQ         04/08/2017      Tạo mới
        /// </modified>
        /// <param name="id">Id bản ghi</param>
        /// <param name="pageNumber">Trang hiện tại</param>
        /// <returns></returns>
        [CheckSessionLogin]
        [CheckAuthorize]
        [HttpGet]
        public ActionResult Update(string id)
        {
            var str = "";

            var obj = _UserService.GetById(id);

            var list = _UserRoleService.GetAllByUserId(obj.Id);
            if (list.Any())
            {
                foreach (var item in list)
                {
                    str += item.RoleId + ",";
                }
            }

            ViewBag.Selected = str;
            ViewBag.urlValue = url;

            return View(obj);
        }

        /// <summary>
        /// Cập nhật
        /// </summary>
        /// <modified>
        /// Author          Date            Comments
        /// TrungNQ         04/08/2017      Tạo mới
        /// </modified>
        /// <param name="obj">Đối tượng</param>
        /// <param name="objId">Id bản ghi hiện tại</param>
        /// <param name="pass">Mật khẩu</param>
        /// <param name="rolevalues">Danh sách quyền</param>
        /// <param name="FileUpload">File Upload</param>
        /// <param name="pageNumber">Trang hiện tại</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Update(User obj, string pass, string repass, string rolevalues, HttpPostedFileBase FileUpload)
        {
            //
            ViewBag.urlValue = url;
            ViewBag.Selected = rolevalues;

            //Kiểm tra
            var oldObj = _UserService.GetById(obj.Id);
            if (oldObj == null)
            {
                ViewBag.Error = "Bản ghi không tồn tại";
                return View(obj);
            }

            if (!ModelState.IsValid)
            {
                return View(oldObj);
            }

            var isExisted = _UserService.GetByUserName_Id(oldObj.Username, oldObj.Id.ToString());
            if (isExisted != null)
            {
                ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại");
                return View(oldObj);
            }

            //Gán giá trị
            oldObj.Username = obj.Username;
            oldObj.Active = obj.Active;
            oldObj.Admin = obj.Admin;

            //Kiểm tra là tài khoản admin
            var currentUser = GetCurrentUser.GetUser();
            if (currentUser.Admin)
            {
                if (oldObj.Id.ToString().Equals(currentUser.Id.ToString()))
                {
                    //oldObj.isAdmin = obj.isAdmin;

                    CreatSession(oldObj);
                }
            }

            //Cập nhật quyền
            if (!string.IsNullOrWhiteSpace(rolevalues))
            {
                var list = _UserRoleService.GetAllByUserId(oldObj.Id).ToList();
                foreach (var item in list)
                {
                    _UserRoleService.DeleteById(item.Id);
                }

                var ids = rolevalues.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (ids.Any())
                {
                    foreach (var id in ids)
                    {
                        var UserRole = new UserRole();
                        UserRole.Id = Common.GenerateId();
                        UserRole.RoleId = id;
                        UserRole.UserId = obj.Id;

                        _UserRoleService.Create(UserRole);
                    }
                }
            }

            //Password
            if (!string.IsNullOrWhiteSpace(pass))
            {
                if (!string.IsNullOrWhiteSpace(repass))
                {
                    if (pass.Equals(repass))
                    {
                        oldObj.Password = pass.PasswordHashed(oldObj.PasswordSalat);
                    }
                    else
                    {
                        ModelState.AddModelError("repass", "Vui lòng nhập lại đúng mật khẩu.");
                        return View(oldObj);
                    }
                }
                else
                {
                    ModelState.AddModelError("repass", "Vui lòng nhập lại mật khẩu.");
                    return View(oldObj);
                }
            }

            //File upload
            if (FileUpload != null)
            {
                var extension = Path.GetExtension(FileUpload.FileName) ?? "";
                var fileName = Path.GetFileName(string.Format("{0}{1}", StringUtil.RemoveSpecialCharactersVn(FileUpload.FileName.Replace(extension, "")).GetNormalizeString(), extension));
                oldObj.UserAvatar = string.Format("{0}{1}", ConfigurationManager.AppSettings["uploadfolder"], fileName);
            }

            //Thực hiện cập nhật
            var result = _UserService.Update(oldObj);
            if (result.isSuccess)
            {
                return Redirect(url);
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(oldObj);
            }
        }

        #endregion Cập nhật

        #region Xóa

        /// <summary>
        /// Xóa
        /// </summary>
        /// <modified>
        /// Author          Date            Comments
        /// TrungNQ         04/08/2017      Tạo mới
        /// </modified>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult Delete(string id)
        {
            var obj = _UserService.GetById(id);
            if (obj.Admin)
            {
                var message = new MessageReport();

                message.isSuccess = false;
                message.Message = "Tài khoản là quyền admin. Không thể xóa";

                return Json(message, JsonRequestBehavior.AllowGet);
            }

            var result = _UserService.DeleteById(id);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion Xóa

        #region Danh sách quyền hạn trong thêm mới, cập nhật

        /// <summary>
        /// Danh sách quyền
        /// </summary>
        /// <modified>
        /// Author          Date            Comments
        /// TrungNQ         04/08/2017      Tạo mới
        /// </modified>
        /// <param name="roles">Danh sách quyền đã chọn</param>
        /// <returns></returns>
        public PartialViewResult RoleListChoice(string roles)
        {
            ViewBag.Selected = roles;
            var list = _RoleService.GetAllActive();
            return PartialView(list);
        }

        #endregion Danh sách quyền hạn trong thêm mới, cập nhật

        private void CreatSession(User obj)
        {
            var host = Request.Url.Host;
            // Create user login
            var userLogin = new User
            {
                Id = obj.Id,
                Admin = obj.Admin,
                UserAvatar = obj.UserAvatar,
                Username = obj.Username
            };

            Session[string.Format("{0}_{1}", SessionConfig.MemberSession, host)] = userLogin;
        }

        #region Cập nhập thông tin người dùng đăng nhập

        /// <summary>
        /// Form sửa thông tin người dùng đăng nhập
        /// </summary>
        /// <modified>
        /// Author              Date            Comments
        /// TrungNQ             10/12/2016      Tạo mới
        /// </modified>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [CheckSessionLogin]
        public ActionResult UserProfile(string id, int tabIndex = 1)
        {
            ViewBag.TabIndex = tabIndex;
            var obj = _UserService.GetById(id);
            if (obj != null)
            {
                return View(obj);
            }
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Thực hiện cập nhật
        /// </summary>
        /// <modified>
        /// Author              Date            Comments
        /// TrungNQ             10/12/2016      Tạo mới
        /// </modified>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UserProfile(User obj, string oldpass, string newpass, string repass, HttpPostedFileBase fileAvatar, FormCollection formCollection, string areacode, int tabIndex = 1)
        {
            //Khai báo biến
            string error = string.Empty;

            var oldObj = _UserService.GetById(obj.Id);
            if (oldObj == null)
            {
                ViewBag.Error = "Bản ghi không tồn tại";
                return View(obj);
            }

            var usernameExist = _UserService.GetByUserName_Id(obj.Username, oldObj.Id);
            if (usernameExist != null)
            {
                ViewBag.Error = "Username đã tồn tại. Vui lòng nhập lại.";
                return View(oldObj);
            }

            //Kiểm tra tồn tại email ở bản ghi khác
            if (!string.IsNullOrWhiteSpace(obj.Email))
            {
                var objExist = _UserService.GetByEmail_Id(obj.Email, oldObj.Id);

                //Tồn tại thì báo trùng
                if (objExist != null)
                {
                    ViewBag.Error = "Email đã tồn tại. Vui lòng nhập lại.";
                    return View(oldObj);
                }
            }

            if (fileAvatar != null)
            {
                oldObj.UserAvatar = string.Format("{0}/{1}", ConfigurationManager.AppSettings["uploadfolder"], Common.UploadImages(out error, Server.MapPath(ConfigurationManager.AppSettings["uploadfolder"]), fileAvatar));
            }

            oldObj.Username = obj.Username;
            oldObj.Name = obj.Name;
            oldObj.Email = obj.Email;
            oldObj.Phone = obj.Phone;

            if (!string.IsNullOrWhiteSpace(newpass) && !string.IsNullOrWhiteSpace(repass))
            {
                oldpass = oldpass.PasswordHashed(oldObj.PasswordSalat);

                //So khớp
                if (oldpass.Equals(oldObj.Password))
                {
                    //Kiểm tra đã nhập đúng lại mật khẩu mới chưa
                    if (newpass.Equals(repass))
                    {
                        oldObj.Password = newpass;
                        oldObj.Password = oldObj.Password.PasswordHashed(oldObj.PasswordSalat);
                    }
                    else
                    {
                        ViewBag.Error = "Vui lòng nhập lại đúng mật khẩu mới";
                        return View(oldObj);
                    }
                }
                else
                {
                    ViewBag.Error = "Vui lòng nhập lại đúng mật khẩu cũ.";
                    return View(oldObj);
                }
            }

            var isSuccess = _UserService.Update(oldObj);
            if (isSuccess.isSuccess)
            {

                //Xóa cache
                var formatUserId = string.Format("{0}_{1}", Kztek.Web.Core.Models.ConstField.MemCacheMember, oldObj.Id);

                CacheLayer.Clear(formatUserId);

                //ViewBag.Message = "Cập nhật thành công";
                //return View(oldObj);
                TempData["Success"] = "Cập nhật thành công";
                return RedirectToAction("UserProfile", "User", new { id = obj.Id, tabIndex = tabIndex });
            }
            else
            {
                ViewBag.Error = "Có lỗi xảy ra trong quá trình cập nhật";
                return View(oldObj);
            }
        }

        #endregion Cập nhập thông tin người dùng đăng nhập

        #region Restore mật khẩu

        /// <summary>
        /// Reset mật khẩu về mđ "123456"
        /// </summary>
        /// <modified>
        /// Author          Date            Comments
        /// TrungNQ         04/08/2017      Tạo mới
        /// </modified>
        /// <param name="id">Id bản ghi</param>
        /// <returns></returns>
        public JsonResult RestorePassToDefault(string id)
        {
            var obj = _UserService.GetById(id);
            if (obj == null)
            {
                return Json(new MessageReport(false, "Bản ghi không tồn tại"), JsonRequestBehavior.AllowGet);
            }

            var newpass = "123456";

            obj.Password = newpass.PasswordHashed(obj.PasswordSalat);
            var result = _UserService.Update(obj);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion Restore mật khẩu
    }
}