using System;
using System.Collections.Generic;
using System.Linq;
using PagedList;
using Kztek.Model.Models;
using Kztek.Data.Repository;
using Kztek.Data.Infrastructure;
using Kztek.Web.Core.Functions;
using Kztek.Model.CustomModel;
using Kztek.Web.Core.Models;

namespace Kztek.Service.Admin
{
    public interface IMenuFunctionService
    {
        MenuFunction GetByName(string name);
        MenuFunction GetByName_Id(string name, string id);
        MenuFunction getById(string id);
        MenuFunction GetByControllerAction(string controller, string action);
        MenuFunction GetByController(string controller);

        IEnumerable<MenuFunction> GetAll();
        IEnumerable<MenuFunction> GetAllActive();
        IEnumerable<MenuFunction> GetAllParentActive();
        IEnumerable<MenuFunction> GetAllChildActive();
        IEnumerable<MenuFunction> GetAllChildByParentId(string id);
        IEnumerable<MenuFunction> GetAllActiveChildByParentId(string id);
        IPagedList<MenuFunction> GetAllParentPagingByFirst(string key, int PageNumber, int pageSize);

        List<MenuFunction> GetAllParentByFirst(string key);
        List<MenuFunction> GetAllMenu(string key);
        List<MenuFunction> GetAllMenuByPermisstion(string uId, bool isAdmin = false); // lay toan bo menu theo quyen cua user hien tai

        MessageReport Create(MenuFunction obj);
        MessageReport Update(MenuFunction obj);
        MessageReport DeleteById(string id);
        MessageReport DeleteByIds(string lstId);
        MessageReport ActiveByIds(string lstId, string active);

        //HNG
        /// <summary>
        /// Hàm cập nhập lại BreadCrumb cho menu không dùng trigger
        /// Cho thêm, sửa, xóa menu
        /// </summary>
        /// <param name="mId">MenuFunctionId</param>
        /// <param name="parentId"></param>
        /// <param name="typ">Dùng cho thêm hay sửa hay xóa: add|update|del</param>
        /// <returns></returns>
        bool UpdateBreadCrumb(MenuFunction model, string typ);

        IEnumerable<MenuFunction> GetAllChildActiveByParentId(string id);
    }
    public class MenuFunctionService : IMenuFunctionService
    {
        private readonly IMenuFunctionRepository _MenuFunctionRepository;
        private readonly IRoleMenuRepository _RoleMenuRepository;
        private readonly IUserRoleRepository _UserRoleRepository;
        private readonly IUnitOfWork _UnitOfWork;

        private readonly ILogService _LogService;

        public MenuFunctionService(IMenuFunctionRepository _MenuFunctionRepository, IRoleMenuRepository _RoleMenuRepository, IUserRoleRepository _UserRoleRepository, IUnitOfWork _UnitOfWork, ILogService _LogService)
        {
            this._MenuFunctionRepository = _MenuFunctionRepository;
            this._RoleMenuRepository = _RoleMenuRepository;
            this._UserRoleRepository = _UserRoleRepository;
            this._UnitOfWork = _UnitOfWork;

            this._LogService = _LogService;
        }

        private User user = GetCurrentUser.GetUser();

        public MessageReport Create(MenuFunction obj)
        {
            var report = new MessageReport(false, "Có lỗi xảy ra");

            try
            {
                _MenuFunctionRepository.Add(obj);
                Save();

                report = new MessageReport(true, "Thêm mới thành công");

                //Update BreadCrumb
                UpdateBreadCrumb(obj, "add");
            }
            catch (Exception ex)
            {
                report = new MessageReport(false, ex.Message);
            }

            _LogService.WriteLog(report, "MenuFunction", obj.Id, ActionConfig.Create, user);

            return report;
        }

        public MessageReport DeleteById(string id)
        {
            var report = new MessageReport(false, "Có lỗi xảy ra");

            try
            {
                var objDelete = _MenuFunctionRepository.GetById(id);
                if (objDelete != null)
                {
                    _MenuFunctionRepository.Delete(objDelete);
                    Save();

                    //Update BreadCrumb
                    UpdateBreadCrumb(objDelete, "add");

                    report = new MessageReport(true, "Xóa thành công");
                }
            }
            catch (Exception ex)
            {
                report = new MessageReport(false, ex.Message);
            }

            _LogService.WriteLog(report, "MenuFunction", id, ActionConfig.Delete, user);

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
                    var objDelete = _MenuFunctionRepository.GetById(id);

                    if (objDelete!=null)
                    {
                        _MenuFunctionRepository.Delete(objDelete);
                        Save();

                        //Update BreadCrumb
                        UpdateBreadCrumb(objDelete, "delete");

                        report = new MessageReport(true, "Xóa thành công");

                        _LogService.WriteLog(report, "MenuFunction", objDelete.Id, ActionConfig.Delete, user);
                    }
                }

