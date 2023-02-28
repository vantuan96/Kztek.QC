using Kztek.Service.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kztek.Web.Controllers
{
    public class ProductController : Controller
    {
        private IProductService _ProductService;
        public ProductController(IProductService _ProductService)
        {
            this._ProductService = _ProductService;
        }

        // GET: Product
        public ActionResult Index()
        {
           var obj = _ProductService.GetAll().ToList();
            return View(obj);
        }

        public ActionResult View( string id)
        {
            var obj = _ProductService.GetById(id);
            return View(obj);
        }

        public PartialViewResult Partial_New_Product()
        {
            var obj = _ProductService.Get_Top3_New().ToList();
            return PartialView(obj);
        }

        public ActionResult ProductByCategory(string groupId)
        {
            var obj = _ProductService.GetListProcutByGroupId(groupId).ToList();
            return View();
        }
    }
}
