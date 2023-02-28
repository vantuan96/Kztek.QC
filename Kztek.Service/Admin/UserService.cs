using Kztek.Data.Infrastructure;
using Kztek.Data.Repository;
using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Service.Admin
{
    public interface IUserService
    {
        IQueryable<User> GetAll();
        IQueryable<User> GetAllActive();
        IPagedList<User> GetAllPagingByFirst(string key, int pageNumber, int pageSize);

        User GetById(string id);
        User GetByUserName(string name);
        User GetByEmail(string email);

        User GetByUserNameOREmail(string name);

        User GetByUserName_Id(string name, string id);
        User GetByEmail_Id(string email, string id);

        MessageReport Create(User obj);
        MessageReport Update(User obj);
        MessageReport DeleteById(string id);
        MessageReport RestorePassById(string id);
    }
    public class UserService : IUserService
    {
        private readonly IUserRepository _UserRepository;

        private readonly ILogService _LogService;

        private readonly IUnitOfWork _UnitOfWork;

        public UserService(IUserRepository _UserRepository, ILogService _LogService, IUnitOfWork _UnitOfWork)
        {
            this._UserRepository = _UserRepository;
            this._LogService = _LogService;
            this._UnitOfWork = _UnitOfWork;
        }

        private User user = GetCurrentUser.GetUser();

        public IQueryable<User> GetAll()
        {
            var query = from n in _UserRepository.Table
                        where n.IsDeleted == false
                        select n;
            return query;
        }

        public IQueryable<User> GetAllActive()
        {
            var query = from n in _UserRepository.Table
                        where n.Active && n.IsDeleted == false
                        select n;
            return query;
        }

        public IPagedList<User> GetAllPagingByFirst(string key, int pageNumber, int pageSize)
        {
            var query = from n in _UserRepository.Table
                        where n.IsDeleted == false
                        select n;
            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.Name.Contains(key) || n.Email.Contains(key));
            }
            var list = new PagedList<User>(query.OrderBy(n => n.Name), pageNumber, pageSize);
            return list;
        }

        public User GetById(string id)
        {
            return _UserRepository.GetById(id);
        }

        public MessageReport Create(User obj)
        {
            var report = new MessageReport(false, "Có lỗi xảy ra");

            try
            {
                _UserRepository.Add(obj);
                Save();

                report = new MessageReport(true, "Thêm thành công");
            }
            catch (Exception ex)
            {
                report = new MessageReport(false, ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
            }

            _LogService.WriteLog(report, "User", obj.Id, ActionConfig.Create, user);

            return report;
        }

        public MessageReport Update(User obj)
        {
            var report = new MessageReport(false, "Có lỗi xảy ra");

            try
            {
                _UserRepository.Update(obj);
                Save();
                report = new MessageReport(true, "Cập nhật thành công");
            }
            catch (Exception ex)
            {
                report = new MessageReport(false, ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
            }

            _LogService.WriteLog(report, "User", obj.Id, ActionConfig.Update, user);

            return report;
        }

        public MessageReport DeleteById(string id)
        {
            var report = new MessageReport(false, "Có lỗi xảy ra");

            try
            {
                var obj = _UserRepository.GetById(id);
                if (obj != null)
                {
                    if (!obj.Active)
                    {
                        if (!obj.Admin)
                        {
                            obj.IsDeleted = true;

                            _UserRepository.Update(obj);

                            Save();

                            report = new MessageReport(true, "Xóa thành công");
                        }
                        else
                        {
                            report = new MessageReport(false, "Không thể xóa");
                        }
                    }
                    else
                    {
                        report = new MessageReport(false, "Thông tin đang sử dụng. Không thể xóa.");
                    }
                }
                else
                {
                    report = new MessageReport(false, "Thông tin không tồn tại");
                }
            }
            catch (Exception ex)
            {
                report = new MessageReport(false, ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
            }

            _LogService.WriteLog(report, "User", id, ActionConfig.Delete, user);

            return report;
        }

        public MessageReport RestorePassById(string id)
        {
            throw new NotImplementedException();
        }

        //Save change
        public void Save()
        {
            _UnitOfWork.Commit();
        }

        public User GetByUserName(string name)
        {
            var query = from n in _UserRepository.Table
                        where n.Username.Equals(name) && n.IsDeleted == false
                        select n;
            return query.FirstOrDefault();
        }

        public User GetByEmail(string email)
        {
            var query = from n in _UserRepository.Table
                        where n.Email.Equals(email) && n.IsDeleted == false
                        select n;
            return query.FirstOrDefault();
        }

        public User GetByUserName_Id(string name, string id)
        {
            var query = from n in _UserRepository.Table
                        where n.Username.Equals(name) && n.Id != id && n.IsDeleted == false
                        select n;
            return query.FirstOrDefault();
        }

        public User GetByEmail_Id(string email, string id)
        {
            var query = from n in _UserRepository.Table
                        where n.Email.Equals(email) && n.Id != id && n.IsDeleted == false
                        select n;
            return query.FirstOrDefault();
        }

        public User GetByUserNameOREmail(string name)
        {
            var query = from n in _UserRepository.Table
                        where (n.Username.Equals(name) || n.Email.Equals(name)) && n.IsDeleted == false
                        select n;
            return query.FirstOrDefault();
        }
    }
}
