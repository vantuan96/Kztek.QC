using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	 public class LogRepository : RepositoryBase<Log>, ILogRepository
	 {
		 public LogRepository(IDatabaseFactory databaseFactory): base(databaseFactory)
		 {
		 }
	 }
	 public interface ILogRepository : IRepository<Log>
	 {
	 }
}
