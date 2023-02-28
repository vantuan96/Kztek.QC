using Kztek.Data.SqlHelper;
using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Web.Core.Extensions;
using Kztek.Web.Core.Helpers;
using Kztek.Web.Core.Models;
using Kztek.Web.Models;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Kztek.Web.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        #region Khai báo services

        private IUserService _UserService;
        private IRoleService _RoleService;
        private IUserRoleService _UserRoleService;
        private IUserConfigService _UserConfigService;
        private ILogService _LogService;

        public LoginController(IUserService _UserService, IRoleService _RoleService, IUserRoleService _UserRoleService, IUserConfigService _UserConfigService, ILogService _LogService)
        {
            this._UserService = _UserService;
            this._RoleService = _RoleService;
            this._UserRoleService = _UserRoleService;
            this._UserConfigService = _UserConfigService;
            this._LogService = _LogService;
        }

        #endregion Khai báo services

        #region Đăng nhập

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(AdminLoginModel obj, bool remember, FormCollection form, string areacode)
        {
            //Kiểm tra hợp lệ
            if (ModelState.IsValid)
            {
                //Lấy khách hàng qua email
                var userInfo = _UserService.GetByUserNameOREmail(obj.UserName);

                //Không có thì báo lỗi
                if (userInfo == null)
                {
                    ModelState.AddModelError("UserName", "UserName không tồn tại hoặc nhập sai. Vui lòng kiểm tra lại!");
                    return View(obj);
                }
                else
                {
                    //Kiểm tra tài khoản đã kích hoạt
                    if (userInfo.Active)
                    {
                        //Có thì so khớp password
                        var password = obj.Password.PasswordHashed(userInfo.PasswordSalat);
                        if (!password.Equals(userInfo.Password))
                        {
                            ModelState.AddModelError("UserName", "Email hoặc mật khẩu không đúng");
                            return View(obj);
                        }
                        else
                        {
                            //Khớp thì lưu vào session
                            createSession(remember, userInfo);

                            MessageReport report = new MessageReport(true, "Đăng nhập thành công");

                            _LogService.WriteLog(report, "User", userInfo.Id, ActionConfig.Login, userInfo);

                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "Tài khoản chưa được kích hoạt");
                        return View(obj);
                    }
                }
            }


            ModelState.AddModelError("error", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");

            return View(obj);
        }

        #endregion Đăng nhập

        #region Đăng xuất

        /// <summary>
        /// Đăng xuất tài khoản
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            var host = Request.Url.Host;
            Session.Abandon();
            var cookie = new HttpCookie(string.Format("{0}_{1}", SessionConfig.MemberCookies, host));
            if (cookie == null)
                return RedirectToAction("Index", "Login");

            cookie.Expires = DateTime.Now.AddDays(-1d);
            Response.Cookies.Add(cookie);

            CacheLayer.Clear(ConstField.AllListMenuFunctionCache);

            return RedirectToAction("Index", "Login");
        }

        #endregion Đăng xuất

        #region Create Sesion + Cookie

        /// <summary>
        /// Lưu Session
        /// </summary>
        /// <param name="rememberMe"></param>
        /// <param name="passwordEntered"></param>
        /// <param name="user"></param>
        [NonAction]
        private void createSession(bool rememberMe, User user)
        {
            var host = Request.Url.Host;
            // Create user login
            var userLogin = new User
            {
                Id = user.Id,
                Name = user.Name,
                Admin = user.Admin,
                UserAvatar = user.UserAvatar,
                Username = user.Username
                //Password = user.Password
            };

            Session[string.Format("{0}_{1}", SessionConfig.MemberSession, host)] = userLogin;
            if (rememberMe)
            {
                var cookies = new HttpCookie(string.Format("{0}_{1}", SessionConfig.MemberCookies, host));
                cookies["cp_UserId"] = user.Id.ToString(CultureInfo.InvariantCulture);
                cookies.Expires = DateTime.Now.AddMonths(1);
                Response.Cookies.Add(cookies);
            }

            //Xóa cache cũ để cập nhật mới nhất
            var formatUserId = string.Format("{0}_{1}", Kztek.Web.Core.Models.ConstField.MemCacheMember, user.Id);

            CacheLayer.Clear(formatUserId);
        }

        #endregion Create Sesion + Cookie

        #region reCaptcha

        public bool reCaptCha(FormCollection form)
        {
            //Thông tin google recaptcha
            string secret = ConfigurationManager.AppSettings["SecretKey"];
            var response = form["g-recaptcha-response"];

            //Khai báo
            var client = new WebClient();
            var reply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));
            var captchaResponse = JsonConvert.DeserializeObject<RecaptchaResult>(reply);

            //Bắt lỗi
            if (!captchaResponse.Success)
            {
                //Lấy lỗi
                var error = captchaResponse.ErrorCodes[0].ToLower();
                //Kiểm tra xem là lỗi nào để thông báo
                switch (error)
                {
                    case ("missing-input-secret"):
                        TempData["CaptChaMessage"] = "The secret parameter is missing.";
                        break;

                    case ("invalid-input-secret"):
                        TempData["CaptChaMessage"] = "The secret parameter is invalid or malformed.";
                        break;

                    case ("missing-input-response"):
                        TempData["CaptChaMessage"] = "The response parameter is missing.";
                        break;

                    case ("invalid-input-response"):
                        TempData["CaptChaMessage"] = "The response parameter is invalid or malformed.";
                        break;

                    default:
                        TempData["CaptChaMessage"] = "Error occured. Please try again";
                        break;
                }
            }

            //Trả kết quả
            return Convert.ToBoolean(captchaResponse.Success);
        }

        #endregion reCaptcha

        #region Check số lần đăng nhập khi đăng nhập thất bại

        private int CheckLogin(string ip)
        {
            int i = 0;
            if (Session[string.Format("{0}_{1}", "Login", ip)] != null)
            {
                i = Convert.ToInt16(Session[string.Format("{0}_{1}", "Login", ip)]);
                i++;
                Session[string.Format("{0}_{1}", "Login", ip)] = i;
            }
            else
            {
                i++;
                Session[string.Format("{0}_{1}", "Login", ip)] = i;
            }
            return i;
        }

        #endregion Check số lần đăng nhập khi đăng nhập thất bại

        #region Đăng ký

        /// <summary>
        /// Form đăng ký
        /// </summary>
        /// <modified>
        /// Author              Date            Comments
        /// TrungNQ             18/12/2016      Tạo mới
        /// </modified>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Thực hiện đăng ký
        /// </summary>
        /// <modified>
        /// Author              Date            Comments
        /// TrungNQ             18/12/2016      Tạo mới
        /// </modified>
        /// <param name="obj"></param>
        /// <param name="active"></param>
        /// <param name="admin"></param>
        /// <param name="repass"></param>
        /// <param name="agree"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Register(User obj, bool active, bool admin, string repass)
        {
            //Kiểm tra
            if (!string.IsNullOrWhiteSpace(obj.Password) || !string.IsNullOrWhiteSpace(repass))
            {
                var existedEmail = _UserService.GetByEmail(obj.Email);
                if (existedEmail != null)
                {
                    ModelState.AddModelError("", "Email đã tồn tại");
                    return View();
                }

                //Kiểm tra email tồn tại
                var objExist = _UserService.GetByUserName(obj.Username);
                if (objExist != null)
                {
                    ModelState.AddModelError("", "Username đã tồn tại");
                    return View();
                }
                else
                {
                    if (obj.Password.Equals(repass))
                    {
                        //Gán
                        obj.Id = Common.GenerateId();
                        obj.PasswordSalat = Guid.NewGuid().ToString();
                        obj.Password = obj.Password.PasswordHashed(obj.PasswordSalat);
                        obj.DateCreated = DateTime.Now;
                        obj.Active = active;
                        obj.Admin = admin;

                        //Kiểm tra hợp lệ
                        if (ModelState.IsValid)
                        {
                            var isSuccess = _UserService.Create(obj);
                            if (isSuccess.isSuccess)
                            {
                                GenerateData(obj.Id);

                                TempData["Success"] = isSuccess.Message;
                                return RedirectToAction("Index", "Login");
                            }
                            else
                            {
                                ModelState.AddModelError("", isSuccess.Message);
                                return View();
                            }
                        }
                        ModelState.AddModelError("", "Có lỗi xảy ra. Vui lòng kiểm tra lại");
                        return View();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Nhập lại đúng mật khẩu");
                        return View();
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Vui lòng nhập mật khẩu và nhập lại mật khẩu.");
                return View();
            }
        }

        #endregion Đăng ký

        #region Clean session login khi đã đăng nhập thành công

        private void CleanSession(string ip)
        {
            Session.Remove(string.Format("{0}_{1}", "Login", ip));
        }

        #endregion Clean session login khi đã đăng nhập thành công

        #region Generate data

        /// <summary>
        /// Generate data khi đăng ký tài khoản đầu tiên
        /// </summary>
        /// <param name="userid"></param>
        public void GenerateData(string userid)
        {
            //Data
            //string script = StringUtil.GetTextFile(Server.MapPath("~/Templates/DataGenerate/DataGenerate.sql"));
            //ExcuteSQL.Execute(script);

            //Trigger
            string strInsert = StringUtil.GetTextFile(Server.MapPath("~/Templates/DataGenerate/TriggerInsertMenuFunction.sql"));
            ExcuteSQL.Execute(strInsert);
            string strUpdate = StringUtil.GetTextFile(Server.MapPath("~/Templates/DataGenerate/TriggerUpdateMenuFunction.sql"));
            ExcuteSQL.Execute(strUpdate);

            var list = _RoleService.GetAllActive();
            foreach (var item in list.ToList())
            {
                UserRole objJoin = new UserRole();
                objJoin.Id = Common.GenerateId();
                objJoin.UserId = userid;
                objJoin.RoleId = item.Id;
                _UserRoleService.Create(objJoin);
            }
        }

        #endregion Generate data

        #region Check tài khoản

        /// <summary>
        /// Check tài khoản login
        /// </summary>
        /// <param name="email"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public JsonResult CheckAccount(string username, string pass)
        {
            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(pass))
            {
                var user = _UserService.GetByUserNameOREmail(username);
                if (user != null)
                {
                    pass = pass.PasswordHashed(user.PasswordSalat);
                    if (pass.Equals(user.Password))
                    {
                        return Json(new { isSuccess = true, userid = user.Id, admin = user.Admin }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(new { isSuccess = false, userid = "", admin = false });
        }

        #endregion Check tài khoản
    }
}