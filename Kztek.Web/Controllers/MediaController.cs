using Kztek.Service.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kztek.Web.Controllers
{
    public class MediaController : Controller
    {
        private IMediaService _MediaService;
        public MediaController(IMediaService _MediaService)
        {
            
            this._MediaService = _MediaService;
           
        }
        // GET: Media
        public ActionResult Index()
        {
            var list = _MediaService.GetAllImageVideo();
            return View(list);
        }
    }
}