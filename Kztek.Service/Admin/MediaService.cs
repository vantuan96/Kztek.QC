/* **************************************
* HỆ THỐNG GENCODE TỰ ĐỘNG
* CREATE: 04/06/2019 3:12:27 PM
* AUTHOR: HNG-0988388000
*/
using Kztek.Data.Infrastructure;
using Kztek.Data.Repository;
using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Models;
using Kztek.Web.Core.Helpers;
using Kztek.Data.SqlHelper;
using PagedList;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
namespace Kztek.Service.Admin
{
    public interface IMediaService
    {
        IQueryable<Media> GetAll();
        IQueryable<Media> GetAllActive();
        IPagedList<Media> GetAllPagingByFirst(string key, int pageNumber, int pageSize);
        Media GetById(string Id);
        MessageReport Create(Media obj);
        MessageReport Update(Media obj);
        MessageReport DeleteById(string Id);
        MessageReport DeleteByIds(string lstId);
        //Phân trang dạng T-SQL
        List<Media> GetAllPagingListTSQL(string key, string mediaPage, string position, string filter, int pageNumber, int pageSize, ref int total);
        IQueryable<Media> GetBanner(string page, string Position, string type);
        List<Media> GetImageHome();
        List<Media> GetAllImageVideo();
    }

    public class MediaService : IMediaService
    {
        private readonly IMediaRepository _MediaRepository;
        private readonly ILogService _LogService;
        private readonly IUnitOfWork _UnitOfWork;
        public MediaService(IMediaRepository _MediaRepository, ILogService _LogService, IUnitOfWork _UnitOfWork)
        {
            this._MediaRepository = _MediaRepository;
            this._LogService = _LogService;
            this._UnitOfWork = _UnitOfWork;
        }
        private User user = GetCurrentUser.GetUser();
        private void ClearCache()
        {
            CacheLayer.Clear("Media_Cache");
        }
        public IQueryable<Media> GetAll()
        {
            var query = from n in _MediaRepository.Table orderby n.DateCreated descending select n;
            return query;
        }
        public IQueryable<Media> GetAllActive()
        {
            var query = from n in _MediaRepository.Table where n.Active == true orderby n.DateCreated descending select n;
            return query;
        }

