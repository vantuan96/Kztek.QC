using Kztek.Data.Repository;
using Kztek.Data.SqlHelper;
using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Helpers;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Kztek.Service.Admin
{
    public interface ILogService
    {
        IPagedList<Log> GetAllPagingByFirst(string key, string fromdate, string todate, string actions, string users, int pageNumber, int pageSize);
        void WriteLog(MessageReport message, string tableName, string columnId, string actionV, User user);
    }

    public class LogService : ILogService
    {
        private ITrashService _TrashService;
        private ILogRepository _LogRepository;
        public LogService(ITrashService _TrashService, ILogRepository _LogRepository)
        {
            this._TrashService = _TrashService;
            this._LogRepository = _LogRepository;
        }
        public IPagedList<Log> GetAllPagingByFirst(string key, string fromdate, string todate, string actions, string users, int pageNumber, int pageSize)
        {
            var query = from n in _LogRepository.Table
                        select n;

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                var fdate = Convert.ToDateTime(fromdate);
                var tdate = Convert.ToDateTime(todate).AddDays(1);

                query = query.Where(n => n.DateCreated >= fdate && n.DateCreated < tdate);
            }
            else
            {
                var fdate = DateTime.Now;
                var tdate = DateTime.Now.AddDays(1);

                query = query.Where(n => n.DateCreated >= fdate && n.DateCreated < tdate);
            }

            if (!string.IsNullOrWhiteSpace(key))
            {
                key = key.ToLower();
                query = query.Where(n => n.TableName.ToLower().Contains(key) || n.UserName.ToLower().Contains(key) || n.Action.ToLower().Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(actions))
            {
                query = query.Where(n => actions.Contains(n.Action));
            }

            if (!string.IsNullOrWhiteSpace(users))
            {
                query = query.Where(n => users.Contains(n.UserId));
            }

            var list = new PagedList<Log>(query.OrderByDescending(n => n.DateCreated), pageNumber, pageSize);

            return list;
        }
        public void WriteLog(MessageReport message, string tableName, string columnId, string actionV, User user)
        {
            var thre = new Thread(() =>
            {
                //Đọc địa chỉ máy tính
                //var computername = Common.GetComputerName(HttpContext.Current.Request.UserHostAddress);

                //Mapping
                var t = new Log()
                {
                    Id = Common.GenerateId(),
                    Action = actionV,
                    ColumnId = columnId,
                    isSuccess = message.isSuccess,
                    Message = message.Message,
                    TableName = tableName,
                    UserId = user != null ? user.Id : "",
                    UserName = user != null ? user.Username : ""
                };

                var str = new StringBuilder();
                str.AppendLine("INSERT INTO dbo.[Log] (");

                str.AppendLine("Id, Action, ColumnId, isSuccess, Message, TableName, UserId, UserName, DateCreated");

                str.AppendLine(") VALUES (");

                str.AppendLine(string.Format("'{0}'", t.Id));
                str.AppendLine(string.Format(", N'{0}'", t.Action));
                str.AppendLine(string.Format(", N'{0}'", t.ColumnId));
                str.AppendLine(string.Format(", {0}", t.isSuccess ? 1 : 0));
                str.AppendLine(string.Format(", N'{0}'", t.Message));
                str.AppendLine(string.Format(", N'{0}'", t.TableName));
                str.AppendLine(string.Format(", N'{0}'", t.UserId));
                str.AppendLine(string.Format(", N'{0}'", t.UserName));
                str.AppendLine(", GETDATE()");

                str.AppendLine(")");

                var k = ExcuteSQL.Execute(str.ToString());
                if (k)
                {
                    if (actionV.Equals("Delete"))
                    {
                        _TrashService.SaveProcess(tableName, columnId, t.Id);
                    }
                }
            });

            thre.Start();
        }
    }
}
