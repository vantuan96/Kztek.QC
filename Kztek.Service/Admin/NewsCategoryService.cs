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
	public interface INewsCategoryService
	{
		IQueryable<NewsCategory> GetAll();
		IQueryable<NewsCategory> GetAllActive();
		IPagedList<NewsCategory> GetAllPagingByFirst(string key, int pageNumber, int pageSize);
		NewsCategory GetById(string Id);
		MessageReport Create(NewsCategory obj);
		MessageReport Update(NewsCategory obj);
		MessageReport DeleteById(string Id);
		MessageReport DeleteByIds(string lstId);
		bool UpdateBreadCrumb(NewsCategory model, string typ);
		//Phân trang dạng T-SQL
		List<NewsCategory> GetAllPagingListTSQL(string key, string filter, int pageNumber, int pageSize, ref int total);
        NewsCategory GetByName(string name);

    }

	public class NewsCategoryService : INewsCategoryService
	{
		private readonly INewsCategoryRepository _NewsCategoryRepository;
		private readonly ILogService _LogService;
		private readonly IUnitOfWork _UnitOfWork;
		public NewsCategoryService(INewsCategoryRepository _NewsCategoryRepository, ILogService _LogService, IUnitOfWork _UnitOfWork)
		{
			this._NewsCategoryRepository = _NewsCategoryRepository;
			this._LogService = _LogService;
			this._UnitOfWork = _UnitOfWork;
		}
		private User user = GetCurrentUser.GetUser();
		private void ClearCache()
		{
			CacheLayer.Clear("NewsCategory_Cache");
		}
		public IQueryable<NewsCategory> GetAll()
		{
			var query = from n in _NewsCategoryRepository.Table  orderby n.DateCreated descending select n;
			return query;
		}
		public IQueryable<NewsCategory> GetAllActive()
		{
			var query = from n in _NewsCategoryRepository.Table where n.Active == true  orderby n.DateCreated descending select n;
			return query;
		}
		
		public IPagedList<NewsCategory> GetAllPagingByFirst(string key, int pageNumber, int pageSize)
		{
			var query = from n in _NewsCategoryRepository.Table  select n;
			if (!string.IsNullOrWhiteSpace(key))
			{
				query = query.Where(n => n.Name.Contains(key));
			}
			var list = new PagedList<NewsCategory>(query.OrderByDescending(n => n.DateCreated), pageNumber, pageSize);
			return list;
		}
		
		public NewsCategory GetById(string Id)
		{
			return _NewsCategoryRepository.GetById(Id);
		}
		
		public MessageReport Create(NewsCategory obj)
		{
			MessageReport report;
			try
			{
				_NewsCategoryRepository.Add(obj);
				Save();
				report = new MessageReport(true, "Thêm thành công");
				//Update BreadCrumb
				UpdateBreadCrumb(obj, "add");
			}
			catch (Exception ex)
			{
			report = new MessageReport(false, ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
			}
			_LogService.WriteLog(report, "NewsCategory", obj.Id, ActionConfig.Create, user);
			return report;
		}
		
		public MessageReport Update(NewsCategory obj)
		{
			MessageReport report;
			try
			{
				_NewsCategoryRepository.Update(obj);
				Save();
				report = new MessageReport(true, "Cập nhật thành công");
				//Update BreadCrumb
				UpdateBreadCrumb(obj, "update");
			}
			catch (Exception ex)
			{
			report = new MessageReport(false, ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
			}
			_LogService.WriteLog(report, "NewsCategory", obj.Id, ActionConfig.Update, user);
			return report;
		}
		
		public MessageReport DeleteById(string Id)
		{
			MessageReport report;
			try
			{
				var obj = _NewsCategoryRepository.GetById(Id);
				if (obj != null)
				{
					_NewsCategoryRepository.Delete(obj);
					Save();
					report = new MessageReport(true, "Xóa thông tin thành công");
				_LogService.WriteLog(report, "NewsCategory", obj.Id, ActionConfig.Delete, user);
				//Update BreadCrumb
				UpdateBreadCrumb(obj, "add");
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
					var objDelete = _NewsCategoryRepository.GetById(id);
				if (objDelete!=null)
				{
					_NewsCategoryRepository.Delete(objDelete);
							Save();
							//Update BreadCrumb
							UpdateBreadCrumb(objDelete, "del");
							report = new MessageReport(true, "Xóa thành công");
							_LogService.WriteLog(report, "NewsCategory", objDelete.Id, ActionConfig.Delete, user);
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
		public bool UpdateBreadCrumb(NewsCategory model, string typ)
		{
			var chk = true;
			try
			{
				//Chỉ update khi menu có tồn tại
				if (model != null)
				{
					if (typ == "add") // trường hợp tạo mới menu
					{
							//Nếu ParentId==0 thì cập nhập luôn BreadCrumb là /{MenuId}/
							if (model.ParentId == "0")
							{
							model.BreadCrumb = string.Format("/{0}/", model.Id);
							model.Depth = 1;
						}
						else
						{
							// dùng đệ quy truy vấn ngược lên từ ParentId hiện tại để lấy hết các Menu parent và lưu vào trong 1 List tuần tự sau này xử lý ngược lại
							// kết quả của truy vấn theo thứ tự từ cháu lên tới cha, ông, cụ, kỵ
							var b = ListBreadCrumb(model);
							model.BreadCrumb = b.BreadCrumb;
							model.Depth = b.Depth;
						}
						// #Save
						_NewsCategoryRepository.Update(model);
						Save();
						}
					else if (typ == "update") // trường hợp cập nhập menu
						{
						// check ParentId
						if (model.ParentId == "0")
						{
								model.BreadCrumb = string.Format("/{0}/", model.Id);
							model.Depth = 1;
						}
						else
						{
							//remove bỏ những breadCrumb cũ
							var oldBreadCrumbs = (from m in _NewsCategoryRepository.Table
							where m.BreadCrumb.Contains("/" + model.Id + "/") 
							select m).ToList();
							foreach (var item in oldBreadCrumbs)
							{
								item.BreadCrumb.Replace(model.ParentId + "/", "");
								_NewsCategoryRepository.Update(item);
							}
						Save();
						var b = ListBreadCrumb(model);
							model.BreadCrumb = b.BreadCrumb;
						model.Depth = b.Depth;
						}
						// #Save new BreadCrumb
						_NewsCategoryRepository.Update(model);
						Save();
					}
					else if (typ == "del") // trường hợp xóa menu
					{
					//remove bỏ những breadCrumb cũ
						var oldBreadCrumbs = from m in _NewsCategoryRepository.Table
						where  m.BreadCrumb.Contains("/" + model.ParentId + "/")
						select m;
						foreach (var item in oldBreadCrumbs)
					{
						item.BreadCrumb.Replace(model.ParentId + "/", "");
						_NewsCategoryRepository.Update(item);
					}
				Save();
				}
				}
			}
			catch (Exception ex)
			{
				chk = false;
			}
			return chk;
		}
		//Hàm trả về string BreadCrumb khi đã tập hợp đc các menu cha bắt đầu từ menu truyền vào
		public NewsCategory ListBreadCrumb(NewsCategory model)
			{
				var listb = new List<NewsCategory>();
					listb.Add(model); // add cấp nhỏ nhất
					DataBreadCrumbs(model, listb);
					var b = "/";
					if (listb.Any())
					{
					for (int i = listb.Count; i > 0; i--)
					{
							var m = listb[i - 1];
							b += m.Id + "/";
					}
				}
			//dùng new MenuFunction làm nơi chứa tạm dữ liệu tìm đc
			return new NewsCategory { BreadCrumb = b, Depth = listb.Count };
		}
		public void DataBreadCrumbs(NewsCategory model, List<NewsCategory> listBreadCrumb)
		{
				//Truy vấn ngược dần lên các cấp menu cha rồi đưa vào một tập hợp
			var parentMenu = GetById(model.ParentId);
			if (parentMenu != null)
			{
				listBreadCrumb.Add(parentMenu);
			// Gọi lại cho đến hết (parentId==0 là hết)
			DataBreadCrumbs(parentMenu, listBreadCrumb);
			}
		}
		//Save change
		public void Save()
		{
			_UnitOfWork.Commit();
		}
		public List<NewsCategory> GetAllPagingListTSQL(string key, string filter, int pageNumber, int pageSize, ref int total)
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
				sb.AppendLine("FROM NewsCategory c WITH(NOLOCK) WHERE 1=1 ");
				if (!string.IsNullOrWhiteSpace(key))
					sb.AppendLine(string.Format(" AND c.[Id] like '%{0}%'  OR c.[Name] like N'%{0}%'", key));
				sb.AppendLine(") as a");
				sb.AppendLine(string.Format("WHERE RowNumber BETWEEN(({0}-1) * {1}+1) AND({0} * {1})", pageNumber, pageSize));
				var listData = SqlExQuery<NewsCategory>.ExcuteQuery(sb.ToString());
				//Tính tổng
				sb.Clear();
					sb.AppendLine("SELECT COUNT(*) TotalCount FROM NewsCategory c WITH(NOLOCK) WHERE 1=1 ");
				if (!string.IsNullOrWhiteSpace(key))
					sb.AppendLine(string.Format(" AND c.[Id] like '%{0}%'  AND c.[Name] like N'%{0}%'", key));
				var _total = SqlExQuery<TotalPaging>.ExcuteQueryFirst(sb.ToString());
				total = _total != null ? _total.TotalCount : 0;
			return listData;
			}
        public NewsCategory GetByName(string name)
        {
            var query = from n in _NewsCategoryRepository.Table
                        where n.Active == true && (n.Name.ToLower().Contains(name.ToLower()) || n.NameUrl.Contains(name.ToLower()))
                        select n;
            return query.FirstOrDefault();
        }
    }
}
