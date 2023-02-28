using Kztek.Model.Models;
using Kztek.Service.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kztek.Web.Controllers
{
    public class ContactController : Controller
    {
        private IContactService _ContactService;
        private IWebInfoService _WebInfoService;
        public ContactController(IWebInfoService _WebInfoService, IContactService _ContactService)
        {
            this._WebInfoService = _WebInfoService;
            this._ContactService = _ContactService;
        }

        // GET: Contact
        public ActionResult Index()
        {
            ContactView obj = new ContactView();
            obj.Info = _WebInfoService.GetDefault();

            return View(obj);
        }
    }
}