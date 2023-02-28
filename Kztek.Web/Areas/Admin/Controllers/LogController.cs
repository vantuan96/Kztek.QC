using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Web.Core.Extensions;
using Kztek.Web.Core.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kztek.Web.Areas.Admin.Controllers
{
    public class LogController : Controller
    {
        private ILogService _LogService;
        private IUserService _UserService;

        public LogController(ILogService _LogService, IUserService _UserService)
        {
            this._LogService = _LogService;
            this._UserService = _UserService;
        }

        private static string url = "";

        public ActionResult Index(string key, string fromdate, string todate, string actions, string users, int page = 1)
        {
            //Khai báo
            int pageSize = 20;

            //Lấy danh sách phân trang
            var list = _LogService.GetAllPagingByFirst(key, fromdate, todate, actions, users, page, pageSize);

            //Đổ lên grid
            var gridModel = PageModelCustom<Log>.GetPage(list, page, pageSize);

            //ViewBag
            ViewBag.keyValue = key;
            ViewBag.actionsValue = actions;
            ViewBag.usersValue = users;
            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = todate;

            url = Request.Url.PathAndQuery;

            ViewBag.actionList = FunctionHelper.LogActions().ToDataTableNullable();
            ViewBag.userList = _UserService.GetAllActive().ToDataTableNullable();

            return View(gridModel);
        }
    }
}