        public IPagedList<Media> GetAllPagingByFirst(string key, int pageNumber, int pageSize)
        {
            var query = from n in _MediaRepository.Table select n;
            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.Name.Contains(key));
            }
            var list = new PagedList<Media>(query.OrderByDescending(n => n.DateCreated), pageNumber, pageSize);
            return list;
        }

        public Media GetById(string Id)
        {
            return _MediaRepository.GetById(Id);
        }

        public MessageReport Create(Media obj)
        {
            MessageReport report;
            try
            {
                _MediaRepository.Add(obj);
                Save();
                report = new MessageReport(true, "Thêm thành công");
            }
            catch (Exception ex)
            {
                report = new MessageReport(false, ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
            }
            _LogService.WriteLog(report, "Media", obj.Id, ActionConfig.Create, user);
            return report;
        }

        public MessageReport Update(Media obj)
        {
            MessageReport report;
            try
            {
                _MediaRepository.Update(obj);
                Save();
                report = new MessageReport(true, "Cập nhật thành công");
            }
            catch (Exception ex)
            {
                report = new MessageReport(false, ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
            }
            _LogService.WriteLog(report, "Media", obj.Id, ActionConfig.Update, user);
            return report;
        }

        public MessageReport DeleteById(string Id)
        {
            MessageReport report;
            try
            {
                var obj = _MediaRepository.GetById(Id);
                if (obj != null)
                {
                    _MediaRepository.Delete(obj);
                    Save();
                    report = new MessageReport(true, "Xóa thông tin thành công");
                    _LogService.WriteLog(report, "Media", obj.Id, ActionConfig.Delete, user);
                }
                else
                {
                    report = new MessageReport(false, "Thông tin này không tồn tại");
                }
            }
            catch (Exception ex)
            {
                report = new MessageReport(false, ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
            }
            return report;
        }
        public MessageReport DeleteByIds(string lstId)
        {
            var report = new MessageReport(false, "Có lỗi xảy ra");
            try
            {
                string[] ids = lstId.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var id in ids)
                {
                    var objDelete = _MediaRepository.GetById(id);
                    if (objDelete != null)
                    {
                        _MediaRepository.Delete(objDelete);
                        Save();
                        report = new MessageReport(true, "Xóa thành công");
                        _LogService.WriteLog(report, "Media", objDelete.Id, ActionConfig.Delete, user);
                    }
                }
                report = new MessageReport(true, "Xóa thành công");
            }
            catch (Exception ex)
            {
                report = new MessageReport(false, ex.Message);
            }
            ClearCache();
            return report;
        }
        //Save change
        public void Save()
        {
            _UnitOfWork.Commit();
        }
        public List<Media> GetAllPagingListTSQL(string key, string mediaPage, string position, string filter, int pageNumber, int pageSize, ref int total)
        {
            if (!string.IsNullOrWhiteSpace(key) && FunctionHelper.DetectSqlInjection(key))
                key = "";
            if (!string.IsNullOrWhiteSpace(filter) && FunctionHelper.DetectSqlInjection(filter))
                filter = "";

            //Lấy danh sách
            var sb = new StringBuilder();
            var _sort = "DateCreated";
            var _order = "desc";
            if (!string.IsNullOrWhiteSpace(filter))
            {
                _sort = filter.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries)[0];
                _order = filter.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries)[1];
            }
            sb.AppendLine("SELECT * FROM(");
            sb.AppendLine(string.Format("SELECT ROW_NUMBER() OVER(ORDER BY c.{0} {1}) AS RowNumber, *", _sort, _order));
            sb.AppendLine("FROM Media c WITH(NOLOCK) WHERE 1=1 ");

            if (!string.IsNullOrWhiteSpace(key))
                sb.AppendLine(string.Format(" AND c.[Id] like '%{0}%'  OR c.[Name] like N'%{0}%'", key));
            if (!string.IsNullOrWhiteSpace(mediaPage))
                sb.AppendLine(string.Format(" AND c.[Page] like '%{0}%'", mediaPage));
            if (!string.IsNullOrWhiteSpace(mediaPage))
                sb.AppendLine(string.Format(" AND c.[Position] like '%{0}%'", position));

            sb.AppendLine(") as a");
            sb.AppendLine(string.Format("WHERE RowNumber BETWEEN(({0}-1) * {1}+1) AND({0} * {1})", pageNumber, pageSize));
            var listData = SqlExQuery<Media>.ExcuteQuery(sb.ToString());
            //Tính tổng
            sb.Clear();
            sb.AppendLine("SELECT COUNT(*) TotalCount FROM Media c WITH(NOLOCK) WHERE 1=1 ");
            if (!string.IsNullOrWhiteSpace(key))
                sb.AppendLine(string.Format(" AND c.[Id] like '%{0}%'  AND c.[Name] like N'%{0}%'", key));
            var _total = SqlExQuery<TotalPaging>.ExcuteQueryFirst(sb.ToString());
            total = _total != null ? _total.TotalCount : 0;
            return listData;
        }

        public IQueryable<Media> GetBanner(string page, string Position, string type)
        {
            var query = from n in _MediaRepository.Table
                        where n.Active == true && n.Page == page && n.Position == Position && n.MediaType == type
                        orderby n.Ordering ascending
                        select n;
            return query;
        }

        public List<Media> GetImageHome()
        {
            //Lấy danh sách
            var sb = new StringBuilder();

            sb.AppendLine("select Top 4 * from Media where MediaType = '0' and Page = 'Home' and Position='Center'");
            sb.AppendLine("union");
            sb.AppendLine("select Top 1 * from Media where MediaType = '1' and Page = 'Home' and Position='Center'");
            var listData = SqlExQuery<Media>.ExcuteQuery(sb.ToString());

            return listData;
        }
        public List<Media> GetAllImageVideo()
        {
            var query = from n in _MediaRepository.Table
                        where n.Active == true && (n.MediaType == "0" || n.MediaType == "1")
                        orderby n.DateCreated descending
                        select n;
            return query.ToList();
        }
    }
}
