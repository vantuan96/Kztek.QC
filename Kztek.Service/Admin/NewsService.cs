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
	public interface INewsService
	{
		IQueryable<News> GetAll();
		IQueryable<News> GetAllActive();
		IPagedList<News> GetAllPagingByFirst(string key, int pageNumber, int pageSize);
		News GetById(string Id);
		MessageReport Create(News obj);
		MessageReport Update(News obj);
		MessageReport DeleteById(string Id);
		MessageReport DeleteByIds(string lstId);
		//Phân trang dạng T-SQL
		List<News> GetAllPagingListTSQL(string key, string filter, int pageNumber, int pageSize, ref int total);
        List<News> GetNewsServiceHome();
        List<News> GetLatestNews();
        List<News> GetPagingList(string newcategory,string type, int pageNumber, int pageSize, ref int total);
        List<News> GetLatestNewsByCategory(string newcategory, string id, string type);
        News GetByTitleUrl(string titleurl);
    }

	public class NewsService : INewsService
	{
		private readonly INewsRepository _NewsRepository;
		private readonly ILogService _LogService;
		private readonly IUnitOfWork _UnitOfWork;
		public NewsService(INewsRepository _NewsRepository, ILogService _LogService, IUnitOfWork _UnitOfWork)
		{
			this._NewsRepository = _NewsRepository;
			this._LogService = _LogService;
			this._UnitOfWork = _UnitOfWork;
		}
		private User user = GetCurrentUser.GetUser();
		private void ClearCache()
		{
			CacheLayer.Clear("News_Cache");
		}
		public IQueryable<News> GetAll()
		{
			var query = from n in _NewsRepository.Table  orderby n.DateCreated descending select n;
			return query;
		}
		public IQueryable<News> GetAllActive()
		{
			var query = from n in _NewsRepository.Table where n.Active == true  orderby n.DateCreated descending select n;
			return query;
		}
		
		public IPagedList<News> GetAllPagingByFirst(string key, int pageNumber, int pageSize)
		{
			var query = from n in _NewsRepository.Table  select n;
			if (!string.IsNullOrWhiteSpace(key))
			{
				query = query.Where(n => n.Name.Contains(key));
			}
			var list = new PagedList<News>(query.OrderByDescending(n => n.DateCreated), pageNumber, pageSize);
			return list;
		}
		
		public News GetById(string Id)
		{
			return _NewsRepository.GetById(Id);
		}
		
		public MessageReport Create(News obj)
		{
			MessageReport report;
			try
			{
				_NewsRepository.Add(obj);
				Save();
				report = new MessageReport(true, "Thêm thành công");
			}
			catch (Exception ex)
			{
			report = new MessageReport(false, ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
			}
			_LogService.WriteLog(report, "News", obj.Id, ActionConfig.Create, user);
			return report;
		}
		
		public MessageReport Update(News obj)
		{
			MessageReport report;
			try
			{
				_NewsRepository.Update(obj);
				Save();
				report = new MessageReport(true, "Cập nhật thành công");
			}
			catch (Exception ex)
			{
			report = new MessageReport(false, ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
			}
			_LogService.WriteLog(report, "News", obj.Id, ActionConfig.Update, user);
			return report;
		}
		
		public MessageReport DeleteById(string Id)
		{
			MessageReport report;
			try
			{
				var obj = _NewsRepository.GetById(Id);
				if (obj != null)
				{
					_NewsRepository.Delete(obj);
					Save();
					report = new MessageReport(true, "Xóa thông tin thành công");
				_LogService.WriteLog(report, "News", obj.Id, ActionConfig.Delete, user);
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
					var objDelete = _NewsRepository.GetById(id);
				if (objDelete!=null)
				{
					_NewsRepository.Delete(objDelete);
							Save();
							report = new MessageReport(true, "Xóa thành công");
							_LogService.WriteLog(report, "News", objDelete.Id, ActionConfig.Delete, user);
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
		public List<News> GetAllPagingListTSQL(string key, string filter, int pageNumber, int pageSize, ref int total)
			{
				if (!string.IsNullOrWhiteSpace(key) && FunctionHelper.DetectSqlInjection(key))
					key = "";
				if (!string.IsNullOrWhiteSpace(filter) && FunctionHelper.DetectSqlInjection(filter))
					filter = "";
				/*
				SELECT * FROM(
				SELECT ROW_NUMBER() OVER(ORDER BY c.Id desc) AS RowNumber, *
				FROM Product c WITH(NOLOCK)
				WHERE c.[Name] LIKE N'%thuốc%' OR c.[nameUnsign] like '%thuoc%'
				) as a
				WHERE RowNumber BETWEEN((1 - 1) * 20 + 1) AND(1 * 20)
				SELECT COUNT(*) TotalCount FROM Product c WITH(NOLOCK)
				*/
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
				sb.AppendLine("FROM News c WITH(NOLOCK) WHERE 1=1 ");
				if (!string.IsNullOrWhiteSpace(key))
					sb.AppendLine(string.Format(" AND c.[Id] like '%{0}%'  OR c.[Name] like N'%{0}%'", key));
				sb.AppendLine(") as a");
				sb.AppendLine(string.Format("WHERE RowNumber BETWEEN(({0}-1) * {1}+1) AND({0} * {1})", pageNumber, pageSize));
				var listData = SqlExQuery<News>.ExcuteQuery(sb.ToString());
				//Tính tổng
				sb.Clear();
					sb.AppendLine("SELECT COUNT(*) TotalCount FROM News c WITH(NOLOCK) WHERE 1=1 ");
				if (!string.IsNullOrWhiteSpace(key))
					sb.AppendLine(string.Format(" AND c.[Id] like '%{0}%'  AND c.[Name] like N'%{0}%'", key));
				var _total = SqlExQuery<TotalPaging>.ExcuteQueryFirst(sb.ToString());
				total = _total != null ? _total.TotalCount : 0;
			return listData;
			}

        public List<News> GetNewsServiceHome()
        {           
            //Lấy danh sách
            var sb = new StringBuilder();
           
            sb.AppendLine("select Top 4 * from News n");
            sb.AppendLine("left join NewsCategory c on n.NewsCategoryId = c.Id");
            sb.AppendLine("where n.Active = '1' and c.Type = '2'");

            var listData = SqlExQuery<News>.ExcuteQuery(sb.ToString());
        
            return listData;
        }

        public List<News> GetLatestNews()
        {
            //Lấy danh sách
            var sb = new StringBuilder();

            sb.AppendLine("select Top 3 * from News n");
            sb.AppendLine("left join NewsCategory c on n.NewsCategoryId = c.Id");
            sb.AppendLine("where n.Active = '1' and c.Type = '3'");

            var listData = SqlExQuery<News>.ExcuteQuery(sb.ToString());

            return listData;
        }

        public List<News> GetPagingList(string newcategory, string type, int pageNumber, int pageSize, ref int total)
        {
           
            //Lấy danh sách
            var sb = new StringBuilder();         
            sb.AppendLine("SELECT * FROM(");
            sb.AppendLine("SELECT ROW_NUMBER() OVER(ORDER BY c.DateCreated desc) AS RowNumber, n.*");
            sb.AppendLine("FROM News n WITH(NOLOCK) ");
            sb.AppendLine("left join NewsCategory c on n.NewsCategoryId = c.Id");
            sb.AppendLine(string.Format("where n.Active = 'True'"));

            if (!string.IsNullOrEmpty(newcategory))
            {
                sb.AppendLine(string.Format("and c.BreadCrumb LIKE '%{0}%'", newcategory));
            }

            if (!string.IsNullOrEmpty(type))
            {
                sb.AppendLine(string.Format("and c.Type = '{0}'", type));
            }

            sb.AppendLine(") as a");
            sb.AppendLine(string.Format("WHERE RowNumber BETWEEN(({0}-1) * {1}+1) AND({0} * {1})", pageNumber, pageSize));
            var listData = SqlExQuery<News>.ExcuteQuery(sb.ToString());
            //Tính tổng
            sb.Clear();       
            sb.AppendLine("SELECT COUNT(*) TotalCount FROM(");
            sb.AppendLine("SELECT ROW_NUMBER() OVER(ORDER BY c.DateCreated desc) AS RowNumber, n.Id");
            sb.AppendLine("FROM News n WITH(NOLOCK) ");
            sb.AppendLine("left join NewsCategory c on n.NewsCategoryId = c.Id");
            sb.AppendLine(string.Format("where n.Active = 'True'"));

            if (!string.IsNullOrEmpty(newcategory))
            {
                sb.AppendLine(string.Format("and c.BreadCrumb LIKE '%{0}%'", newcategory));
            }

            if (!string.IsNullOrEmpty(type))
            {
                sb.AppendLine(string.Format("and c.Type = '{0}'", type));
            }
            sb.AppendLine(") as a");

            var _total = SqlExQuery<TotalPaging>.ExcuteQueryFirst(sb.ToString());
            total = _total != null ? _total.TotalCount : 0;
            return listData;
        }
        public News GetByTitleUrl(string titleurl)
        {
            var query = from n in _NewsRepository.Table
                        where (n.NameUrl.Equals(titleurl) || n.Name.Contains(titleurl)) && n.Active
                        select n;
            return query.FirstOrDefault();
        }

        public List<News> GetLatestNewsByCategory(string newcategory, string id,string type)
        {

            //Lấy danh sách
            var sb = new StringBuilder();
          
            sb.AppendLine("SELECT Top 3 n.*");
            sb.AppendLine("FROM News n WITH(NOLOCK) ");
            sb.AppendLine("left join NewsCategory c on n.NewsCategoryId = c.Id");
            sb.AppendLine(string.Format("where n.Active = 'True'"));

            if (!string.IsNullOrEmpty(newcategory))
            {
                sb.AppendLine(string.Format("and c.BreadCrumb LIKE '%{0}%'", newcategory));
            }
            if (!string.IsNullOrEmpty(type))
            {
                sb.AppendLine(string.Format("and c.Type = '{0}'", type));
            }

            if (!string.IsNullOrEmpty(id))
            {
                sb.AppendLine(string.Format("and n.Id != '{0}'", id));
            }

           // sb.AppendLine(") as a");
           
            var listData = SqlExQuery<News>.ExcuteQuery(sb.ToString());
          
            return listData;
        }
    }
}