                report = new MessageReport(true, "Xóa thành công");
            }
            catch (Exception ex)
            {
                report = new MessageReport(false, ex.Message);
            }

            return report;
        }

        public IEnumerable<MenuFunction> GetAll()
        {
            var query = from n in _MenuFunctionRepository.Table
                        select n;
            return query;
        }

        public IEnumerable<MenuFunction> GetAllChildActive()
        {
            var query = from n in _MenuFunctionRepository.Table
                        where n.Active && n.ParentId != "0"
                        select n;
            return query;
        }

        public IPagedList<MenuFunction> GetAllParentPagingByFirst(string key, int PageNumber, int PageSize)
        {
            var query = from n in _MenuFunctionRepository.Table
                        where n.ParentId == "0"
                        select n;
            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.MenuName.Equals(key));
            }
            var list = new PagedList<MenuFunction>(query.OrderBy(n => n.OrderNumber), PageNumber, PageSize);
            return list;
        }

        public MessageReport Update(MenuFunction obj)
        {
            var report = new MessageReport(false, "Có lỗi xảy ra");

            try
            {
                _MenuFunctionRepository.Update(obj);
                Save();

                report = new MessageReport(true, "Cập nhật thành công");

                //Update BreadCrumb
                UpdateBreadCrumb(obj, "update");
            }
            catch (Exception ex)
            {
                report = new MessageReport(false, ex.Message);
            }

            return report;
        }

        public MessageReport ActiveByIds(string lstId, string active)
        {
            var report = new MessageReport(false, "Có lỗi xảy ra");

            try
            {
                string[] ids = lstId.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var id in ids)
                {
                    var obj = _MenuFunctionRepository.GetById(id);
                    if (obj != null)
                    {
                        if (active.Equals("True"))
                        {
                            obj.Active = true;
                        }

                        if (active.Equals("False"))
                        {
                            obj.Active = false;
                        }

                        _MenuFunctionRepository.Update(obj);
                        Save();
                    }
                }

                report = new MessageReport(true, "Cập nhật thành công");
            }
            catch (Exception ex)
            {
                report = new MessageReport(false, ex.Message);
            }

            return report;
        }

        public void Save()
        {
            _UnitOfWork.Commit();
        }

        public IEnumerable<MenuFunction> GetAllActive()
        {
            var query = from n in _MenuFunctionRepository.Table
                        where n.Active
                        orderby n.OrderNumber ascending
                        select n;
            return query;
        }

        public IEnumerable<MenuFunction> GetAllChildByParentId(string id)
        {
            var query = from n in _MenuFunctionRepository.Table
                        where n.ParentId.Equals(id)
                        orderby n.OrderNumber ascending
                        select n;
            return query;
        }

        public MenuFunction GetByName(string name)
        {
            var query = from n in _MenuFunctionRepository.Table
                        select n;
            return query.FirstOrDefault(n => n.MenuName.Equals(name) && n.ActionName.Equals("Index"));
        }

        public MenuFunction GetByName_Id(string name, string id)
        {
            var query = from n in _MenuFunctionRepository.Table
                        select n;
            return query.FirstOrDefault(n => n.MenuName.Equals(name) && n.ActionName.Equals("Index") && n.Id != id);
        }

        public MenuFunction getById(string id)
        {
            var query = from n in _MenuFunctionRepository.Table
                        where n.Id == id
                        select n;

            return query.FirstOrDefault();
        }

        public List<MenuFunction> GetAllParentByFirst(string key)
        {
            var query = from n in _MenuFunctionRepository.Table
                        where n.ParentId == "0"
                        select n;
            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.MenuName.Contains(key));
            }

            return query.ToList();
        }

        public List<MenuFunction> GetAllMenu(string key)
        {
            var query = from n in _MenuFunctionRepository.Table
                        select n;
            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.MenuName.Contains(key));
            }

            return query.OrderBy(c=>c.OrderNumber).ToList();
        }

        public MenuFunction GetByControllerAction(string controller, string action)
        {
            var query = from n in _MenuFunctionRepository.Table
                        select n;
            return query.FirstOrDefault(n => n.ControllerName.Equals(controller) && n.ActionName.Equals(action));
        }

        public MenuFunction GetByController(string controller)
        {
            var query = from n in _MenuFunctionRepository.Table
                        select n;
            return query.FirstOrDefault(n => n.ControllerName.Equals(controller));
        }

        public IEnumerable<MenuFunction> GetAllParentActive()
        {
            var query = from n in _MenuFunctionRepository.Table
                        where n.ParentId == "0" && n.Active
                        select n;
            return query;
        }

        public IEnumerable<MenuFunction> GetAllActiveChildByParentId(string id)
        {
            var query = from n in _MenuFunctionRepository.Table
                        where n.ParentId.Equals(id) && n.Active
                        orderby n.OrderNumber ascending
                        select n;
            return query;
        }

        public List<MenuFunction> GetAllMenuByPermisstion(string uId, bool isAdmin = false)
        {
            var query = from m in _MenuFunctionRepository.Table
                        join rm in _RoleMenuRepository.Table on m.Id equals rm.MenuId into rMenu
                        from rm in rMenu.DefaultIfEmpty()
                        join ur in _UserRoleRepository.Table on rm.RoleId equals ur.RoleId into uRole
                        from ur in uRole.DefaultIfEmpty()
                        where ur.UserId == uId && m.Active /*&& m.MenuType == "1"*/
                        orderby m.OrderNumber
                        select m;

            if (isAdmin)
            {
                query = from m in _MenuFunctionRepository.Table
                        join rm in _RoleMenuRepository.Table on m.Id equals rm.MenuId into rMenu
                        from rm in rMenu.DefaultIfEmpty()
                        where m.Active /*&& m.MenuType == "1"*/
                        orderby m.OrderNumber
                        select m;
            }

            return query.ToList();
        }


        public bool UpdateBreadCrumb(MenuFunction model, string typ)
        {
            var chk = true;
            try
            {
                //Chỉ update khi menu có tồn tại
                //var model = _MenuFunctionRepository.GetById(mId);
                if (model != null)
                {
                    if (typ == "add") // trường hợp tạo mới menu
                    {
                        //Nếu ParentId==0 thì cập nhập luôn BreadCrumb là /{MenuId}/
                        if (model.ParentId == "0")
                        {
                            model.Breadcrumb = string.Format("/{0}/", model.Id);
                            model.Dept = 1;
                        }
                        else
                        {
                            // dùng đệ quy truy vấn ngược lên từ ParentId hiện tại để lấy hết các Menu parent và lưu vào trong 1 List tuần tự sau này xử lý ngược lại
                            // kết quả của truy vấn theo thứ tự từ cháu lên tới cha, ông, cụ, kỵ
                            var b = ListBreadCrumb(model);
                            model.Breadcrumb = b.Breadcrumb;
                            model.Dept = b.Dept;
                        }

                        // #Save
                        _MenuFunctionRepository.Update(model);
                        Save();
                    }
                    else if (typ == "update") // trường hợp cập nhập menu
                    {
                        // check ParentId
                        if (model.ParentId == "0")
                        {
                            model.Breadcrumb = string.Format("/{0}/", model.Id);
                            model.Dept = 1;
                        }
                        else
                        {
                            //remove bỏ những breadCrumb cũ
                            var oldBreadCrumbs = (from m in _MenuFunctionRepository.Table
                                                  where m.Breadcrumb.Contains("/" + model.Id + "/")
                                                  select m).ToList();

                            foreach (var item in oldBreadCrumbs)
                            {
                                item.Breadcrumb.Replace(model.ParentId + "/", "");
                                _MenuFunctionRepository.Update(item);
                            }
                            Save();

                            var b = ListBreadCrumb(model);
                            model.Breadcrumb = b.Breadcrumb;
                            model.Dept = b.Dept;
                        }

                        // #Save new BreadCrumb
                        _MenuFunctionRepository.Update(model);
                        Save();
                    }
                    else if (typ == "del") // trường hợp xóa menu
                    {
                        //remove bỏ những breadCrumb cũ
                        var oldBreadCrumbs = from m in _MenuFunctionRepository.Table
                                             where m.Breadcrumb.Contains("/" + model.ParentId + "/")
                                             select m;

                        foreach (var item in oldBreadCrumbs)
                        {
                            item.Breadcrumb.Replace(model.ParentId + "/", "");
                            _MenuFunctionRepository.Update(item);
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
        public MenuFunction ListBreadCrumb(MenuFunction model)
        {
            var listb = new List<MenuFunction>();
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
            return new MenuFunction { Breadcrumb = b, Dept = listb.Count };
        }

        public void DataBreadCrumbs(MenuFunction model, List<MenuFunction> listBreadCrumb)
        {
            //Truy vấn ngược dần lên các cấp menu cha rồi đưa vào một tập hợp
            var parentMenu = getById(model.ParentId);
            if (parentMenu != null)
            {
                listBreadCrumb.Add(parentMenu);
                // Gọi lại cho đến hết (parentId==0 là hết)
                DataBreadCrumbs(parentMenu, listBreadCrumb);
            }
        }

        public IEnumerable<MenuFunction> GetAllChildActiveByParentId(string id)
        {
            var model = from n in _MenuFunctionRepository.Table
                        where n.ParentId == id && n.Active
                        select n;

            return model;
        }
    }
}
