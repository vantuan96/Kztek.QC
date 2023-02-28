using Kztek.Data.SqlHelper;
using Kztek.Model.CustomModel;
using Kztek.Web.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Service.Admin
{
    public interface ITrashService
    {
        MessageReport SaveProcess(string TableName, string ColumnId, string LogId);
    }

    public class TrashService : ITrashService
    {
        public TrashService()
        {

        }

        public MessageReport SaveProcess(string TableName, string ColumnId, string LogId)
        {
            MessageReport rs = new MessageReport();
            rs.isSuccess = false;
            rs.Message = "Có lỗ xảy ra";

            try
            {
                var str = new StringBuilder();
                str.AppendLine("INSERT INTO Trash (Id, TableName, ColumnId, LogId, DateCreated) VALUES (");
                str.AppendLine(string.Format("'{0}'", Common.GenerateId()));
                str.AppendLine(string.Format(", '{0}'", TableName));
                str.AppendLine(string.Format(", '{0}'", ColumnId));
                str.AppendLine(string.Format(", '{0}'", LogId));
                str.AppendLine(", GETDATE()");
                str.AppendLine(")");

                var k = ExcuteSQL.Execute(str.ToString());

                if (k)
                {
                    rs.isSuccess = k;
                    rs.Message = "Thêm thành công";
                }
            }
            catch (Exception ex)
            {
                rs.isSuccess = false;
                rs.Message = ex.Message;
            }

            return rs;
        }
    }
}
