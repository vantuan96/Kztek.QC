using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	 public class UserConfigRepository : RepositoryBase<UserConfig>, IUserConfigRepository
	 {
		 public UserConfigRepository(IDatabaseFactory databaseFactory): base(databaseFactory)
		 {
		 }
	 }
	 public interface IUserConfigRepository : IRepository<UserConfig>
	 {
	 }
}
