using System.Web;
using System.Web.Optimization;

namespace Kztek.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Areas/Admin/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Areas/Admin/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Areas/Admin/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Areas/Admin/Scripts/bootstrap.js",
                      "~/Areas/Admin/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Areas/Admin/Content/bootstrap.css",
                      "~/Areas/Admin/Content/site.css"));

            //ACE css
            bundles.Add(new StyleBundle("~/Content/AdminLayout/Admincss").Include(
                      "~/Areas/Admin/Content/AdminLayout/css/bootstrap.css",
                      "~/Areas/Admin/Content/AdminLayout/css/colorbox.css",
                      "~/Areas/Admin/Content/AdminLayout/css/ace.css",
                      "~/Areas/Admin/Content/AdminLayout/css/ace-rtl.css",
                      "~/Areas/Admin/Content/AdminLayout/css/daterangepicker.css",
                      "~/Areas/Admin/Content/AdminLayout/css/chosen.css",
                      "~/Areas/Admin/Content/AdminLayout/css/jquery-ui.css",
                      "~/Areas/Admin/Content/AdminLayout/css/jquery-ui.custom.css",
                      "~/Areas/Admin/Content/AdminLayout/css/bootstrap-timepicker.css",
                      "~/Areas/Admin/Content/AdminLayout/css/bootstrap-multiselect.css"
                ));

            bundles.Add(new StyleBundle("~/Content/AdminLayout/AdminExtcss").Include(
                    "~/Areas/Admin/Content/NewConfigStyle.css",
                    "~/Areas/Admin/Content/ToastrJquery/toastr.css"
                ));

            bundles.Add(new StyleBundle("~/Content/Akamecss").Include(                   
                     "~/Templates/akame/style.css" ,
                     "~/Content/ToastrJquery/toastr.css"
               ));

            //ACE js
            bundles.Add(new ScriptBundle("~/Content/AdminLayout/Adminjs").Include(
                      "~/Areas/Admin/Content/AdminLayout/js/bootstrap.js",
                      "~/Areas/Admin/Content/AdminLayout/js/jquery.colorbox.js",
                      "~/Areas/Admin/Content/AdminLayout/js/ace-elements.js",
                      "~/Areas/Admin/Content/AdminLayout/js/ace.js",
                      "~/Areas/Admin/Content/AdminLayout/js/moment.js",
                      "~/Areas/Admin/Content/AdminLayout/js/ace-extra.js",
                      "~/Areas/Admin/Content/AdminLayout/js/daterangepicker.js",
                      "~/Areas/Admin/Content/AdminLayout/js/chosen.jquery.js",
                      "~/Areas/Admin/Content/AdminLayout/js/bootstrap-tag.js",
                      "~/Areas/Admin/Content/AdminLayout/js/bootbox.js",
                      "~/Areas/Admin/Content/AdminLayout/js/jquery-ui.js",
                      "~/Areas/Admin/Content/AdminLayout/js/jquery-ui.custom.js",
                      "~/Areas/Admin/Content/AdminLayout/js/spinbox.js",
                      "~/Areas/Admin/Content/AdminLayout/js/bootstrap-timepicker.js",
                      "~/Areas/Admin/Content/AdminLayout/js/bootstrap-multiselect.js",
                      "~/Areas/Admin/Content/AdminLayout/js/jquery.maskedinput.js",
                      "~/Areas/Admin/Content/AdminLayout/js/bootbox.js"
                ));

            bundles.Add(new ScriptBundle("~/Content/Akamejs").Include(
                    "~/Templates/akame/js/jquery.min.js",
                    "~/Templates/akame/js/bootstrap.min.js",
                    "~/Templates/akame/js/popper.min.js",
                    "~/Templates/akame/js/akame.bundle.js",
                    "~/Templates/akame/js/default-assets/active.js",
                    "~/Scripts/Ext/Contact.js",
                     "~/Scripts/Ext/Comment.js",
                    "~/Scripts/ToastrJquery/toastr.js"

              ));

            bundles.Add(new ScriptBundle("~/Content/AdminLayout/AdminExtjs").Include(
                      "~/Areas/Admin/Scripts/Ext/Common.js",
                      //"~/Scripts/function.Ext.js",
                      "~/Areas/Admin/Scripts/ToastrJquery/toastr.js",
                //"~/Scripts/jquery.signalR-2.2.2.js",
                //"~/Scripts/AjaxPaging/jquery.twbsPagination.js"
                "~/Areas/Admin/Scripts/jquery.mask.js"
                ));

            //bundles.Add(new ScriptBundle("~/bundles/inputmask").Include(
            //"~/Scripts/InputMask/inputmask.js",
            //"~/Scripts/InputMask/jquery.inputmask.js"
            ////"~/Scripts/jInputMask/inputmask.extensions.js",
            ////"~/Scripts/InputMask/inputmask.date.extensions.js",
            //////and other extensions you want to include
            ////"~/Scripts/InputMask/inputmask.numeric.extensions.js"
            //));
		BundleTable.EnableOptimizations = false;
        }
    }
}
