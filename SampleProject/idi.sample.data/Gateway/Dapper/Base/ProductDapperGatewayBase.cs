using Dapper;
using Dapper.SQLGateway;
using Dapper.SQLGateway.Interfaces;
using Dapper.SQLGateway.Models;
using idi.sample.data.Entities;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Data;

namespace idi.sample.Data.Gateway.Base 
{
	public class ProductDapperBaseGateway : DapperAsyncGateway, IEntityDapperGateway<Product, long> 
	{
		public ProductDapperBaseGateway(string connectionString, ILoggerFactory loggerFactory) : base(connectionString, loggerFactory) { }
		
		public virtual async Task<InsertResponse<long>> CreateAsync(Product product)
		{
			var sql = @"Execute [dbo].[Product_Insert] 
				 @name
				, @description
				, @isActive
				, @createdBy
				, @createdDate
				, @modifiedBy
				, @modifiedDate
				, @id OUTPUT
			";
			var _params = new DynamicParameters();
			_params.Add("name", product.Name);
			_params.Add("description", product.Description);
			_params.Add("isActive", product.IsActive);
			_params.Add("createdBy", product.CreatedBy);
			_params.Add("createdDate", product.CreatedDate);
			_params.Add("modifiedBy", product.ModifiedBy);
			_params.Add("modifiedDate", product.ModifiedDate);
			_params.Add("id", dbType: DbType.Int64, direction: ParameterDirection.Output);
			var result = await base.ExecuteAsync(sql, _params);

			var id = _params.Get<long>("id");
			product.Id = id;
			return InsertResponse<long>.GetInsertResponse(result, id);
		}
	
		public virtual async Task<TransactionResponse> UpdateAsync(Product product) 
		{
			var sql = @"Execute [dbo].[Product_Update] 
				 @id
				, @name
				, @description
				, @isActive
				, @createdBy
				, @createdDate
				, @modifiedBy
				, @modifiedDate
			";
			var result = await base.ExecuteAsync(sql, product);
			return result;
		}

		public virtual async Task<TransactionResponse> DeleteAsync(long id)
		{
			var sql = "Execute [dbo].[Product_Delete] @id";
			var result = await base.ExecuteAsync(sql, new { @id = id });
			return result;
		}

		public virtual async Task<Product> SelectByIdAsync(long id)
		{
			var sql = "select * from [dbo].[Product] where Id = @id";
			return await this.QuerySingleAsync<Product>(sql, new { @id = id });
		}

		public virtual async Task<IDataResult<Product>> ReadAllAsync(PageInfo pageInfo) 
		{
			var sql = "select * from [dbo].[Product]";
			return await this.QueryDynamicAsync<Product>(sql, new DynamicParameters(), pageInfo);
		}
	}
}