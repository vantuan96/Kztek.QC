using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	 public class TrashRepository : RepositoryBase<Trash>, ITrashRepository
	 {
		 public TrashRepository(IDatabaseFactory databaseFactory): base(databaseFactory)
		 {
		 }
	 }
	 public interface ITrashRepository : IRepository<Trash>
	 {
	 }
}
