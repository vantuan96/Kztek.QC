using Kztek.Data.Infrastructure;
using Kztek.Data.Repository;
using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Service.Admin
{
    public interface IUserConfigService
    {
        UserConfig GetById(string id);
        UserConfig GetByUserId(string id);

        MessageReport Create(UserConfig obj);
        MessageReport Update(UserConfig obj);
        MessageReport DeleteById(string id);
    }
    public class UserConfigService : IUserConfigService
    {
        private readonly IUserConfigRepository _UserConfigRepository;
        private readonly IUnitOfWork _UnitOfWork;

        public UserConfigService(IUserConfigRepository _UserConfigRepository, IUnitOfWork _UnitOfWork)
        {
            this._UserConfigRepository = _UserConfigRepository;
            this._UnitOfWork = _UnitOfWork;
        }

        public void Save()
        {
            _UnitOfWork.Commit();
        }

        public UserConfig GetById(string id)
        {
            return _UserConfigRepository.GetById(id);
        }

        public UserConfig GetByUserId(string id)
        {
            var query = from n in _UserConfigRepository.Table
                        where n.UserId.Equals(id)
                        select n;
            return query.FirstOrDefault();
        }

        public MessageReport Create(UserConfig obj)
        {
            MessageReport report;
            try
            {
                _UserConfigRepository.Add(obj);
                Save();
                report = new MessageReport(true, "Thêm cấu hình người dùng thành công");
            }
            catch (Exception ex)
            {
                report = new MessageReport(false, ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
            }
            return report;
        }

        public MessageReport DeleteById(string id)
        {
            MessageReport report;
            try
            {
                var obj = _UserConfigRepository.GetById(id);
                if (obj != null)
                {
                    _UserConfigRepository.Delete(obj);
                    Save();
                    report = new MessageReport(true, "Xóa người dùng thành công");
                }
                else
                {
                    report = new MessageReport(false, "Người dùng không tồn tại");
                }
            }
            catch (Exception ex)
            {
                report = new MessageReport(false, ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
            }
            return report;
        }

        public MessageReport Update(UserConfig obj)
        {
            MessageReport report;
            try
            {
                _UserConfigRepository.Update(obj);
                Save();
                report = new MessageReport(true, "Cập nhật cấu hình người dùng thành công");
            }
            catch (Exception ex)
            {
                report = new MessageReport(false, ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
            }
            return report;
        }
    }
}
