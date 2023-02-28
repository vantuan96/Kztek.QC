using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	 public class WebInfoRepository : RepositoryBase<WebInfo>, IWebInfoRepository
	 {
		 public WebInfoRepository(IDatabaseFactory databaseFactory): base(databaseFactory)
		 {
		 }
	 }
	 public interface IWebInfoRepository : IRepository<WebInfo>
	 {
	 }
}
