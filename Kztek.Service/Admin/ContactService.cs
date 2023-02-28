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
	public interface IContactService
	{
		IQueryable<Contact> GetAll();
		IPagedList<Contact> GetAllPagingByFirst(string key, int pageNumber, int pageSize);
		Contact GetById(string Id);
		MessageReport Create(Contact obj);
		MessageReport Update(Contact obj);
		MessageReport DeleteById(string Id);
		MessageReport DeleteByIds(string lstId);
		//Phân trang dạng T-SQL
		List<ContactView> GetAllPagingListTSQL(string key, string filter, int pageNumber, int pageSize, ref int total);
	}

	public class ContactService : IContactService
	{
		private readonly IContactRepository _ContactRepository;
		private readonly ILogService _LogService;
		private readonly IUnitOfWork _UnitOfWork;
		public ContactService(IContactRepository _ContactRepository, ILogService _LogService, IUnitOfWork _UnitOfWork)
		{
			this._ContactRepository = _ContactRepository;
			this._LogService = _LogService;
			this._UnitOfWork = _UnitOfWork;
		}
		private User user = GetCurrentUser.GetUser();
		private void ClearCache()
		{
			CacheLayer.Clear("Contact_Cache");
		}
		public IQueryable<Contact> GetAll()
		{
			var query = from n in _ContactRepository.Table  orderby n.DateCreated descending select n;
			return query;
		}
		public IPagedList<Contact> GetAllPagingByFirst(string key, int pageNumber, int pageSize)
		{
			var query = from n in _ContactRepository.Table  select n;
			if (!string.IsNullOrWhiteSpace(key))
			{
			}
			var list = new PagedList<Contact>(query.OrderByDescending(n => n.DateCreated), pageNumber, pageSize);
			return list;
		}
		
		public Contact GetById(string Id)
		{
			return _ContactRepository.GetById(Id);
		}
		
		public MessageReport Create(Contact obj)
		{
			MessageReport report;
			try
			{
				_ContactRepository.Add(obj);
				Save();
				report = new MessageReport(true, "Thêm thành công");
			}
			catch (Exception ex)
			{
			report = new MessageReport(false, ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
			}
			_LogService.WriteLog(report, "Contact", obj.Id, ActionConfig.Create, user);
			return report;
		}
		
		public MessageReport Update(Contact obj)
		{
			MessageReport report;
			try
			{
				_ContactRepository.Update(obj);
				Save();
				report = new MessageReport(true, "Cập nhật thành công");
			}
			catch (Exception ex)
			{
			report = new MessageReport(false, ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
			}
			_LogService.WriteLog(report, "Contact", obj.Id, ActionConfig.Update, user);
			return report;
		}
		
		public MessageReport DeleteById(string Id)
		{
			MessageReport report;
			try
			{
				var obj = _ContactRepository.GetById(Id);
				if (obj != null)
				{
					_ContactRepository.Delete(obj);
					Save();
					report = new MessageReport(true, "Xóa thông tin thành công");
				_LogService.WriteLog(report, "Contact", obj.Id, ActionConfig.Delete, user);
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
					var objDelete = _ContactRepository.GetById(id);
				if (objDelete!=null)
				{
					_ContactRepository.Delete(objDelete);
							Save();
							report = new MessageReport(true, "Xóa thành công");
							_LogService.WriteLog(report, "Contact", objDelete.Id, ActionConfig.Delete, user);
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
		public List<ContactView> GetAllPagingListTSQL(string key, string filter, int pageNumber, int pageSize, ref int total)
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
				sb.AppendLine(string.Format("SELECT ROW_NUMBER() OVER(ORDER BY c.{0} {1}) AS RowNumber, c.*,CU.FullName AS CustomerName", _sort, _order));
				sb.AppendLine("FROM Contact c WITH(NOLOCK) LEFT JOIN Customer CU ON CU.Id = c.CustomerId WHERE 1=1 ");
				if (!string.IsNullOrWhiteSpace(key))
					sb.AppendLine(string.Format(" AND c.[Id] like '%{0}%' ", key));
				sb.AppendLine(") as a");
				sb.AppendLine(string.Format("WHERE RowNumber BETWEEN(({0}-1) * {1}+1) AND({0} * {1})", pageNumber, pageSize));
				var listData = SqlExQuery<ContactView>.ExcuteQuery(sb.ToString());
				//Tính tổng
				sb.Clear();
					sb.AppendLine("SELECT COUNT(*) TotalCount FROM Contact c WITH(NOLOCK) WHERE 1=1 ");
				if (!string.IsNullOrWhiteSpace(key))
					sb.AppendLine(string.Format(" AND c.[Id] like '%{0}%' ", key));
				var _total = SqlExQuery<TotalPaging>.ExcuteQueryFirst(sb.ToString());
				total = _total != null ? _total.TotalCount : 0;
			return listData;
			}
	}
}
