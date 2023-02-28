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
	public interface ICustomerService
	{
		IQueryable<Customer> GetAll();
		IQueryable<Customer> GetAllActive();
		IPagedList<Customer> GetAllPagingByFirst(string key, int pageNumber, int pageSize);
		Customer GetById(string Id);
		MessageReport Create(Customer obj);
		MessageReport Update(Customer obj);
		MessageReport DeleteById(string Id);
		MessageReport DeleteByIds(string lstId);
		//Phân trang dạng T-SQL
		List<Customer> GetAllPagingListTSQL(string key, string filter, int pageNumber, int pageSize, ref int total);
        IQueryable<Customer> GetHome();
    }

	public class CustomerService : ICustomerService
	{
		private readonly ICustomerRepository _CustomerRepository;
		private readonly ILogService _LogService;
		private readonly IUnitOfWork _UnitOfWork;
		public CustomerService(ICustomerRepository _CustomerRepository, ILogService _LogService, IUnitOfWork _UnitOfWork)
		{
			this._CustomerRepository = _CustomerRepository;
			this._LogService = _LogService;
			this._UnitOfWork = _UnitOfWork;
		}
		private User user = GetCurrentUser.GetUser();
		private void ClearCache()
		{
			CacheLayer.Clear("Customer_Cache");
		}
		public IQueryable<Customer> GetAll()
		{
			var query = from n in _CustomerRepository.Table  orderby n.DateCreated descending select n;
			return query;
		}
		public IQueryable<Customer> GetAllActive()
		{
			var query = from n in _CustomerRepository.Table where n.Active == true  orderby n.DateCreated descending select n;
			return query;
		}
		
		public IPagedList<Customer> GetAllPagingByFirst(string key, int pageNumber, int pageSize)
		{
			var query = from n in _CustomerRepository.Table  select n;
			if (!string.IsNullOrWhiteSpace(key))
			{
				query = query.Where(n => n.FullName.Contains(key));
			}
			var list = new PagedList<Customer>(query.OrderByDescending(n => n.DateCreated), pageNumber, pageSize);
			return list;
		}
		
		public Customer GetById(string Id)
		{
			return _CustomerRepository.GetById(Id);
		}
		
		public MessageReport Create(Customer obj)
		{
			MessageReport report;
			try
			{
				_CustomerRepository.Add(obj);
				Save();
				report = new MessageReport(true, "Thêm thành công");
			}
			catch (Exception ex)
			{
			report = new MessageReport(false, ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
			}
			_LogService.WriteLog(report, "Customer", obj.Id, ActionConfig.Create, user);
			return report;
		}
		
		public MessageReport Update(Customer obj)
		{
			MessageReport report;
			try
			{
				_CustomerRepository.Update(obj);
				Save();
				report = new MessageReport(true, "Cập nhật thành công");
			}
			catch (Exception ex)
			{
			report = new MessageReport(false, ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
			}
			_LogService.WriteLog(report, "Customer", obj.Id, ActionConfig.Update, user);
			return report;
		}
		
		public MessageReport DeleteById(string Id)
		{
			MessageReport report;
			try
			{
				var obj = _CustomerRepository.GetById(Id);
				if (obj != null)
				{
					_CustomerRepository.Delete(obj);
					Save();
					report = new MessageReport(true, "Xóa thông tin thành công");
				_LogService.WriteLog(report, "Customer", obj.Id, ActionConfig.Delete, user);
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
					var objDelete = _CustomerRepository.GetById(id);
				if (objDelete!=null)
				{
					_CustomerRepository.Delete(objDelete);
							Save();
							report = new MessageReport(true, "Xóa thành công");
							_LogService.WriteLog(report, "Customer", objDelete.Id, ActionConfig.Delete, user);
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
		public List<Customer> GetAllPagingListTSQL(string key, string filter, int pageNumber, int pageSize, ref int total)
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
				sb.AppendLine("FROM Customer c WITH(NOLOCK) WHERE 1=1 ");
				if (!string.IsNullOrWhiteSpace(key))
					sb.AppendLine(string.Format(" AND c.[Id] like '%{0}%' ", key));
				sb.AppendLine(") as a");
				sb.AppendLine(string.Format("WHERE RowNumber BETWEEN(({0}-1) * {1}+1) AND({0} * {1})", pageNumber, pageSize));
				var listData = SqlExQuery<Customer>.ExcuteQuery(sb.ToString());
				//Tính tổng
				sb.Clear();
					sb.AppendLine("SELECT COUNT(*) TotalCount FROM Customer c WITH(NOLOCK) WHERE 1=1 ");
				if (!string.IsNullOrWhiteSpace(key))
					sb.AppendLine(string.Format(" AND c.[Id] like '%{0}%' ", key));
				var _total = SqlExQuery<TotalPaging>.ExcuteQueryFirst(sb.ToString());
				total = _total != null ? _total.TotalCount : 0;
			return listData;
			}
        public IQueryable<Customer> GetHome()
        {
            var query = from n in _CustomerRepository.Table
                        where n.Active == true
                        orderby n.DateCreated descending select n;
            return query.Take(4);
        }
    }
}
