/* **************************************
* HỆ THỐNG GENCODE TỰ ĐỘNG
* CREATE: 04/06/2019 3:12:27 PM
* AUTHOR: HNG-0988388000
*/
using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	public class ProductRepository : RepositoryBase<Product>, IProductRepository
	{
		public ProductRepository(IDatabaseFactory databaseFactory): base(databaseFactory)
		{
		}
	}
	public interface IProductRepository : IRepository<Product>
	{
	}
}
