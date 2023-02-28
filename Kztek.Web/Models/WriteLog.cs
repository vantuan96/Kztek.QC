using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Web.Core.Helpers;
using log4net;
using System.Web;

namespace Kztek.Web.Models
{
    public class WriteLog
    {
        private static WriteLog _Instance = new WriteLog();

        public static WriteLog Instance
        {
            get { return WriteLog._Instance; }
            set { WriteLog._Instance = value; }
        }

        public void Write(MessageReport report, User currentuser, string objId, string objname, string classname)
        {
            var computername = Common.GetComputerName(HttpContext.Current.Request.UserHostAddress);

            //Lấy classname đang giao tiếp
            ILog log = log4net.LogManager.GetLogger(classname);

            if (report.isSuccess)
            {
                log.InfoFormat("{0}: {1} ({2}_{3}) - {4}", currentuser.Name, report.Message, objId, objname, !string.IsNullOrWhiteSpace(computername) ? computername : "");
            }
            else
            {
                log.ErrorFormat("{0}: {1} ({2}_{3}) - {4}", currentuser.Name, report.Message, objId, objname, !string.IsNullOrWhiteSpace(computername) ? computername : "");
            }
        }
    }
}