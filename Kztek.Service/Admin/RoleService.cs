using Kztek.Data.Repository;
using Kztek.Data.Infrastructure;
using Kztek.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kztek.Model.CustomModel;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Models;

namespace Kztek.Service.Admin
{
    public interface IRoleService
    {
        IEnumerable<Role> GetAllByFirst(string key);
        IEnumerable<Role> GetAllActive();
        IEnumerable<Role> GetAllByUserId(string id);

        Role GetById(string id);
        Role GetByName(string name);

        MessageReport Create(Role obj);
        MessageReport Update(Role obj);
        MessageReport DeleteById(string id);
    }
    public class RoleService: IRoleService
    {
        private readonly IUserRepository _UserRepository;
        private readonly IRoleRepository _RoleRepository;
        private readonly IUserRoleRepository _UserRoleRepository;

        private readonly ILogService _LogService;

        private readonly IUnitOfWork _UnitOfWork;

        public RoleService(IRoleRepository _RoleRepository, ILogService _LogService, IUnitOfWork _UnitOfWork, IUserRoleRepository _UserRoleRepository, IUserRepository _UserRepository)
        {
            this._RoleRepository = _RoleRepository;
            this._UnitOfWork = _UnitOfWork;
            this._UserRoleRepository = _UserRoleRepository;
            this._UserRepository = _UserRepository;
            this._LogService = _LogService;
        }

        private User user = GetCurrentUser.GetUser();

        public MessageReport Create(Role obj)
        {
            var report = new MessageReport(false, "Có lỗi xảy ra");

            try
            {
                _RoleRepository.Add(obj);
                Save();

                report = new MessageReport(true, "Thêm mới thành công");
            }
            catch (Exception ex)
            {
                report = new MessageReport(false, ex.Message);
            }

            _LogService.WriteLog(report, "Role", obj.Id, ActionConfig.Create, user);

            return report;
        }

        public MessageReport DeleteById(string id)
        {
            var report = new MessageReport(false, "Có lỗi xảy ra");

            try
            {
                var obj = _RoleRepository.GetById(id);
                if (obj != null)
                {
                    obj.IsDeleted = true;

                    _RoleRepository.Update(obj);
                    Save();

                    report = new MessageReport(true, "Xóa thành công");
                }
            }
            catch (Exception ex)
            {
                report = new MessageReport(false, ex.Message);
            }

            _LogService.WriteLog(report, "Role", id, ActionConfig.Delete, user);

            return report;
        }

        public Role GetById(string id)
        {
            return _RoleRepository.GetById(id);
        }

        public MessageReport Update(Role obj)
        {
            var report = new MessageReport(false, "Có lỗi xảy ra");

            try
            {
                _RoleRepository.Update(obj);
                Save();

                report = new MessageReport(true, "Cập nhật thành công");
            }
            catch (Exception ex)
            {
                report = new MessageReport(false, ex.Message);
            }

            _LogService.WriteLog(report, "Role", obj.Id, ActionConfig.Update, user);

            return report;
        }

        //Save change
        public void Save()
        {
            _UnitOfWork.Commit();
        }

        public IEnumerable<Role> GetAllByFirst(string key)
        {
            var query = from n in _RoleRepository.Table
                        where !n.IsDeleted
                        select n;
            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.RoleName.Contains(key));
            }
            return query;
        }

        public IEnumerable<Role> GetAllActive()
        {
            var query = from n in _RoleRepository.Table
                        where n.Active && !n.IsDeleted
                        select n;
            return query;
        }

        public Role GetByName(string name)
        {
            var query = from n in _RoleRepository.Table
                        where n.RoleName.Equals(name) && !n.IsDeleted
                        select n;
            return query.FirstOrDefault();
        }

        public IEnumerable<Role> GetAllByUserId(string id)
        {
            var query = from ur in _UserRoleRepository.Table
                        join r in _RoleRepository.Table on ur.RoleId equals r.Id into ur_r
                        from r in ur_r.DefaultIfEmpty()
                        join u in _UserRepository.Table on ur.UserId equals u.Id into ur_u
                        from u in ur_u.DefaultIfEmpty()
                        where ur.UserId.Equals(id) && r.IsDeleted == false
                        select r;
            return query;
        }
    }
}
