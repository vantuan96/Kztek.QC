using Kztek.Data.Infrastructure;
using Kztek.Data.Repository;
using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Web.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Service.Admin
{
    public interface IWebInfoService
    {
        //Lấy 1 bản ghi duy nhất - chưa có khởi tạo
        WebInfo GetDefault();

        //Tạo bản ghi mới
        MessageReport Create(WebInfo obj);

        //Cập nhật bản ghi
        MessageReport Update(WebInfo obj);
    }

    public class WebInfoService : IWebInfoService
    {
        #region Repository
        private IWebInfoRepository _WebInfoRepository;
        private IUnitOfWork _UnitOfWork;

        public WebInfoService(IWebInfoRepository _WebInfoRepository, IUnitOfWork _UnitOfWork)
        {
            this._WebInfoRepository = _WebInfoRepository;
            this._UnitOfWork = _UnitOfWork;
        }
        #endregion

        #region Save
        //Hàm save thay đổi
        private void Save()
        {
            _UnitOfWork.Commit();
        }
        #endregion

        #region Thêm mới
        public MessageReport Create(WebInfo obj)
        {
            MessageReport report;
            try
            {
                _WebInfoRepository.Add(obj);
                Save();
                report = new MessageReport(true, "Thêm thành công");
            }
            catch (Exception ex)
            {
                report = new MessageReport(false, ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
            }
            return report;
        }
        #endregion

        #region Lấy 1 bản ghi
        public WebInfo GetDefault()
        {
            var query = from n in _WebInfoRepository.Table
                        select n;

            var obj = query.FirstOrDefault();
            if (obj == null)
            {
                obj = new WebInfo();
                obj.Id = Common.GenerateId();

                Create(obj);
            }

            return obj;
        }
        #endregion

        #region Cập nhật
        public MessageReport Update(WebInfo obj)
        {
            MessageReport report;
            try
            {
                _WebInfoRepository.Update(obj);
                Save();
                report = new MessageReport(true, "Cập nhật thành công");
            }
            catch (Exception ex)
            {
                report = new MessageReport(false, ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
            }

            return report;
        } 
        #endregion
    }
}
